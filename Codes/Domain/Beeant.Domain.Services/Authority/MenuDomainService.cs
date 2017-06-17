using System.Collections.Generic;
using Beeant.Domain.Entities.Authority;
using System.Linq;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Authority
{
    public class MenuDomainService : RealizeDomainService<MenuEntity>
    {




        /// <summary>
        /// 订单明细
        /// </summary>
        public IDomainService AbilityDomainService { get; set; }



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
                        {"Abilities", new UnitofworkHandle<AbilityEntity>{DomainService= AbilityDomainService}},
                        {"Children", new UnitofworkHandle<MenuEntity>{DomainService= this}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        #region 验证

        #region 验证添加
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(MenuEntity info)
        {
            return ValidateParent(info,null) && ValidateMenuType(info,null);  
        }
        #endregion

        #region 验证修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(MenuEntity info)
        {
            var dataEntity = Repository.Get<MenuEntity>(info.Id);
            return ValidateParent(info, dataEntity) && ValidateMenuType(info, dataEntity) && ValidateBranch(info, dataEntity);
        }
        /// <summary>
        /// 验证支点
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        protected virtual bool ValidateBranch(MenuEntity info, MenuEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id)) return true;
            if (dataEntity.Parent != null && info.Parent.Id == dataEntity.Parent.Id) return true;
            var data = Repository.Get<MenuEntity>(info.Parent.Id);
            do
            {
                if (data == null) break;
                if (data.Id == info.Id || data.Parent != null && data.Parent.Id == info.Id)
                {
                    info.AddError("NotAllowParent");
                    return false;
                }
                if (data.Parent == null || data.Parent.Id==0)
                    break;
                data = Repository.Get<MenuEntity>(data.Parent.Id);
            } while (data.Parent != null && dataEntity.Parent.Id!=0);
            return true;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(MenuEntity info)
        {
            return ValidateMenuLeaf(info); 
        }
        /// <summary>
        /// 验证菜单类型
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateMenuTypeExist(MenuEntity info, MenuEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Subsystem.Id))
                return true;
            if (info.Subsystem != null && info.Subsystem.SaveType == SaveType.Add)
                return true;
            if (info.Subsystem != null && info.Subsystem.Id!=0)
            {
                if (dataEntity != null && dataEntity.Subsystem != null && dataEntity.Subsystem.Id == info.Subsystem.Id)
                    return true;
                if (Repository.Get<SubsystemEntity>(info.Subsystem.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(SubsystemEntity).FullName, "NoExist");
            return false;
        }
     
        /// <summary>
        /// 验证类型
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateMenuType(MenuEntity info,MenuEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Subsystem.Id)) return true;
            if (!info.HasSaveProperty(it => it.Parent.Id) && dataEntity!=null)
                info.Parent = dataEntity.Parent;
            if (dataEntity != null && dataEntity.Parent != null && info.Parent != null &&
                info.Parent.Id == dataEntity.Parent.Id)
                return true;
            if (info.Parent == null || info.Parent.Id==0) return true;
            var parent = Repository.Get<MenuEntity>(info.Parent.Id);
            if (parent == null || parent.Subsystem.Id.Equals(info.Subsystem.Id))
                return true;
            info.AddError("InconformityMenuType");
            return false;
        }
        /// <summary>
        /// 验证Menu是否是页节点
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateMenuLeaf(MenuEntity info)
        {
            var query = new QueryInfo();
            query.Query<MenuEntity>().Where(it => it.Parent.Id == info.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<MenuEntity>(query);
            if (infos != null && infos.Count == 0) return true;
            info.AddError("ExistChild");
            return false;
        }
        #endregion

        /// <summary>
        /// 验证父类
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateParent(MenuEntity info, MenuEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id) || info.Parent.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.Parent != null && dataEntity.Parent.Id == info.Parent.Id)
                return true;
            var parent = Repository.Get<MenuEntity>(info.Parent.Id);
            if (parent == null)
            {
                info.AddError("NoParent");
                return false;
            }
            return true;
        }
        #endregion


    }
}
