﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    /// <summary>
    /// Аккаунт пользователя
    /// </summary>
    internal class Account
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required]
        [StringLength(20)]
        public String Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public Byte[] Password { get; set; }

        /// <summary>
        /// Активная учетная запись
        /// </summary>
        [Required]
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
