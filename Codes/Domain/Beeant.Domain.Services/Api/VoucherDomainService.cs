using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Api;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Api
{
    public class VoucherDomainService : RealizeDomainService<VoucherEntity> 
    {
     

        #region 重写验证
 
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(VoucherEntity info)
        {
            return ValidateAccount(info, null) && ValidateExist(info, null) && ValidateToken(info, null);
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(VoucherEntity info)
        {
            var dataEntity = Repository.Get<VoucherEntity>(info.Id);
            return ValidateAccount(info, dataEntity) && ValidateExist(info,dataEntity) && ValidateToken(info,dataEntity);
        }
        /// <summary>
        /// 验证功能
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateToken(VoucherEntity info, VoucherEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Token))
                return true;
            if (dataEntity != null && dataEntity.Token == info.Token)
                return true;
            var query=new QueryInfo();
            query.SetPageSize(1)
                .Query<VoucherEntity>()
                .Where(it => it.Token == info.Token && it.Id != info.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<VoucherEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "TokenExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证功能
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(VoucherEntity info, VoucherEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id != 0)
            {
                if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
                    return true;
                if (Repository.Get<AccountEntity>(info.Account.Id) != null)
                {
                    return true;
                }
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(VoucherEntity info, VoucherEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account.Id==0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
                return true;
            var query = new QueryInfo();
            query.Query<VoucherEntity>()
                 .Where(it => it.Account.Id == info.Account.Id && it.Id != info.Id)
                 .Select(it => it.Id);
            var infos = Repository.GetEntities<VoucherEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("AccountHasVoucher");
                return false;
            }
            return true;
        }
        #endregion

        
    }
}
