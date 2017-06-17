using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;

namespace Beeant.Cloud.Crm.Mobile.Models.Staff
{
    public class StaffModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public ReadCustomerType? ReadCustomerType { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPassword { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public IList<DepartmentEntity> Departments { get; set; }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public virtual StaffEntity CreateEntity(SaveType saveType,AccountEntity account)
        {
            var entity = new StaffEntity
            {
                Name = Name,
                Department=new DepartmentEntity { Id= DepartmentId.Convert<long>() },
                Account=new AccountEntity { Id=account==null?0:account.Id},
                SaveType = saveType
           };
            if (ReadCustomerType != null)
            {
                entity.SetSetting("ReadCustomerType", (int) ReadCustomerType);
            }
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id.Convert<long>();
                if (Name != null)
                    entity.SetProperty(it => it.Name);
                if (DepartmentId != null)
                    entity.SetProperty(it => it.Department.Id);
                if (ReadCustomerType != null)
                    entity.SetProperty(it => it.Setting);
                if (account != null)
                    entity.SetProperty(it => it.Account.Id);
            }
            entity.SerializeSetting();
            return entity;
        }
    }
}