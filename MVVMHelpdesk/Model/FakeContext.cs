using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    public class FakeContext: DbContext, IHelpdeskContext
    {
        public FakeContext()
        {
            Accounts.Add(new Account { Id = Guid.NewGuid(), Login = "root", Password = Imagio.Helpdesk.ViewModel.Helper.Hash.Calc("root"), IsActive = true });
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Account> Accounts { get; set; }
    }
}
