using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Entity;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class AccountViewModel: EntityViewModel<Account>
    {
        public AccountViewModel(Account model, HelpdeskContext context)
            :base(model, context) 
        {
        }

        public string Password 
        {
            set { Model.Password = Helper.Hash.Calc(value); }
        }
    }
}
