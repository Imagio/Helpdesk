using System;
using System.Linq;
using System.Windows.Data;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.Converter
{
    public class ConsumableConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var consumable = (Consumable)value;
            var context = new HelpdeskContext();
            var pl = context.Set<ConsumableAccounting>().Where(w => w.Consumable.Id == consumable.Id).Where(w => w.Sign).Sum(s => s.Count);
            var mi = context.Set<ConsumableAccounting>().Where(w => w.Consumable.Id == consumable.Id).Where(w => !w.Sign).Sum(s => s.Count);
            return pl - mi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
