using System;
using System.Configuration;

namespace UserServices.ConfigurationCMDRun
{

/// <summary>
/// Ёл-ты параметров программы
/// </summary>  //ConfigurationCommandLineRun
    public class CMDPropertyElement: ConfigurationElement
    {
    /// <summary>
    /// »м€ подкомманды
    /// </summary>
    [ConfigurationProperty("subcommand", IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 20*/)]
    public String Subcommand
        {  //»м€ подкомманды
            get { return (string)this["subcommand"]; }
            set { this["subcommand"] = value; }
        }

    /// <summary>
    /// им€ свойства
    /// </summary>
    [ConfigurationProperty("nameProperty", IsRequired = true)]
	    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 32*/)]
        public String NameProperty
    { // им€ свойства
            get { return (string)this["nameProperty"]; }
            set { this["nameProperty"] = value; }
        }

    /// <summary>
    /// значение свойства
    /// </summary>
    [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {  // значение свойства
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

    /// <summary>
    /// описание назначени€ свойства
    /// </summary>
    [ConfigurationProperty("description", IsRequired = false)]
        public String Description
        {// описание назначени€ свойства
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

    public CMDPropertyElement()
    	{ }
    public CMDPropertyElement( string subcommand, string nameProperty,
               string value)
        {
            Subcommand = subcommand;
            NameProperty = nameProperty;
            Value = value;
        }

    public CMDPropertyElement(string subcommand, string nameProperty,
       string value, string description)
    {
        Subcommand = subcommand;
        NameProperty = nameProperty;
        Value = value;
        Description = description;
    }
  }
}