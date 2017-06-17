using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Merchant;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;
using Winner.Persistence.Linq;
using FileEntity = Beeant.Domain.Entities.Utility.FileEntity;

namespace Beeant.Domain.Services.Merchant
{
    public class MerchantOrderDomainService : RealizeDomainService<MerchantOrderEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
 

    

        #region 验证添加

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(MerchantOrderEntity info)
        {
            return ValidateOrder(info) && ValidateAccount(info) && ValidateSaleExist(info);
        }

        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(MerchantOrderEntity info)
        {
            var order = info.Order == null ? null
                           : info.Order.SaveType == SaveType.Add
                                 ? info.Order
                                 : Repository.Get<OrderEntity>(info.Order.Id);
            if (order == null)
            {
                info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(MerchantOrderEntity info)
        {
            var order = info.Account == null ? null
                           : info.Account.SaveType == SaveType.Add
                                 ? info.Account
                                 : Repository.Get<AccountEntity>(info.Account.Id);
            if (order == null)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证RoleAbility是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateSaleExist(MerchantOrderEntity info)
        {
            if (info.Order != null && info.Order.SaveType == SaveType.Add ||
                info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<MerchantOrderEntity>().Where(it => it.Order.Id == info.Order.Id
                                                      && it.Account.Id == info.Account.Id);
            var infos = Repository.GetEntities<MerchantOrderEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("Exist");
            return false;
        }
        #endregion


    }
}
