using System.Collections.Generic;
using System.Linq;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Site;


namespace Beeant.Cloud.Site.Mobile.Models.Catalog
{
    public class CatalogListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 24; }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public IList<CatalogEntity> Catalogs { get; set; }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <returns></returns>
        public virtual string CatalogFullFileName(CatalogEntity catalog)
        {
            if (!string.IsNullOrEmpty(catalog.FileName))
                return catalog.FullFileName;
            if (catalog.Commodities == null || catalog.Commodities.Count == 0)
                return SharedHelperExtension.GetNoPicture();
            var commodity = catalog.Commodities.First();
            if (string.IsNullOrEmpty(commodity.FileName))
                return SharedHelperExtension.GetNoPicture();
            if (!string.IsNullOrEmpty(commodity.Password))
                return string.Format("{0}/Images/lock.png", Configuration.ConfigurationManager.GetSetting<string>("CloudSiteMobileUrl"));
            return commodity.GetFullFileName(commodity.FileName, "i");
        }
    }
}
