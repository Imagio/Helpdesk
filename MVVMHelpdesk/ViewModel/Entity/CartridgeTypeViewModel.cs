using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class CartridgeTypeViewModel: EntityViewModel<CartridgeType>
    {
        public CartridgeTypeViewModel(CartridgeType model, HelpdeskContext context)
            : base(model, context) { }
    }
}
