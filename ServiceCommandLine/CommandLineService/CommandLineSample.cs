using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Patterns.SpecificationClassic;
// using UserServices.ICommandLineService;
// using UserServices.SpecificationCommandLine;
// using UserServices.UserAttributedLib;

/// <summary>
/// Шаблон подкоманды вызова приложения из командной строки
/// Создаем спецификация совместимости экземпляров шаблонов, 
/// который соответствует критериям указываемых при составлении шаблона. 
/// </summary>
public class CommandLineSample : ICommandLineSample
{ // 
    /// <summary>
    /// Шаблон для формирования имени исполняющего команду класса.
    /// с имененем нelp - класс нelpCommandLine
    /// </summary>
    public const string constSufficsClass = "CommandLine";

    public CommandLineSample(string commandName)
    {
        CommandName = commandName;
    }
    /// <summary>
    /// выполнить 
    /// </summary>
    public virtual void Run() => commandClass?.Run(); 

    /// <summary>
    /// справка
    /// </summary>
    public virtual void Help() =>commandClass?.Help();

    /// <summary>
    /// Показать правила вызова подкоманды
    /// </summary>
    public virtual void RulesOfchallenge() =>commandClass?.RulesOfchallenge();

    /// <summary>
    /// Выполнены ли условия начала работы класса.
    /// True - Выполнены.
    /// </summary>
    public bool CommandLineOK { get; protected set; } = true;

    /// <summary>
    /// Установить - условия начала работы класса не выполнимы.
    /// </summary>
    public void SetCommandLineBad() => CommandLineOK = false;

    /// <summary>
    /// Имя подкомманды
    /// </summary>
    public string CommandName { get; set; } // => _commandName;

    /// <summary>
    /// Имя исполняющего команду класса вместе с именем пространства
    /// </summary>
    protected string NameClassRun { get; set; }

    /// <summary>
    /// Имя namespace для commandClass( класса реализущий работу команды). Если нужно
    /// </summary>
    //public string namespaceClass;

    /// <summary>
    /// класс реализущий работу подкоманды.
    /// </summary>
    protected ICommandLineRun commandClass { get; set; }

    /// <summary>
    /// True - Класс реализущий работу подкоманды создан.
    /// </summary>
    public bool IscommandClass => commandClass != null;

    /// <summary>
    /// Параметры подкомманды
    /// </summary>
    public Dictionary<string, string> Parameters { get; protected set; } = new Dictionary<string, string>();

    /// <summary>
    /// Обязательные свойства подкомманды
    /// </summary>
    public Dictionary<string, string> MandatoryProperties { get; protected set; } = new Dictionary<string, string>();

    /// <summary>
    /// Не обязательные свойства подкомманды
    /// </summary>
    public Dictionary<string, string> OptionalProperties { get; protected set; } = new Dictionary<string, string>();
    /// <summary>
    /// Заявить обязательное свойство подкомманды
    /// </summary>
    /// <param name="name">имя свойства</param>
    /// <returns>сам класс</returns>
    public CommandLineSample MandatoryProperty(string name)
    {
        MandatoryProperties.Add(name.Substring(1).TrimEnd() , null);
        return this;
    }

    /// <summary>
    /// Заявить не обязательное свойство подкомманды
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public CommandLineSample OptionalProperty(string name)
    {
        OptionalProperties.Add(name.Substring(1).TrimEnd(), null);
        return this;
    }
    /// <summary>
    /// Заявить параметр подкомманды
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public CommandLineSample ParameterCmdLine(string name)
    {
        Parameters.Add(name.Trim(), null);
        return this;
    }
    /// <summary>
    ///  Имя исполняющего подкоманду класса вместе с именем пространства
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public CommandLineSample PerformingClass(string name)
    //public CommandLineSample Withnamespace(string name)
    {
        this.NameClassRun = name; 
        return this;
    }

    /// <summary>
    /// свойства комманды не найденные в свойствах  //Параметры комманды
    /// </summary>
    public virtual Dictionary<string, string> PropertiesParamNotFound { get; set; }

