using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employee: IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [Display(Name="Имя")]
        public String FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Required]
        [Display(Name="Отчество")]
        public String SecondName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        [Display(Name="Фамилия")]
        public String LastName { get; set; }
    }
}
