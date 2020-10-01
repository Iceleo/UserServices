using System;
using System.Configuration;

namespace UserServices.ConfigurationCMDRun
{

/// <summary>
/// Эл-ты параметров программы
/// </summary>  //CMDRunSpecificationElement
    public class CMDRunSpecificationElement: ConfigurationElement
    {
    /// <summary>
    /// Имя подкомманды левой
    /// </summary>
    [ConfigurationProperty("subcommandLeft", IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 20*/)]
    public String SubcommandLeft
        {  //Имя подкомманды
            get { return (string)this["subcommandLeft"]; }
            set { this["subcommandLeft"] = value; }
        }

    /// <summary>
    /// имя спецификации(XOR, AND, OR) совместимости вызова подкоманд.
    /// </summary>
    [ConfigurationProperty("specification", IsRequired = true)]
	    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 32*/)]
        public String Specification
    { // 
            get { return (string)this["specification"]; }
            set { this["specification"] = value; }
        }

    /// <summary>
    /// Имя подкомманды левой
    /// </summary>
    [ConfigurationProperty("subcommandRight", IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 20*/)]
    public String SubcommandRight
        {  //Имя подкомманды
            get { return (string)this["subcommandRight"]; }
            set { this["subcommandRight"] = value; }
        }


    /// <summary>
    /// описание назначения свойства
    /// </summary>
    [ConfigurationProperty("description", IsRequired = false)]
        public String Description
        {// описание назначения свойства
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

    public CMDRunSpecificationElement()
    	{ }
    public CMDRunSpecificationElement( string subcommandLeft, string  subcommandRight, 
		string specification)
    {
        SubcommandLeft = subcommandLeft;
        SubcommandRight = subcommandRight;
        Specification = specification;
    }

    public CMDRunSpecificationElement(string subcommandLeft, string  subcommandRight, 
		string specification, string description)
    {
        SubcommandLeft = subcommandLeft;
        SubcommandRight = subcommandRight;
        Specification = specification;
        Description = description;
    }
  }
}