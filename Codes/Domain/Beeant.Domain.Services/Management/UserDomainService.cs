using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;

namespace Beeant.Domain.Services.Management
{
    public class UserDomainService : RealizeDomainService<UserEntity>
    {

        /// <summary>
        /// 服务实例
        /// </summary>
        public virtual IDomainService AccountDomainService { get; set; }
        public IDomainService RoleAccountDomainService { set; get; }
        public IDomainService OwnerAccountDomainService { set; get; }
        public IDomainService GroupAccountDomainService { set; get; }
        public IDomainService AuditorAccountDomainService { set; get; }
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
                    {"Account", new UnitofworkHandle<AccountEntity> {DomainService = AccountDomainService}},
                    {"RoleAccounts", new UnitofworkHandle<RoleAccountEntity> {DomainService = RoleAccountDomainService}},
                    {
                        "OwnerAccounts",
                        new UnitofworkHandle<OwnerAccountEntity> {DomainService = OwnerAccountDomainService}
                    },
                    {
                        "GroupAccounts",
                        new UnitofworkHandle<OwnerAccountEntity> {DomainService = GroupAccountDomainService}
                    },
                    {
                        "AuditorAccounts",
                        new UnitofworkHandle<OwnerAccountEntity> {DomainService = AuditorAccountDomainService}
                    }
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
        protected override bool ValidateAdd(UserEntity info)
        {
            var rev = ValidateAccount(info);
            return rev;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(UserEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null ||
                info.Account.Id == 0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
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
            return true;
        }

        #endregion




    }
}
