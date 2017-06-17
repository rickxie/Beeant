using Component.Extension;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Cloud.Site.Admin.Models.News
{
    public class NewsModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 拍下
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public virtual NewsEntity CreateEntity(SaveType saveType)
        {
            var entity = new NewsEntity
            {
                Title = string.IsNullOrWhiteSpace(Title) ? "" : Title,
                Content = string.IsNullOrWhiteSpace(Content) ? "" : Content,
                Sequence = Sequence==null?1: Sequence.Value,
                SaveType = saveType
            };
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id.Convert<long>();
                if (Title != null)
                    entity.SetProperty(it => it.Title);
                if (Content != null)
                    entity.SetProperty(it => it.Content);
                if (Sequence != null)
                    entity.SetProperty(it => it.Sequence);
            }
            return entity;
        }
    }
}