using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Wms
{
    [Serializable]
    public class StorehouseEntity : BaseEntity<StorehouseEntity>
    {

        /// <summary>
        ///名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
      
        /// <summary>
        ///排序
        /// </summary>
        public int Sequence { get; set; }
     
        /// <summary>
        /// 是否启用
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
  

        /// <summary>
        /// 库存清单
        /// </summary>
        public IList<InventoryEntity> Inventories { get; set; }
        /// <summary>
        /// 出入库清单
        /// </summary>
        public IList<StockEntity> Stocks { get; set; }
     
    }
}
