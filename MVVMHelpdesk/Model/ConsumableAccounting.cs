using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Imagio.Helpdesk.Model.Interfaces;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Учет расходных материалов
    /// </summary>
    public class ConsumableAccounting: EntityModel, IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [Display(Name = @"Дата")]
        [Required]
        [DefaultValue(null)]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Расходный материал
        /// </summary>
        [Display(Name = @"Расходный материал")]
        [Required]
        public Consumable Consumable { get; set; }

        /// <summary>
        /// Ответственный специалист
        /// </summary>
        [Display(Name = @"Ответственный специалист")]
        public Employee Employee { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        [Display(Name = @"Количество")]
        public Int32 Count { get; set; }

        /// <summary>
        /// Приход/расход 
        /// TRUE - приход на склад
        /// FALSE - расход
        /// </summary>
        [Display(Name = @"Приход/Расход")]
        public bool Sign { get; set; }
    }
}
