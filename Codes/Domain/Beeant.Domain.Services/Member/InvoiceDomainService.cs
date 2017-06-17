using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Member;
using Winner.Persistence;

namespace Beeant.Domain.Services.Member
{
    public class InvoiceDomainService : RealizeDomainService<InvoiceEntity>
    {
        
        #region 重写验证

        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(InvoiceEntity info)
        {
            return ValidateAccount(info); 
        }


        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(InvoiceEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                info.Account = Repository.Get<AccountEntity>(info.Account.Id);
                if (info.Account == null)
                {
                    info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                    return false;
                }
                if (!info.Account.IsUsed)
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

        #region 修改验证

        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(InvoiceEntity info)
        {
            return ValidateAccount(info); 
        }


        #endregion



        #endregion


    }
}
