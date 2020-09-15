using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Patterns.SpecificationClassic;
//using UserServices.ICommandLineService;

/// <summary>
///  Спецификация разбора подкоманд командной строки вызова приложения
/// </summary>
public abstract class SpecificationCommandLine<T> : SpecificationClassic<T>
//SpecificationExpression<CommandLineSample>
{
	/// <summary>
	/// Левая подкоманда спецификации 
	/// </summary>
	protected readonly ICommandLineSample leftCommand ;
	/// <summary>
	/// Правая подкоманда спецификации 
	/// </summary>
	protected readonly ICommandLineSample rightCommand;

	/// <summary>
	/// Ошибка проверки спецификации
	/// </summary>
	protected string _error;
	public string GetError()=>  _error;

	/// <summary>
	/// Обозначение спецификации разбора подкоманд
	/// </summary>
	String operation;
	public SpecificationCommandLine(ICommandLineSample _leftCommand, 
		ICommandLineSample _rightCommand, string _operation )
    {
		leftCommand  = _leftCommand ;
		rightCommand = _rightCommand;
		operation = _operation;
	}
	
	//
	public override String ToString() => $"SpecificationCommandLine /{leftCommand.CommandName} " +
		$"{operation} /{rightCommand.CommandName}.";
}
