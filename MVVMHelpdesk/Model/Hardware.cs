using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Оборудование
    /// </summary>
    public class Hardware : EntityModel, IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        [Display(Name = "Наименование")]
        public String Name { get; set; }

        [Display(Name = "Тип")]
        public HardwareType HardwareType { get; set; }

        [Display(Name="Производитель")]
        public Firm Maker { get; set; }

        [Display(Name = "Ответственный специалист")]
        public Employee Master { get; set; }

        [Display(Name = "Инвентарный номер")]
        public String InventoryNumber { get; set; }
    }
}
