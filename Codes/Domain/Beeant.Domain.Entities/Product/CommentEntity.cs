using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class CommentEntity : BaseEntity<CommentEntity>
    {
  
        /// <summary>
        /// 商品
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        ///账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 评分级别
        /// </summary>
        public CommentType Type { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 是否展示名称
        /// </summary>
        public string IsShowName
        {
            get { return this.GetShowName(IsShow); }
        }
        /// <summary>
        /// 评分级别
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }
        /// <summary>
        /// 订单明细
        /// </summary>
        public IList<OrderProductEntity> OrderProducts { get; set; }

        /// <summary>
        /// 设置添加
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetName();
            SetOrderItem();
        }

        /// <summary>
        /// 设置商品和名称
        /// </summary>
        protected virtual void SetName()
        {
            InvokeItemLoader("Product");
            if (Product == null) return;
            Name = Product.Name;
            if(Properties==null)return;
            SetProperty(it => it.Name);
        }

        /// <summary>
        /// 设置商品和名称
        /// </summary>
        protected virtual void SetOrderItem()
        {
            InvokeItemLoader("OrderProducts");
            if (OrderProducts == null) return;
            foreach (var orderProduct in OrderProducts)
            {
                orderProduct.IsAppraisement = true;
                orderProduct.SaveType=SaveType.Modify;
                orderProduct.SetProperty(it => it.IsAppraisement);
            }
          
        }
    }
}
