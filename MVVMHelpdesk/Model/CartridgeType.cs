﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Тип картриджа
    /// </summary>
    public class CartridgeType
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Required]
        public String Name { get; set; }
    }
}