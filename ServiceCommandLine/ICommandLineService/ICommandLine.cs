using System;

 public interface ICommandLine
 {
    /// <summary>
    /// выполнить 
    /// </summary>
    void Run();

    /// <summary>
    /// справка
    /// </summary>
    void Help();

    /// <summary>
    /// правила вызова
    /// </summary>
    void RulesOfchallenge();	

    /// <summary>
    /// Имя комманды
    /// </summary>
    string CommandName { get; set;}
 }
