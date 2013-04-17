using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class ConsumableViewModel: EntityViewModel<Consumable>
    {
        public ConsumableViewModel(Consumable model, HelpdeskContext context)
            : base (model, context)
        {

        }

        public IList<ConsumableType> ConsumableTypeCollection
        {
            get
            {
                return Context.Set<ConsumableType>().ToList();
            }
        }

        public IList<Firm> FirmCollection
        {
            get
            {
                return Context.Set<Firm>().ToList();
            }
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
