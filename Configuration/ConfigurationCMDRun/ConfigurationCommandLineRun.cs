using System;
using System.Configuration;

namespace UserServices.ConfigurationCMDRun
{

/// <summary>
/// ��-�� ���������� ���������
/// </summary>  //ConfigurationCommandLineRun
    public class ConfigurationCommandLineRun: ConfigurationElement
    {
    /// <summary>
    /// ��� �����������
    /// </summary>
    [ConfigurationProperty("subcommand", IsRequired = true, IsKey = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 20*/)]
    public String Subcommand
        {  //��� �����������
            get { return (string)this["subcommand"]; }
            set { this["subcommand"] = value; }
        }

    /// <summary>
    /// ��� ������ � �����
    /// </summary>
    [ConfigurationProperty("assemblyNameWithPath", IsRequired = false)]
	    [StringValidator(InvalidCharacters = "[]{}/;'\"|"
            /*, MinLength = 1*/, MaxLength = 256)]
        public String AssemblyNameWithPath
    { // ��� ��������
            get { return (string)this["assemblyNameWithPath"]; }
            set { this["assemblyNameWithPath"] = value; }
    }

    /// <summary>
    /// ����� ���������� ����������
    /// </summary>
    [ConfigurationProperty("numberExecution", IsRequired = false)]
        public int NumberExecution
        {// ����� ����������
            get { return (int)this["numberExecution"]; }
            set { this["numberExecution"] = value; }
        }

    /// <summary>
    /// �������� ���������� 
    /// </summary>
    [ConfigurationProperty("description", IsRequired = false)]
        public String Description
        {// �������� ���������� 
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

    public ConfigurationCommandLineRun()
    	{ }
    public ConfigurationCommandLineRun( string subcommand, string assemblyNameWithPath = "")
    {
        Subcommand = subcommand;
        AssemblyNameWithPath = assemblyNameWithPath;
    }

    public ConfigurationCommandLineRun(string subcommand, 
                string assemblyNameWithPath ="", string description = "", int numberExecution = 0)
    {
        Subcommand = subcommand;
         AssemblyNameWithPath = assemblyNameWithPath;
        Description = description;
	    NumberExecution = numberExecution;
    }
  }
}