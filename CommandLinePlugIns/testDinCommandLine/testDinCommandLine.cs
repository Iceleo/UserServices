using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using Patterns.SpecificationClassic;
//using UserServices.ICommandLineService;
//using UserServices.CommandLineService;
//using UserServices.SpecificationCommandLine;
//using UserServices.UserAttributedLib;


    /// <summary>
    /// Класс работы консольного приложения. 
    /// Обработка командной строки.
    /// Обработка данных из конфигурационного файла 
    /// </summary>
    [Serializable, XmlRoot(Namespace = "http://www.MyCompany.com")]
    public class testDinCommandLine : CommandLineRun
    {
        // В командной строке  " .. testDin 
       
        /// <summary>
        /// 
        /// </summary>
        private static testDinCommandLine _testCommand;       
        public static testDinCommandLine countCommand =>_testCommand; // Объект для доступа

        [XmlAttribute]
        public override string CommandName { get; set; }

        [XmlAttribute]
        public string testconfig { get; set; }  //Путь к конфигурационному файлу
        [XmlAttribute]
        public string testbase { get; set; }    //Путь к корневой папке проекта
        [XmlAttribute]
        public string targethost { get; set; }  //Целевой домен

        // восстанавление/сохраненние данных из/в файла.
        /// <summary>
        /// Параметр Load Serialize data
        /// </summary>
        public bool LoadData { get; set; } = false;
        /// <summary>
        /// Параметр Save Serialize data
        /// </summary
        public bool SaveData { get; set; } = false;
        public string FileSerialize { get; set; }

        // из конфигурации
        [XmlAttribute]
        protected string _shemas;  // Схема подключения к сайту 
        public testDinCommandLine()
        {
            _testCommand = this;
        }


        /// <summary>
        /// выполнить 
        /// </summary>
        public override void Run()
        {// в режиме Console
            Console.WriteLine($"{AppName} вызов команды /testDin -testbase={testbase}  -testconfig={testconfig} -targethost={targethost} .");
        }

        /// <summary>
        /// справка
        /// </summary>
        public override void Help()
        {
            // в режиме Console
            RulesOfchallenge();
        }
        /// <summary>
        /// правила вызова
        /// </summary>
        public override void RulesOfchallenge()
        {  // в режиме Console
            //Console.WriteLine(" Синтаксис вызова команды testDin :");
            //Console.WriteLine("The syntax for calling testDin command:");
            Console.WriteLine($"{AppName} /testDin -testbase=pathprogram  -testconfig=pathlocal -targethost=host ");
            Console.WriteLine("Например, {0} с командой testDin можно запустить: ", AppName);
            Console.WriteLine(AppName,
               @" /testDin -testbase=C:\work\my_exe\ -testconfig=C:\work\my_test\ -targethost=en.wikipedia.org");
            Console.WriteLine("Параметры  -testconfig, -targethost должны быть реально существующими.");
            Console.WriteLine("Директория у -testbase при отсуствии создается, если путь правильный.");
        }
  
        /// <summary>
        /// Описываем главную спецификацию проверки свойств класса
        /// </summary>
        protected override void BuildMainSpecification()
        {
            this.ClearErrors(); // очистим список
            // формируем итоговою спецификаций для проверки
            string nameProp = nameof(testconfig);
            SpecificationClassic<ICommandLineRun> pathconfigSp = (SpecificationClassic<ICommandLineRun>)
              (new SpecificationFromAnnotations(nameProp)). //Проверки на основе аннотаций данных
                    And(new SpecificationDirectoryExist(nameProp));//Проверка существования директории
            nameProp = nameof(testbase);
            SpecificationClassic<ICommandLineRun> pathbaseSp = (SpecificationClassic<ICommandLineRun>)
              (new SpecificationFromAnnotations(nameProp)).//Проверки на основе аннотаций данных
                    And(new SpecificationDirectoryExist(nameProp));//Проверка существования директории

            _mainSpecification = pathconfigSp.And(pathbaseSp);//
            if (this.LoadData && this.SaveData) //нужно загрузить/сохранить из файла
            {
                    _mainSpecification = _mainSpecification.
                        And(new SpecificationFileExist(nameof(FileSerialize)));
            }
        }
    }
