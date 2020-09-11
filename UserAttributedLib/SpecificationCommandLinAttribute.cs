using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//[assembly: CLSCompliant(true)]

    #region Custom attribute!
    // A custom attribute.
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct,AllowMultiple = false, Inherited = false)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public sealed class SpecificationCommandLineAttribute : System.Attribute
    {
        public string Description { get; set; }
        public string Name { get; set; }
	//  Тип спецификация разбора опций командной строки
        public string SpecificationCommandLineTip  { get; set; }

        public SpecificationCommandLineAttribute(string tip, string name )
	{ 
         Name = name;
		 SpecificationCommandLineTip = tip;
	}
        public SpecificationCommandLineAttribute() { }
    }
    #endregion

