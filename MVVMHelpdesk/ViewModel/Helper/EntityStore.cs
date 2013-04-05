using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    internal static class EntityStore
    {
        delegate DbSet TableDelegate(HelpdeskContext context);

        private static Dictionary<Type, TableDelegate> _workspaceDictionary = new Dictionary<Type, TableDelegate> 
        { 
            { typeof(Software), o => o.Softwares }
        };

        public static DbSet Workspace<TE>(HelpdeskContext context)
        {
            Type type = typeof(TE);
            if (_workspaceDictionary.ContainsKey(type))
                return _workspaceDictionary[type](context);
            return null;
        }
    }
}
