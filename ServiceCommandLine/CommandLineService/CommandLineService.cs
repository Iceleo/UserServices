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

/// <summary>
/// Сервис разбора шаблона  командной строки: команд, опций, параметров. 
/// Разбор конкретного вызова приложения из командной строки.
/// Создаем спецификация совместимости экземпляров шаблонов, 
/// который соответствует критериям совместимости, указываемых при составлении шаблона.
/// </summary>
public static class CommandLineService //: INotifyDataErrorInfo
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
    /// Список спецификаций вытекающих из правил записи шаблона командной строки.
    /// </summary>
    public static List<SpecificationCommandLine<List<ICommandLineSample>>> ListSpecificationCommandLine = 
        new List<SpecificationCommandLine<List<ICommandLineSample>>>();

    /// <summary>
    /// Создание команды вызова приложения из командной строки
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static CommandLineSample WithCommand(string name)
    {
    CommandLineSample cmdln = new CommandLineSample(name.Substring(1).TrimEnd());
        cmdlines.Add(cmdln);
        return cmdln;
    }

    /// <summary>
    ///  Разбор вызова. Разбор конкретного вызова приложения из командной строки.
    ///  Выполнено с учетом нескольких команд в командной строке.
    /// </summary>
    /// <param name="args"></param>
    /// <param name="result">класс резуализущий работу команды, с которого начинаем</param>
    /// <returns>Результат разбора</returns>
    public static bool ParseCommandLine(string[] args) //, out ICommandLineRun result)
    {
        bool rc = false;
        //result = null;        
        for (int ind = 0; ind < args.Length; ind++)// разбор всех аргументов
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            List<string> Parameters = new List<string>();
            CommandLineSample @this = null;
            if (args[ind].StartsWith(slesh2)) 
            {// команда
                args[ind] = args[ind].Substring(1); // имя команды
                @this = cmdlines.FirstOrDefault((s => s.CommandName == args[ind]));

                if (@this == null)  // не наша команда
                { //  TODO фиксируем ошибку в командной строке
                    continue;
                }
                //ICommandLineRun resultLast = null;
                ind++; // след. аргумент
                //int ParameterIndex = 0;
                bool IsParameter = false; //

                while( ind < args.Length) // разбор опций и параметров для команды
                {
                    if (args[ind].StartsWith(const_PREF)) // префикс
                    { // опция
                        string parameterWithoutHyphen = args[ind].Substring(1); //без разделителя
                                                                                // разделитель значение параметра командной строки 
                                                                                // возможен неправильный вызов
                        string[] nameValue = args[ind].Substring(1).Split(const_SPLIT); // разделитель значение параметра командной строки 
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
                        properties.Add(nameValue[0], ValueOpt);
                        ind++; // след. аргумент
                    }
                    else if (args[ind].StartsWith(slesh2))
                    { // Еще команда.  
                        break; // прервем. Обработаем в цикле for
                    }
                    else
                    { // параметрический вызов. Вообще Позиционный вызов. В тесте только для Help
                        //if (!IsParameter) { // пока допускаем 1 параметр
                        IsParameter = true;
                        Parameters.Add(args[ind]);
                        ind++; // след. аргумент
                        break; // следующая команда
                    }
                }
                // TODO проверим на совместимость
                // продолжаем разбор в команде(CommandLineSample). Создаем исполняющий класс
                rc = @this.ParseCommandLine(properties, Parameters);//Создали исполняющий класс
                properties.Clear();
                Parameters.Clear();
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
    /// Удовлетворяются ли условия начала работы класса
    /// Проверяем по спецификациям совместимости экземпляров команд, 
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

    /// <summary>
    /// Устанавливаем зн-ние свойству созданого объекта ICommandLineRun
    /// </summary>
    /// <param name="pp"> Св-во которому устанавливаем зн-ние</param>
    /// <param name="ObjectImp">объект, который заполняем</param>
    /// <param name="st2">устанавливаемое зн-ние</param>
    static public void SetValue<ICommandLineRun>(PropertyInfo pp, ICommandLineRun ObjectImp,
        string st2, out string error)
    {
        error = string.Empty;
        try
        {
            Type typrFld = pp.PropertyType;
            Type[] typrFld2 = pp.GetOptionalCustomModifiers();
            if (typrFld == Type.GetType("string")) // if (typrFld.ToString().ToLower() == "string")
            {
                pp.SetValue(ObjectImp, st2);
            }
            else if (typrFld == Type.GetType("int"))
            {
                if (int.TryParse(st2, out int iRes))
                    pp.SetValue(ObjectImp, iRes);
                else //
                    error = "Неправильный формат данных.";
            }
            else if (typrFld == Type.GetType("double"))
            {
                if (double.TryParse(st2, out double dRes))
                    pp.SetValue(ObjectImp, dRes);
                else //
                    error = "Неправильный формат данных.";
            }
            else if (typrFld == Type.GetType("bool"))
            {
                if (bool.TryParse(st2, out bool bRes))
                    pp.SetValue(ObjectImp, bRes);
                else //
                    error = "Неправильный формат данных.";
            }
            else // 
                error = "Неподдерживаемый формат данных.";
        }
        catch (System.ArgumentException exp)
        // Не найден метод доступа set свойства. -или- value невозможно преобразовать в
        // тип System.Reflection.PropertyInfo.PropertyType.
        {
            error = exp.Message;
        }
        catch (System.Reflection.TargetException exp)
        //  Тип obj не соответствует целевому типу, или свойство является свойством экземпляра, но obj равен null.
        {
            error = exp.Message;
        }
        catch (System.MethodAccessException exp)
        //     Возникла недопустимая попытка доступа к частному или защищенному методу внутри  класса.
        {
            error = exp.Message;
        }
        catch (System.Reflection.TargetInvocationException exp)
        //     Ошибка при установке значения свойства.
        {
            error = exp.Message;
        }
    }


    #region INotifyDataErrorInfo
    /// <summary>
    /// ошибки проверки для всего CommandLineRun.
    /// указанное свойствщ, список ошибок
    /// </summary>
    static readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
    /// <summary>
    /// ошибки проверки для всего CommandLineRun.
    /// указанное свойство, список ошибок
    /// </summary>
    public static Dictionary<string, string> Errors => _errors;
    /// <summary>
    /// имеет ли сущность ошибки проверки.
    /// </summary>
    public static bool HasErrors => _errors.Count > 0;

    /// <summary>
    /// ошибки проверки для указанного свойства или для всей сущности
    /// </summary>
    /// <param name="propName"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetErrors() => _errors;
    /// <summary>
    /// ошибки проверки для указанного свойства или для всей сущности
    /// </summary>
    /// <param name="propName"></param>
    /// <returns></returns>
    public static IList<string> GetErrors(string propName)
    {
        IList<string> Er;
        if (string.IsNullOrEmpty(propName))
        {
            Er = _errors.Values.ToList();
        }
        else //
        {
            Er = _errors.ContainsKey(propName) ? new List<string> { _errors[propName] } : null;
        }
        return Er;
    }

    /// <summary>
    /// Добавить ошибку для свойства
    /// </summary>
    /// <param name="propName">Имя свойства.</param>
    /// <param name="error">Выявленная ошибка.</param>
    public static void AddError(string propName, string error)
    {
        _errors.Add(propName,  error);
    }

    #endregion INotifyDataErrorInfo
}