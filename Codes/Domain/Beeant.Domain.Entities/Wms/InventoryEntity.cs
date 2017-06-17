using System;
using Component.Extension;
using Beeant.Domain.Entities.Product;

namespace Beeant.Domain.Entities.Wms
{
    [Serializable]
    public class InventoryEntity : BaseEntity<InventoryEntity>
    {
        /// <summary>
        ///仓库
        /// </summary>
        public StorehouseEntity Storehouse { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public ProductEntity Product { get; set; }

        /// <summary>
        /// 供应城市
        /// </summary>
        public string Cities { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 流程中数量
        /// </summary>
        public int LockCount { set; get; }

        /// <summary>
        /// 可用库存
        /// </summary>
        public int EnableCount
        {
            get { return Count + LockCount; }
        }

        /// <summary>
        /// 在途数量
        /// </summary>
        public int TransitCount { get; set; }

        /// <summary>
        /// 库存警戒线
        /// </summary>
        public int WarningCount { get; set; }

        /// <summary>
        /// 周期
        /// </summary>
        public int Recycle { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public InvertoryType Type { get; set; }
        /// <summary>
        /// 定时时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 定时时间
        /// </summary>
        public string TimingTime { get; set; }
        /// <summary>
        /// 执行星期
        /// </summary>
        public string Weeks { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Months { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }

        /// <summary>
        /// 日期数组
        /// </summary>
        public string[] MonthsArray
        {
            get
            {
                if (string.IsNullOrEmpty(Months)) return null;
                return Months.Split(',');
            }
        }
  
        /// <summary>
        /// 星期名称
        /// </summary>
        public string WeekName
        {
            get { return Weeks.GetEnums<DayOfWeek>().BuildeName(); }
        }
        /// <summary>
        /// 星期绑定
        /// </summary>
        public string BindWeeks
        {
            get
            {
                return Weeks.GetEnumComboStringValue<DayOfWeek>();
            }
            set { Weeks = value.GetEnumComboIntValue<DayOfWeek>(); }
        }
    }

}
