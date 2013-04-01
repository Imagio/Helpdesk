using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Производитель
    /// </summary>
    internal class Firm
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        public String Nam { get; set; }
    }
}
