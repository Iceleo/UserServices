using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Digital_Patterns.SpecificationClassic;

/// <summary>
/// логическое исключение ИЛИ для вызова  приложения из командной строки. 
/// Обе подкоманды указываться вместе при вызыве не могут
/// </summary>
public class SpecificationCommandLineXor : SpecificationCommandLine<List<CommandLineSample>>
//SpecificationExpression<CommandLineSample>
{
    public SpecificationCommandLineXor(CommandLineSample _leftCommand, CommandLineSample _rightCommand)
		: base (_leftCommand, _rightCommand, "^") { }
/* 
    public override Expression<Func<CommandLineSample, bool>> ToExpression()
    {
        return movie => movie.MpaaRating <= _rating;
    }
*/
    public override bool IsSatisfiedBy(List<CommandLineSample>  cmdList)
	{ //
	//return (cmd.Contains(leftCommand)) ^ (cmd.Contains(rightCommand));
        bool rc = false;
		if (cmdList.Contains(leftCommand))
			if (!cmdList.Contains(rightCommand)) //leftCommand запускаем
				rc = true;	
			else { 
				_error = String.Format($"Команда {leftCommand.CommandName} " +
					$"не может выполняться с командой {rightCommand.CommandName}.");
				rightCommand.SetCommandLineBad(); //rightCommand нельзя запускать
	       		//leftCommand.SetCommandLineBad(); //leftCommand нельзя запускать
		   }
		else //
			if (cmdList.Contains(rightCommand))  //rightCommand запускаем
				rc = true;	
			else // не представлены обе. 
				if (cmdList.Count > 0) // Возможна сложная схема вызова
					rc = true;
				else // Ошибка вызова
				_error = String.Format($"В командной строке нужно указать " +
				$"команду {leftCommand.CommandName} или команду {rightCommand.CommandName} .");

		return	rc;
	}
}
