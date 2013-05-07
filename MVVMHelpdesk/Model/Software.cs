using System;
using System.ComponentModel.DataAnnotations;
using Imagio.Helpdesk.Model.Interfaces;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Программное обеспечение
    /// </summary>
    public class Software : EntityModel, IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        [Display(Name = @"Наименование")]
        public String Name { get; set; }

        /// <summary>
        /// Ответственный специалист
        /// </summary>
        [Display(Name = @"Ответственный специалист")]
        public Employee Master { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        [Display(Name = @"Производитель")]
        public Firm Maker { get; set; }

        [Display(Name = @"Инвентарный номер")]
        public String InventoryNumber { get; set; }
    }
}
