using System;
using System.Collections; //обязательна для Enumerable<AssemblyDependencyResolver>
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

/// Источник https://github.com/dotnet/samples/blob/master/core/extensions/AppWithPlugin/AppWithPlugin/PluginLoadContext.cs
/// Немного переработан.
/// <summary>
/// Загрузчик библиотек
/// </summary>
public class CMDPluginLoadContext : AssemblyLoadContext , IEnumerable<AssemblyDependencyResolver>, IDisposable // IComparable<PluginLoadContext>
    {
    // Если использовать другой экземпляр CMDPluginLoadContext для каждого подключаемого модуля, 
    // подключаемые модули смогут иметь разные или даже конфликтующие зависимости без проблем.

    #region static
    /// <summary>
    /// Все созданные экземпляры. 
    /// </summary>
    static protected List<CMDPluginLoadContext> ListPluginLoadContext = new List<CMDPluginLoadContext>();

        /// <summary>
        /// Выбираем объект или создаем объект если нет подходящего
        /// </summary>
        /// <param name="pluginPath">библиотека с именем и путем</param>
        /// <returns></returns>
        static public CMDPluginLoadContext CreateInstance(string pluginPath) //
        {
            CMDPluginLoadContext rc = ListPluginLoadContext.FirstOrDefault(
                (pn => pn._pluginPath == pluginPath));
            if (rc == null) //
            { // нет с таким путем
                rc = new CMDPluginLoadContext(pluginPath);
                if (rc.IsAssemblyDependencyResolver) // создали  нормально
                    ListPluginLoadContext.Add(rc);
                else // создали не нормально
                    rc = null;
            }
            return rc;
        }
        #endregion static

	public IEnumerator<AssemblyDependencyResolver> GetEnumerator() //IEnumerable<PluginLoadContext>.IEnumerable<AssemblyDependencyResolver>.
        {
            foreach (CMDPluginLoadContext plc in ListPluginLoadContext)
            {
                yield return plc._resolver;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()        
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Сравнить  на равенство
        /// </summary>
        /// <param name="other">величина для сравнения</param>
        /// <returns>результат сравнения</returns>
        public override bool Equals(Object other)
        {
            return (other is CMDPluginLoadContext) && this.IsAssemblyDependencyResolver && //
                ((CMDPluginLoadContext)other).IsAssemblyDependencyResolver //
                && Equals((CMDPluginLoadContext)other);
        }

        /// <summary>
        /// Сравнить  на равенство
        /// </summary>
        /// <param name="other">величина для сравнения</param>
        /// <returns>результат сравнения</returns>
        public bool Equals(CMDPluginLoadContext other)
        { 
            return this._resolver.Equals( other._resolver) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.IsAssemblyDependencyResolver ? ( String.GetHashCode(this._pluginPath)) : 0;
        }

        void IDisposable.Dispose()
        {
            ListPluginLoadContext.Remove(this);
            //Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// false если PluginLoadContext представляет неизвестное значение (_resolver не создан)
        /// </summary>
        public readonly bool IsAssemblyDependencyResolver;

        #region protected
        /// <summary>
        /// позволяет изолировать загруженные сборки в разные группы, 
        /// чтобы версии сборок не конфликтовали друг с другом.
        /// </summary>
        protected AssemblyDependencyResolver _resolver;

        /// <summary>
        /// путь к библиотеке классов .NET.
        /// </summary>
        protected string _pluginPath;

        /// <summary>
        /// конструктор по пути к библиотеке классов .NET.
        /// </summary>
        /// <param name="pluginPath">путь к библиотеке классов .NET.</param>
        protected CMDPluginLoadContext(string pluginPath) //
        {
            if (string.IsNullOrEmpty(pluginPath))
                IsAssemblyDependencyResolver = false;
            else //
            {
                _pluginPath = pluginPath;
                try
                {
                    _resolver = new AssemblyDependencyResolver(pluginPath);
                    IsAssemblyDependencyResolver = true;
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Error {ex.Message}\t -pluginPath: {pluginPath}");
                    IsAssemblyDependencyResolver = false;
                }
            }
        }

        /// <summary>
        /// позволяет сборке разрешаться и загружаться в зависимости от AssemblyName
        /// </summary>
        /// <param name="assemblyName">имя библиотеки классов .NET.</param>
        /// <returns>библиотека классов .NET</returns>
        protected override Assembly Load(AssemblyName assemblyName)
        {
            if ( IsAssemblyDependencyResolver && assemblyName != null)
            {
                string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
                if (assemblyPath != null)
                {
                    return LoadFromAssemblyPath(assemblyPath);
                }
            }
            
            return null;
        }

        /// <summary>
        /// Загрузить неуправляемую библиотеку по имени
        /// </summary>
        /// <param name="unmanagedDllName">имя библиотеки </param>
        /// <returns>Handle библиотеки.</returns>
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            else 
                return IntPtr.Zero;
        }
        #endregion protected
   }
