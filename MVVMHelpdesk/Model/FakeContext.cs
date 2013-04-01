using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    internal class FakeContext: DbContext, IHelpdeskContext
    {
        public FakeContext()
        {
            AccountSet.Add(new Account { Id = Guid.NewGuid(), Login = "root", Password = Imagio.Helpdesk.ViewModel.Helper.Hash.Calc("root"), IsActive = true });
        }

        public DbSet<Employee> EmployeeSet { get; set; }

        public DbSet<Account> AccountSet { get; set; }
    }
}
