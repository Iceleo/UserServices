using System;
using Patterns.SpecificationClassic;
//using UserServices.ICommandLineService;
//using UserServices.CommandLineService;
//using UserServices.SpecificationCommandLine;
//using UserServices.UserAttributedLib;

namespace UserServices.CommandLineWPF
{
    class Program
    {
        /// <summary>
        ///  шаблон параметров командной строки
        /// </summary>
        public static readonly CommandLineSample comndline =
            CommandLineService.CmdRunCommand("/parse")
                               .MandatoryProperty("-targethost")
                               .MandatoryProperty("-pathbase")
                               .MandatoryProperty("-pathconfig")
                               .OptionalProperty("-depthtransitions")
                   .ParameterCmdLine("LoadData")
                   .ParameterCmdLine("SaveData")
                               .OptionalProperty("-FileSerialize")
                 //имя исполняющего команду класса. Пишем вместе с именем пространства. Важно если много сборок.
                 //Иначе имя пространства шаблона и исполняющего команду класса совпадают.
                 .PerformingClass("UserServices.CommandLineTest.parseCommandLine")
            ^ // XOR. Может выполняется только одна: или /help или /parse
            CommandLineService.CmdRunCommand("/help")
                          .ParameterCmdLine("CommandNameHelp")
            //имя исполняющего команду класса вместе с именем пространства
            ^ // XOR. Может выполняется  или /parse или /test
            CommandLineService.CmdRunCommand("/test")
                               .MandatoryProperty("-testbase")
                               .MandatoryProperty("-testconfig");
//                 .PerformingClass("UserServices.CommandLineTest.testCommandLine");
        // :  /parse -pathbase=C:\work\test1\ -pathconfig=C:\work\my_parse -targethost=https://panel.qnits.ru/manager_mvc/offers
        // :  /test -testbase=C:\work\test1\ -testconfig=C:\work\my_parse -targethost=https://panel.qnits.ru/manager_mvc/offers

        static void Main(string[] args)
        {
            Console.WriteLine("Привет от SpecificationCommandLine.");
            if (comndline != null & CommandLineService.ParseAllProp(args))  //после обработанная
            {
                // сделать проверку спецификаций
                if (CommandLineService.IsSatisfiedByService()) // прошла проверка
                {
                    CommandLineService.Run();
                    foreach (var cc1 in CommandLineService.cmdlines)
                    {
                        foreach (var pp1 in cc1.MandatoryProperties)
                        {
                            Console.WriteLine($"MandatoryProperties: Свойство {pp1.Key }:{pp1.Value} .");
                        }
                        cc1.Parameters.ToString();
                    }

                    foreach (var cc1 in CommandLineService.cmdlines)
                    {
                        foreach (var pp1 in cc1.OptionalProperties)
                        {
                            Console.WriteLine($"OptionalProperties: Свойство {pp1.Key } :{pp1.Value} .");
                        }
                        cc1.Parameters.ToString();
                    }

                    foreach (var cc1 in CommandLineService.cmdlines)
                    {
                        cc1.Parameters.ToString();
                    }
                    //string[] st = CommandLineService.GetErrors().`  Tuple<string , string >
                    foreach (var st1 in CommandLineService.GetErrors())
                    {
                        Console.WriteLine($"Свойство {st1.Key } ошибка:{st1.Value} .");
                    }
                }
                else // вывод ошибок
                    ;
            }
            else
            { // если без параметров вывод  или неправильный вызов
                CommandLineService.RulesOfchallenge();
            }
            Console.WriteLine("Expect  Enter.");
            Console.ReadKey();
        }
    }
}