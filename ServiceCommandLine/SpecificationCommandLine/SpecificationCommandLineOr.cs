using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Digital_Patterns.SpecificationClassic;

/// <summary>
/// логическое И/ИЛИ для вызова  приложения из командной строки. 
/// Обе подкоманды указываться вместе при вызыве не могут
/// </summary>
public class SpecificationCommandLineOr : SpecificationCommandLine<List<CommandLineSample>>
//SpecificationExpression<List<CommandLineSample>>
{
 
    public SpecificationCommandLineOr(CommandLineSample _leftCommand, CommandLineSample _rightCommand)
		: base (_leftCommand, _rightCommand, "|") { }
   public override bool IsSatisfiedBy(List<CommandLineSample> cmdList)
	{ //
	  bool rc = (cmdList.Contains(leftCommand)) | (cmdList.Contains(rightCommand));
	  if ( !rc) //
		if (cmdList.Count > 0) // Возможна сложная схема вызова
			rc = true;
		else // Ошибка вызова
		_error = String.Format($"В командной строке нужно указать " +
		$"команду {leftCommand.CommandName} и/или команду {rightCommand.CommandName} .");
	return rc;
	}
}
