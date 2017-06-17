using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Crm;
using Beeant.Domain.Entities.Hr;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Crm
{
    public class CustomerDomainService : RealizeDomainService<CustomerEntity>
    {
        private IDictionary<string, ItemLoader<CustomerEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<CustomerEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<CustomerEntity>>
                    {
                        {"Type", LoadType}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadType(CustomerEntity info)
        {
            info.Type = info.Type.SaveType == SaveType.Add ? info.Type : Repository.Get<CustomerTypeEntity>(info.Type.Id);

        }


        #region 重写验证

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CustomerEntity info)
        {
            var rev = ValidateCrm(info) && ValidateExist(info,null)&& ValidateType(info, null) && ValidateChannel(info,null) && ValidateStaff(info,null);
            return rev;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(CustomerEntity info)
        {
            var dataEntity = Repository.Get<CustomerEntity>(info.Id);
            return ValidateExist(info, dataEntity) && ValidateType(info, dataEntity) && ValidateChannel(info, dataEntity) && ValidateStaff(info, dataEntity);
        }

        #endregion
        /// <summary>
        /// 验证客户类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCrm(CustomerEntity info)
        {
            if (!info.HasSaveProperty(it => it.Crm.Id))
                return true;
            if (info.Crm.SaveType == SaveType.Add)
                return true;
            var crm = Repository.Get<CrmEntity>(info.Crm.Id);
            if (crm == null)
            {
                info.AddErrorByName(typeof(CrmEntity).FullName, "NoExist");
                return false;
            }
            return true;

          
        }
        /// <summary>
        /// 验证客户类型
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(CustomerEntity info, CustomerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Name) && !info.HasSaveProperty(it => it.Mobile))
                return true;
            var name = info.HasSaveProperty(it => it.Name) || dataEntity == null ? info.Name : dataEntity.Name;
            var mobile = info.HasSaveProperty(it => it.Mobile) || dataEntity == null ? info.Mobile : dataEntity.Mobile;
            if (dataEntity != null && dataEntity.Name == name && dataEntity.Mobile == mobile)
                return true;
            var crmId = info.Crm == null ? 0 : info.Crm.Id;
            var id = dataEntity == null ? 0 : dataEntity.Id;
            var query=new QueryInfo();
            query.Query<CustomerEntity>()
                .Where(it => it.Crm.Id == crmId && (it.Name == info.Name || it.Mobile == info.Mobile) && it.Id != id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<CustomerEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddErrorByName(typeof(CustomerEntity).FullName, "Exist");
                return false;
            }
            return true;
           
        }

        /// <summary>
        /// 验证客户类型
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateType(CustomerEntity info, CustomerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Type.Id))
                return true;
            if (info.Type != null && info.Type.SaveType == SaveType.Add)
                return true;
            if (info.Type != null && info.Type.Id!=0)
            {
                if (dataEntity != null && dataEntity.Type != null && dataEntity.Type.Id == info.Type.Id)
                    return true;
                info.Type = Repository.Get<CustomerTypeEntity>(info.Type.Id);
                if (info.Type == null)
                {
                    info.AddErrorByName(typeof(CustomerTypeEntity).FullName, "NoExist");
                    return false;
                }
                var crm = dataEntity == null ? info.Crm : dataEntity.Crm;
                if (crm == null || info.Type.Crm==null || info.Type.Crm.Id!=crm.Id)
                {
                    info.AddErrorByName(typeof(CustomerTypeEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(CustomerTypeEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证客户渠道
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateChannel(CustomerEntity info, CustomerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Channel.Id))
                return true;
            if (info.Channel != null && info.Channel.SaveType == SaveType.Add)
                return true;
            if (info.Channel != null && info.Channel.Id != 0)
            {
                if (dataEntity != null && dataEntity.Channel != null && dataEntity.Channel.Id == info.Channel.Id)
                    return true;
                info.Channel = Repository.Get<CustomerChannelEntity>(info.Channel.Id);
                if (info.Channel == null)
                {
                    info.AddErrorByName(typeof(CustomerChannelEntity).FullName, "NoExist");
                    return false;
                }
                var crm = dataEntity == null ? info.Crm : dataEntity.Crm;
                if (crm == null || info.Channel.Crm == null || info.Channel.Crm.Id != crm.Id)
                {
                    info.AddErrorByName(typeof(CustomerChannelEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(CustomerChannelEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证客户渠道
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateStaff(CustomerEntity info, CustomerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Staff.Id))
                return true;
            if (info.Staff != null && info.Staff.SaveType == SaveType.Add)
                return true;
            if (info.Staff != null && info.Staff.Id != 0)
            {
                if (dataEntity != null && dataEntity.Staff != null && dataEntity.Staff.Id == info.Staff.Id)
                    return true;
                info.Staff = Repository.Get<StaffEntity>(info.Staff.Id);
                if (info.Staff == null)
                {
                    info.AddErrorByName(typeof(StaffEntity).FullName, "NoExist");
                    return false;
                }
                var crm = dataEntity == null ? info.Crm : dataEntity.Crm;
                if (crm == null || crm.Account == null || info.Staff.Hr == null || info.Staff.Hr.Account == null || info.Staff.Hr.Account.Id != crm.Account.Id)
                {
                    info.AddErrorByName(typeof(StaffEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(StaffEntity).FullName, "NoExist");
            return false;
        }
        #endregion

    }
}
