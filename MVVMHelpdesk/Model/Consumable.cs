using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Расходный материал
    /// </summary>
    public class Consumable : EntityModel, IEntity
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
        /// Артикул
        /// </summary>
        [Required]
        [Display(Name="Артикул")]
        public String Article { get; set; }

        /// <summary>
        /// Ответственный специалист
        /// </summary>
        [Display(Name="Ответственный специалист")]
        public Employee Master { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        [Display(Name="Производитель")]
        public Firm Maker { get; set; }

        /// <summary>
        /// Тип расходного материала
        /// </summary>
        [Display(Name="Тип")]
        public ConsumableType ConsumableType { get; set; }
    }
}
