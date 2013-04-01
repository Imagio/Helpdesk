using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    internal class HelpdeskContext: DbContext, IHelpdeskContext
    {
        /// <summary>
        /// Сотрудники
        /// </summary>
        public DbSet<Employee> EmployeeSet { get; set; }

        /// <summary>
        /// Аккаунты
        /// </summary>
        public DbSet<Account> AccountSet { get; set; }
    }
}
