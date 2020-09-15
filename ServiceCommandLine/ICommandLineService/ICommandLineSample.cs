using System;
using System.Collections.Generic;

public interface ICommandLineSample : ICommandLine
 {
    /// <summary>
    /// Разбор вызова
    /// </summary>
    /// <param name="properties">Свойства комманды</param>
    /// <param name="ParametersCmd">Параметры комманды</param>
    /// <returns>Результат разбора</returns>
    bool ParseCommandLine(Dictionary<string, string> properties,
            List<string> ParametersCmd);
    /// <summary>
    /// Удовлетворяются ли условия начала работы класса
    /// </summary>
    /// <returns>True - условия начала работы класса удовлетворены</returns>
    bool IsSatisfiedBy();

    /// <summary>
    /// Установить - условия начала работы класса не выполнимы.
    /// </summary>
    void SetCommandLineBad();

 }
