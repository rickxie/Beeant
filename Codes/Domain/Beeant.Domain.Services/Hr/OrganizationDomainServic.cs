using Beeant.Domain.Entities.Hr;
using Winner.Persistence;

namespace Beeant.Domain.Services.Hr
{
    public class OrganizationDomainService : RealizeDomainService<OrganizationEntity>
    {
       
        
 
  
        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrganizationEntity info)
        {
            var rev = ValidateHr(info);
            return rev;
        }


        #endregion



        /// <summary>
        /// 验证部门
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateHr(OrganizationEntity info)
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


        #endregion

    }
}
