using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Authority
{
    public class OwnerDomainService : RealizeDomainService<OwnerEntity>
    {


       
        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OwnerEntity info)
        {
            return  ValidateName(info,null) && ValidateAccount(info); 
        }
        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(OwnerEntity info)
        {
            var dataEntity = Repository.Get<OwnerEntity>(info.Id);
            return ValidateName(info,dataEntity);
        }


        /// <summary>
        /// 验证功能
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateName(OwnerEntity info, OwnerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Name))
                return true;
            if (dataEntity != null && dataEntity.Name == info.Name)
                return true;
            var query=new QueryInfo();
            query.Query<OwnerEntity>().Where(it => it.Name == info.Name && it.Id!=info.Id).Select(it => it.Id);
            var infos = Repository.GetEntities<OwnerEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("NameExists");
                return false;
            }
            return false;
        }
    
        /// <summary>
        /// 验证角色
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(OwnerEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && (info.Account.SaveType == SaveType.Add || info.Account.Id == 0))
                return true;
            if (info.Account != null && info.Account.Id != 0)
            {
                if (Repository.Get<AccountEntity>(info.Account.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
        #endregion


    }
}
