using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Entity
{
    public class CartridgeViewModel: EntityViewModel<Cartridge>
    {
        public CartridgeViewModel(Cartridge model, HelpdeskContext context)
            : base(model, context) { }

        public IList<CartridgeType> CartridgeTypeCollection
        {
            get
            {
                return Context.Set<CartridgeType>().ToList();
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
