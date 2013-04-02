using System.Data.Entity;

namespace Imagio.Helpdesk.Model
{
    internal class HelpdeskContext: DbContext, IHelpdeskContext
    {
        /// <summary>
        /// Аккаунты
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Картриджы
        /// </summary>
        public DbSet<Cartridge> Cartridges { get; set; }

        /// <summary>
        /// Типы картриджей
        /// </summary>
        public DbSet<CartridgeType> CartridgeTypes { get; set; }

        /// <summary>
        /// Расходники
        /// </summary>
        public DbSet<Consumable> Consumables { get; set; }

        /// <summary>
        /// Типы расходников
        /// </summary>
        public DbSet<ConsumableType> ConsumableTypes { get; set; }

        /// <summary>
        /// Сотрудники
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Производители
        /// </summary>
        public DbSet<Firm> Firms { get; set; }

        /// <summary>
        /// Оборудование
        /// </summary>
        public DbSet<Hardware> Hardware { get; set; }

        /// <summary>
        /// Типы оборудования
        /// </summary>
        public DbSet<HardwareType> HardwareTypes { get; set; }

        /// <summary>
        /// Программное обеспечение
        /// </summary>
        public DbSet<Software> Softwares { get; set; }
    }
}
