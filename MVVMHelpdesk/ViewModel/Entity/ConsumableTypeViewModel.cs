using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class ConsumableTypeViewModel: EntityViewModel<ConsumableType>
    {
        public ConsumableTypeViewModel(ConsumableType model, HelpdeskContext context)
            : base(model, context) { }
    }
}
