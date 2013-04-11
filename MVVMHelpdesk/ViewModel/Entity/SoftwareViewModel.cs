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

        public IList<Employee> EmployeeCollection 
        {
            get
            {
                return Context.Set<Employee>().ToList();
            }
        }
    }
}
