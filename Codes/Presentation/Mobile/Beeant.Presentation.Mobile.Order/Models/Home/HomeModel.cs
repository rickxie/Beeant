using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Order;

namespace Beeant.Presentation.Mobile.Order.Models.Home
{
    public class HomeModel:PagerModel
    {
        public override int PageSize => 3;
        /// <summary>
        /// 付款
        /// </summary>
        public string PayTypesJson
        {
            get
            {
                if (PayTypes == null)
                    return "[]";
                var arrays=new List<IDictionary<string,object>>();
                foreach (var payType in PayTypes)
                {
                    arrays.Add(new Dictionary<string, object>
                    {
                        {"Name",payType.Name },
                        {"Url",payType.Url }
                    });
                }
                return arrays.SerializeJson();
            }
        }
        /// <summary>
        /// 付款方式
        /// </summary>
        public IList<PayTypeEntity> PayTypes { get; set; } 
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginInsertTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? EndInsertTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool? IsAppraisement { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public IList<OrderEntity> Orders { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 得到快递费用
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public virtual decimal GetExpressAmount(OrderEntity order)
        {
            if (order == null || order.OrderExpresses == null)
                return 0;
            return order.OrderExpresses.Sum(it => it.Amount);
        }
    }
}