using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services.Search;
using Beeant.Domain.Entities.Search;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Presentation.Mvc.Shared.Models.Home;

using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Shared.Controllers.Home
{
    public class HomeController : BaseController
    {

        /// <summary>
        /// 顶部
        /// </summary>
        /// <returns></returns>
        public ActionResult Header()
        {
            return View("~/Views/Home/_Header.cshtml");
        }
        /// <summary>
        /// 底部
        /// </summary>
        /// <returns></returns>
        public ActionResult Footer()
        {
            return View("~/Views/Home/_Footer.cshtml");
        }
    
        #region 头部
        /// <summary>
        /// 头部
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Searcher(bool? isExpand)
        {
            var model = new SearchModel
            {
                HotKeys = GetHotWord(),
                IsExpand = isExpand.HasValue && isExpand.Value
            };

            return View("~/Views/Home/_Searcher.cshtml", model);
        }


        /// <summary>
        /// 获取热词
        /// </summary>
        /// <returns></returns>
        protected virtual IList<string> GetHotWord()
        {
            var query = new QueryInfo();
            query.SetCacheName("热词缓存").SetCacheTime(DateTime.Now.AddDays(1)).SetPageSize(6)
                 .Query<WordEntity>()
                 .OrderByDescending(it => it.Count)
                 .Where(it => !it.IsForbid)
                 .Select(it => it.Name);
            var infos = this.GetEntities<WordEntity>(query);
            if (infos != null) return infos.Select(it => it.Name).ToList();
            return new List<string>();
        }

        #endregion
        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public virtual string QuickSearch(string key)
        {
            key = key.Trim();
            if (string.IsNullOrEmpty(key))
                return null;
            var model = new List<QuickSearchModel>();
            var words = Ioc.Resolve<ISearchApplicationService>().QuickSearch("Goods", key);
            if (words == null) return null;
            model.AddRange(
                words
                    .Select(it => new QuickSearchModel { Name = it.Name, DataCount = it.DocumentCount }));
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
    }
}
