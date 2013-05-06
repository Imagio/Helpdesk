using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Imagio.Helpdesk.ViewModel.Helper.ViewGenerator
{
    public class DataItem : ViewModelBase, IDataErrorInfo
    {
        public DataItem(Object model, string property, Func<Object, Object> setter = null)
        {
            _setter = setter;

            Model = model;
            PropertyName = property;

            _propertyInfo = model.GetType().GetProperty(PropertyName);
            var propertyAttribute = _propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
            Label = propertyAttribute == null ? PropertyName : propertyAttribute.Name;
        }

        public DataItem(Object model, Expression<Func<Object, Object>> property, Func<Object, Object> setter = null )
        {
            _setter = setter;

            Model = model;
            PropertyName = GetPropertyName(property);

            _propertyInfo = model.GetType().GetProperty(PropertyName);
            var propertyAttribute = _propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
            Label = propertyAttribute == null ? PropertyName : propertyAttribute.Name;
        }

        private String GetPropertyName(Expression<Func<Object, Object>> f)
        {
            var body = f.Body;
            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;
            if ((body as MemberExpression) != null)
            {
                return (body as MemberExpression).Member.Name;
            }
            return "";
        }

        private Func<Object, Object> _setter;

        private PropertyInfo _propertyInfo; 

        protected object Model { get; private set; }

        public string Label { get; private set; }

        protected string PropertyName { get; private set; }

        public Object Value
        {
            get
            {
                return _propertyInfo.GetValue(Model, null);
            }
            set
            {
                try
                {
                    Object newValue = _setter != null ? _setter(value) : value;
                    _propertyInfo.SetValue(Model, newValue, null);
                    OnPropertyChanged(() => Value);
                }
                catch
                {
                }
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get 
            {
                return (Model as IDataErrorInfo)[PropertyName];
            }
        }
    }
}
