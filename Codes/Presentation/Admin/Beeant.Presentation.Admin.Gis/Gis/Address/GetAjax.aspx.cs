using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Gis;
using Beeant.Basic.Services.WebForm.Pages;
using Dependent;
using Beeant.Application.Services.Gis;

namespace Beeant.Presentation.Admin.Gis.Gis.Address
{
    public partial class GetAjax : AjaxPageBase<AreaEntity>
    {
        /// <summary>
        /// 城市
        /// </summary>
        public virtual string City
        {
            get { return Server.UrlDecode(Request.QueryString["city"]); }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address
        {
            get { return Server.UrlDecode(Request.QueryString["address"]); }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Type
        {
            get { return Server.UrlDecode(Request.QueryString["Type"]); }
        }
        protected override void WriteEntities()
        {
            if(string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(City))
                return;
            var match = Ioc.Resolve<IAreaApplicationService>().Match(City, Address, "");
            var result = new Dictionary<string, object>();
            result.Add("Result", new Dictionary<string, object> { { "lng", match.Lng }, { "lat", match.Lat } });
            result.Add("Areas",match.Areas==null?null:match.Areas.Select(it=>it.Name).ToArray());
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));

        }
      
      
    }
}