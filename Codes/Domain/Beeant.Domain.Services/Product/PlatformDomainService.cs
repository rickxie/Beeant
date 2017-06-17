using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Product
{
    public class PlatformDomainService : RealizeDomainService<PlatformEntity>
    {


        #region 重写验证
     

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PlatformEntity info)
        {
            var rev = ValidateGoods(info, null) && ValidateDataId(info, null);
            return rev;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(PlatformEntity info)
        {
            var dataEntity = Repository.Get<PlatformEntity>(info.Id);
            return ValidateGoods(info, dataEntity) && ValidateDataId(info,dataEntity);
        }

        #endregion

        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateGoods(PlatformEntity info, PlatformEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Goods.Id))
                return true;
            if (info.Goods != null && info.Goods.SaveType == SaveType.Add)
                return true;
            if (info.Goods != null && info.Goods.Id != 0)
            {
                if (dataEntity != null && dataEntity.Goods != null && dataEntity.Goods.Id == info.Goods.Id)
                    return true;
                var category = Repository.Get<CategoryEntity>(info.Goods.Id);
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
        /// <summary>
        /// 验证平台数据编号
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateDataId(PlatformEntity info, PlatformEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.DataId) || string.IsNullOrEmpty(info.DataId))
                return true;
            if (dataEntity != null && dataEntity.DataId == info.DataId)
                return true;
            var type = !info.HasSaveProperty(it => it.Type) && dataEntity != null ? dataEntity.Type : info.Type;
            var query = new QueryInfo();
            query.Query<PlatformEntity>().Where(it => it.DataId == info.DataId && it.Type == type).Select(it=>it.Id);
            var infos = Repository.GetEntities<PlatformEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("DataIdExist");
                return false;
            }
            return true;
        }
        #endregion

    }
}
