using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class FirmViewModel: EntityViewModel<Firm>
    {
        public FirmViewModel(Firm model, HelpdeskContext context)
            : base(model, context)
        {

        }
    }
}
