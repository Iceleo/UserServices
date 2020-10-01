using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
//using System.Threading.Tasks;
using System.Collections;
using System.Reflection;

/// <summary>
/// Сервис разбора шаблона  командной строки: команд, опций, параметров. 
/// Разбор конкретного вызова приложения из командной строки.
/// Создаем спецификация совместимости экземпляров шаблонов, 
/// который соответствует критериям совместимости, указываемых при составлении шаблона.
/// </summary>
public static partial class CommandLineService //: INotifyDataErrorInfo
{
    /// <summary>
    ///  префекс параметра командной строки
    /// </summary>
    public const char const_PREF = '-';
    public const char sleshBack = '\\';
    /// <summary>
    ///  префекс команды командной строки
    /// </summary>
    public const char slesh2 = '/';
    /// <summary>
    /// разделитель значение параметра командной строки 
    /// </summary>
    public const char const_SPLIT = '='; //

    /// <summary>
    /// Шаблоны вызова команд приложения
    /// </summary>
    public static List<CommandLineSample> cmdlines = new List<CommandLineSample>();

    /// <summary>
    /// Последний номер исполнения подкоманды
    /// </summary>
    public static int LastNumberExecution { get; set; } = 0;
    /// <summary>
    /// Список спецификаций вытекающих из правил записи шаблона командной строки.
    /// </summary>
    public static List<SpecificationCommandLine<List<ICommandLineSample>>> ListSpecificationCommandLine =
        new List<SpecificationCommandLine<List<ICommandLineSample>>>();

    /// <summary>
    ///  справочник подкоманд со свойствами.<подкоманда, Dictionary<имя св-ва, значение>>
    /// </summary>
    public readonly static Dictionary<string, Dictionary<string, string>> subComs = //
              new Dictionary<string, Dictionary<string, string>>();


    /// <summary>
    /// Создание подкоманды вызова приложения из командной строки
    /// </summary>
    /// <param name="name">'/' + имя_подкоманды.</param>
    /// <returns></returns>
    public static CommandLineSample CmdRunCommand(string name)
    {
        CommandLineSample cmdS =  GetOrAddCommand(name.Substring(1).Trim());
        return cmdS;
     }

    /// <summary>
    /// Дать экземпляр CommandLineSample по имени подкоманды
    /// или создать подкоманду если  f_add = true
    /// </summary>
    /// <param name="realName"> имя подкоманды</param>
    /// <param name="f_add"> добавлять подкоманду в случае отсутствия.</param>
    /// <returns></returns>
    public static CommandLineSample GetOrAddCommand( string realName, bool f_add = true)
    {        
        CommandLineSample cmdln = cmdlines.FirstOrDefault(
                cmd => cmd.CommandName == realName);
        if (cmdln == null && f_add) // нет такой + нужно создавать
        { // создаем команду
            cmdln = new CommandLineSample(realName);
            cmdlines.Add(cmdln);
        }
        return cmdln;
    }

