using System;
using System.Configuration;

namespace UserServices.ConfigurationCMDRun
{

/// <summary>
/// Эл-ты параметров программы
/// </summary>  //ConfigurationCommandLineRun
    public class ConfigurationCommandLineRun: ConfigurationElement
    {
    /// <summary>
    /// Имя подкомманды
    /// </summary>
    [ConfigurationProperty("subcommand", IsRequired = true, IsKey = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 20*/)]
    public String Subcommand
        {  //Имя подкомманды
            get { return (string)this["subcommand"]; }
            set { this["subcommand"] = value; }
        }

    /// <summary>
    /// имя сборки с путем
    /// </summary>
    [ConfigurationProperty("assemblyNameWithPath", IsRequired = false)]
	    [StringValidator(InvalidCharacters = "[]{}/;'\"|"
            /*, MinLength = 1*/, MaxLength = 256)]
        public String AssemblyNameWithPath
    { // имя свойства
            get { return (string)this["assemblyNameWithPath"]; }
            set { this["assemblyNameWithPath"] = value; }
    }

    /// <summary>
    /// Номер исполнения подкоманды
    /// </summary>
    [ConfigurationProperty("numberExecution", IsRequired = false)]
        public int NumberExecution
        {// Номер исполнения
            get { return (int)this["numberExecution"]; }
            set { this["numberExecution"] = value; }
        }

    /// <summary>
    /// описание назначения 
    /// </summary>
    [ConfigurationProperty("description", IsRequired = false)]
        public String Description
        {// описание назначения 
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

    public ConfigurationCommandLineRun()
    	{ }
    public ConfigurationCommandLineRun( string subcommand, string assemblyNameWithPath = "")
    {
        Subcommand = subcommand;
        AssemblyNameWithPath = assemblyNameWithPath;
    }

    public ConfigurationCommandLineRun(string subcommand, 
                string assemblyNameWithPath ="", string description = "", int numberExecution = 0)
    {
        Subcommand = subcommand;
         AssemblyNameWithPath = assemblyNameWithPath;
        Description = description;
	    NumberExecution = numberExecution;
    }
  }
}