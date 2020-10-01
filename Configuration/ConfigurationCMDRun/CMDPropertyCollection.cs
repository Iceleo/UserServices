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
/// Коллекция для секции дополнительных параметров программы
/// </summary>
    public class CMDPropertyCollection : ConfigurationElementCollection
    {

    public CMDPropertyCollection()
    {
        //ConfigurationCommandLineRun cmd = (ConfigurationCommandLineRun)CreateNewElement();
        //Add(cmd);
    }
    /// <summary>
    /// Создание элемента в коллекцию
    /// </summary>
    /// <param name="element">
    protected override ConfigurationElement CreateNewElement()
        {
            return new CMDPropertyElement();
        }

    /// <summary>
    ///  Получить ключ элемента
    /// </summary>
    /// <param name="element"></param> элемента
    /// <returns></returns> ключ элемента
    protected override object GetElementKey(ConfigurationElement element)
        {
        CMDPropertyElement _element = (CMDPropertyElement)element;
            return (_element.Subcommand + "."+ _element.NameProperty) ;
        }

    /// <summary>
    /// Добавление элемента в коллекцию
    /// </summary>
    /// <param name="element">
    public void Add(CMDPropertyElement element)
    {
        BaseAdd((ConfigurationElement)element);
    }

    public new CMDPropertyElement this[string key]
        {
            get{     return (CMDPropertyElement)BaseGet(key); }
        }
        public CMDPropertyElement this[int key]
        {
            get { return (CMDPropertyElement)BaseGet(key); }
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