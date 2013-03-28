using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}
