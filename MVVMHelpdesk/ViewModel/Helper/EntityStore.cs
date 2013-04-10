using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Entity;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    internal static class EntityStore
    {
        delegate DbSet TableDelegate(HelpdeskContext context);

        private static Dictionary<Type, TableDelegate> _workspaceDictionary = new Dictionary<Type, TableDelegate> 
        { 
            { typeof(Software), context => context.Softwares },
            { typeof(Hardware), context => context.Hardwares },
            { typeof(Consumable), context => context.Consumables },
            { typeof(Cartridge), context => context.Cartridges}

        };

        public static Dictionary<Type, Type> _entityViewModelDictionary = new Dictionary<Type, Type>
        {
            { typeof(Software), typeof(SoftwareViewModel) }
        };

        public static DbSet Workspace<TE>(HelpdeskContext context)
        {
            Type type = typeof(TE);
            if (_workspaceDictionary.ContainsKey(type))
                return _workspaceDictionary[type](context);
            return null;
        }

        public static EntityViewModel<TE> ViewModel<TE>(TE model, HelpdeskContext context) where TE : class, IEntity
        {
            if (_entityViewModelDictionary.ContainsKey(typeof(TE)))
            {
                Type viewModelType = _entityViewModelDictionary[typeof(TE)];
                return viewModelType == null ? null : Activator.CreateInstance(viewModelType, model, context) as EntityViewModel<TE>;
            }
            return null;
        }
    }
}
