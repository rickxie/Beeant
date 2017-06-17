using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Beeant.Presentation.Mobile.Detail.Models.Home;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Detail.Controllers.Home
{
    public class HomeController : MobileBaseController
    {
    

        #region 详情

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="productId"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public virtual ActionResult Index(long? goodsId, long? productId, string mark)
        {
            if (!goodsId.HasValue && productId.HasValue)
            {
                var product = this.GetEntity<ProductEntity>(productId.Value);
                if(product==null)
                    return View("~/Views/Home/Unsale.cshtml");
                goodsId = product.Goods.Id;
            }
            var model = new GoodsModel
            {
                ProductId=productId,
                Goods = GetGoods(goodsId.HasValue?goodsId.Value:0, mark),
                IdentityId = Identity == null ? 0 : Identity.Id,
                Agent= GetAgent()

            };
            if (model.Goods == null || model.Goods.Products == null || model.Goods.Products.Count == 0)
            {
                return View("~/Views/Home/Unsale.cshtml", model);
            }
            if (model.Product == null)
                return View("~/Views/Home/Unsale.cshtml", model);
            model.SetSkuProperties();
            model.SetPrice();
            return View("~/Views/Home/index.cshtml", model);
        }


 
       
  
        /// <summary>
        /// 得到商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="mark"></param>
        protected virtual GoodsEntity GetGoods(long goodsId, string mark)
        {
            var query = new QueryInfo();
            if (!string.IsNullOrEmpty(mark) && mark.ToLower().Equals(Winner.Creator.Get<Winner.Base.ISecurity>()
                                                                           .EncryptMd5(
                                                                               DateTime.Now.ToString("yyyy-MM-dd"))))
            {
                query.Query<GoodsEntity>().Where(it => it.Id == goodsId);
            }
            else
            {
                query.Query<GoodsEntity>().Where(it => it.Id == goodsId && it.IsSales);
            }
            query.Query<GoodsEntity>()
                 .Select(
                     it =>
                     new object[]
                         {
                             it.Id,it.Freight.Id,
                             it.Products.Select(s=>new object[]{s.Id,s.Sku,s.Name,s.OrderStepCount,s.OrderMinCount,s.Count,s.Price,s.FileName})
                         });
            var infos = this.GetEntities<GoodsEntity>(query);
            
            return infos?.FirstOrDefault();
        }

        /// <summary>
        /// 得到促销商品
        /// </summary>
        public virtual ActionResult Promotion(long productId)
        {

            var query = PromotionEntity.GetUsedQuery(new [] { productId });
            query.Query<PromotionEntity>();
            var infos= this.GetEntities<PromotionEntity>(query);
            return View("_Promotion", infos?.FirstOrDefault());
        }
        #endregion

        #region 得到产品图片

        /// <summary>
        /// 得到产品图片
        /// </summary>
        public virtual ActionResult GoodsImages(long goodsId, long productId)
        {
            var query=new QueryInfo();
            query.Query<GoodsImageEntity>()
                .Where(it => (it.Goods.Id == goodsId && it.Product.Id==0) || it.Product.Id == productId)
                .Select(it => new object[] {it.Product.Id, it.FileName });
            var infos = this.GetEntities<GoodsImageEntity>(query);
            if (infos != null)
            {
                if (infos.Any(it => it.Product != null && it.Product.Id == productId))
                    infos = infos.Where(it => it.Product.Id == productId).ToList();
            }
            return View("~/Views/Home/_GoodsImage.cshtml", infos);
        }
        #endregion

        #region 得到产品详情
   
        /// <summary>
        /// 得到产品图片
        /// </summary>
        public virtual ActionResult GoodsDetail(long goodsId, long productId)
        {
            var model = new GoodsDetailModel
            {
                GoodsDetail = GetGoodsDetail(goodsId, productId),
                GoodsProperties = GetGoodsProperties(goodsId, productId)
            };
            return View("~/Views/Home/_GoodsDetail.cshtml", model);
        }
        /// <summary>
        /// 得到产品图片
        /// </summary>
        protected virtual GoodsDetailEntity GetGoodsDetail(long goodsId, long productId)
        {
            var query = new QueryInfo();
            query.Query<GoodsDetailEntity>()
                .Where(it => (it.Goods.Id == goodsId && it.Product.Id == 0) || it.Product.Id == productId)
                .Select(it => new object[] { it.Detail, it.Name });
            var infos = this.GetEntities<GoodsDetailEntity>(query);
            GoodsDetailEntity info = infos?.FirstOrDefault(it => it.Product != null && it.Product.Id == productId);
            info = info ?? infos.FirstOrDefault();
            return info;
        }
        /// <summary>
        /// 得到产品图片
        /// </summary>
        protected virtual IList<GoodsPropertyEntity> GetGoodsProperties(long goodsId, long productId)
        {
            var query = new QueryInfo();
            query.Query<GoodsPropertyEntity>()
                .Where(it => (it.Goods.Id == goodsId && it.Product.Id == 0) || it.Product.Id == productId)
                .Select(it => new object[] { it.Property.Name, it.Value });
            return this.GetEntities<GoodsPropertyEntity>(query);
           
        }
        #endregion

        #region 产品咨询
        /// <summary>
        /// 商品咨询
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual ActionResult Comment(long goodsId, int page)
        {
            var model = new CommentListModel
            {
                PageIndex = page
            };
            var query = new QueryInfo();
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<CommentEntity>().Where(it => it.Product.Goods.Id == goodsId)
                .OrderByDescending(it => it.Id).Select(it => new object[] { it.Account.Name, it.Detail,it.InsertTime,it.Type });
            model.Comments = this.GetEntities<CommentEntity>(query);
            if (model.Comments == null || model.Comments.Count == 0)
                return Content("");
            return View("~/Views/Home/_Comment.cshtml", model);
        }
        #endregion
        /// <summary>
        /// 得到代理
        /// </summary>
        /// <returns></returns>
        public virtual AgentEntity GetAgent()
        {
            if (Identity == null)
                return null;
            var query = new QueryInfo();
            query.Query<AgentEntity>().Where(it => it.Account.Id == Identity.Id && it.IsUsed)
                .Select(it => it.Setting);
            return this.GetEntities<AgentEntity>(query)?.FirstOrDefault();
        }
    }
}
