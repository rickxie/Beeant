using System.Collections.Generic;
using Beeant.Domain.Entities.Authority;
using Winner.Persistence;

namespace Beeant.Domain.Services.Authority
{
    public class AbilityDomainService : RealizeDomainService<AbilityEntity>
    {


        /// <summary>
        /// 资源明细
        /// </summary>
        public IDomainService ResourceDomainService { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public IDomainService RoleAbilityDomainService { get; set; }

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
                        {"Resources", new UnitofworkHandle<ResourceEntity>{DomainService= ResourceDomainService}},
                        {"RoleAbilities", new UnitofworkHandle<RoleAbilityEntity>{DomainService= RoleAbilityDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }


        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(AbilityEntity info)
        {
            return ValidateMenu(info); 
        }
    

        /// <summary>
        /// 验证集成商类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateMenu(AbilityEntity info)
        {
            if (!info.HasSaveProperty(it => it.Menu.Id))
                return true;
            if (info.Menu != null && info.Menu.SaveType == SaveType.Add)
                return true;
            if (info.Menu != null && info.Menu.Id!=0)
            {
                var mainEntity = Repository.Get<MenuEntity>(info.Menu.Id);
                if (mainEntity != null)
                    return true;
            }
            info.AddErrorByName(typeof(MenuEntity).FullName, "NoExist");
            return false;
        }
     
        #endregion

    }
}
