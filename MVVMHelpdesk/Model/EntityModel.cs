using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    public abstract class EntityModel: IDataErrorInfo
    {
        [NotMapped]
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        [NotMapped]
        public string this[string columnName]
        {
            get 
            { 
                var prop = GetType().GetProperty(columnName);
                var validationMap = prop
                    .GetCustomAttributes(typeof(ValidationAttribute), true)
                    .Cast<ValidationAttribute>();
                foreach(var v in validationMap) {
                    try {
                        v.Validate(prop.GetValue(this, null), columnName);
                    } catch (Exception ex) {
                        return ex.Message;
                    }
                }
                
                return null;
            }
        }
    }
}
