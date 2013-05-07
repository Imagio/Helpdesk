using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Imagio.Helpdesk.Model;
using Imagio.Helpdesk.Model.Interfaces;
using Imagio.Helpdesk.ViewModel.Entity;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    internal static class EntityStore
    {
        #region ViewModel
        /*
        private static Dictionary<Type, Type> _entityViewModelDictionary = new Dictionary<Type, Type>
        {
            { typeof(Account), typeof(AccountViewModel) },
            { typeof(Cartridge), typeof(CartridgeViewModel) },
            { typeof(CartridgeType), typeof(CartridgeTypeViewModel) },
            { typeof(Consumable), typeof(ConsumableViewModel) },
            { typeof(ConsumableType), typeof(ConsumableTypeViewModel) },
            { typeof(Employee), typeof(EmployeeViewModel) },
            { typeof(Firm), typeof(FirmViewModel) },
            { typeof(Hardware), typeof(EntityViewModel<Hardware>) },
            { typeof(HardwareType), typeof(HardwareTypeViewModel) },
            { typeof(Software), typeof(SoftwareViewModel) },
            { typeof(ConsumableAccounting), typeof(ConsumableAccountingViewModel) }

        };*/

        public static EntityViewModel<TE> ViewModel<TE>(TE model, HelpdeskContext context) where TE : class, IEntity
        {
            return new EntityViewModel<TE>(model, context);
            /*
            if (_entityViewModelDictionary.ContainsKey(typeof(TE)))
            {
                Type viewModelType = EntityViewModel<TE>; //_entityViewModelDictionary[typeof(TE)];
                return viewModelType == null ? null : Activator.CreateInstance(viewModelType, model, context) as EntityViewModel<TE>;
            }
            return null;*/
        }

        #endregion

        #region Query

        private delegate IQueryable QueryDelegate(HelpdeskContext context);

        private static readonly Dictionary<Type, QueryDelegate> EntityQuery = new Dictionary<Type,QueryDelegate>
        {
            { typeof(Account), o => o.Set<Account>() },
            { typeof(Cartridge), o => o.Set<Cartridge>().Include(i => i.Maker).Include(i => i.Master).Include(i => i.CartridgeType) },
            { typeof(CartridgeType), o => o.Set<CartridgeType>() },
            { typeof(Consumable), o => o.Set<Consumable>().Include(i => i.Master).Include(i => i.Maker).Include(i => i.ConsumableType) },
            { typeof(ConsumableType), o => o.Set<ConsumableType>() },
            { typeof(Employee), o => o.Set<Employee>() },
            { typeof(Firm), o => o.Set<Firm>() },
            { typeof(Hardware), o => o.Set<Hardware>().Include(i => i.Maker).Include(i => i.Master) },
            { typeof(HardwareType), o => o.Set<HardwareType>() },
            { typeof(Software), o => o.Set<Software>().Include(i => i.Maker).Include(i => i.Master) },
            { typeof(ConsumableAccounting), o => o.Set<ConsumableAccounting>().Include(i => i.Consumable).Include(i => i.Employee)},
            { typeof(CartridgeAccounting), o => o.Set<CartridgeAccounting>().Include(i => i.Cartridge)}
        };

        public static IQueryable<Object> GetEntityQuery(HelpdeskContext context, Type type)
        {
            if (EntityQuery.ContainsKey(type))
            {
                return EntityQuery[type].Invoke(context) as IQueryable<Object>;
            }
            return context.Set(type) as IQueryable<Object>;
        }

        public static IQueryable<TE> GetEntityQuery<TE>(HelpdeskContext context) where TE: class, IEntity
        {
            if (EntityQuery.ContainsKey(typeof(TE)))
            {
                return EntityQuery[typeof(TE)].Invoke(context) as IQueryable<TE>;
            }
            return context.Set<TE>();
        }

        #endregion
    }
}
