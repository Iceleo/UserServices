using System;
using System.Configuration;

namespace UserServices.ConfigurationCMDRun
{

/// <summary>
/// ��-�� ���������� ���������
/// </summary>  //CMDRunSpecificationElement
    public class CMDRunSpecificationElement: ConfigurationElement
    {
    /// <summary>
    /// ��� ����������� �����
    /// </summary>
    [ConfigurationProperty("subcommandLeft", IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 20*/)]
    public String SubcommandLeft
        {  //��� �����������
            get { return (string)this["subcommandLeft"]; }
            set { this["subcommandLeft"] = value; }
        }

    /// <summary>
    /// ��� ������������(XOR, AND, OR) ������������� ������ ���������.
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
    /// ��� ����������� �����
    /// </summary>
    [ConfigurationProperty("subcommandRight", IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\"
            /*, MinLength = 1, MaxLength = 20*/)]
    public String SubcommandRight
        {  //��� �����������
            get { return (string)this["subcommandRight"]; }
            set { this["subcommandRight"] = value; }
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