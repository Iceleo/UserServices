using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.IO;
//using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using Patterns.SpecificationClassic;
//using UserServices.ICommandLineService;
//using UserServices.SpecificationCommandLine;
//using UserServices.UserAttributedLib;
using System.Configuration;
using UserServices.ConfigurationCMDRun;


/// <summary>
/// Сервис разбора шаблона  командной строки: команд, опций, параметров. 
/// Разбор конкретного вызова приложения из командной строки.
/// Создаем спецификация совместимости экземпляров шаблонов, 
/// который соответствует критериям совместимости, указываемых при составлении шаблона.
/// </summary>
public static partial class CommandLineService //: INotifyDataErrorInfo
{

    /// <summary>
    ///  Разбор конкретного вызова приложения из файла конфигурации.
    ///  Выполнено с учетом нескольких подкоманд в файле конфигурации.
    /// </summary>
    /// <returns>Результат разбора</returns>
    public static bool ParseConfig() //, out ICommandLineRun result)
    {
        bool rc = false;
        CommandLineSample cmdSample;
        // Get the current configuration file.
        System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        // Обработка секции CmdRunConfs
        Type t4 = typeof(CommandLineRunSection);
        CommandLineRunSection AddConfSections = config.GetSection("CmdRunConfs")
                as CommandLineRunSection;
        if (AddConfSections != null) // получили секцию CmdRunConfs из конфигурации
        {
            for (int ii = 0; ii < AddConfSections.CmdRunConfs.Count; ii++)
            {
                string subCom = AddConfSections.CmdRunConfs[ii].Subcommand;
                if (string.IsNullOrEmpty(subCom)) //  сбой значения
                {
                    continue;
                }

                cmdSample = CommandLineService.GetOrAddCommand(subCom, true);
                if (cmdSample == null)  // Нет обработки
                    continue;

                cmdSample.NumberExecution = AddConfSections.CmdRunConfs[ii].NumberExecution;
                cmdSample.AssemblyNameWithPath = AddConfSections.CmdRunConfs[ii].AssemblyNameWithPath;
            }
        }

        // Обработка секции CMDProperties
        Type tPr = typeof(CMDPropertySection);
        CMDPropertySection cmdProperties = config.GetSection("CMDProperties")
                as CMDPropertySection;
        if (cmdProperties != null) // получили секцию CMDProperties из конфигурации
        {
            for (int ii = 0; ii < cmdProperties.CMDProperties.Count; ii++)
            {
                string subCom = cmdProperties.CMDProperties[ii].Subcommand;
                if (string.IsNullOrEmpty(subCom)) // сбой значения
                    continue;
                else
                {
                    cmdSample = CommandLineService.GetOrAddCommand(subCom, true);
                    if (cmdSample == null)  // Нет обработки
                        continue;

                    Dictionary<string, string> properties;
                    if (!subComs.TryGetValue(subCom, out properties)) // нет в справочнике
                    { // создаем
                        properties = new Dictionary<string, string>();
                        subComs.Add(subCom, properties);
                    }

                    string val = cmdProperties.CMDProperties[ii].Value;
                    string key = cmdProperties.CMDProperties[ii].NameProperty;
                    if (string.IsNullOrEmpty(key)) // сбой значения
                        continue;
                    else
                    {
                        properties.Add(key, val);
                        //Console.WriteLine($"Add: {key} -  {val}.");
                        rc = true;
                    }
                }
            }
        }

        // Обработка секции CmdRunSpecifications
        Type tPS = typeof(CMDPropertySection);
        CMDRunSpecificationSection cmdSpecifications = config.GetSection("CmdRunSpecifications")
                as CMDRunSpecificationSection;
        if (cmdSpecifications != null) // получили секцию CmdRunSpecifications из конфигурации
        {
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
        return rc;
    }
}