using System;
using System.ComponentModel.DataAnnotations;
using Imagio.Helpdesk.Model.Interfaces;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Производитель
    /// </summary>
    public class Firm : EntityModel, IEntity
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
