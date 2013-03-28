using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    public class HelpdeskContext: DbContext
    {
        /// <summary>
        /// Сотрудники
        /// </summary>
        DbSet<Employee> EmployeeSet { get; set; }

        /// <summary>
        /// Аккаунты
        /// </summary>
        DbSet<Account> AccountSet { get; set; }
    }
}
