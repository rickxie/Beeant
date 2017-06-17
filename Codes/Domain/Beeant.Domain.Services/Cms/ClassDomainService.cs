using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Cms;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Cms
{
    public class ClassDomainService : RealizeDomainService<ClassEntity>
    {
        #region 验证
        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<ClassEntity> infos)
        {
            var temps =
                infos.Where(it => it.HasSaveProperty(s => s.Tag))
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
        
     

        #region 验证添加
        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateAdd(ClassEntity info)
        {
            return ValidateAddTag(info) && ValidateParent(info,null);
        }
     
        /// <summary>
        /// 验证标签
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidateAddTag(ClassEntity info)
        {
            if (!info.HasSaveProperty(it => it.Tag) || string.IsNullOrEmpty(info.Tag))
                return true;
            var query = new QueryInfo();
            query.Query<ClassEntity>().Where(it => it.Tag == info.Tag);
            var infos = Repository.GetEntities<ClassEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("TagExist");
                return false;
            }
            return true;
        }

        #endregion

        #region 验证修改
        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(ClassEntity info)
        {
            var dataEntity = Repository.Get<ClassEntity>(info.Id);
            return ValidateParent(info, dataEntity) && ValidateModifyTag(info, dataEntity) && ValidateBranch(info, dataEntity);
        }


        /// <summary>
        /// 验证标签
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidateModifyTag(ClassEntity info ,ClassEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Tag) || string.IsNullOrEmpty(info.Tag))
                return true;
            if (dataEntity != null && dataEntity.Tag == info.Tag)
                return true;
            var query = new QueryInfo();
            query.Query<ClassEntity>().Where(it => it.Tag == info.Tag && it.Id!=dataEntity.Id);
            var infos = Repository.GetEntities<ClassEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("TagExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证支点
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        protected virtual bool ValidateBranch(ClassEntity info, ClassEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id)) return true;
            if (dataEntity.Parent != null && info.Parent.Id == dataEntity.Parent.Id) return true;
            var data = Repository.Get<ClassEntity>(info.Parent.Id);
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
                data = Repository.Get<ClassEntity>(data.Parent.Id);
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
        protected override bool ValidateRemove(ClassEntity info)
        {
            return ValidateMenuLeaf(info); 
        }

    
        /// <summary>
        /// 验证Menu是否是页节点
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateMenuLeaf(ClassEntity info)
        {
            var query = new QueryInfo();
            query.Query<ClassEntity>().Where(it => it.Parent.Id == info.Id)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<ClassEntity>(query);
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
        protected virtual bool ValidateParent(ClassEntity info, ClassEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Parent.Id) || info.Parent.Id==0)
                return true;
            if (dataEntity != null && dataEntity.Parent != null && dataEntity.Parent.Id == info.Parent.Id)
                return true;
            var parent = Repository.Get<ClassEntity>(info.Parent.Id);
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
