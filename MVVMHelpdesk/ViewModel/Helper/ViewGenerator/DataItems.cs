using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Imagio.Helpdesk.ViewModel.Helper.ViewGenerator
{
    public class StringDataItem: DataItem
    {
        public StringDataItem(Object model, Expression<Func<Object, Object>> property)
            : base(model, property) { }
    }

    public class PasswordDataItem : DataItem
    {
        public PasswordDataItem(Object model, Expression<Func<Object, Object>> property, Func<Object, Object> setter = null)
            : base(model, property, setter) { }
    }

    public class BoolDataItem : DataItem
    {
        public BoolDataItem(Object model, Expression<Func<Object, Object>> property)
            : base(model, property) { }
    }
}
