using System.Data.Entity;

namespace Imagio.Helpdesk.Model.Interfaces
{
    internal interface IHelpdeskContext
    {
        /// <summary>
        /// Список сотрудников
        /// </summary>
        DbSet<Employee> Employees { get; }

        /// <summary>
        /// Список учетных записей
        /// </summary>
        DbSet<Account> Accounts { get; }
    }
}
