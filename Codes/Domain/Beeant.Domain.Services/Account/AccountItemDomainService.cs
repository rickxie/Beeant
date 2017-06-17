using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Beeant.Domain.Entities;

namespace Beeant.Domain.Services.Account
 
{
    public class AccountItemDomainService : RealizeDomainService<AccountItemEntity>
    {
        public IDomainService AccountDomainService { get; set; }

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
                        {"Account", new UnitofworkHandle<AccountEntity>{DomainService = AccountDomainService}},
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<AccountItemEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<AccountItemEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<AccountItemEntity>>
                    {
                        {"Account", LoadAccount},
                        {"DataEntity", LoadDataEntity}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
     
        #region 重写加载

        /// <summary>
        /// 加载原始数据
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadAccount(AccountItemEntity info)
        {
            info.Account = Repository.Get<AccountEntity>(info.Account.Id);
        }
        /// <summary>
        /// 加载原始数据
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadDataEntity(AccountItemEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            info.DataEntity = Repository.Get<AccountItemEntity>(info.Id);
        }
        #endregion

        #region 验证添加
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateAdd(AccountItemEntity info)
        {
            return ValidateAccount(info);
        }
        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(AccountItemEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                var account= info.Account = Repository.Get<AccountEntity>(info.Account.Id);
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
                return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
     
 
        #endregion

 
    }
}
