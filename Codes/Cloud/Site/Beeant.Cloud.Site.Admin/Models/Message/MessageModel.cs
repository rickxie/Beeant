using Component.Extension;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Cloud.Site.Admin.Models.Message
{
    public class MessageModel
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
        /// 名称
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public MessageEntity Message { get; set; }
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public virtual MessageEntity CreateEntity(SaveType saveType)
        {
            var entity = new MessageEntity
            {
                Name = string.IsNullOrWhiteSpace(Name) ? "" : Name,
                Content = string.IsNullOrWhiteSpace(Content) ? "" : Content,
               
                SaveType = saveType
            };
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id.Convert<long>();
                if (Name != null)
                    entity.SetProperty(it => it.Name);
                if (Content != null)
                    entity.SetProperty(it => it.Content);
            }
            return entity;
        }
    }
}