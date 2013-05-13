using System;
using System.ComponentModel.DataAnnotations;
using Imagio.Helpdesk.Model.Interfaces;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Тип картриджа
    /// </summary>
    public class CartridgeType : EntityModel, IEntity
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
