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
    ///  Секция дополнительных параметров программы "Подкоманды и спецификации"
    /// </summary>
    public sealed class CMDRunSpecificationSection : ConfigurationSection
    { // дополнительная настройка программы 
        
        public CMDRunSpecificationSection()
        {
        }
        
    [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CMDRunSpecificationCollection)
	     //,AddItemName = "add"
         //,ClearItemsName = "clear"
         //,RemoveItemName = "remove"
            )]
        public CMDRunSpecificationCollection  CMDRunSpecifications
        {
            get { return (CMDRunSpecificationCollection)base[""]; }
        }

        [ConfigurationProperty("default")]
        public string Default
        {
            get { return (string)base["default"]; }
            set { base["default"] = value; }
        }

    }
}