using System;
using System.Collections.Generic;
using System.Text;
using Component.Extension;
using Beeant.Domain.Entities.Merchant;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Order;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class GoodsEntity : BaseEntity<GoodsEntity>
    {
        /// <summary>
        /// 父类
        /// </summary>
        public CategoryEntity Category { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string FileName { get; set; }

     
 
        /// <summary>
        /// 订单在什么状态下不占用库存
        /// </summary>
        public string UnusedStatus { get; set; }
   
        /// <summary>
        /// 是否销售
        /// </summary>
        public bool IsSales { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
  

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 最小订单数量
        /// </summary>
        public int OrderMinCount { get; set; }
        /// <summary>
        /// 增加步长数量
        /// </summary>
        public int OrderStepCount { get; set; }
        /// <summary>
        /// 访问数量
        /// </summary>
        public int VisitCount { get; set; }
        /// <summary>
        /// 关注数量
        /// </summary>
        public int AttentionCount { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int SalesCount { get; set; }
        /// <summary>
        /// 定金比率
        /// </summary>
        public decimal DepositRate { get; set; }
        /// <summary>
        /// 是否定制
        /// </summary>
        public bool IsCustom { get; set; }
        /// <summary>
        /// 是否支持退货
        /// </summary>
        public bool IsReturn { get; set; }
        /// <summary>
        /// 关联同步产品
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayTypes { get; set; }
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
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public FreightEntity Freight { get; set; }
        /// <summary>
        ///商品图片
        /// </summary>
        public IList<GoodsImageEntity> GoodsImages { get; set; }
        /// <summary>
        ///商品详情
        /// </summary>
        public IList<GoodsDetailEntity> GoodsDetails { get; set; }
        /// <summary>
        /// 子类
        /// </summary>
        public IList<CatalogueGoodsEntity> CatalogueGoodses { get; set; }
        /// <summary>
        /// 订单在什么状态下减少库存
        /// </summary>
        public int[] UnusedStatusArray
        {
            get
            {
                if (string.IsNullOrEmpty(UnusedStatus)) return null;
                var arr = UnusedStatus.Split(',');
                return arr.Select(s => s.Convert<int>()).ToArray();
            }
        }

        /// <summary>
        /// 处理的状态名称
        /// </summary>
        public string UnusedStatusName
        {
            get
            {
                if (UnusedStatusArray == null) return "";
                var names = new StringBuilder();
                foreach (var value in UnusedStatusArray)
                {
                    names.AppendFormat("{0},", GetLanguage(typeof(OrderEntity).FullName, "Status", value));
                }
                if (names.Length > 0) names.Remove(names.Length - 1, 1);
                return names.ToString();
            }
        }
     

        /// <summary>
        /// 商品属性
        /// </summary>
        public IList<GoodsPropertyEntity> GoodsProperties { get; set; }


        /// <summary>
        /// 产品
        /// </summary>
        public IList<ProductEntity> Products { get; set; }
        /// <summary>
        /// 平台同步
        /// </summary>
        public IList<PlatformEntity> Platforms { get; set; }
        
        /// <summary>
        /// 文件名
        /// </summary>
        public string FullFileName
        {
            get { return this.GetFullFileName(FileName); }
        }
   
        /// <summary>
        /// 是否销售
        /// </summary>
        public string IsSalesName
        {
            get
            {
                return this.GetSalesName(IsSales);
            }
        }

        /// <summary>
        /// 是否支持退货
        /// </summary>
        public string IsReturnName
        {
            get
            {
                return this.GetStatusName(IsReturn);
            }
        }
        /// <summary>
        /// 定金比率
        /// </summary>
        public string PercentageOfDepositRate
        {
            get { return DepositRate<=0? "" : (DepositRate*100)+"%";} 
        }
 


        public GoodsEntity DataEntity { get; set; }



        /// <summary>
        /// 修改
        /// </summary>
        protected override void SetModifyBusiness()
        {
            if (!HasSaveProperty(it => it.IsSales) || IsSales)
                return;
            InvokeItemLoader("Products");
            if (Products == null)
                return;
            foreach (var product in Products)
            {
                product.IsSales = false;
                if (product.SaveType == SaveType.None)
                {
                    product.SetProperty(it => it.IsSales);
                    product.SaveType = SaveType.Modify;
                }
                else if (product.Properties != null)
                {
                    product.SetProperty(it => it.IsSales);
                }
            }
        }


        /// <summary>
        /// she
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("Products");
            if (Products != null)
            {
                foreach (var inventory in Products)
                {
                    inventory.SaveType = SaveType.Remove;
                }
            }
        }
    }
}
