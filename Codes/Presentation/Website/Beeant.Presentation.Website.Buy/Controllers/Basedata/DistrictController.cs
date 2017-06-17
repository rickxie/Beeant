using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.DrowDownList;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Buy.Controllers.Basedata
{
    [AuthorizeFilter]
    public class DistrictController : Controller
    {
        /// <summary>
        /// 下拉框
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult DropDownList(DropDownListModel model)
        {
            return View("DropDownList/_District",model);
        }

        /// <summary>
        /// 得到区域
        /// </summary>
        /// <returns></returns>
        public virtual string GetAllDistricts()
        {
            var infos = GetDistricts();
            if (infos == null) return "[]";
            var json =
                infos.OrderByDescending(it => it.Sequence)
                     .Select(
                         info =>
                         new Dictionary<string, object>
                             {
                                 {"Id", info.Id},
                                 {"Name", info.Name},
                                 {"ParentId", info.Parent == null ? 0 : info.Parent.Id}
                             })
                     .Cast<object>()
                     .ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 得到区域
        /// </summary>
        /// <returns></returns>
        protected virtual IList<DistrictEntity> GetDistricts()
        {
            var query = new QueryInfo();
            query.SetCacheTime(DateTime.MaxValue).Query<DistrictEntity>();
            return this.GetEntities<DistrictEntity>(query);
        }
    }
}
