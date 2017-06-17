using Beeant.Domain.Entities.Editor;
using Component.Extension;
using Winner.Persistence;

namespace Beeant.Presentation.Website.Editor.Models.Editor
{
    public class FolderModel 
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
        /// 类型
        /// </summary>
        public FolderType? Type { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }

        public virtual FolderEntity CreateEntity(SaveType saveType)
        {
            var entity = new FolderEntity
            {
                Id = Id.Convert<long>(),
                Name = Name,
                Type = Type.HasValue? Type.Value: FolderType.Image,
                Sequence = Sequence.HasValue? Sequence.Value:1,
                SaveType = saveType
            };
            if (saveType == SaveType.Add)
                return entity;
            entity.SetProperty(it => it.Name).SetProperty(it => it.Type).SetProperty(it => it.Sequence);
            return entity;
        }
    }
}