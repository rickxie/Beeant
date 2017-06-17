using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Website.Models.Home
{
    public class HomeModel
    {
        /// <summary>
        /// 横幅
        /// </summary>
        public IList<BannerEntity> Banners { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public IList<TagEntity> Tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commodityTag"></param>
        /// <returns></returns>
        public virtual string GetFullFileName(CommodityTagEntity commodityTag)
        {
            if (commodityTag == null || commodityTag.Commodity == null ||
                string.IsNullOrEmpty(commodityTag.Commodity.FileName))
                return SharedHelperExtension.GetNoPicture();
            if (!string.IsNullOrEmpty(commodityTag.Commodity.Password))
                return string.Format("{0}/Images/lock.png", Configuration.ConfigurationManager.GetSetting<string>("CloudSiteMobileUrl"));
            return string.Format("{0}?v={1}", commodityTag.Commodity.GetFullFileName(commodityTag.Commodity.FileName, "l"), DateTime.Now.Ticks);
        }
    }
}
