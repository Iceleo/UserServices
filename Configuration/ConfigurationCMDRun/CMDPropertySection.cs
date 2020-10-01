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
    ///  Секция дополнительных параметров программы  "Свойства и значения подкоманд".
    /// </summary>
    public sealed class CMDPropertySection : ConfigurationSection
    { //         
        public CMDPropertySection()
        {
        }
        
    [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CMDPropertyCollection)
	     //,AddItemName = "add"
         //,ClearItemsName = "clear"
         //,RemoveItemName = "remove"
            )]
        public CMDPropertyCollection CMDProperties
        {
            get { return (CMDPropertyCollection)base[""]; }
        }

        [ConfigurationProperty("default")]
        public string Default
        {
            get { return (string)base["default"]; }
            set { base["default"] = value; }
        }

    }
}