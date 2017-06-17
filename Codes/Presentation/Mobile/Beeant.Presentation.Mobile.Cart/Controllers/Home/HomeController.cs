using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Cart;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Beeant.Presentation.Mobile.Cart.Models.Home;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Cart.Controllers.Home
{

    public class HomeController : MobileBaseController
    {
        #region 首页
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public virtual ActionResult Index()
        {
            var model = new HomeModel();
            return View("Index", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [AuthorizeFilter]
        public virtual ActionResult List(int? page)
        {
            var model = new HomeModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Shopcarts = GetShopcarts(model);
            if (model.Shopcarts == null || model.Shopcarts.Count == 0)
                return Content("");
            return View("_List",model);
        }
        #endregion

        #region 导航购物车
        /// <summary>
        /// 得到购物车
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Bar()
        {
            return View("Bar");
        }
        /// <summary>
        /// 得到购物车
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Dialog()
        {
            var model = new HomeModel();
            return View("Dialog", model);
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        [AuthorizeFilter]
        public virtual ActionResult Add(int productId, string tag)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            var query = new QueryInfo();
            query.Query<ShopcartEntity>().Where(it => it.Product.Id == productId && it.Account.Id == Identity.Id)
                .Select(it => new object[] { it.Id, it.Count });
            var infos = this.GetEntities<ShopcartEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                var info = infos.First();
                rev = UpdateShopcartCount(info.Id, info.Count + 1);
                result.Add("Id", info.Id);
                result.Add("Count", info.Count+1);
            }
            else
            {

                var product = this.GetEntity<ProductEntity>(productId);
                if (product != null)
                {
                    var info = new ShopcartEntity
                    {
                        Tag = tag,
                        Name = product.Name,
                        Price = product.Price,
                        Count = 1,
                        Product = new ProductEntity { Id = productId },
                        Account = new AccountEntity { Id = Identity.Id },
                        SaveType = SaveType.Add
                    };
                    rev = this.SaveEntity(info);
                    result.Add("Id", info.Id);
                    result.Add("Count", info.Count);
                }
            }
            result.Add("Status", rev);
  
            return this.Jsonp(result);
        }
        /// <summary>
        /// 删除购物车
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeFilter]
        [DataFilter(EntityType = typeof(ShopcartEntity))]
        public virtual ActionResult Remove(long[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<ShopcartEntity>();
                foreach (var i in id)
                {
                    var info = new ShopcartEntity
                    {
                        Id = i,
                        SaveType = SaveType.Remove
                    };
                    infos.Add(info);
                }
                rev = this.SaveEntities(infos);
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }

        /// <summary>
        /// 更新购物车
        /// </summary>
        /// <param name="id">
        /// <param name="count"></param>
        /// </param>
        /// <param name="count"></param>
        /// <returns></returns>
        [AuthorizeFilter]
        [DataFilter(IdParamterName = "id",EntityType = typeof(ShopcartEntity))]
        public virtual ActionResult UpdateCount(long id, int count)
        {
            var result = new Dictionary<string, object>();
            var rev = UpdateShopcartCount(id, count);
            result.Add("Status", rev);
            return this.Jsonp(result);
        }

        /// <summary>
        /// 更新购物车
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        [AuthorizeFilter]
        [DataFilter(IdParamterName = "id", EntityType = typeof(ShopcartEntity))]
        public virtual ActionResult UpdateTag(long id, string tag)
        {
            var result = new Dictionary<string, object>();
            var info = new ShopcartEntity
            {
                Id = id,
                Tag = tag,
                SaveType = SaveType.Modify
            };
            info.SetProperty(it => it.Tag);
            var rev = this.SaveEntity(info);
            result.Add("Status", rev);
            return this.Jsonp(result);
        }
        /// <summary>
        /// 得到促销商品
        /// </summary>
        /// <param name="productIds"></param>
        protected virtual IList<PromotionEntity> GetPromotions(long[] productIds)
        {
            if (productIds == null)
                return null;
            var query = PromotionEntity.GetUsedQuery(productIds);
            query.Query<PromotionEntity>()
                 .Select(it => new object[]
                     {
                        it.Id,it.Product.Id,it.OrderLimitCount,it.Cities,it.PayTypes,it.Price
                     });
            return this.GetEntities<PromotionEntity>(query);
        }
        #endregion
        /// <summary>
        /// 得到购物车
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ShopcartEntity> GetShopcarts(HomeModel model)
        {
            var query = new QueryInfo();
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex).Query<ShopcartEntity>().Where(it => it.Account.Id == Identity.Id)
                .OrderByDescending(it => it.Id)
                .Select(it => new object[] { it.Id,it.Count, it.Tag, it.Product.Id,it.Product.Name, it.Product.FileName, it.Product.Price, it.Product.Count,it.Product.OrderMinCount,it.Product.OrderStepCount});
            var infos = this.GetEntities<ShopcartEntity>(query);
            if (infos != null)
            {
                var promotions = GetPromotions(infos.Where(it => it.Product != null).Select(it => it.Product.Id).ToArray());
                if (promotions != null)
                {
                    foreach (var info in infos)
                    {
                        if (info.Product == null)
                            continue;
                        info.Product.Promotion = promotions.FirstOrDefault(it => it.Product != null && it.Product.Id == info.Product.Id);
                    }
                }
            }
            return infos;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected virtual bool UpdateShopcartCount(long id, int count)
        {
            var info = new ShopcartEntity
            {
                Id = id,
                Count = count,
                SaveType = SaveType.Modify
            };
            info.SetProperty(it => it.Count);
            return this.SaveEntity(info);
        }
    }
}
