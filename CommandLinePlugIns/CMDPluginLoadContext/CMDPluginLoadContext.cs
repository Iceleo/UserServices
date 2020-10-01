using System;
using System.Collections; //����������� ��� Enumerable<AssemblyDependencyResolver>
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

/// �������� https://github.com/dotnet/samples/blob/master/core/extensions/AppWithPlugin/AppWithPlugin/PluginLoadContext.cs
/// ������� �����������.
/// <summary>
/// ��������� ���������
/// </summary>
public class CMDPluginLoadContext : AssemblyLoadContext , IEnumerable<AssemblyDependencyResolver>, IDisposable // IComparable<PluginLoadContext>
    {
    // ���� ������������ ������ ��������� CMDPluginLoadContext ��� ������� ������������� ������, 
    // ������������ ������ ������ ����� ������ ��� ���� ������������� ����������� ��� �������.

    #region static
    /// <summary>
    /// ��� ��������� ����������. 
    /// </summary>
    static protected List<CMDPluginLoadContext> ListPluginLoadContext = new List<CMDPluginLoadContext>();

        /// <summary>
        /// �������� ������ ��� ������� ������ ���� ��� �����������
        /// </summary>
        /// <param name="pluginPath">���������� � ������ � �����</param>
        /// <returns></returns>
        static public CMDPluginLoadContext CreateInstance(string pluginPath) //
        {
            CMDPluginLoadContext rc = ListPluginLoadContext.FirstOrDefault(
                (pn => pn._pluginPath == pluginPath));
            if (rc == null) //
            { // ��� � ����� �����
                rc = new CMDPluginLoadContext(pluginPath);
                if (rc.IsAssemblyDependencyResolver) // �������  ���������
                    ListPluginLoadContext.Add(rc);
                else // ������� �� ���������
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
        /// ��������  �� ���������
        /// </summary>
        /// <param name="other">�������� ��� ���������</param>
        /// <returns>��������� ���������</returns>
        public override bool Equals(Object other)
        {
            return (other is CMDPluginLoadContext) && this.IsAssemblyDependencyResolver && //
                ((CMDPluginLoadContext)other).IsAssemblyDependencyResolver //
                && Equals((CMDPluginLoadContext)other);
        }

        /// <summary>
        /// ��������  �� ���������
        /// </summary>
        /// <param name="other">�������� ��� ���������</param>
        /// <returns>��������� ���������</returns>
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
        /// false ���� PluginLoadContext ������������ ����������� �������� (_resolver �� ������)
        /// </summary>
        public readonly bool IsAssemblyDependencyResolver;

        #region protected
        /// <summary>
        /// ��������� ����������� ����������� ������ � ������ ������, 
        /// ����� ������ ������ �� ������������� ���� � ������.
        /// </summary>
        protected AssemblyDependencyResolver _resolver;

        /// <summary>
        /// ���� � ���������� ������� .NET.
        /// </summary>
        protected string _pluginPath;

        /// <summary>
        /// ����������� �� ���� � ���������� ������� .NET.
        /// </summary>
        /// <param name="pluginPath">���� � ���������� ������� .NET.</param>
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
        /// ��������� ������ ����������� � ����������� � ����������� �� AssemblyName
        /// </summary>
        /// <param name="assemblyName">��� ���������� ������� .NET.</param>
        /// <returns>���������� ������� .NET</returns>
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
        /// ��������� ������������� ���������� �� �����
        /// </summary>
        /// <param name="unmanagedDllName">��� ���������� </param>
        /// <returns>Handle ����������.</returns>
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
