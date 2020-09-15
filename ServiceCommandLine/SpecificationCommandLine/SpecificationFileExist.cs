using System;
using System.Linq.Expressions;
using System.Reflection;
using Patterns.SpecificationClassic;
//using UserServices.ICommandLineService;


/// <summary>
///  Проверка существования файла
/// </summary>
public class SpecificationFileExist : SpecificationClassic<ICommandLineRun>
//SpecificationExpression<CommandLineSample>
{
	readonly string propName;
    public SpecificationFileExist(string NameProp)
    {
        propName = NameProp;
    }

    /// <summary>
    /// Проверка существования файла
    /// </summary>
    /// <param name="boss">класс командной строки</param>
    /// <returns>true - файл существует.</returns>
    public override bool IsSatisfiedBy(ICommandLineRun boss)
    { //
	    bool rc = false;
        PropertyInfo pp = boss.GetType().GetProperty(propName);
	    string file = pp.GetValue( boss, null).ToString(); 

        //// проверка параметров командной строки        
        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file)) //есть файл
        {            
	        rc = true;
        }
        else
        {
            boss.AddError( propName, string.Format(
		        $" Путь или имя файла {file} - ошибочны."));
        }
        return	rc;
	}
}
