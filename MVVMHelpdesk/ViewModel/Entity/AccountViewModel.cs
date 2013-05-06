using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Entity;
using Imagio.Helpdesk.ViewModel.Helper.ViewGenerator;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class AccountViewModel: EntityViewModel<Account>
    {
        public AccountViewModel(Account model, HelpdeskContext context)
            :base(model, context) 
        {
            
        }
    }
}
