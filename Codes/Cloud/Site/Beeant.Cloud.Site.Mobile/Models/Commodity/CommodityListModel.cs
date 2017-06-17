using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Site;


namespace Beeant.Cloud.Site.Mobile.Models.Commodity
{
    public class CommodityListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 24; }
        }
        /// <summary>
        /// 类目
        /// </summary>
        public long? CatalogId { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public long? TagId { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public IList<CommodityEntity> Commodities { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public WechatModel Wechat { get; set; }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <returns></returns>
        public virtual string CatalogFullFileName(CatalogEntity catalog)
        {
            if (catalog.Commodities == null || catalog.Commodities.Count == 0)
                return "";
            var commodity = catalog.Commodities.First();
            if (string.IsNullOrEmpty(commodity.FileName))
                return "";
            return commodity.GetFullFileName(commodity.FileName, "i");
        }
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullFileName(CommodityEntity commodity)
        {
            if (!string.IsNullOrEmpty(commodity.Password))
                return string.Format("{0}/Images/lock.png",Configuration.ConfigurationManager.GetSetting<string>("CloudSiteMobileUrl"));
            return string.Format("{0}?v={1}", commodity.GetFullFileName(commodity.FileName, "i"), DateTime.Now.Ticks);
        }
        /// <summary>
        /// 得到头部名称
        /// </summary>
        /// <returns></returns>
        public virtual string GetToperName()
        {
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["catalogname"]))
                return System.Web.HttpContext.Current.Request["catalogname"];
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["tagname"]))
                return System.Web.HttpContext.Current.Request["tagname"];
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["key"]))
                return System.Web.HttpContext.Current.Request["key"];
            return "";
        }
    }
}
