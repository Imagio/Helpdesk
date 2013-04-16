﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employee : EntityModel, IEntity
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

        [NotMapped]
        public string FullName
        {
            get { return String.Format("{0} {1} {2}", LastName, FirstName, SecondName); }
        }

        [NotMapped]
        public string ShortName
        {
            get
            {
                String fName = String.IsNullOrEmpty(FirstName) ? "" : String.Format("{0}.", FirstName[0]);
                String sName = String.IsNullOrEmpty(SecondName) ? "" : String.Format("{0}.", SecondName[0]);
                return String.Format("{0} {1}{2}", LastName, fName, sName);
            }
        }
    }
}
