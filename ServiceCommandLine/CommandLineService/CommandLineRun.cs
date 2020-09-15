using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Patterns.SpecificationClassic;
// using UserServices.ICommandLineService;
// using UserServices.SpecificationCommandLine;
// using UserServices.UserAttributedLib;


public abstract partial class CommandLineRun : ICommandLineRun
{

    /// <summary>
    /// имя приложения 
    /// </summary>
    public virtual string AppName { get; set; }

    [DataMember]
    [Required(ErrorMessage = "Please enter a ame CommandLine")]
    [DisplayName("Name CommandLine")]
    [StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
    /// <summary>
    /// Имя комманды
    /// </summary>
    public virtual string CommandName { get; set; }

    /// <summary>
    /// Параметры комманды
    /// </summary>
    public Dictionary<string, string> Parameters { get; protected set; } = new Dictionary<string, string>();

    /// <summary>
    /// свойства комманды не найденные в свойствах  //Параметры комманды
    /// </summary>
    public virtual Dictionary<string, string> PropertiesNotFound { get; set; }

    public  CommandLineRun ()
	{
		PropertiesNotFound = new Dictionary<string, string>();
 	        AppName =  Environment.GetCommandLineArgs()[0]+ ".exe";
            	AppName = Path.GetFileName(AppName);
	}
    /// <summary>
    /// выполнить 
    /// </summary>
    public abstract void Run();

    /// <summary>
    /// справка
    /// </summary>
    public virtual void Help()
    {
        Console.WriteLine("The syntax for help: {0} /{1} CommandName", AppName, CommandName);
    }

    /// <summary>
    /// правила вызова
    /// </summary>
    public abstract void RulesOfchallenge();

    /// <summary>
    /// Удовлетворяются ли условия начала работы класса
    /// </summary>
    /// <returns>True - условия начала работы класса удовлетворены</returns>
    public virtual bool IsSatisfiedBy(Dictionary<string, string> _propertiesNotFound)
    {
        bool rc = true;
        if (_mainSpecification == null) // не создавали
            BuildMainSpecification();
        // проверка условий начала работы класса
        if (_mainSpecification != null) //
            rc = _mainSpecification.IsSatisfiedBy(this);
        // доп. проверка условий для работы класса
        rc = (MakeAdditionalCheck() && rc) ? true : false;
        return rc;
    }

    /// <summary>
    /// Cделать доп. проверку
    /// </summary>
    /// <returns>True - условия начала работы класса удовлетворены</returns>/// 
    public virtual bool MakeAdditionalCheck() => true;
    

    #region MainSpecification

    /// <summary>
    /// Главная спецификация проверки свойств класса
    /// </summary>
    protected ISpecification<ICommandLineRun> _mainSpecification;
    public ISpecification<ICommandLineRun> GetMainSpecification =>_mainSpecification;
    /// <summary>
    /// Описываем главную спецификацию проверки свойств класса
    /// </summary>
    protected virtual void BuildMainSpecification() { }

    #endregion MainSpecification

}


