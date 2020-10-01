using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServices.ConfigurationCMDRun; // UserServices.ConfigurationCMDRun;

namespace UserServices.ConsoleNewThought
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Dictionary<string, string>> subComs2;
            Console.WriteLine("Greetings to the user from UserServices.ConfigurationCommandLineRun!");
            ParseConfigurationCommandLineRun(out subComs2);
        }

        /// <summary>
        ///  справочник подкоманд со свойствами
        /// </summary>
        static Dictionary<string, Dictionary<string, string>> subComs22;

        public static Dictionary<string, string> subComAssemblys;
        /// <summary>
        /// Шаблоны вызова команд приложения
        /// </summary>
        public static List<CommandLineSample> cmdlines = new List<CommandLineSample>();

        static bool ParseConfigurationCommandLineRun(out Dictionary<string, Dictionary<string, string>> subComs)
        {
            subComs = null;
            bool rc = false;
            // Get the current configuration file.
            System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None);

            ConfigurationSection appS = config.GetSection("appSettings");
            Type t0 = typeof(ConfigurationSection);
            AppSettingsSection appSettings = config.AppSettings;
            
            if (appSettings != null) //
            {
                //string ss = appSettings?.Settings?.AllKeys[0];
                //string ss1 = appSettings?.Settings?.AllKeys[1];
                //string ss2 = appSettings?.Settings.GetEnumerator.
            }
            //UserServices.ConfigurationCMDRun.
            Type t2 = typeof(ValidatedCMRunSection);
            ValidatedCMRunSection CMRunSection = config.GetSection("cmdrun") 
                as ValidatedCMRunSection;
            if (CMRunSection != null) //
            {
                Console.WriteLine("CMRunSection not null.");
            }
            // var AddConfSections1 = (CommandLineRunSection)config.GetSection("CmdRunConfs");
            Type t4 = typeof(CommandLineRunSection);
            CommandLineRunSection AddConfSections = config.GetSection("CmdRunConfs")
                as CommandLineRunSection;
            if (AddConfSections != null) // получили секцию AddConfSections из конфигурации
            {
                if (subComs == null)//
                    subComs = new Dictionary<string, Dictionary<string, string>>();
                for (int ii = 0; ii < AddConfSections.CmdRunConfs.Count; ii++)
                {
                    string subCom = AddConfSections.CmdRunConfs[ii].Subcommand;
                    if (string.IsNullOrEmpty(subCom)) // сбой значения
                        continue;
                    else
                    {
                        CommandLineSample cmdSample = CommandLineService.GetOrAddCommand(subCom, false );
                        if (cmdSample == null)  // Нет обработки
                            continue;

                        string assemblyNameWithPath = AddConfSections.CmdRunConfs[ii].AssemblyNameWithPath;
                        // создаем имя
                        cmdSample.AssemblyNameWithPath = assemblyNameWithPath;
                        int num = AddConfSections.CmdRunConfs[ii].NumberExecution;
                        Console.WriteLine($"Add: {subCom} -  {assemblyNameWithPath}.");
                    }
                }
            }
            Type tPr = typeof(CMDPropertySection);
            CMDPropertySection cmdProperties = config.GetSection("CMDProperties")
                    as CMDPropertySection;
            if (cmdProperties != null) // получили секцию AddConfSections из конфигурации
            {
                if (subComs == null)//
                    subComs = new Dictionary<string, Dictionary<string, string>>();
                for (int ii = 0; ii < cmdProperties.CMDProperties.Count; ii++)
                {
                    string subCom = cmdProperties.CMDProperties[ii].Subcommand;
                    if (string.IsNullOrEmpty(subCom)) // сбой значения
                        continue;
                    else
                    {
                        Dictionary<string, string> properties;
                        string val = cmdProperties.CMDProperties[ii].Value;
                        string key = cmdProperties.CMDProperties[ii].NameProperty;
                        if (!subComs.TryGetValue(subCom, out properties)) // нет в справочнике
                        { // создаем
                            properties = new Dictionary<string, string>();
                            subComs.Add(subCom, properties);
                        }
                        if (string.IsNullOrEmpty(key)) // сбой значения
                            continue;
                        else
                        {
                            properties.Add(key, val);
                            Console.WriteLine($"Add: {key} -  {val}.");
                            rc = true;
                        }
                    }
                }
            }

            // Обработка секции CmdRunSpecifications
            Type tPS = typeof(CMDPropertySection);
            CMDRunSpecificationSection cmdSpecifications = config.GetSection("CMDRunSpecifications")
                    as CMDRunSpecificationSection;
            if (cmdSpecifications != null) // получили секцию CmdRunSpecifications из конфигурации
            {
                if (subComs == null)// ??? нужно ли выполнять
                    subComs = new Dictionary<string, Dictionary<string, string>>();

                for (int ii = 0; ii < cmdSpecifications.CMDRunSpecifications.Count; ii++)
                {
                    string subComLeft = cmdSpecifications.CMDRunSpecifications[ii].SubcommandLeft;
                    if (string.IsNullOrEmpty(subComLeft)) // сбой значения
                        continue;
                    string subComRight = cmdSpecifications.CMDRunSpecifications[ii].SubcommandRight;
                    if (string.IsNullOrEmpty(subComRight)) // сбой значения
                        continue;
                    string specification = cmdSpecifications.CMDRunSpecifications[ii].Specification;
                    if (string.IsNullOrEmpty(specification)) // сбой значения
                        continue;
                    else
                    {
                        specification = specification.ToUpper();
                        CommandLineSample cmdSampleL = CommandLineService.GetOrAddCommand(subComLeft, false);
                        if (cmdSampleL == null)  // Нет обработки
                            continue;
                        CommandLineSample cmdSampleR = CommandLineService.GetOrAddCommand(subComRight, false);
                        if (cmdSampleR == null)  // Нет обработки
                            continue;
                        else
                        {
                            switch (specification)
                            {
                                case "XOR":
                                    cmdSampleL = cmdSampleL ^ cmdSampleR;
                                    break;
                                case "OR":
                                    cmdSampleL = cmdSampleL | cmdSampleR;
                                    break;
                                case "AND":
                                    cmdSampleL = cmdSampleL & cmdSampleR;
                                    break;
                                default:
                                    continue;
                            }

                        }
                    }
                }
            }
            Console.WriteLine("Expect  Enter.");
            Console.ReadKey();

            return rc;
        }
    }
}
