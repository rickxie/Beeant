using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Product;
using Beeant.Application.Services.Search;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Search;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Presentation.Website.Search.Models.Home;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Winner.Search;

namespace Beeant.Presentation.Website.Search.Controllers.Home
{
    public class HomeController : BaseController
    {

        #region 搜索

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="searchkey"></param>
        /// <param name="relateKey"></param>
        /// <param name="oname"></param>
        /// <param name="ps"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual ActionResult Index(string searchkey, string relateKey, string oname, string ps, int? page)
        {
            var model = new GoodsListModel
            {
                ActionName = "Search",
                SearchKey = string.IsNullOrEmpty(searchkey) ? "" : searchkey.Trim(),
                PropertyName = Server.UrlDecode(ps),
                OrderbyName = oname,
                PageIndex = page.HasValue ? page.Value : 0,
            };
            try
            {
                model.SetExistSearchProperties(model.PropertyName);
                var searchQuery = new SearchQueryInfo { Name = "Goods", Key = model.SearchKey };
                var result = Ioc.Resolve<ISearchApplicationService>().Search(searchQuery);
                FillSearchGoods(model, result);
                FillCategory(model);
                SetSearchPropertiesByCategory(model);
                SaveKey(searchkey, relateKey);
                SetSearchView(model, result);
            }
            catch (Exception)
            {

            }
            return View("/Views/Home/index.cshtml", model);
        }
        /// <summary>
        /// 设置搜索
        /// </summary>
        /// <param name="model"></param>
        protected virtual void SetSearchPropertiesByCategory(GoodsListModel model)
        {
            var query = new QueryInfo();
            query.SetCacheTime(DateTime.Now.AddDays(1)).Query<PropertyEntity>()
                      .Where(it => it.Category.Id == model.CategoryId && it.SearchType != PropertySearchType.None).OrderByDescending(it => it.Sequence);
            model.SearchProperties = this.GetEntities<PropertyEntity>(query);
        }
        /// <summary>
        /// 设置搜索
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        protected virtual void SetSearchView(GoodsListModel model, SearchResultInfo result)
        {
            ViewBag.SearchKey = model.SearchKey;
            if (result == null || result.Words == null || result.Words.Count == 0)
                return;
            var names = result.Words.Select(it => it.Name).ToArray();
            var query = new QueryInfo();
            query.SetCacheTime(DateTime.Now.AddDays(1)).SetPageIndex(0).SetPageSize(5).Query<SimilarEntity>().Where(it => names.Contains(it.Word.Name)).OrderByDescending(it => it.Count);
            var infos = this.GetEntities<SimilarEntity>(query);
            if (infos != null)
            {
                ViewBag.HotKeys = infos.Select(it => it.Name).ToList();
            }
        }

        /// <summary>
        /// 填充类目
        /// </summary>
        /// <param name="model"></param>
        protected virtual void FillCategory(GoodsListModel model)
        {
            var query = new QueryInfo();
            query.Query<CategoryEntity>().Where(it => it.Name == model.SearchKey).Select(it => it.Id);
            var info = this.GetEntities<CategoryEntity>(query).FirstOrDefault();
            if (info == null) return;
            model.CategoryId = info.Id;
        }


        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="result"></param>
        protected virtual void FillSearchGoods(GoodsListModel model, SearchResultInfo result)
        {
            if (result == null || result.Documents == null)
                return;
            var ids = new List<long>();
            var documents = result.Documents.Skip(model.PageIndex * model.PageSize).Take(model.PageSize);
            foreach (var document in documents)
            {
                ids.Add(document.Feilds[0].Text.Convert<long>());
            }
            model.GoodsList = Ioc.Resolve<IGoodsApplicationService>().GetGoodsByCache(ids.ToArray());
            if (model.ExistSearchProperties != null)
            {
                foreach (var esp in model.ExistSearchProperties)
                {
                    if (esp.Name.Contains("价格"))
                    {
                        var values = esp.Value.Split('-');
                        if (values.Length != 2)
                            continue;
                        model.GoodsList =
                      model.GoodsList.Where(it => it.GoodsProperties != null && it.GoodsProperties.Count(s => s.Id == esp.Id
                          && s.Value.Convert<decimal>() >= values[0].Convert<decimal>() && s.Value.Convert<decimal>() <= values[1].Convert<decimal>()) > 0)
                           .ToList();
                    }
                    else
                    {
                        model.GoodsList =
                      model.GoodsList.Where(it => it.GoodsProperties != null && it.GoodsProperties.Count(s => s.Id == esp.Id
                          && s.Value.Contains(esp.Value)) > 0)
                           .ToList();
                    }
                }
            }
            var dataCount = result.Documents.Count;
            model.DataCount = dataCount > int.MaxValue ? int.MaxValue : dataCount;
            OrderbySearchGoods(model);
        }

        /// <summary>
        /// 得到搜索目录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual void OrderbySearchGoods(GoodsListModel model)
        {
            if (model.GoodsList == null)
                return;
            switch (model.OrderbyName)
            {
                case "prasc":
                    model.GoodsList =
                        model.GoodsList.OrderBy(it => it.Price).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "prdesc":
                    model.GoodsList =
                         model.GoodsList.OrderByDescending(it => it.Price).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "ptasc":
                    model.GoodsList =
                         model.GoodsList.OrderBy(it => it.PublishTime).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "ptdesc":
                    model.GoodsList =
                                     model.GoodsList.OrderByDescending(it => it.PublishTime).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "vcasc":
                    model.GoodsList =
                                      model.GoodsList.OrderBy(it => it.VisitCount).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "vcdesc":
                    model.GoodsList =
                                    model.GoodsList.OrderByDescending(it => it.VisitCount).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "acasc":
                    model.GoodsList =
                                   model.GoodsList.OrderBy(it => it.AttentionCount).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "acdesc":
                    model.GoodsList =
                                    model.GoodsList.OrderByDescending(it => it.AttentionCount).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "scasc":
                    model.GoodsList =
                                    model.GoodsList.OrderBy(it => it.SalesCount).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;
                case "scdesc":
                    model.GoodsList =
                                    model.GoodsList.OrderByDescending(it => it.SalesCount).Take(model.PageSize).Skip(model.PageIndex).ToList();
                    break;

            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="relateKey"></param>
        /// <returns></returns>
        protected virtual bool SaveKey(string key, string relateKey)
        {
            var info = new KeyEntity
            {
                SaveType = SaveType.Add,
                Name = key,
                Ip = System.Web.HttpContext.Current.Request.UserHostAddress,
                Source = "B2C"
            };
            Ioc.Resolve<IApplicationService, KeyEntity>()
               .Save(info);
            if (string.IsNullOrEmpty(relateKey) || key == relateKey) return true;
            Ioc.Resolve<IApplicationService, RelateKeyEntity>()
               .Save(new RelateKeyEntity
               {
                   SaveType = SaveType.Add,
                   KeyName = key,
                   Name = relateKey,
                   Ip = System.Web.HttpContext.Current.Request.UserHostAddress,
                   Source = "B2C"
               });
            return true;
        }
        #endregion

    }
}
