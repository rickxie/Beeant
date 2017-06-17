using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Basedata
{
    public class TagDomainService : RealizeDomainService<TagEntity>
    {
        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
        #region 重写验证
        /// <summary>
        /// 批量验证
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override bool ValidateBatch(IList<TagEntity> infos)
        {
            var temps =
                infos.Where(it => it.HasSaveProperty(s=>s.Value))
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
   

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(TagEntity info)
        {
            return ValidateTag(info, null) && ValidateTagGroup(info,null); 
        }
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(TagEntity info)
        {
            var dataEntity = Repository.Get<TagEntity>(info.Id);
            return ValidateTag(info, dataEntity) && ValidateTagGroup(info, dataEntity); 
        }

        /// <summary>
        /// 验证标签组
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateTagGroup(TagEntity info, TagEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.TagGroup.Id))
                return true;
            if (dataEntity != null && dataEntity.TagGroup.Id == info.TagGroup.Id)
                return true;
            var tagGroup = Repository.Get<TagGroupEntity>(info.TagGroup.Id);
            if (tagGroup == null)
            {
                info.AddErrorByName(typeof(TagGroupEntity).FullName,"NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证标签
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidateTag(TagEntity info, TagEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Value))
                return true;
            if (dataEntity != null && dataEntity.Value == info.Value)
                return true;
            var query = new QueryInfo();
            query.Query<TagEntity>().Where(it => it.Value == info.Value);
            var infos = Repository.GetEntities<TagEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("Exist");
                return false;
            }
            return true;
        }
        #endregion
    }
}
