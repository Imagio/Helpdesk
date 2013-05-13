using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model.Interfaces;

namespace Imagio.Helpdesk.Model
{
    public class CartridgeAccounting: EntityModel, IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = @"Дата регистрации")]
        [Required]
        [DefaultValue(null)]
        public DateTime? StartDate { get; set; }

        [Display(Name = @"Картридж")]
        [Required]
        public Cartridge Cartridge { get; set; }

        [Display(Name = @"Кабинет")]
        public String Room { get; set; }

        [Display(Name = @"Заправка")]
        public Boolean IsRefill { get; set; }

        [Display(Name = @"Фоторецептор")]
        public Boolean IsPhotoreceptor { get; set; }

        [Display(Name = @"Ролик")]
        public Boolean IsRoll { get; set; }

        [Display(Name = @"Дата исполнения")]
        [DefaultValue(null)]
        public DateTime? EndDate { get; set; }
    }
}
