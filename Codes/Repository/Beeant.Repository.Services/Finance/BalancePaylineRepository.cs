using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Winner;
using Winner.Persistence;

namespace Beeant.Repository.Services.Finance
{
    public class BalancePaylineRepository : PaylineRepository
    {
  
        #region 创建

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Create(PaylineEntity info)
        {
            if (info == null || info.Account == null)
                return false;
            var account = Creator.Get<IContext>().Get<AccountEntity>(info.Account.Id);
            if (account == null || account.Balance < info.Amount)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "BalanceEnough");
                return false;
            }
            info.AccountItem = new AccountItemEntity
            {
                Account = account,
                Amount = 0 - info.Amount,
                Data = info,
                Name = info.TypeName,
                Remark = info.Remark,
                SaveType = SaveType.Add,
            };
            info.Forms = null;
            info.Request ="";
            info.OutNumber =info.Number;
            info.Status = PaylineStatusType.Success;
            info.Response = "";
            info.Amount = 0;
            return true;
        }



        #endregion

        #region 处理

        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        public override PaylineEntity Process()
        {
            return null;
        }




        #endregion

        #region 退款
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Refund(PaylineEntity info)
        {
            var account = Creator.Get<IContext>().Get<AccountEntity>(info.Account.Id);
            if (account == null)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                return false;
            }
            info.AccountItem = new AccountItemEntity
            {
                Account = account,
                Amount =info.Amount,
                Data = info,
                Name = info.TypeName,
                Remark = info.Remark,
                SaveType = SaveType.Add,
            };
            info.Forms = null;
            info.Request = "";
            info.OutNumber = info.Number;
            info.Status = PaylineStatusType.Success;
            info.Response = "";
            info.Amount = 0;
            return true;
        }

        #endregion



    }
}
