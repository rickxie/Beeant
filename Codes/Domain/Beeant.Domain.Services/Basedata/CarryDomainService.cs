using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Basedata;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Basedata
{
    public class CarryDomainService : RealizeDomainService<CarryEntity>
    {
    

        /// <summary>
        /// 是否有重复信息添加
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<CarryEntity> infos)
        {
            bool rev = true;
            for (int i = 0; i < infos.Count; i++)
                for (int k = i + 1; k < infos.Count; k++)
                {
                    rev &= CompareEntities(infos[i], infos[k]);
                }
            return rev;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="info"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        protected virtual bool CompareEntities(CarryEntity info, CarryEntity compare)
        {
            string errorName = null;
            if (info.Freight.Id.Equals(compare.Freight.Id) && info.Region==compare.Region)
                errorName = "RepeatInList";
            if (string.IsNullOrEmpty(errorName)) return true;
            info.AddErrorByName(typeof(BaseEntity).FullName,errorName);
            return false;
        }



        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CarryEntity info)
        {
            return ValidateFreightExist(info) && ValidateCarryExist(info);
        }



        /// <summary>
        /// 验证RoleAbility是否已经存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCarryExist(CarryEntity info)
        {
            if (info.Freight != null && info.Freight.SaveType == SaveType.Add)
                return true;
            var query = new QueryInfo();
            query.Query<CarryEntity>().Where(it => it.Freight.Id == info.Freight.Id
                                                      && it.Region == info.Region );
            var infos = Repository.GetEntities<CarryEntity>(query);
            if (infos == null || infos.Count == 0) return true;
            info.AddError("Exist");
            return false;
        }

        /// <summary>
        /// 验证角色类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateFreightExist(CarryEntity info)
        {
            if (!info.HasSaveProperty(it => it.Freight.Id))
                return true;
            if (info.Freight != null && info.Freight.SaveType == SaveType.Add)
                return true;
            if (info.Freight != null && info.Freight.Id!=0)
            {
                if (Repository.Get<FreightEntity>(info.Freight.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(FreightEntity).FullName, "NoExist");
            return false;
        }

    
    }
}
