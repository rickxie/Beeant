using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Merchant;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Merchant
{
    public class CatalogueDomainService : RealizeDomainService<CatalogueEntity>
    {
        #region 加载
        private IDictionary<string, ItemLoader<CatalogueEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<CatalogueEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<CatalogueEntity>>
                    {
                        {"Parent", LoadParent}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
     

        protected virtual void LoadParent(CatalogueEntity info)
        {
            info.Parent = Repository.Get<CatalogueEntity>(info.Parent.Id) ?? new CatalogueEntity{Id = 0};
        }
        #endregion

        #region 验证
      
        #region 验证添加
        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateAdd(CatalogueEntity info)
        {
            return ValidateParent(info,null) && ValidateAccount(info);
        }
        #endregion

        #region 验证修改

        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(CatalogueEntity info)
        {
            var dataEntity = Repository.Get<CatalogueEntity>(info.Id);
            return ValidateParent(info, dataEntity) && ValidateBranch(info, dataEntity);
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(CatalogueEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                var account = Repository.Get<AccountEntity>(info.Account.Id);
                if (account == null)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                    return false;
                }
                if (!account.IsUsed)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "UnUsed");
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 验证支点
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        protected virtual bool ValidateBranch(CatalogueEntity info, CatalogueEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id)) return true;
            if (dataEntity.Parent != null && info.Parent.Id == dataEntity.Parent.Id) return true;
            var data = Repository.Get<CatalogueEntity>(info.Parent.Id);
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
                data = Repository.Get<CatalogueEntity>(data.Parent.Id);
            } while (data.Parent != null && dataEntity.Parent.Id!=0);
            return true;
        }
        #endregion

        #region 验证删除
        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(CatalogueEntity info)
        {
            return ValidateMenuLeaf(info);
        }


        /// <summary>
        /// 验证Menu是否是页节点
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateMenuLeaf(CatalogueEntity info)
        {
            var query = new QueryInfo();
            query.Query<CatalogueEntity>().Where(it => it.Parent.Id == info.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<CatalogueEntity>(query);
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
        protected virtual bool ValidateParent(CatalogueEntity info, CatalogueEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id) || info.Parent.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.Parent != null && dataEntity.Parent.Id == info.Parent.Id)
                return true;
            var parent = Repository.Get<CatalogueEntity>(info.Parent.Id);
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
