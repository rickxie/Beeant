using Beeant.Domain.Entities.Product;

namespace Beeant.Domain.Entities.Wms
{
    public class ShelfEntity :BaseEntity<ShelfEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public StorehouseEntity Storehouse { get; set; }
        /// <summary>
        /// 相关产品
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 是否
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsUsedName
        {
            get
            {
                return this.GetStatusName(IsUsed);
            }
        }
    }
}
