using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Cloud.Site.Admin.Models.Inquery;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Inquery
{
    [SiteAuthorizeFilter]
    public class InqueryController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model=new InqueryListModel();
       
            return View("~/Views/Inquery/index.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int? page)
        {
            var model = new InqueryListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };

            model.Inqueries = GetInqueries(model);
            if (model.Inqueries == null || model.Inqueries.Count == 0)
                return Content("");
            return View("~/Views/Inquery/_Inquery.cshtml", model);
        }



        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<InqueryEntity> GetInqueries(InqueryListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex)
                .Query<InqueryEntity>().Where(it=>it.Site.Id==SiteId).OrderByDescending(it=>it.Id)
                .Select(it => new object[] {it.Id, it.Content,it.Linkman,it.Mobile,it.InsertTime});
            return this.GetEntities<InqueryEntity>(query);
        }


    

        #region 删除
        [SiteDataFilter(EntityType = typeof(InqueryEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<InqueryEntity>();
                foreach (var i in id)
                {
                    var info = new InqueryEntity
                    {
                        Id = i.Convert<long>(),
                        SaveType = SaveType.Remove
                    };
                    infos.Add(info);
                }
                rev = this.SaveEntities(infos);
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }
        #endregion
    }
}
