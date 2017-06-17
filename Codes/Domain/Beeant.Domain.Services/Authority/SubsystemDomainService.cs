using System.Collections.Generic;
using Beeant.Domain.Entities.Authority;

namespace Beeant.Domain.Services.Authority
{
    public class SubsystemDomainService : RealizeDomainService<SubsystemEntity>
    {

        /// <summary>
        /// 订单明细
        /// </summary>
        public IDomainService MemuDomainService { get; set; }
      


        private IDictionary<string, IUnitofworkHandle> _itemHandles;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, IUnitofworkHandle> ItemHandles
        {
            get
            {
                return _itemHandles ?? (_itemHandles = new Dictionary<string, IUnitofworkHandle>
                    {
                        {"Menus", new UnitofworkHandle<MenuEntity>{DomainService= MemuDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
    }
}
