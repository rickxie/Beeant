using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Promotion;
using Winner.Persistence;

namespace Beeant.Domain.Services.Promotion
{
    public class CouponerDomainService : RealizeDomainService<CouponerEntity>
    {


        #region 重写验证

        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CouponerEntity info)
        {
            return ValidateAccount(info, null);
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(CouponerEntity info, CouponerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null || info.Account.Id==0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
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
        protected override bool ValidateModify(CouponerEntity info)
        {
            var dataEntity = Repository.Get<CouponerEntity>(info.Id);
            return ValidateAccount(info, dataEntity);
        }


        #endregion

   



        #endregion


    }
}
