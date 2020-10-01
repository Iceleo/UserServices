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
    public class CMDRunSpecificationCollection : ConfigurationElementCollection
    {

    public CMDRunSpecificationCollection()
    {
        //ConfigurationCMDRunSpecification cmd = (ConfigurationCMDRunSpecification)CreateNewElement();
        //Add(cmd);
    }
    /// <summary>
    /// Создание элемента в коллекцию
    /// </summary>
    /// <param name="element">
    protected override ConfigurationElement CreateNewElement()
        {
            return new CMDRunSpecificationElement();
        }

    /// <summary>
    ///  Получить ключ элемента
    /// </summary>
    /// <param name="element"></param> элемента
    /// <returns></returns> ключ элемента
    protected override object GetElementKey(ConfigurationElement element)
        {
        CMDRunSpecificationElement _element = (CMDRunSpecificationElement)element;
            return (_element.SubcommandLeft + "."  //
		+ _element.Specification + "." + _element.SubcommandRight);
        }

    /// <summary>
    /// Добавление элемента в коллекцию
    /// </summary>
    /// <param name="element">
    public void Add(CMDRunSpecificationElement element)
    {
        BaseAdd((ConfigurationElement)element);
    }

    public new CMDRunSpecificationElement this[string key]
    {
        get{     return (CMDRunSpecificationElement)BaseGet(key); }
    }

    public CMDRunSpecificationElement this[int key]
    {
        get { return (CMDRunSpecificationElement)BaseGet(key); }
    }
  }
}