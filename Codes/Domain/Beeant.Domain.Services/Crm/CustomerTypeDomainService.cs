using System.Linq;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Crm
{
    public class CustomerTypeDomainService : RealizeDomainService<CustomerTypeEntity>
    {


        #region 重写验证


        #region 删除验证
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CustomerTypeEntity info)
        {
            return ValidateCrm(info);
        }

        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(CustomerTypeEntity info)
        {
            return ValidateCustomers(info) ;
        }
        /// <summary>
        /// 验证是否有客户存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCustomers(CustomerTypeEntity info)
        {
            var query = new QueryInfo();
            query.SetPageIndex(0).SetPageSize(1).Query<CustomerEntity>().Where(it => it.Type.Id == info.Id).Select(it=>it.Id);
            var infos = Repository.GetEntities<CustomerEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("HasCustomerNotAllowRemove");
                return false;
            }
            return true;
        }

        #endregion
        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="info"></param>

        /// <returns></returns>
        protected virtual bool ValidateCrm(CustomerTypeEntity info)
        {
            if (!info.HasSaveProperty(it => it.Crm.Id) || info.Crm == null || info.Crm.Id == 0)
                return true;
            if (info.Crm != null && info.Crm.SaveType == SaveType.Add)
                return true;
            var crm = Repository.Get<CrmEntity>(info.Crm.Id);
            if (crm == null)
            {
                info.AddErrorByName(typeof(CrmEntity).FullName, "NoExist");
                return false;
            }
          

            return true;
        }
        #endregion

    }
}
