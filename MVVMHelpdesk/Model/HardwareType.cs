using System;
using System.ComponentModel.DataAnnotations;
using Imagio.Helpdesk.Model.Interfaces;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Тип оборудования
    /// </summary>
    public class HardwareType : EntityModel, IEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        [Display(Name=@"Наименование")]
        public String Name { get; set; }
    }
}
