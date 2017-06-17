using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;

namespace Beeant.Domain.Services.Product
{
    public class PropertyDomainService : RealizeDomainService<PropertyEntity>
    {


        #region 重写验证
        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<PropertyEntity> infos)
        {
            var temps =
                infos.Where(it => it.HasSaveProperty(s => s.Name))
                     .ToList();
            if (temps.Count > 1)
            {
                foreach (var temp in temps)
                {
                    temp.AddErrorByName(typeof(BaseEntity).FullName, "NoAllowBatchSave");
                }
                return false;
            }
            return true;
        }
      

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PropertyEntity info)
        {
            var rev = ValidateCategory(info, null);
            return rev;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(PropertyEntity info)
        {
            var dataEntity = Repository.Get<PropertyEntity>(info.Id);
            return ValidateCategory(info, dataEntity);
        }

        #endregion

        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCategory(PropertyEntity info, PropertyEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Category.Id))
                return true;
            if (info.Category != null && info.Category.SaveType == SaveType.Add)
                return true;
            if (info.Category != null && info.Category.Id!=0)
            {
                if (dataEntity != null && dataEntity.Category != null && dataEntity.Category.Id == info.Category.Id)
                    return true;
                var category = Repository.Get<CategoryEntity>(info.Category.Id);
                if (category == null)
                {
                    info.AddErrorByName(typeof(CategoryEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(CategoryEntity).FullName, "NoExist");
            return false;
        }
        #endregion

    }
}
