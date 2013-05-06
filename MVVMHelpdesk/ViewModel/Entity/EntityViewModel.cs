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
using System.Collections.ObjectModel;
using Imagio.Helpdesk.ViewModel.Helper.ViewGenerator;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public abstract class EntityViewModel : ViewModelBase
    {
    }

    public class EntityViewModel<TE>: EntityViewModel, IDataErrorInfo where TE: class
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

            DataItemCollection = new ObservableCollection<DataItem>();
            AddDataItems();
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

        public ObservableCollection<DataItem> DataItemCollection { get; private set; }
        protected virtual void AddDataItems()
        {
            var propertyList = Model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty).Where(w => w.GetSetMethod() != null).ToList();
            foreach (var property in propertyList)
            {
                var propertyType = property.PropertyType;
                if (propertyType == typeof(String))
                {
                    DataItemCollection.Add(new StringDataItem(Model, property.Name));
                } 
                else if (propertyType == typeof(byte[]))
                {
                    DataItemCollection.Add(new PasswordDataItem(Model, property.Name));
                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    DataItemCollection.Add(new BoolDataItem(Model, property.Name));
                }
                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    DataItemCollection.Add(new DateTimeDataItem(Model, property.Name));
                }
                else if (propertyType == typeof(Int32) || propertyType == typeof(Int32?))
                {
                    DataItemCollection.Add(new IntDataItem(Model, property.Name));
                }
                else if (propertyType.BaseType == typeof(EntityModel))
                {
                    var path = "";
                    if (propertyType == typeof(Employee))
                        path = "ShortName";
                    else
                        path = "Name";
                    DataItemCollection.Add(new CollectionDataItem(Model, property.Name, Context, propertyType, path));
                }
            }
        }
    }
}
