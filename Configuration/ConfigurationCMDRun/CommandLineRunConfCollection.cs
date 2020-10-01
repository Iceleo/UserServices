using System;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UserServices.ConfigurationCMDRun
{

/// <summary>
/// ��������� ��� ������ �������������� ���������� ���������
/// </summary>
    public class CommandLineRunConfCollection : ConfigurationElementCollection
    {

    public CommandLineRunConfCollection()
    {
        //ConfigurationCommandLineRun cmd = (ConfigurationCommandLineRun)CreateNewElement();
        //Add(cmd);
    }
    /// <summary>
    /// �������� �������� � ���������
    /// </summary>
    /// <param name="element">
    protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigurationCommandLineRun();
        }

    /// <summary>
    ///  �������� ���� ��������
    /// </summary>
    /// <param name="element"></param> ��������
    /// <returns></returns> ���� ��������
    protected override object GetElementKey(ConfigurationElement element)
        {
        ConfigurationCommandLineRun _element = (ConfigurationCommandLineRun)element;
            return (_element.Subcommand) ;
        }

    /// <summary>
    /// ���������� �������� � ���������
    /// </summary>
    /// <param name="element">
    public void Add(ConfigurationCommandLineRun element)
    {
        BaseAdd((ConfigurationElement)element);
    }

    public new ConfigurationCommandLineRun this[string key]
        {
            get{     return (ConfigurationCommandLineRun)BaseGet(key); }
        }
        public ConfigurationCommandLineRun this[int key]
        {
            get { return (ConfigurationCommandLineRun)BaseGet(key); }
        }
    /*
    public new string AddElementName
    {
        get { return base.AddElementName; }
        set { base.AddElementName = value; }
    }

    public new string ClearElementName
    {
        get { return base.ClearElementName; }
        set { base.ClearElementName = value; }
    }

    public new string RemoveElementName
    {
        get => base.RemoveElementName;
        set => base.RemoveElementName = value;
    }
    */
  }
}