﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Imagio.Helpdesk.Model.Interfaces;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Аккаунт пользователя
    /// </summary>
    public class Account: EntityModel, IEntity
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required]
        [StringLength(20)]
        [Display(Name=@"Имя пользователя")]
        public String Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [Display(Name=@"Пароль")]
        public Byte[] Password { get; set; }

        /// <summary>
        /// Активная учетная запись
        /// </summary>
        [Required]
        [Display(Name=@"Активная учетная запись")]
        [DefaultValue(true)]
        public Boolean IsActive { get; set; }

        /// <summary>
        /// Строковое представление хэша пароля
        /// </summary>
        [NotMapped]
        public String PasswordString
        {
            get
            {
                var sb = new StringBuilder("*");
                foreach (var b in Password)
                {
                    sb.AppendFormat("{0:x2}", b);
                }
                return sb.ToString().ToUpper();
            }
        }
    }
}
