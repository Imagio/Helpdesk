using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;
using System.Reflection;
using System.Windows.Input;
using Imagio.Helpdesk.ViewModel.Helper;
using System.Data.Entity.Validation;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public abstract class EntityViewModel<TE>: ViewModelBase, IDataErrorInfo where TE: class
    {
        public delegate void SaveEventHandler(object sender);
        public event SaveEventHandler OnSave;

        public delegate void CancelEventHandler(object sender);
        public event CancelEventHandler OnCancel;

        protected HelpdeskContext Context;

        public EntityViewModel(TE model, HelpdeskContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            if (context == null)
                throw new ArgumentNullException("context");

            Model = model;
            Context = context;
        }

        public TE Model { get; private set; }

        #region IDataErrorInfo

        private Dictionary<string, string> _errorList = new Dictionary<string, string>();

        public string Error
        {
            get 
            {
                if (_errorList.Count == 0)
                    return null;
                var sb = new StringBuilder();
                foreach (var item in _errorList)
                {
                    if (sb.Length == 0)
                        sb.AppendFormat("{0}", item.Value);
                    else
                        sb.AppendFormat("\n{0}", item.Value);
                }
                return sb.ToString();
            }
        }

        public bool HasError
        {
            get { return _errorList.Count > 0; }
        }

        public string this[string columnName]
        {
            get
            {
                return _errorList.ContainsKey(columnName) ? _errorList[columnName] : null;
            }
        }

        public void RemoveError(string propertyName)
        {
            if (_errorList.ContainsKey(propertyName))
                _errorList.Remove(propertyName);
        }

        public void AddError(string propertyName, string error)
        {
            if (!_errorList.ContainsKey(propertyName))
                _errorList.Add(propertyName, error);
            RaisePropertyChanged(propertyName);
        }

        public void UpdateError()
        {
            OnPropertyChanged(() => Error);
            OnPropertyChanged(() => HasError);
        }

        #endregion

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                _saveCommand = _saveCommand ?? new RelayCommand(() =>
                {
                    var allErrors = Context.GetValidationErrors();
                    var errors = allErrors.Where(o => o.Entry.Entity == Model).FirstOrDefault();
                    if (errors != null)
                    {
                        foreach (var item in errors.ValidationErrors)
                        {
                            AddError(item.PropertyName, item.ErrorMessage);
                            UpdateError();
                        }
                    }
                    else
                        if (OnSave != null)
                            OnSave(this);
                });
                return _saveCommand;
            }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                _cancelCommand = _cancelCommand ?? new RelayCommand(() =>
                {
                    if (OnCancel != null)
                        OnCancel(this);
                });
                return _cancelCommand;
            }
        }
    }
}
