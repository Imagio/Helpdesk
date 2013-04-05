using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;
using System.Reflection;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public abstract class EntityViewModel<TE>: TabViewModel, IDataErrorInfo where TE: class
    {
        protected HelpdeskContext Context;

        public EntityViewModel(TE model, HelpdeskContext context)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            //if (context == null)
            //    throw new ArgumentNullException("context");

            Model = model;
            Context = context;

            PropertyChanged += EntityViewModel_PropertyChanged;

            InitValidation();
        }

        public TE Model { get; private set; }

        #region IDataErrorInfo

        void EntityViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Error")
            {
                RemoveError(e.PropertyName);
                CheckErrors(e.PropertyName);
                UpdateError();
            }
        }

        protected void InitValidation()
        {
            foreach (var prop in typeof(TE).GetProperties())
            {
                if (prop.Name != "Error")
                    CheckErrors(prop.Name);
            }
        }

        private Dictionary<string, string> _errorList = new Dictionary<string, string>();

        protected abstract void CheckErrors(string propertyName);

        public string Error
        {
            get 
            {
                if (_errorList.Count == 0)
                    return null;
                var sb = new StringBuilder();
                foreach (var item in _errorList)
                {
                    sb.AppendFormat("{0}\n", item.Value);
                }
                return sb.ToString();
            }
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

        public void AddError(string propertyName, string error = "Поле {0} должно быть заполнено")
        {
            if (_errorList.ContainsKey(propertyName))
                return;

            var name = propertyName;

            var propertyInfo = Model.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var displayName = propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true) as DisplayAttribute[];
                if (displayName != null && displayName.Count() > 0)
                {
                    name = displayName[0].Name;
                }
            }

            _errorList.Add(propertyName, String.Format(error, '[' + name + ']'));
        }

        public void UpdateError()
        {
            OnPropertyChanged(() => Error);
        }

        #endregion
    }
}
