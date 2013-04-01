using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    internal interface IHelpdeskContext
    {
        DbSet<Employee> EmployeeSet { get; set; }
        DbSet<Account> AccountSet { get; set; }
    }
}
