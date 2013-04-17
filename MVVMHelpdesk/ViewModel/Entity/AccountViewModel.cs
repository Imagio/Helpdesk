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

        protected override void AddDataItems()
        {
            base.AddDataItems();
            DataItemCollection.Add(new StringDataItem(Model, m => (m as Account).Login));
            DataItemCollection.Add(new PasswordDataItem(Model, m => (m as Account).Password, o => { return Helper.Hash.Calc(o.ToString()); }));
            DataItemCollection.Add(new BoolDataItem(Model, m => (m as Account).IsActive));
        }

        public string Password 
        {
            set { Model.Password = Helper.Hash.Calc(value); }
        }
    }
}
