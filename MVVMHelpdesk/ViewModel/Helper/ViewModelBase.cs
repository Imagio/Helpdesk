using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    public class ViewModelBase: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            var memberExpression = property.Body as MemberExpression;
            if (memberExpression != null)
            {
                var propertyName = memberExpression.Member.Name;
                RaisePropertyChanged(propertyName);
            }
        }

        public void OnPropertyChanged<T>(Expression<Func<T>> property, ref T result, T newValue)
        {
            if (!Equals(result, newValue))
            {
                result = newValue;
                var memberExpression = property.Body as MemberExpression;
                if (memberExpression != null)
                {
                    var propertyName = memberExpression.Member.Name;
                    RaisePropertyChanged(propertyName);
                }
            }
        }
    }
}
