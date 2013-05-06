using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.Converter
{
    public class ConsumableConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var consumable = (Consumable)value;
                var context = new HelpdeskContext();
                var pl = context.Set<ConsumableAccounting>().Where(w => w.Consumable.Id == consumable.Id).Where(w => w.Sign).Sum(s => s.Count);
                var mi = context.Set<ConsumableAccounting>().Where(w => w.Consumable.Id == consumable.Id).Where(w => !w.Sign).Sum(s => s.Count);
                return pl - mi;
            }
            finally
            {
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
