using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Imagio.Helpdesk.Model.Interfaces;

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
        [Display(Name = @"Наименование")]
        public String Name { get; set; }

        [Display(Name = @"Тип")]
        public HardwareType HardwareType { get; set; }

        [Display(Name = @"Производитель")]
        public Firm Maker { get; set; }

        [Display(Name = @"Ответственный специалист")]
        public Employee Master { get; set; }

        [Display(Name = @"Инвентарный номер")]
        public String InventoryNumber { get; set; }

        [Display(Name = @"Дата постановки на учет")]
        [DefaultValue(null)]
        public DateTime? StartDate { get; set; }

        [Display(Name = @"Дата списания")]
        [DefaultValue(null)]
        public DateTime? EndDate { get; set; }

        [NotMapped]
        public bool IsUsing
        {
            get
            {
                return EndDate == null;
            }
        }
    }
}
