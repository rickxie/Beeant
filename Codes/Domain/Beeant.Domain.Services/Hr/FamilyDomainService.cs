using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Hr;
using Winner.Persistence;
using StaffEntity = Beeant.Domain.Entities.Hr.StaffEntity;

namespace Beeant.Domain.Services.Hr
{
    public class FamilyDomainService : RealizeDomainService<FamilyEntity>
    {
       
        
   
        private IDictionary<string, ItemLoader<FamilyEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<FamilyEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<FamilyEntity>>
                    {
                        {"Staff", LoadStaff}
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
        protected virtual void LoadStaff(FamilyEntity info)
        {
            info.Staff = info.Staff.SaveType == SaveType.Add ? info.Staff : Repository.Get<StaffEntity>(info.Staff.Id);

        }
        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(FamilyEntity info)
        {
            var rev = ValidateStaff(info);
            return rev;
        }

        
        #endregion

  
 
        /// <summary>
        /// 验证部门
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateStaff(FamilyEntity info)
        {
            
            if (!info.HasSaveProperty(it => it.Staff.Id))
                return true;
            if (info.Staff.SaveType == SaveType.Add)
                return true;
            var staff = Repository.Get<StaffEntity>(info.Staff.Id);
            if (staff == null)
            {
                info.AddErrorByName(typeof(StaffEntity).FullName, "NoExist");
                return false;
            }
            if (!staff.IsUsed)
            {
                info.AddErrorByName(typeof(StaffEntity).FullName, "UnUsed");
                return false;
            }
            return true;
            
        }

      
        #endregion

    }
}
