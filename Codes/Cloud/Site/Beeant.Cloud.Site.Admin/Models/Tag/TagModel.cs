using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Cloud.Site.Admin.Models.Tag
{
    public class TagModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 拍下
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public virtual TagEntity CreateEntity(SaveType saveType)
        {
            var entity = new TagEntity
            {
                Name = Name,
                Sequence = Sequence==null?1: Sequence.Value,
                SaveType = saveType
            };
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id;
                if (Name != null)
                    entity.SetProperty(it => it.Name);
                if (Sequence != null)
                    entity.SetProperty(it => it.Sequence);
            }
            return entity;
        }
    }
}