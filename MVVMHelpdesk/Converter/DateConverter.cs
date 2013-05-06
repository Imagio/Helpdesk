using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Imagio.Helpdesk.Converter
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime && (DateTime)value > DateTime.MinValue)
            {
                var dt = (DateTime)value;
                if (parameter == null)
                    return dt.ToShortDateString();
                return dt.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dt;
            if (DateTime.TryParse(value.ToString(), out dt))
            {
                return dt;
            }
            return null;
        }
    }
}
