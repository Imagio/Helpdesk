using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class SoftwareViewModel: EntityViewModel<Software>
    {
        public SoftwareViewModel(Software software, HelpdeskContext context)
            : base(software, context)
        {

        }

        public String Name
        {
            get { return Model.Name; }
            set
            {
                Model.Name = value;
                OnPropertyChanged(() => Name);
            }
        }

        protected override void CheckErrors(string propertyName)
        {
            if (propertyName == "Name" && String.IsNullOrEmpty(Model.Name))
                AddError(propertyName);
        }
    }
}
