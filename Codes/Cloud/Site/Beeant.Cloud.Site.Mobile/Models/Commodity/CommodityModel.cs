using System.Collections.Generic;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Mobile.Models.Commodity
{
    public class CommodityParamModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Passowrd { get; set; }
    }

    public class CommodityModel
    {
        public IList<CommodityParamModel> Params { get; set; }

        /// <summary>
        /// 详情页
        /// </summary>
        public IList<CommodityEntity> Commodities { get; set; }
    }
}
