using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Order
{
    [Serializable]
    public class OrderNoteEntity : BaseEntity<OrderNoteEntity>
    {
        
        /// <summary>
        /// 订单
        /// </summary>
        public OrderEntity Order { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public AccountEntity Account { get; set; }
       
    }
}
