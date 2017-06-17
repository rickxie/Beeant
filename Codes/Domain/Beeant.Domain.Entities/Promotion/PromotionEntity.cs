using System;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Entities.Promotion
{

    [Serializable]
    public class PromotionEntity : BaseEntity<PromotionEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 促销类型
        /// </summary>
        public string Tag { set; get; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 周
        /// </summary>
        public string  Weeks { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public string  Months { get; set; }
        /// <summary>
        /// 测试
        /// </summary>
        public string Cities { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayTypes { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 限购数量
        /// </summary>
        public int OrderLimitCount { set; get; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string[] PayTypeArray
        {
            get
            {
                if (string.IsNullOrEmpty(PayTypes))
                    return null;
                return PayTypes.Split(',');
            }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string[] CityArray
        {
            get
            {
                if (string.IsNullOrEmpty(Cities))
                    return null;
                return Cities.Split(',');
            }
        }
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
                return this.GetVerifyName(IsUsed);
            }
        }
      
       
        /// <summary>
        /// 备注
        /// </summary>
        public string  Remark { get; set; }
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
        public object[] Weekss
        {
            get
            {
                if (string.IsNullOrEmpty(Weeks)) return null;
                return Weeks.Split(',').Cast<object>().ToArray();
            }
        }
        public object[] Monthss
        {
            get
            {
                if (string.IsNullOrEmpty(Months)) return null;
                return Months.Split(',').Cast<object>().ToArray();
            }
        }
        /// <summary>
        /// 设置业务
        /// </summary>
        protected override void SetBusiness()
        {
            if (HasSaveProperty(it => it.BeginTime))
                BeginTime = BeginTime.ToString("1900-01-01 HH:mm:ss").Convert<DateTime>();
            if (HasSaveProperty(it => it.EndTime))
                EndTime = EndTime.ToString("1900-01-01 HH:mm:ss").Convert<DateTime>();
        }

  
        /// <summary>
        /// 检查是否有效
        /// </summary>
        /// <returns></returns>
        static public QueryInfo GetUsedQuery(long[] productIds)
        {
            var query=new QueryInfo();
            query.Query<PromotionEntity>()
                .Where(it =>productIds.Contains(it.Product.Id) && it.IsUsed && it.EndDate >= DateTime.Now.Date && it.BeginDate <= DateTime.Now.Date
                             && it.Months.Contains(DateTime.Now.Day.ToString()) &&
                             it.Weeks.Contains(DateTime.Now.DayOfWeek.Convert<int>().ToString())
                             && it.EndTime >= DateTime.Now.ToString("1900-01-01 HH:mm:ss").Convert<DateTime>()
                             && it.BeginTime <= DateTime.Now.ToString("1900-01-01 HH:mm:ss").Convert<DateTime>());
            return query;
        }


  
        
    }
}
