using System;
using System.Collections.Generic;
using System.ComponentModel;

public interface ICommandLineRun : ICommandLine, INotifyDataErrorInfo
 {
    /// <summary>
    /// имя приложения 
    /// </summary>
    string AppName { get; set; }
    /// <summary>
    /// свойства комманды не найденные в свойствах  //Параметры комманды
    /// </summary>
    Dictionary<string, string> PropertiesNotFound { get; set; }

    /// <summary>
    /// Проверка удовлетворяются ли условия начала работы класса
    /// </summary>
    /// <returns>True - условия начала работы класса удовлетворены</returns>
    bool IsSatisfiedBy(Dictionary<string, string> PropertiesNotFound);

        /// <summary>
        /// Добавить ошибку для свойства
        /// </summary>
        /// <param name="propName">Имя свойства.</param>
        /// <param name="error">Выявленная ошибка.</param>
    void AddError(string propName, string error);

    /// <summary>
    /// Добавить ошибки для свойства
    /// </summary>
    /// <param name="propName">Имя свойства.</param>
    /// <param name="errorList">Выявленные ошибки.</param>
    void AddListErrors(string propName, List<string> errorList);

}
