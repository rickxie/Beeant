using System.Linq;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Product
{
    public class CategoryDomainService : RealizeDomainService<CategoryEntity>
    {
      

        #region 验证
   
        #region 验证添加
        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateAdd(CategoryEntity info)
        {

            return ValidateParent(info,null);
        }
        #endregion

        #region 验证修改
        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(CategoryEntity info)
        {
            var dataEntity = Repository.Get<CategoryEntity>(info.Id);
            return ValidateParent(info,dataEntity) && ValidateBranch(info, dataEntity);
        }

      
       
        /// <summary>
        /// 验证支点
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        protected virtual bool ValidateBranch(CategoryEntity info, CategoryEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id)) return true;
            if (dataEntity.Parent != null && info.Parent.Id == dataEntity.Parent.Id) return true;
            var data = Repository.Get<CategoryEntity>(info.Parent.Id);
            do
            {
                if(data==null)break;
                if (data.Id == info.Id || data.Parent != null && data.Parent.Id == info.Id)
                {
                    info.AddError("NotAllowParent");
                    return false;
                }
                if (data.Parent == null || data.Parent.Id==0)
                    break;
                data = Repository.Get<CategoryEntity>(data.Parent.Id);
            } while (data.Parent != null && dataEntity.Parent.Id!=0);
            return true;
        }
        #endregion

        #region 验证删除
        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(CategoryEntity info)
        {
            return ValidateMenuLeaf(info); 
        }

    
        /// <summary>
        /// 验证Menu是否是页节点
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateMenuLeaf(CategoryEntity info)
        {
            var query = new QueryInfo();
            query.Query<CategoryEntity>().Where(it => it.Parent.Id == info.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<CategoryEntity>(query);
            if (infos != null && infos.Count == 0) return true;
            info.AddError("ExistChild");
            return false;
        }
        #endregion

        /// <summary>
        /// 验证父类
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateParent(CategoryEntity info, CategoryEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id) || info.Parent.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.Parent != null && dataEntity.Parent.Id == info.Parent.Id)
                return true;
            var parent = Repository.Get<CategoryEntity>(info.Parent.Id);
            if (parent == null)
            {
                info.AddError("NoParent");
                return false;
            }
            return true;
        }
        #endregion
    }
}
