using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            var propertyName = (property.Body as MemberExpression).Member.Name;
            RaisePropertyChanged(propertyName);
        }

        public void OnPropertyChanged<T>(Expression<Func<T>> property, ref T result, T newValue)
        {
            if (!Equals(result, newValue))
            {
                result = newValue;
                var propertyName = (property.Body as MemberExpression).Member.Name;
                RaisePropertyChanged(propertyName);
            }
        }
    }
}
