using System;
using System.ComponentModel.DataAnnotations;
using Imagio.Helpdesk.Model.Interfaces;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Тип расходного материала
    /// </summary>
    public class ConsumableType : EntityModel, IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        [Display(Name=@"Наименование")]
        public String Name { get; set; }
    }
}
