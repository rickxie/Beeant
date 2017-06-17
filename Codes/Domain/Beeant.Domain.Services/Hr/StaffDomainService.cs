using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Hr;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Hr
{
    public class StaffDomainService : RealizeDomainService<StaffEntity>
    {
        /// <summary>
        /// 发票
        /// </summary>
        public IDomainService FamilyDomainService { get; set; }
        /// <summary>
        /// 发票
        /// </summary>
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
                        {"Families", new UnitofworkHandle<FamilyEntity>{DomainService= FamilyDomainService}},
                         {"Account", new UnitofworkHandle<AccountEntity>{DomainService= AccountDomainService,IsAppend=false}}

                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
      
     
        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(StaffEntity info)
        {
            var rev = ValidateHr(info) && ValidateAccount(info) && ValidateAccountExist(info,null);
            return rev;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(StaffEntity info)
        {
            var dataEntity = Repository.Get<StaffEntity>(info.Id);
            return ValidateAccountExist(info, dataEntity);
        }

        #endregion

  
 
        /// <summary>
        /// 验证部门
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateHr(StaffEntity info)
        {
            
            if (!info.HasSaveProperty(it => it.Hr.Id))
                return true;
            if (info.Hr.SaveType == SaveType.Add)
                return true;
            var hr = Repository.Get<HrEntity>(info.Hr.Id);
            if (hr == null)
            {
                info.AddErrorByName(typeof(HrEntity).FullName, "NoExist");
                return false;
            }
            if (!hr.IsUsed)
            {
                info.AddErrorByName(typeof(HrEntity).FullName, "UnUsed");
                return false;
            }
            return true;
            
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(StaffEntity info)
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

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccountExist(StaffEntity info,StaffEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account.SaveType == SaveType.Add)
                return true;
            if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
                return true;
            var query = new QueryInfo();
            query.Query<StaffEntity>().Where(it => it.Account.Id == info.Account.Id).Select(it => it.Id);
            var infos = Repository.GetEntities<StaffEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("AccountHasStaff");
                return false;
            }
            return true;
        }
        #endregion

    }
}
