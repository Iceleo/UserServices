using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Шаблон подкоманды вызова приложения из командной строки
/// Создаем спецификация совместимости экземпляров шаблонов, 
/// который соответствует критериям указываемых при составлении шаблона. 
/// </summary>
public partial class  CommandLineSample : ICommandLineSample
{ // 

  #region static
    /// <summary>
    /// Создаем спецификация XOR совместимости экземпляров шаблонов, 
    /// который соответствует критериям указываемых при составлении шаблона. 
    /// </summary>
    /// <param name="leftCommand"></param>
    /// <param name="rightCommand"></param>
    /// <returns></returns>
    public static CommandLineSample operator |(CommandLineSample leftCommand, CommandLineSample rightCommand)
    {
        // создаем спецификацию для проверки совместимости вызова XOR
        SpecificationCommandLineOr ss = new SpecificationCommandLineOr(leftCommand, rightCommand);
        CommandLineService.ListSpecificationCommandLine.Add(ss);        
        return leftCommand; // rigthCommand;
    }

    /// <summary>
    /// Создаем спецификацию OR совместимости экземпляров шаблонов, 
    /// который соответствует критериям указываемых при составлении шаблона. 
    /// </summary>
    /// <param name="leftCommand"></param>
    /// <param name="rigthCommand"></param>
    /// <returns></returns>
    public static CommandLineSample operator ^ (CommandLineSample leftCommand, CommandLineSample rightCommand)
    {
        // создаем спецификацию для проверки совместимости вызова XOR
        SpecificationCommandLineXor ss = new SpecificationCommandLineXor(leftCommand, rightCommand);
        CommandLineService.ListSpecificationCommandLine.Add(ss);
        return leftCommand; // rigthCommand;
    }

    /// <summary>
    /// Создаем спецификация And совместимости экземпляров шаблонов, 
    /// который соответствует критериям указываемых при составлении шаблона. 
    /// </summary>
    /// <param name="leftCommand"></param>
    /// <param name="rightCommand"></param>
    /// <returns></returns>
    public static CommandLineSample operator & (CommandLineSample leftCommand, CommandLineSample rightCommand)
    {
        // создаем спецификацию для проверки совместимости вызова XOR
        SpecificationCommandLineAnd ss = new SpecificationCommandLineAnd(leftCommand, rightCommand);
        CommandLineService.ListSpecificationCommandLine.Add(ss);
        return leftCommand; // rigthCommand;
    }

    #endregion static
}