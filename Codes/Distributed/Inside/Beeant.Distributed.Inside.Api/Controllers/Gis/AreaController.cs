using System.Collections.Generic;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services.Gis;

namespace Beeant.Distributed.Inside.Api.Controllers.Gis
{

    public class AreaController : Controller
    {
        //
        // GET: /Test/
 
        public virtual ActionResult Get(string city,string address,string tag)
        {
            var result = new List<IDictionary<string, object>>();
            var match = Ioc.Resolve<IAreaApplicationService>().Match(city, address, tag);
            if (match!=null && match.Areas  != null)
            {
                foreach (var info in match.Areas)
                {
                    result.Add(new Dictionary<string, object> { { "Name", info.Name }, { "Value", info.Value }, { "Tag", info.Tag } });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
