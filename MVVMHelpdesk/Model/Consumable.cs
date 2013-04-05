using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Расходный материал
    /// </summary>
    public class Consumable
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        public String Name { get; set; }

        /// <summary>
        /// Артикул
        /// </summary>
        [Required]
        public String Article { get; set; }

        /// <summary>
        /// Ответственный специалист
        /// </summary>
        public Employee Master { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        public Firm Maker { get; set; }

        /// <summary>
        /// Тип расходного материала
        /// </summary>
        public ConsumableType ConsumableType { get; set; }
    }
}
