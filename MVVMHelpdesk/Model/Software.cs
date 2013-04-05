using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Программное обеспечение
    /// </summary>
    public class Software
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        [Display(Name="Наименование")]
        public String Name { get; set; }

        /// <summary>
        /// Ответственный специалист
        /// </summary>
        [Display(Name="Ответственный специалист")]
        public Employee Master { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        [Display(Name = "Производитель")]
        public Firm Maker { get; set; }
    }
}
