using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public abstract partial class CommandLineRun : ICommandLineRun//, System.ComponentModel.INotifyDataErrorInfo
{
    #region INotifyDataErrorInfo
        /// <summary>
        /// ошибки проверки для всего CommandLineRun.
        /// указанное свойствщ, список ошибок
        /// </summary>
        public Dictionary<string, List<string>> Errors => _errors;
        /// <summary>
        /// имеет ли сущность ошибки проверки.
        /// </summary>
        public virtual bool HasErrors => _errors.Count > 0;

        /// <summary>
        /// ошибки проверки для указанного свойства или для всей сущности
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public virtual IEnumerable GetErrors(string propName)
        {
            GetErrorsStart( this);
        // return null;
            IEnumerable Er;
            if (string.IsNullOrEmpty(propName))
            {
                Er = _errors.Values;
            }
            else //
            {
                Er = _errors.ContainsKey(propName) ? _errors[propName] : null;
            }
            GetErrorsEnd( Er, this);
            return Er;            
        }

        /// <summary>
        /// Cделать проверку
        /// </summary>
        /// <param name="IsClear"> Нужно предварительно очистить cписок ошибок сущности</param>
        public virtual void MakeAdditionalCheck(bool IsClear = true)
        {
            if (IsClear) // чистим ошибки
                this.ClearErrors( true, null);
            AddListErrors(nameof(CommandName), GetErrorsFromAnnotations(nameof(CommandName), this.CommandName));
        }

        /// <summary>
        /// Добавить ошибку для свойства
        /// </summary>
        /// <param name="propName">Имя свойства.</param>
        /// <param name="error">Выявленная ошибка.</param>
        public void AddError(string propName, string error)
        {
            AddListErrors(propName, new List<string> { error });
        }


    #region protected 
        /// <summary>
        /// ошибки проверки для всего CommandLineRun.
        /// указанное свойствщ, список ошибок
        /// </summary>
        protected readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propName"></param>
        protected void OnErrorsChanged(string propName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
        }

        /// <summary>
        /// Проверки достоверности на основе аннотаций данных 
        /// ошибки проверки сущности
        /// </summary>
        /// <typeparam name="T"> тип проверяемого значения </typeparam>
        /// <param name="propName">Имя свойства.</param>
        /// <param name="value">проверяемое значение</param>
        /// <returns>Выявленные ошибки проверки свойства сущности.</returns>
        protected List<string> GetErrorsFromAnnotations<TEntity>(string propName, TEntity value)
        {
            var results = new List<ValidationResult>(); //информацией о возникших ошибках. 
            var vc = new ValidationContext(this, null, null) { MemberName = propName };
	// Validator позволяет проверять, есть ли в объекте ошибки, связанные с аннотациями данных, в ValidationContext. 
            var isValid = Validator.TryValidateProperty(value, vc, results);
            return (isValid) ? null : results.ConvertAll( o => o.ErrorMessage);
        }
        /// <summary>
        /// Добавить ошибки для свойства
        /// </summary>
        /// <param name="propName">Имя свойства.</param>
        /// <param name="errors">Выявленные ошибки.</param>
        /// AddListError
        public void AddListErrors(string propName, List<string> errors)
        {
            if (errors?.Count > 0  && !string.IsNullOrEmpty( propName))
            {
                bool changed = false;
                if (!_errors.ContainsKey(propName))
                {// не присутствует
                    _errors.Add(propName, new List<string>());
                    changed = true;
                }
                var erlist = _errors[propName];
                foreach (var err in errors)
                {
                    if (erlist.Contains(err)) // уже присутствует
                        continue;
                    erlist.Add(err);
                    changed = true;
                }
                if (changed)
                {
                    OnErrorsChanged(propName);
                }
            }
        }

        protected void ClearErrors(bool IsEvent = false, string propName = "")
        {
            if (String.IsNullOrEmpty(propName))
            {
                _errors.Clear();
            }
            else
            {
                    _errors.Remove(propName);
            }
            if (IsEvent) //
                OnErrorsChanged(propName);
        }
    #endregion protected 


    /// <summary>
    /// делегат метода, обрабатывающий событие, когда событие предоставляет данные.
    /// DataErrorsChangedEventArgs Предоставляет данные для события System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged.
    /// </summary>
    public event EventHandler<System.ComponentModel.DataErrorsChangedEventArgs> ErrorsChanged;
     //   System.ComponentModel.INotifyDataErrorInfo ErrorsChanged1;


        #region partial Частичные методы
        /// <summary>
        /// Частичный метод. настройка выборки GetErrors.
        /// </summary>
        /// <param name="countS"> собственно сам объект</param>
        partial void GetErrorsStart(CommandLineRun countE);
        /// <summary>
        /// Частичный метод. Утвердим выборку GetErrors.
        /// </summary>
        /// <param name="_t"></param>
        /// <param name="countS"> собственно сам объект</param>
        partial void GetErrorsEnd(IEnumerable _t, CommandLineRun countE);

        #endregion partial

    #endregion INotifyDataErrorInfo
}


