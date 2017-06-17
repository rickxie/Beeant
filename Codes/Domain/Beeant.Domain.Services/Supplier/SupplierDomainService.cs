using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Supplier;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Supplier
{
    public class SupplierDomainService : RealizeDomainService<SupplierEntity>
    {

        #region 重写事务
        private IDictionary<string, ItemLoader<SupplierEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<SupplierEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<SupplierEntity>>
                    {
                        {"Service", LoadService}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadService(SupplierEntity info)
        {
            info.Service = Repository.Get<UserEntity>(info.Service.Id);
        }
        #endregion

        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(SupplierEntity info)
        {
            var rev = ValidateUser(info, null) && ValidateAccount(info, null) && ValidateService(info, null) && ValidateName(info,null);
            return rev;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(SupplierEntity info)
        {
            var dataEntity = Repository.Get<SupplierEntity>(info.Id);
            return ValidateUser(info, dataEntity) && ValidateAccount(info, dataEntity) && ValidateService(info, dataEntity)
                && ValidateName(info,dataEntity);
        }

        #endregion

        #region 删除验证

        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(SupplierEntity info)
        {
            var dataEntity = Repository.Get<SupplierEntity>(info.Id);
            return ValidateSupplierAccount(info, dataEntity);
        }
        /// <summary>
        /// 验证是否存在账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateSupplierAccount(SupplierEntity info,SupplierEntity dataEntity)
        {
            
            if (dataEntity.Account != null && dataEntity.Account.Id != 0)
            {
                info.AddError("OpenAccountNotAllowRemove");
                return false;
            }

            return true;
        }
        

        #endregion

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(SupplierEntity info ,SupplierEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null || info.Account.Id==0)
                return true;
            if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id) 
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            var account = Repository.Get<AccountEntity>(info.Account.Id);
            if (account == null)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                return false;
            }
            var query = new QueryInfo();
            query.Query<SupplierEntity>().Where(it => it.Account.Id == info.Account.Id);
            var infos = Repository.GetEntities<SupplierEntity>(query);
            if (infos != null && infos.Count(it => it.Id != info.Id) > 0)
            {
                info.AddError("AccountHasSupplier");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateUser(SupplierEntity info, SupplierEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Service.Id))
                return true;
            if (dataEntity != null && dataEntity.Service.Id == info.Service.Id)
                return true;
            var user = Repository.Get<UserEntity>(info.Service.Id);
            if (user == null)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证客服
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateService(SupplierEntity info, SupplierEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.ServiceId) || info.ServiceId == 0)
                return true;
            if (dataEntity != null && dataEntity.ServiceId == info.ServiceId)
                return true;
            var user = Repository.Get<UserEntity>(info.ServiceId);
            if (user == null)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "NoExist");
                return false;
            }
            if (!user.IsUsed)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "UnUsed");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证客服
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateName(SupplierEntity info, SupplierEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Name))
                return true;
            if (dataEntity != null && dataEntity.Name == info.Name)
                return true;
            var query = new QueryInfo();
            query.SetPageSize(1).Query<SupplierEntity>().Where(it => it.Name == info.Name).Select(it => it.Name);
            var infos = Repository.GetEntities<SupplierEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("NameExists");
                return false;
            }
            return true;
        }
        #endregion

    }
}
