﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.ViewModel.Entity;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    internal static class EntityStore
    {
        #region ViewModel

        private static Dictionary<Type, Type> _entityViewModelDictionary = new Dictionary<Type, Type>
        {
            { typeof(Account), typeof(AccountViewModel) },
            { typeof(Cartridge), typeof(EntityViewModel<Cartridge>) },
            { typeof(CartridgeType), typeof(EntityViewModel<CartridgeType>) },
            { typeof(Consumable), typeof(EntityViewModel<Consumable>) },
            { typeof(ConsumableType), typeof(EntityViewModel<ConsumableType>) },
            { typeof(Employee), typeof(EmployeeViewModel) },
            { typeof(Firm), typeof(FirmViewModel) },
            { typeof(Hardware), typeof(EntityViewModel<Hardware>)},
            { typeof(HardwareType), typeof(EntityViewModel<HardwareType>) },
            { typeof(Software), typeof(SoftwareViewModel) }

        };

        public static EntityViewModel<TE> ViewModel<TE>(TE model, HelpdeskContext context) where TE : class, IEntity
        {
            if (_entityViewModelDictionary.ContainsKey(typeof(TE)))
            {
                Type viewModelType = _entityViewModelDictionary[typeof(TE)];
                return viewModelType == null ? null : Activator.CreateInstance(viewModelType, model, context) as EntityViewModel<TE>;
            }
            return null;
        }

        #endregion

        #region Query

        private delegate IQueryable QueryDelegate(HelpdeskContext context);

        private static Dictionary<Type, QueryDelegate> _entityQuery = new Dictionary<Type,QueryDelegate>
        {
            { typeof(Account), o => o.Set<Account>() },
            { typeof(Cartridge), o => o.Set<Cartridge>().Include(i => i.Maker).Include(i => i.Master).Include(i => i.CartridgeType) },
            { typeof(CartridgeType), o => o.Set<CartridgeType>() },
            { typeof(Consumable), o => o.Set<Consumable>().Include(i => i.Master).Include(i => i.Maker).Include(i => i.ConsumableType) },
            { typeof(ConsumableType), o => o.Set<ConsumableType>() },
            { typeof(Employee), o => o.Set<Employee>() },
            { typeof(Firm), o => o.Set<Firm>() },
            { typeof(Hardware), o => o.Set<Hardware>() },
            { typeof(HardwareType), o => o.Set<HardwareType>() },
            { typeof(Software), o => o.Set<Software>().Include(i => i.Maker).Include(i => i.Master) }
        };

        public static IQueryable<TE> EntityQuery<TE>(HelpdeskContext context) where TE: class, IEntity
        {
            if (_entityQuery.ContainsKey(typeof(TE)))
            {
                return _entityQuery[typeof(TE)].Invoke(context) as IQueryable<TE>;
            }
            return context.Set<TE>();
        }

        #endregion
    }
}
