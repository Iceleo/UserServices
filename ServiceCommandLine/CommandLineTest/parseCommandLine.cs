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
    public class parseCommandLine : CommandLineRun
    {
        // В командной строке  " .. parse 
        //            public string Variant { get; set; }          // если  углублять логику

        /// <summary>
        /// 
        /// </summary>
        private static parseCommandLine _parseCommand;
        public static parseCommandLine CountCommand { get { return _parseCommand; } } // Объект для доступа

        [XmlAttribute]
        public override string CommandName { get; set; }
        [XmlAttribute]
        public override string AppName { get; set; }// имя приложения 

        /// <summary>
        /// команда parse
        /// </summary>
        public bool ParseCommand { get; set; }
        [XmlAttribute]
        [SpecificationCommandLineAttribute ("1", "pathconfig")]
        public string pathconfig { get; set; }  //Путь к конфигурационному файлу
        [XmlAttribute]
        public string pathbase { get; set; }    //Путь к корневой папке проекта
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
        protected string strproject;       // {project} - корневая директория проекта 
        [XmlAttribute]
        protected string strhtml_files;    // {html_files} - директория расположения HTML страниц
        [XmlAttribute]
        protected string strstatic_files;  // {static_files} - директория расположения статичных ресурсов
        [XmlAttribute]
        protected int depthtransitions;   // depthtransitions Глубина переходов на внутренние страницы  
        protected string _shemas;  // Схема подключения к сайту 
        /// <summary>
        ///  Глубина переходов на внутренние страницы.  depthtransitions
        /// </summary>
        public int Depthtransitions { get { return depthtransitions; } } //
        /// <summary>
        /// логин пользователя для целевого домена. Из конфигурации.
        /// </summary>
        protected string _loginhost;            // логин пользователя для целевого домена
        /// <summary>
        /// пароль пользователя для целевого домена. Из конфигурации.
        /// </summary>
        protected string _passwhost;
        //[NonSerialized]
        //        public static Logger          logger     { get { return _logger;   }}     // логер
        //        private static Logger _logger; // логер
        public parseCommandLine()
        {
            _parseCommand = this;
        }


        /// <summary>
        /// выполнить 
        /// </summary>
        public override void Run()
        {
            Console.WriteLine($"{AppName} вызов команды /parse -pathbase={pathbase}  -pathconfig={pathconfig} -targethost={targethost} .");
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
        {
            //Console.WriteLine(" Синтаксис вызова команды parse :");
            //Console.WriteLine("The syntax for calling parse command:");
            Console.WriteLine($"{AppName} /parse -pathbase=pathprogram  -pathconfig=pathlocal -targethost=host ");
            Console.WriteLine("Например, {0} с командой parse можно запустить: ", AppName);
            Console.WriteLine(AppName,
               @" /parse -pathbase=C:\work\my_exe\ -pathconfig=C:\work\my_parse\ -targethost=en.wikipedia.org");
            Console.WriteLine("Параметры  -pathconfig, -targethost должны быть реально существующими.");
            Console.WriteLine("Директория у -pathbase при отсуствии создается, если путь правильный.");
        }

        /// <summary>
        /// Удовлетворяются ли условия начала работы класса
        /// </summary>
        /// <returns>True - условия начала работы класса удовлетворены</returns>
        public virtual bool Initial()
        {
            bool rc = true;
            return rc;
        }
        /// <summary>
        /// Описываем главную спецификацию проверки свойств класса
        /// </summary>
        protected override void BuildMainSpecification()
        {
            this.ClearErrors(); // очистим список
            // формируем итоговою спецификаций для проверки
            string nameProp = nameof(pathconfig);
            SpecificationClassic<ICommandLineRun> pathconfigSp = (SpecificationClassic<ICommandLineRun>)  
              (new SpecificationFromAnnotations(nameProp)). //Проверки на основе аннотаций данных
                    And( new SpecificationDirectoryExist(nameProp));//Проверка существования директории
            nameProp = nameof(pathbase);
            SpecificationClassic<ICommandLineRun> pathbaseSp = (SpecificationClassic<ICommandLineRun>)
              (new SpecificationFromAnnotations(nameProp)).//Проверки на основе аннотаций данных
                    And(new SpecificationDirectoryExist(nameProp));//Проверка существования директории

            //SpecificationClassic<ICommandLineRun> FileSerializeSp;
            if (this.LoadData) //нужно загрузить из файла
            {
                _mainSpecification = new SpecificationFileExist(nameof(FileSerialize));
            }
            else
            {
                _mainSpecification = pathconfigSp.And(pathbaseSp);//
                if (this.SaveData) //нужно сохранить
                {
                    _mainSpecification = _mainSpecification.
                        And(new SpecificationFileExist(nameof(FileSerialize)));
                }

            }

        }
    }