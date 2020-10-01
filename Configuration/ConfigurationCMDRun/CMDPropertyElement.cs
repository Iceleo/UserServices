using System;
using System.Configuration;

namespace UserServices.ConfigurationCMDRun
{

/// <summary>
/// ��-�� ���������� ���������
/// </summary>  //ConfigurationCommandLineRun
    public class CMDPropertyElement: ConfigurationElement
    {
    /// <summary>
    /// ��� �����������
    /// </summary>
    [ConfigurationProperty("subcommand", IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 20*/)]
    public String Subcommand
        {  //��� �����������
            get { return (string)this["subcommand"]; }
            set { this["subcommand"] = value; }
        }

    /// <summary>
    /// ��� ��������
    /// </summary>
    [ConfigurationProperty("nameProperty", IsRequired = true)]
	    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 32*/)]
        public String NameProperty
    { // ��� ��������
            get { return (string)this["nameProperty"]; }
            set { this["nameProperty"] = value; }
        }

    /// <summary>
    /// �������� ��������
    /// </summary>
    [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {  // �������� ��������
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

    /// <summary>
    /// �������� ���������� ��������
    /// </summary>
    [ConfigurationProperty("description", IsRequired = false)]
        public String Description
        {// �������� ���������� ��������
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