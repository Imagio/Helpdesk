using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    internal interface IHelpdeskContext
    {
        /// <summary>
        /// Список сотрудников
        /// </summary>
        DbSet<Employee> EmployeeSet { get; }

        /// <summary>
        /// Список учетных записей
        /// </summary>
        DbSet<Account> AccountSet { get; }
    }
}
