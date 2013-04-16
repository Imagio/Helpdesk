using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class HardwareTypeViewModel: EntityViewModel<HardwareType>
    {
        public HardwareTypeViewModel(HardwareType model, HelpdeskContext context)
            : base(model, context) { }
    }
}
