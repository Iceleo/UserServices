using System;
using System.Collections.Generic;
//using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Patterns.SpecificationClassic;
//using UserServices.ICommandLineService;

/// <summary>
/// Проверки достоверности свойства на основе аннотаций данных 
/// ошибки проверки сущности заносим в список ошибок класс командной строки
/// </summary>
public class SpecificationFromAnnotations : SpecificationClassic<ICommandLineRun>
//SpecificationExpression<CommandLineSample>
{
    /// <summary>
    /// Имя свойства для проверки достоверности на основе аннотаций данных 
    /// </summary>
	readonly string propName;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="NameProp">Имя свойства для проверки</param>
    public SpecificationFromAnnotations(string NameProp)
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
        PropertyInfo pp = boss.GetType().GetProperty(propName);
	    var value = pp.GetValue( boss, null).ToString();
            List<ValidationResult> results = new List<ValidationResult>(); //информацией о возникших ошибках. 
            var vc = new ValidationContext(this, null, null) { MemberName = propName };
        bool rc = false;
        try
        {
            // Validator позволяет проверять, есть ли в объекте ошибки, связанные с аннотациями данных, в ValidationContext. 
            rc = Validator.TryValidateProperty(value, vc, results);

            if (!rc) //
                boss.AddListErrors(propName, results.ConvertAll<string>(
                        (o => o.ErrorMessage)));
        }
        catch (System.ArgumentNullException e1)
        {//     value cannot be assigned to the property. -or- value is null.
            boss.AddError(propName, string.Format(
           $"Value cannot be assigned to the property. -or- value is null."));
        }
        catch (System.ArgumentException e2)
        //     The System.ComponentModel.DataAnnotations.ValidationContext.MemberName property
        //     of validationContext is not a valid property.)
        {
            boss.AddError(propName, string.Format(
                   $"Property {propName} is not a valid."));
        }
        return	rc;
	}
}
