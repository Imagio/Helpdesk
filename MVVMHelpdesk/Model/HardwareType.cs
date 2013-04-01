using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Тип оборудования
    /// </summary>
    internal class HardwareType
    {
        [Key]
        [Required]
        public Guid ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        public String Name { get; set; }
    }
}
