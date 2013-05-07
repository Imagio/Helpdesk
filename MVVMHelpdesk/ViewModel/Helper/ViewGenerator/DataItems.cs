using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Helper.ViewGenerator
{
    public class StringDataItem: DataItem
    {
        public StringDataItem(Object model, Expression<Func<Object, Object>> property)
            : base(model, property) { }

        public StringDataItem(Object model, String property)
            : base(model, property) { }
    }

    public class PasswordDataItem : DataItem
    {
        public PasswordDataItem(Object model, Expression<Func<Object, Object>> property)
            : base(model, property, o => Hash.Calc(o.ToString())) { }

        public PasswordDataItem(Object model, String property)
            : base(model, property, o => Hash.Calc(o.ToString())) { }
    }

    public class BoolDataItem : DataItem
    {
        public BoolDataItem(Object model, Expression<Func<Object, Object>> property)
            : base(model, property) { }
        public BoolDataItem(Object model, String property)
            : base(model, property) { }
    }

    public class DateTimeDataItem : DataItem
    {
        public DateTimeDataItem(Object model, Expression<Func<Object, Object>> property)
            : base(model, property) { }
        public DateTimeDataItem(Object model, String property)
            : base(model, property) { }
    }

    public class IntDataItem : DataItem
    {
        public IntDataItem(Object model, Expression<Func<Object, Object>> property)
            : base(model, property, o =>
                {
                    int res;
                    try
                    {
                        Int32.TryParse(o.ToString(), out res);
                    }
                    catch
                    {
                        return 0;
                    }
                    return res;
                }) { }
        public IntDataItem(Object model, String property)
            : base(model, property, o =>
            {
                int res;
                try
                {
                    Int32.TryParse(o.ToString(), out res);
                }
                catch
                {
                    return 0;
                }
                return res;
            }) { }
    }

    public class CollectionDataItem: DataItem
    {
        private readonly HelpdeskContext _context;
        private readonly Type _type;

        public CollectionDataItem(Object model, Expression<Func<Object, Object>> property, HelpdeskContext context, Type type, string path)
            : base(model, property) 
        {
            _context = context;
            _type = type;
            Path = path;
            _init();
        }

        public CollectionDataItem(Object model, String property, HelpdeskContext context, Type type, string path)
            : base(model, property) 
        {
            _context = context;
            _type = type;
            Path = path;
            _init();
        }

        private void _init()
        {
            ItemsSource = EntityStore.GetEntityQuery(_context, _type).ToList();
        }

        public IList<Object> ItemsSource { get; private set; }

        public string Path { get; private set; }
    }
}
