using System.Linq;
using Beeant.Domain.Entities.Gis;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Gis
{
    public class AddressDomainService : RealizeDomainService<AddressEntity>
    {
      
        #region 重写验证

        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(AddressEntity info)
        {
            return ValidateName(info,null); 
        }

 
        /// <summary>
        /// 验证集成商类型
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateName(AddressEntity info, AddressEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Name))
                return true;
            if (dataEntity!=null && dataEntity.Name==info.Name)
                return true;
          var query=new QueryInfo();
            query.SetPageSize(1)
                .Query<AddressEntity>()
                .Where(it => it.Name == info.Name && it.Id != info.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<AddressEntity>(query);
            if (infos != null && infos.Count == 0)
                return true;
            info.AddErrorByName(typeof(AddressEntity).FullName, "NameExist");
            return false;
        }
        #endregion

        #region 修改验证

        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(AddressEntity info)
        {
            var dataEntity = Repository.Get<AddressEntity>(info.Id);
            return ValidateName(info, dataEntity);
        }


        #endregion

     


        #endregion


    }
}
