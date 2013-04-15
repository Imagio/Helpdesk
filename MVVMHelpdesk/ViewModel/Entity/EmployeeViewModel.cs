using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class EmployeeViewModel: EntityViewModel<Employee>
    {
        public EmployeeViewModel(Employee model, HelpdeskContext context)
            : base(model, context) { }
    }
}
