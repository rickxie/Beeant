using System.Collections.Generic;
using Beeant.Domain.Entities.Product;
using Winner.Filter;

namespace Beeant.Presentation.Mobile.Order.Models.Product
{
    public class CommentModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 点评类型
        /// </summary>
        public CommentType Type { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 错误
        /// </summary>
        public string Message { get; set; }
    }
}
