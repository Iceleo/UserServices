using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Patterns.SpecificationClassic;
//using UserServices.ICommandLineService;

/// <summary>
/// логическое И для вызова  приложения из командной строки. 
/// Обе подкоманды должны вместе указываться при вызыве 
/// </summary>
public class SpecificationCommandLineAnd : SpecificationCommandLine<List<ICommandLineSample>>
//SpecificationExpression<CommandLineSample>
{
    public SpecificationCommandLineAnd(ICommandLineSample _leftCommand, ICommandLineSample _rightCommand) 
		: base (_leftCommand, _rightCommand, "&") { }
    public override bool IsSatisfiedBy(List<ICommandLineSample> cmdList)
    { //
        bool rc = false;
		if (cmdList.Contains(leftCommand))
			if (cmdList.Contains(rightCommand)) {
				rc = true;
			}
			else { //leftCommand нельзя запускать
				_error = String.Format($"Команда {leftCommand.CommandName} " +
					$"не может выполняться без команды {rightCommand.CommandName}.");
				leftCommand.SetCommandLineBad();
			}
		else if (cmdList.Contains(rightCommand)) 
		{ //rightCommand нельзя запускать
				rightCommand.SetCommandLineBad();
				_error = String.Format($"Команда {rightCommand.CommandName} " +
				$"не может выполняться без {leftCommand.CommandName}.");
		}
		else // не представлены обе. 
			if (cmdList.Count > 0) // Возможна сложная схема вызова
				rc = true;
			else // Ошибка вызова
				_error = String.Format($"В командной строке нужно указать " +
				$"команду {leftCommand.CommandName} и команду {rightCommand.CommandName} .");

		return	rc;
	//(cmd.leftCommand.IscommandClass & rightCommand.IscommandClass);
    }
}
