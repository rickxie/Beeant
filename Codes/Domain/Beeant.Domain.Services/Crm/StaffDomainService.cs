using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Crm
{
    public class StaffDomainService : RealizeDomainService<StaffEntity>
    {
        private IDictionary<string, ItemLoader<StaffEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<StaffEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<StaffEntity>>
                    {
                        {"Department", LoadDepartment}
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
        protected virtual void LoadDepartment(StaffEntity info)
        {
            info.Department = info.Department.SaveType == SaveType.Add ? info.Department : Repository.Get<DepartmentEntity>(info.Department.Id);

        }


        #region 重写验证

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(StaffEntity info)
        {
            var rev = ValidateDepartment(info, null) && ValidateAccount(info,null) && ValidateAccountExist(info,null);
            return rev;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(StaffEntity info)
        {
            var dataEntity = Repository.Get<StaffEntity>(info.Id);
            return ValidateDepartment(info, dataEntity) && ValidateAccount(info, dataEntity) && ValidateAccountExist(info, dataEntity);
        }

        #endregion



        /// <summary>
        /// 验证客户类型
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateDepartment(StaffEntity info, StaffEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Department.Id))
                return true;
            if (info.Department != null && info.Department.SaveType == SaveType.Add)
                return true;
            if (info.Department != null && info.Department.Id != 0)
            {
                if (dataEntity != null && dataEntity.Department != null && dataEntity.Department.Id == info.Department.Id)
                    return true;
                info.Department = Repository.Get<DepartmentEntity>(info.Department.Id);
                if (info.Department == null)
                {
                    info.AddErrorByName(typeof(DepartmentEntity).FullName, "NoExist");
                    return false;
                }

                return true;
            }
            info.AddErrorByName(typeof(DepartmentEntity).FullName, "NoExist");
            return false;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(StaffEntity info, StaffEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null ||
                info.Account.Id == 0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
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

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccountExist(StaffEntity info, StaffEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
                return true;
            var query = new QueryInfo();
            query.Query<StaffEntity>().Where(it => it.Account.Id == info.Account.Id).Select(it => it.Id);
            var infos = Repository.GetEntities<StaffEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("AccountHasStaff");
                return false;
            }
            return true;
        }
        #endregion


    }
}