    /// <summary>
    ///  Разбор вызова. Разбор конкретного вызова приложения из файла конфигурации.
    ///  Выполнено с учетом нескольких команд в файла конфигурации.
    /// </summary>
    /// <returns>Результат разбора. Хотя бы один алгоритм нормально отработал.</returns>
    public static bool ParseAllProp(string[] args) //, out ICommandLineRun result)
    {
        bool rc = false;
        List<string> Parameters; // Список параметров подкомманды (например режимы работы класса).
        //Разбор конкретного вызова приложения из файла конфигурации. 
        if (ParseConfig()) //
            rc = true;
        //Разбор конкретного вызова приложения из командной строки.
        if (ParseCommandLine(args, out Parameters))//
            rc = true;
        
        // продолжаем разбор в команде(CommandLineSample). 
        // Создаем исполняющий класс. 
        // Присваиваем свойствам значения.
        foreach (KeyValuePair<string, Dictionary<string, string>> propSubCom in subComs)
        {
            string subCom = propSubCom.Key;
            CommandLineSample cmdSample = CommandLineService.GetOrAddCommand(subCom, false);
            if (cmdSample == null)  // Нет обработки
                continue;

            Dictionary<string, string> properties = propSubCom.Value;
            rc = cmdSample.ParseCommandLine(properties,  Parameters);//Создали исполняющий класс
        }

        return rc;
    }
    /// <summary>
    ///  Разбор вызова. Разбор конкретного вызова приложения из командной строки.
    ///  Выполнено с учетом нескольких подкоманд в командной строке.
    /// </summary>
    /// <param name="args">подкоманды в командной строке.</param>
    /// <param name="Parameters"> Список параметров подкомманды</param>
    /// <returns>Результат разбора</returns>
    public static bool ParseCommandLine(string[] args, out List<string> Parameters) //, out ICommandLineRun result)
    {
        bool rc = false;
        Parameters = new List<string>();

        Dictionary<string, string> properties;
        for (int ind = 0; ind < args.Length; ind++)// разбор всех аргументов
        {
            if (args[ind].StartsWith(slesh2)) 
            {// подкоманда
                string subCom = args[ind].Substring(1).Trim(); // имя команды

                CommandLineSample cmdSample = CommandLineService.GetOrAddCommand(subCom, true);
                if (cmdSample == null)  // Нет обработки
                    continue;

                if (!subComs.TryGetValue(subCom, out properties)) //нет в справочнике. Новая подкоманда
                { // создаем
                    properties = new Dictionary<string, string>();
                    subComs.Add(subCom, properties);
                }
                
                ind++; // след. аргумент в ком. строке
                while( ind < args.Length) // разбор опций и параметров для команды
                { //
                    if (args[ind].StartsWith(const_PREF)) // префикс '-'
                    { // опция
                        string KeyOpt = args[ind].Substring(1); //без разделителя
                        if (KeyOpt.StartsWith(const_PREF)) // префикс ='--'
                            KeyOpt = KeyOpt.Substring(1); //без префикса '--'
                        string[] nameValue = KeyOpt.Split(const_SPLIT); // разделитель значение параметра командной строки 
                        if (nameValue.Length == 0)
                        { // неправильный вызов
                            break; //??? return rc;
                        }
                        string ValueOpt;
                        switch (nameValue.Length)
                        {
                            case 1: // возможно неправильный вызов
                                ValueOpt = null;
                                break;
                            case 2:  // сейчас правильный вызов
                                ValueOpt = nameValue[1];
                                break;
                            default:  // сейчас неправильный вызов. но берем только 2
                                ValueOpt = nameValue[1];
                                break;
                        }

                        if (!properties.TryGetValue(nameValue[0], out string ss))
                        { // Нет такого свойства в справочнике.
                            properties.Add(nameValue[0], ValueOpt);
                        }
                        else
                        { // Значение было в файле конфигурации. Обновим значение.
                            properties[nameValue[0]] = ValueOpt;
                        }
                    }
                    else if (args[ind].StartsWith(slesh2))
                    { // Еще команда. 
                        ind--;
                        break; // прервем. Обработаем в цикле for
                    }
                    else
                    { // параметрический вызов. Вообще Позиционный вызов. В тесте только для Help
                        if ( Parameters.Find( (pp => pp == args[ind])) != null)
                            Parameters.Add(args[ind]);
                    }
                    ind++; // след. аргумент
                }
                // TODO проверим на совместимость
            }
        }

        return rc;
    }

    /// <summary>
    /// Вызов логики приложения заявленных для выполнения команд 
    /// удовлетворяющих условия начала работы по спецификациям совместимости 
    /// экземпляров команд, соответствие критериям совместимости, указываемых при составлении шаблона
    /// </summary>
    public static void Run() { // ???
        var cmdRun = CommandLineService.cmdlines.Where((cm1) => 
                (cm1.IscommandClass && cm1.CommandLineOK) ).ToList();
        foreach (ICommandLineSample cmdrun in cmdRun) //
             cmdrun?.Run();
    }
    /// <summary>
    /// правила вызова приложения 
    /// </summary>
    public static void RulesOfchallenge() 
    {
        if (cmdlines.Count == 0)
            Rulecall();
        else //
            foreach (CommandLineSample cmdln in cmdlines) //
                cmdln?.RulesOfchallenge();
    }

    /// <summary>
    /// правила вызова приложения 
    /// </summary>
    public static void Help()
    {
        if (cmdlines.Count == 0)
            Rulecall();
        else //
            foreach (CommandLineSample cmdln in cmdlines) //
            cmdln?.Help();
    }
    private static void Rulecall()
    {
        string AppName = Environment.GetCommandLineArgs()[0];
        AppName = Path.GetFileName(AppName);
        // в части парсинга
        Console.WriteLine("Error calling program {0}.", AppName);
        Console.WriteLine("The syntax for help: {0} /help CommandName", AppName);
    }

    /// <summary>
    /// Проверка удовлетворяются ли условия начала работы классов подкоманд.
    /// Проверяем по спецификациям совместимости экземпляров подкоманд, 
    /// соответствие критериям совместимости, указываемых при составлении шаблона.
    /// </summary>
    /// <returns>True - условия начала работы класса удовлетворены</returns>
    public static bool IsSatisfiedByService()
    {
        bool rc = true;
        // формируем список заявленных для выполнения команд
        List<ICommandLineSample> cmds = CommandLineService.cmdlines.Where(
            cm1 => (cm1.IscommandClass & cm1.CommandLineOK)).
            Select(cm2=>(ICommandLineSample)cm2).ToList(); //&& cm1.CommandLineOk
                                             
        //CommandLineService.ListSpecificationCommandLine.IsSatisfiedBy(cmd);
        foreach (SpecificationCommandLine<List<ICommandLineSample>> sp in ListSpecificationCommandLine)
            if (!sp.IsSatisfiedBy(cmds)) // не прошла проверка
            {
                _errors.Add( sp.ToString() , sp.GetError());
                rc = false;
            }

        // выполним проверки опций и параметров
        foreach (CommandLineSample cmd in cmds)
        {
            if (!cmd.IsSatisfiedBy()) // не прошла проверка
                rc = false;
        }

        //cmds.Select(c => c.IsSatisfiedBy() ); отложенное выполнение
        return rc;
    }
}