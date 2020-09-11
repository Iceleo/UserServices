using System;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;
using Digital_Patterns.SpecificationClassic;

/// <summary>
///  ѕроверка существовани€ директории
/// </summary>
public class SpecificationDirectoryExist : SpecificationClassic<ICommandLineRun>
//SpecificationExpression<CommandLineSample>
{
	readonly string propName;
    public SpecificationDirectoryExist(string NameProp)//, string propName)
    {
        propName = NameProp;
    }

    public override bool IsSatisfiedBy(ICommandLineRun boss)
    { //
	    bool rc = false;
        PropertyInfo pp = boss.GetType().GetProperty(propName);
        string directory = pp.GetValue( boss, null).ToString();
//// проверка параметров командной строки
       if (directory[directory.Length-1] != CommandLineService.sleshBack) // нет закрывающего слеша
           directory += CommandLineService.sleshBack;

      if ( !string.IsNullOrEmpty(directory) && 
            System.IO.Directory.Exists(directory)) //  "ƒиректорий {directory} существует.";
	        rc = true;
       else
       {
         boss.AddError( propName,  string.Format(
             $" ѕуть директории {directory} - ошибочен."));
        } 
	  return	rc;
	}
}
