using Component.Extension;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;

namespace Beeant.Cloud.Crm.Mobile.Models.CustomerType
{
    public class CustomerTypeModel
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
        /// 拍下
        /// </summary>
        public int? Sequence { get; set; }
 
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public virtual CustomerTypeEntity CreateEntity(SaveType saveType)
        {
            var entity = new CustomerTypeEntity
            {
                Name = string.IsNullOrWhiteSpace(Name) ? "" : Name,
                Sequence = Sequence==null?1: Sequence.Value,
                SaveType = saveType
            };
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id.Convert<long>();
                if (Name != null)
                    entity.SetProperty(it => it.Name);
                if (Sequence != null)
                    entity.SetProperty(it => it.Sequence);
             
            }
            return entity;
        }
    }
}