    /// <summary>
    /// Разбор вызова
    /// </summary>
    /// <param name="properties">Свойства комманды</param>
    /// <param name="ParametersCmd">Параметры комманды</param>
    /// <param name="result">класс реализущий работу команды</param>
    /// <returns>Результат разбора</returns>
    public bool ParseCommandLine( Dictionary<string, string> properties,
            List<string> ParametersCmd)
    {
        bool rc = false;
        ICommandLineRun result;
        PropertiesParamNotFound = new Dictionary<string, string>();
        foreach (var property in properties)
        {
            string nameValue = property.Key;
            string ValueOpt = property.Value;
            if (this.MandatoryProperties.TryGetValue(nameValue, out string val1))
                this.MandatoryProperties[nameValue] = ValueOpt;
            else if (this.OptionalProperties.TryGetValue(nameValue, out string val2))
                this.OptionalProperties[nameValue] = ValueOpt;
            else // незнамо что
                PropertiesParamNotFound.Add( nameValue, ValueOpt);
        }

        foreach (string nameParam in ParametersCmd)
        {
            if (this.Parameters.TryGetValue(nameParam, out string val3))
                this.Parameters[nameParam] = string.Empty;
            else // незнамо что
                  PropertiesParamNotFound.Add(nameParam, string.Empty);
        }
        try
        {
            string classNameCmd = string.Empty;
            if ( string.IsNullOrEmpty(this.NameClassRun))
            {
                //имя исполняющего команду класса вместе с именем пространства
                // Для команды с имененем нelp - класс нelpCommandLine:
                // Для команды с имененем parse - класс parseCommandLine:
                // именем пространства совпадают
                classNameCmd += this.GetType().Namespace + '.' 
                    +this.CommandName + constSufficsClass;                
            }
            else //
                classNameCmd = this.NameClassRun;
            
            Type typeclass = Type.GetType(classNameCmd); // 
            // И создадим его экземпляр:  
            result = (ICommandLineRun)Activator.CreateInstance(typeclass);            
            this.commandClass = result;
            result.CommandName = this.CommandName; // имена совпадают
            // свойства - свойства для созданного экземляра:
            foreach (var property in properties)
                if (!string.IsNullOrEmpty(property.Key))
                { //
                    PropertyInfo pp = typeclass.GetProperty(property.Key);
                    if (pp != null) //
                    {
                        CommandLineService.SetValue(pp, result, property.Value, out string error); // заполним значение в свойстве
                        pp.SetValue(result, property.Value); // значения параметров пишем в свойства
                    }    
                }

            // свойства - параметры для созданного экземляра:
            foreach (string nameParam in ParametersCmd) //
                if ( !string.IsNullOrEmpty(nameParam)) //
                    typeclass.GetProperty(nameParam)?.SetValue(result, true);

            rc = true;
        }
        catch (System.ArgumentException exc1)
        { //     Параметр type не является объектом типа RuntimeType. -или- Значение параметра
          //     type является открытым универсальным типом (иными словами, свойство 
        }
        catch (System.NotSupportedException exc2)
        {//     Тип, заданный параметром type, не может быть System.Reflection.Emit.TypeBuilder.
         //     -или- Создание типов System.TypedReference, System.ArgIterator, System.Void
         //     и System.RuntimeArgumentHandle или массивов этих типов не поддерживается.
         //     -или- Сборка, содержащая тип type, является динамической сборкой, созданной
         //     с помощью System.Reflection.Emit.AssemblyBuilderAccess.Save.
        }
        catch (System.Reflection.TargetInvocationException exc3)
        { //     Вызываемый конструктор создает исключение.
        }
        catch (System.MethodAccessException exc4)
        {//     Вызывающий код не имеет разрешения на вызов этого конструктора.
        }
        catch (System.MemberAccessException exc5)
        { //     Не удается создать экземпляр абстрактного класса, или этот член был вызван
          //     при помощи механизма поздней привязки.
        }
        catch (System.Runtime.InteropServices.InvalidComObjectException exc6)
        { //     COM-тип не был получен посредством Overload:System.Type.GetTypeFromCLSID
          //     или Overload:System.Type.GetTypeFromProgID.
        }
        catch (System.Runtime.InteropServices.COMException exc8)
        { //     Параметр type представляет COM-объект, но идентификатор класса, используемый
          //     для получения типа, является недопустимым, или идентифицируемый класс не
          //     зарегистрирован.
        }
        return rc;
    }

    /// <summary>
    /// Удовлетворяются ли условия начала работы подкоманды
    /// </summary>
    /// <returns>True - условия начала работы класса удовлетворены</returns>
    public bool IsSatisfiedBy()
    {
        bool rc = true;
        // d
        foreach (var property in MandatoryProperties.Where( val => val.Value == null))
        {// обязательные не указанные при вызове
            ;//if 
        }
        rc = (bool) commandClass?.IsSatisfiedBy( PropertiesParamNotFound);
        return rc;
    }

  #region static
    /// <summary>
    /// Создаем спецификация XOR совместимости экземпляров шаблонов, 
    /// который соответствует критериям указываемых при составлении шаблона. 
    /// </summary>
    /// <param name="leftCommand"></param>
    /// <param name="rightCommand"></param>
    /// <returns></returns>
    public static CommandLineSample operator |(CommandLineSample leftCommand, CommandLineSample rightCommand)
    {
        // создаем спецификацию для проверки совместимости вызова XOR
        SpecificationCommandLineOr ss = new SpecificationCommandLineOr(leftCommand, rightCommand);
        CommandLineService.ListSpecificationCommandLine.Add(ss);        
        return leftCommand; // rigthCommand;
    }

    /// <summary>
    /// Создаем спецификацию OR совместимости экземпляров шаблонов, 
    /// который соответствует критериям указываемых при составлении шаблона. 
    /// </summary>
    /// <param name="leftCommand"></param>
    /// <param name="rigthCommand"></param>
    /// <returns></returns>
    public static CommandLineSample operator ^ (CommandLineSample leftCommand, CommandLineSample rightCommand)
    {
        // создаем спецификацию для проверки совместимости вызова XOR
        SpecificationCommandLineXor ss = new SpecificationCommandLineXor(leftCommand, rightCommand);
        CommandLineService.ListSpecificationCommandLine.Add(ss);
        return leftCommand; // rigthCommand;
    }

    /// <summary>
    /// Создаем спецификация And совместимости экземпляров шаблонов, 
    /// который соответствует критериям указываемых при составлении шаблона. 
    /// </summary>
    /// <param name="leftCommand"></param>
    /// <param name="rightCommand"></param>
    /// <returns></returns>
    public static CommandLineSample operator & (CommandLineSample leftCommand, CommandLineSample rightCommand)
    {
        // создаем спецификацию для проверки совместимости вызова XOR
        SpecificationCommandLineAnd ss = new SpecificationCommandLineAnd(leftCommand, rightCommand);
        CommandLineService.ListSpecificationCommandLine.Add(ss);
        return leftCommand; // rigthCommand;
    }

    #endregion static
}