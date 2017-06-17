using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Account
{
    public class AccountDomainService : RealizeDomainService<AccountEntity>
    {

        /// <summary>
        /// 登录
        /// </summary>
        public IDomainService AccountIdentityDomainService { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public IDomainService AccountNumberDomainService { get; set; }
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
                        {"AccountIdentites", new UnitofworkHandle<AccountIdentityEntity>{DomainService= AccountIdentityDomainService}},
                        {"AccountNumbers", new UnitofworkHandle<AccountNumberEntity>{DomainService= AccountNumberDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }


        #region 重写事务

        /// <summary>
        /// 设置操作
        /// </summary>
        /// <param name="unitofworks"></param>
        /// <param name="info"></param>
        protected override bool SetBusiness(IList<IUnitofwork> unitofworks, AccountEntity info)
        {
            if (info.HasSaveProperty(it => it.Password) && !string.IsNullOrEmpty(info.Password))
               info.SetEncryptPassword();
            if (info.HasSaveProperty(it => it.Payword) && !string.IsNullOrEmpty(info.Payword))
                info.SetEncryptPayword();
            return base.SetBusiness(unitofworks,info);
        }

        #endregion

  

        #region 修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(AccountEntity info)
        {
            var dataEntity = Repository.Get<AccountEntity>(info.Id);
            return ValidateModifyName(info, dataEntity) && ValidateAccountBalance(info,dataEntity);
        }
 
        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateModifyName(AccountEntity info, AccountEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Name)) return true;
            if (dataEntity != null && dataEntity.Name == info.Name) return true;
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => it.Id != info.Id
                                               && it.Name == info.Name ).Select(it=>it.Id);
            var infos = Repository.GetEntities<AccountEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("NameExist");
                return false;
            }
            return true;
        }


        /// <summary>
        /// 验证账户余额
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccountBalance(AccountEntity info,AccountEntity dataEntity)
        {
            if (info.HasSaveProperty(it => it.Balance))
            {
             
                if (info.Balance < 0)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "NotEnoughBalance");
                    return false;
                }
            }
         
            return true;
        }

     
        #endregion

        #region 删除验证

        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(AccountEntity info)
        {
            info.AddError("AccountNotAllowRemove");
            return false;
        }

        #endregion
 
    }
}
