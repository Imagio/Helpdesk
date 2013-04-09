using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imagio.Helpdesk.Model
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
