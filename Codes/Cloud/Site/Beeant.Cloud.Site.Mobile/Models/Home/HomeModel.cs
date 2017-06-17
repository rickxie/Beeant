using System;
using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Mobile.Models.Home
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

        public virtual string GetFullFileName(CommodityTagEntity commodityTag)
        {
            if (commodityTag == null || commodityTag.Commodity == null ||
                string.IsNullOrEmpty(commodityTag.Commodity.FileName))
                return SharedHelperExtension.GetNoPicture();
            if (!string.IsNullOrEmpty(commodityTag.Commodity.Password))
                return string.Format("{0}/Images/lock.png", Configuration.ConfigurationManager.GetSetting<string>("CloudSiteMobileUrl"));
            return string.Format("{0}?v={1}", commodityTag.Commodity.GetFullFileName(commodityTag.Commodity.FileName, "i"), DateTime.Now.Ticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public virtual string GetFullFileName(TagEntity tag)
        {
            if (tag == null || tag.CommodityTags==null || tag.CommodityTags.Count == 0 || tag.CommodityTags[0].Commodity==null ||
                string.IsNullOrEmpty(tag.CommodityTags[0].Commodity.FileName))
                return SharedHelperExtension.GetNoPicture();
            var commodity = tag.CommodityTags[0].Commodity;
            if (!string.IsNullOrEmpty(commodity.Password))
                return string.Format("{0}/Images/lock.png", Configuration.ConfigurationManager.GetSetting<string>("CloudSiteMobileUrl"));
            return string.Format("{0}?v={1}", commodity.GetFullFileName(commodity.FileName, "i"), DateTime.Now.Ticks);
        }
    }
}
