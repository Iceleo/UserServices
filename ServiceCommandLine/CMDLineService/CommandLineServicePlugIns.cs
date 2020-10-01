using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.IO;
//using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using System.Runtime.Loader;
using System.ComponentModel;
//using Patterns.SpecificationClassic;
//using UserServices.ICommandLineService;
//using UserServices.SpecificationCommandLine;
//using UserServices.UserAttributedLib;


/// <summary>
/// Сервис разбора шаблона  командной строки: команд, опций, параметров. 
/// Разбор конкретного вызова приложения из командной строки.
/// Создаем спецификация совместимости экземпляров шаблонов, 
/// который соответствует критериям совместимости, указываемых при составлении шаблона.
/// </summary>
 public static partial class CommandLineService //: INotifyDataErrorInfo
{
    /// <summary>
    ///   из файла конфигурации.
    ///  Выполнено с учетом нескольких команд в файла конфигурации.
    /// </summary>
    /// <returns>Результат разбора</returns>
    public static Assembly LoadPlugin(string relativePath)
    {
        Assembly rc = null;
	    // Navigate up to the solution root
	    string root = Path.GetFullPath(Path.Combine(
        	Path.GetDirectoryName(
	            Path.GetDirectoryName(
        	        Path.GetDirectoryName(
                	    Path.GetDirectoryName(
                        	Path.GetDirectoryName(typeof(CommandLineSample).Assembly.Location)))))));

	    string pluginLocation = Path.GetFullPath(Path.Combine(root, 
			relativePath.Replace('\\', Path.DirectorySeparatorChar)));
		if (!string.IsNullOrEmpty( pluginLocation))
		{
			// Loading commands from: 
			CMDPluginLoadContext loadContext = CMDPluginLoadContext.CreateInstance(pluginLocation);
			AssemblyName an = new AssemblyName(
					Path.GetFileNameWithoutExtension(pluginLocation));

			if (an != null && loadContext != null) //
				rc = loadContext.LoadFromAssemblyName(an);
		}
		return rc;
    }
	
	// Посмотреть AssemblyLoadContext
    public static void	LearnAssemblyLoadContext()
	{
#nullable enable
		foreach (AssemblyLoadContext asC in AssemblyLoadContext.All)
		{
			string? str = asC.Name;
			string? str2;
			foreach (Assembly sas in asC.Assemblies )
				str2 = sas.FullName ;
#nullable disable
		}
	}
}