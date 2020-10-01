using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
//using System.Threading.Tasks;
using System.Collections;
using System.Reflection;

/// <summary>
/// Сервис разбора шаблона  командной строки: команд, опций, параметров. 
/// Разбор конкретного вызова приложения из командной строки.
/// Создаем спецификация совместимости экземпляров шаблонов, 
/// который соответствует критериям совместимости, указываемых при составлении шаблона.
/// </summary>
public static partial class CommandLineService //: INotifyDataErrorInfo
{
    #region INotifyDataErrorInfo, but static class
    /// <summary>
    /// ошибки проверки для всего CommandLineRun.
    /// указанное свойствщ, список ошибок
    /// </summary>
    static readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
    /// <summary>
    /// ошибки проверки для всего CommandLineRun.
    /// указанное свойство, список ошибок
    /// </summary>
    public static Dictionary<string, string> Errors => _errors;
    /// <summary>
    /// имеет ли сущность ошибки проверки.
    /// </summary>
    public static bool HasErrors => _errors.Count > 0;

    /// <summary>
    /// ошибки проверки для указанного свойства или для всей сущности
    /// </summary>
    /// <param name="propName"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetErrors() => _errors;
    /// <summary>
    /// ошибки проверки для указанного свойства или для всей сущности
    /// </summary>
    /// <param name="propName"></param>
    /// <returns></returns>
    public static IList<string> GetErrors(string propName)
    {
        IList<string> Er;
        if (string.IsNullOrEmpty(propName))
        {
            Er = _errors.Values.ToList();
        }
        else //
        {
            Er = _errors.ContainsKey(propName) ? new List<string> { _errors[propName] } : null;
        }
        return Er;
    }

    /// <summary>
    /// Добавить ошибку для свойства
    /// </summary>
    /// <param name="propName">Имя свойства.</param>
    /// <param name="error">Выявленная ошибка.</param>
    public static void AddError(string propName, string error)
    {
        _errors.Add(propName,  error);
    }

    #endregion INotifyDataErrorInfo
}