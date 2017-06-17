using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Gis;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class FreightEntity : BaseEntity<FreightEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string FreeRegion { get; set; }

        /// <summary>
        /// 利润比例
        /// </summary>
        public decimal FreeProfit { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string[] FreeRegionArray
        {
            get
            {
                if (string.IsNullOrEmpty(FreeRegion))
                    return null;
                return FreeRegion.Split(',');
            }
        }

        /// <summary>
        /// 是否自动取值
        /// </summary>
        public bool IsDelivery { get; set; }

        /// <summary>
        /// 所属账户
        /// </summary>
        public AccountEntity Account { get; set; }

        /// <summary>
        /// 运价方式
        /// </summary>
        public IList<CarryEntity> Carries { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 成本
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 确认是否包邮
        /// </summary>
        /// <returns></returns>
        public virtual void Set(FreightCalculatorEntity calculator)
        {
            if (IsDelivery)
            {
                SetDelivery(calculator);
                return;
            }
            if (calculator.Calculate(FreeRegionArray))
            {
                return;
            }
            var price = GetMarryPrice(calculator);
            if (calculator.GetProfit(price) >= FreeProfit)
                return;
            Price = Cost = price;
        }

        /// <summary>
        /// 设置配送
        /// </summary>
        /// <param name="calculate"></param>
        protected virtual void SetDelivery(FreightCalculatorEntity calculate)
        {
            if (calculate.Area == null || string.IsNullOrEmpty(calculate.Area.Value))
                return;
            var vals = calculate.Area.Value.Split('-');
            Price = vals[0].Convert<decimal>()*calculate.Count;
            if (vals.Length > 1)
            {
                Cost = vals[1].Convert<decimal>()*calculate.Count;
            }
        }

        /// <summary>
        /// 得到价格
        /// </summary>
        /// <param name="calculator"></param>
        /// <returns></returns>
        public virtual decimal GetMarryPrice(FreightCalculatorEntity calculator)
        {

            var carry = Carries.FirstOrDefault(it => it.RegionArray != null
                                                     &&
                                                     (it.RegionArray.Contains(calculator.Province) ||
                                                      it.RegionArray.Contains(calculator.City)
                                                      || it.RegionArray.Contains(calculator.County)));
            if (carry == null)
            {
                carry = Carries.FirstOrDefault(it => string.IsNullOrWhiteSpace(it.Region));
            }
            if (carry == null)
                return 0;
            var defaultPrice = carry.DefaultPrice;
            if (calculator.Count > carry.DefaultCount)
            {
                var overCount = calculator.Count - carry.DefaultCount;
                var priceCount =
                    (int) Math.Ceiling((double) overCount/(carry.ContinueCount == 0 ? 1 : carry.ContinueCount));
                return defaultPrice + priceCount*carry.ContinuePrice;
            }
            return defaultPrice;
        }


    }

    [Serializable]
    public class FreightCalculatorEntity
    {
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 县
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 成本
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public AreaEntity Area { get; set; }

        /// <summary>
        /// 利润
        /// </summary>
        public virtual decimal GetProfit(decimal freight)
        {
            return ((Price - Cost)*Count - freight)/(Price*Count);

        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public virtual bool Calculate(string[] region)
        {
            if (region == null)
                return false;
            return region.Contains(Province) || region.Contains(City) || region.Contains(County);
        }
    }

}
