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
    /// ������ �������������� ���������� ��������� "����������, ������, ������� ������"
    /// </summary>
    public sealed class CommandLineRunSection : ConfigurationSection
    { // �������������� ��������� ��������� AddConfSections
        
        public CommandLineRunSection()
        {
        }
        
    [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(CommandLineRunConfCollection)
	     //,AddItemName = "add"
         //,ClearItemsName = "clear"
         //,RemoveItemName = "remove"
            )]
        public CommandLineRunConfCollection CmdRunConfs
        {
            get { return (CommandLineRunConfCollection)base[""]; }
        }

        [ConfigurationProperty("default")]
        public string Default
        {
            get { return (string)base["default"]; }
            set { base["default"] = value; }
        }

    }
}