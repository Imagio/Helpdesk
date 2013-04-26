using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Картридж
    /// </summary>
    public class Cartridge : EntityModel, IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        [Display(Name="Наименование")]
        public String Name { get; set; }

        /// <summary>
        /// Тип картриджа
        /// </summary>
        [Display(Name="Тип")]
        public CartridgeType CartridgeType { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        [Display(Name="Производитель")]
        public Firm Maker { get; set; }

        /// <summary>
        /// Ответственный специалист
        /// </summary>
        [Display(Name="Ответственный специалист")]
        public Employee Master { get; set; }

        [Display(Name = "Инвентарный номер")]
        public string InventoryNumber { get; set; }
    }
}
