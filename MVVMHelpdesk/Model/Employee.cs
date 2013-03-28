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
    internal class Employee
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public String FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Required]
        public String SecondName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public String LastName { get; set; }
    }
}
