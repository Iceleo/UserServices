using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Шаблон подкоманды вызова приложения из командной строки
/// Создаем спецификация совместимости экземпляров шаблонов, 
/// который соответствует критериям указываемых при составлении шаблона. 
/// </summary>
public partial class  CommandLineSample : ICommandLineSample
{ // 
    /// <summary>
    /// Суфикс для формирования имени исполняющего подкоманду класса из имени текущего класса.
    /// Пример: c имененем нelp - класс нelpCommandLine
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
    /// Номер исполнения подкоманды
    /// </summary>
    public int NumberExecution { get; set; } = 0;

    /// <summary>
    /// Имя исполняющего команду класса вместе с именем пространства
    /// </summary>
    protected string NameClassRun { get; set; }

    /// <summary>
    /// класс реализущий работу подкоманды.
    /// </summary>
    protected ICommandLineRun commandClass { get; set; }

    /// <summary>
    /// Можно ли вызывать класс. True - Можно работать с классом реализущим работу подкоманды.
    /// </summary>
    public bool IscommandClass => commandClass != null && NumberExecution > 0;

    /// <summary>
    /// Параметры подкомманды. Обозначаются например режимы работы класса.
    /// </summary>
    public Dictionary<string, string> Parameters { get; protected set; } = new Dictionary<string, string>();

    /// <summary>
    /// Обязательные свойства подкомманды
    /// </summary>
    public Dictionary<string, string> MandatoryProperties { get; protected set; } = new Dictionary<string, string>();

    /// <summary>
    /// Заявить обязательное свойство подкомманды
    /// </summary>
    /// <param name="name">имя свойства</param>
    /// <returns>сам класс</returns>
    public CommandLineSample MandatoryProperty(string name)
        => AddMandatoryProperty(name.Substring(1).Trim(), null);

    /// <summary>
    /// Создание обязательное свойство подкомманды 
    /// </summary>
    /// <param name="realName"> имя свойства</param>
    /// <returns></returns>
    public CommandLineSample AddMandatoryProperty(string realName, string value)
    {
        if (!MandatoryProperties.TryGetValue(realName, out string val)) // нет такого
            MandatoryProperties.Add(realName, value);// создаем свойство
        else // обновим значение 
            MandatoryProperties[realName] = value;

        return this;
    }

    /// <summary>
    /// Не обязательные свойства подкомманды
    /// </summary>
    public Dictionary<string, string> OptionalProperties { get; protected set; } = new Dictionary<string, string>();

    /// <summary>
    /// Заявить не обязательное свойство подкомманды
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public CommandLineSample OptionalProperty(string name)
                => AddOptionalProperty(name.Substring(1).Trim(), null);

    /// <summary>
    /// Создание не обязательное свойство подкомманды 
    /// </summary>
    /// <param name="realName"> имя свойства</param>
    /// <returns></returns>
    public CommandLineSample AddOptionalProperty(string realName, string value)
    {
        if (!OptionalProperties.TryGetValue(realName, out string val)) // нет такого
            OptionalProperties.Add(realName, value);// создаем свойство
        else // обновим значение 
            OptionalProperties[realName] = value;

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
    /// библиотека с именем и путем
    /// </summary>
    public string AssemblyNameWithPath { get; set; }

    /// <summary>
    /// Загрузчик библиотек
    /// </summary>
    public CMDPluginLoadContext plc { get; set; }

    Assembly pluginAssembly;

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

        if (CreateInstance(out result))
        {
            Type typeclass = result.GetType();
            // свойства - свойства для созданного экземляра:
            foreach (var property in properties) //
                if (!string.IsNullOrEmpty(property.Key))
                { //
                    PropertyInfo pp = typeclass.GetProperty(property.Key);
                    if (pp != null) //
                    {
                        //CommandLineService.SetValue(pp, result, property.Value, out string error); // заполним значение в свойстве
                        pp.SetValue(result, property.Value); // значения параметров пишем в свойства
                    }
                }

            // свойства - параметры для созданного экземляра:
            foreach (string nameParam in ParametersCmd) //
                if (!string.IsNullOrEmpty(nameParam)) //
                    typeclass.GetProperty(nameParam)?.SetValue(result, true);

            rc = true;
        }

        return rc;
    }

    /// <summary>
    /// Создаем класс реализущий работу подкоманды.
    /// </summary>
    /// <param name="result">класс реализущий работу подкоманды.</param>
    /// <returns>true - класс реализущий работу подкоманды создан.</returns>
    public bool CreateInstance(out ICommandLineRun result)
    {
        bool rc = false;
        result = null;
        try
        {
            string classNameCmd = string.Empty;
            if (string.IsNullOrEmpty(this.NameClassRun))
            {//строим имя исполняющего команду класса вместе с именем пространства и суффикс
                 // суффикс = constSufficsClass = "CommandLine"
                 // Для команды с имененем нelp - класс нelpCommandLine:
                 // Для команды с имененем parse - класс parseCommandLine:
             // именя пространства NameClassRun и CommandLineSample совпадают
                classNameCmd += this.GetType().Namespace + '.'
                    + this.CommandName + constSufficsClass;
            }
            else //
                classNameCmd = this.NameClassRun;

            Type typeclass = Type.GetType(classNameCmd); 
            // Определяем, может ли экземпляр указанного типа быть назначен 
            // переменной типа ICommandLineRun
            if (typeclass != null && 
                    typeof(ICommandLineRun).IsAssignableFrom(typeclass))
            {//      - typeclass является производным от ICommandLineRun.               
                // разрешим и загрузим библиотеку  
                if (!string.IsNullOrEmpty(AssemblyNameWithPath)) // есть библиотека для загрузки
                    this.pluginAssembly = CommandLineService.LoadPlugin( this.AssemblyNameWithPath);
                // создадим экземпляр: 
                result = (ICommandLineRun)Activator.CreateInstance(typeclass);
                this.commandClass = result;
                result.CommandName = this.CommandName; // имена совпадают.НУ пока так.
                rc = true;
            }
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
    /// Проверка удовлетворяются ли условия начала работы подкоманды
    /// </summary>
    /// <returns>True - условия начала работы класса удовлетворены</returns>
    public bool IsSatisfiedBy()
    {
        bool rc = true;
        // 
        foreach (var property in MandatoryProperties.Where( val => val.Value == null))
        {// обязательные не указанные при вызове
            ;//if 
        }
        rc = (bool) commandClass?.IsSatisfiedBy( PropertiesParamNotFound);
        return rc;
    }
}