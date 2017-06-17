using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Product;

namespace Beeant.Domain.Entities.Cart
{
    [Serializable]
    public class AttentionEntity : BaseEntity<AttentionEntity>
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
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 设置添加
        /// </summary>
        protected override void SetAddBusiness()
        {
            InvokeItemLoader("Product");
            if (Product == null) return;
            Name = Product.Name;
            Price = Product.Price;
            if (Properties != null)
            {
                SetProperty(it => it.Name);
                SetProperty(it => it.Price);
            }
          
        }

   
    }
}
