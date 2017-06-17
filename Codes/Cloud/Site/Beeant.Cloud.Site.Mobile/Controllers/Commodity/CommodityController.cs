using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.Extension.Mobile;
using Beeant.Cloud.Site.Mobile.Models.Commodity;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Mobile.Controllers.Commodity
{
    public class CommodityController : MobileSiteBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string catalogId,string tagId,string key)
        {
            var model = new CommodityListModel
            {
                CatalogId = string.IsNullOrWhiteSpace(catalogId)?null: (long?)catalogId.Convert<long>(),
                TagId = string.IsNullOrWhiteSpace(tagId) ? null : (long?)tagId.Convert<long>(),
                Key =key,
                Wechat = GetWechatModel()
            };
            return View(GetViewPath("~/Views/Commodity/index.cshtml"), model);
        }
        /// <summary>
        ///得到js验证
        /// </summary>
        /// <returns></returns>
        protected virtual WechatModel GetWechatModel()
        {
            if (!Site.IsOpenImages)
                return null;
            var url = Request.Url.AbsoluteUri;
            var siteWechat = this.Wechat(null);
            string key = Winner.Creator.Get<Winner.Base.ISecurity>().EncryptMd5(string.Format("{0}{1}", siteWechat.AppId, url));
            var rev = Ioc.Resolve<ICacheApplicationService>().Get<IDictionary<string, string>>(key);
            if (rev == null)
            {
                rev = siteWechat.GetJsSdkSignature(url);
                if (rev == null)
                    return null;
                Ioc.Resolve<ICacheApplicationService>().Set(key, rev, DateTime.Now.AddSeconds(7100));
            }
            var model = new WechatModel
            {
                AppId = siteWechat.AppId,
                Timestamp = rev["timestamp"],
                NonceStr = rev["noncestr"],
                Signature = rev["signature"]
            };
            return model;
        }
    
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string catalogId,string tagId, string key, int? page)
        {
            var model = new CommodityListModel
            {
                Key=key,
        
                PageIndex = page.HasValue ? page.Value : 0
            };
            if (!string.IsNullOrEmpty(catalogId))
                model.CatalogId = catalogId.Convert<long>();
            if (!string.IsNullOrEmpty(tagId))
                model.TagId = tagId.Convert<long>();
            model.Commodities = GetCommodities(model);
            if (model.Commodities == null || model.Commodities.Count == 0)
                return Content("");
            return View(GetViewPath("~/Views/Commodity/_Commodity.cshtml") , model);
        }
     
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CommodityEntity> GetCommodities(CommodityListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<CommodityEntity>()
                .Where(it => it.Site.Id == SiteId && it.Status==CommodityStatusType.Normal).
                OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Name,it.Description,it.IsShowPrice,it.Price, it.FileName,it.Password});
            if (model.CatalogId.HasValue)
            {
                query.Where(string.Format("{0} && Catalog.Id==@CatalogId", query.WhereExp))
                    .SetParameter("CatalogId", model.CatalogId);
            }
            if (model.TagId.HasValue)
            {
                query.Where(string.Format("{0} && CommodityTags.Count(Tag.Id==@TagId)>0", query.WhereExp))
                    .SetParameter("TagId", model.TagId);
            }
            if (!string.IsNullOrEmpty(model.Key))
            {
                query.Where(string.Format("{0} && (Name.Contains(@Key) || Description.Contains(@Key))", query.WhereExp))
                    .SetParameter("Key", model.Key);
            }
            return this.GetEntities<CommodityEntity>(query);
        }

        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetFileName(long id,string password)
        {
            var query=new QueryInfo();
            query.Query<CommodityEntity>().Where(it => it.Id == id && it.Site.Id == SiteId)
                .Select(it => new object[] { it.FileName ,it.Password} );
            var entities = this.GetEntities<CommodityEntity>(query);
            var enitty = entities?.FirstOrDefault();
            var dis=new Dictionary<string,object>();
            if (enitty != null && enitty.Password== password)
            {
                dis.Add("FileName", enitty.FullFileName);
            }
            return this.Jsonp(dis);
        }

        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Images(long id, string password)
        {
            var dis = new Dictionary<string, object>();
            if (string.IsNullOrWhiteSpace(password) || CheckPass(id, password))
            {
                var query = new QueryInfo();
                query.Query<CommodityImageEntity>().Where(it => it.Commodity.Id == id && it.Site.Id == SiteId)
                    .Select(it => new object[] {it.FileName});
                var entities = this.GetEntities<CommodityImageEntity>(query);

                if (entities != null)
                {
                    dis.Add("FileNames", entities.Select(it => it.FullFileName).ToArray());
                }
            }
            return this.Jsonp(dis);
        }
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected virtual bool CheckPass(long id, string password)
        {
            var query = new QueryInfo();
            query.Query<CommodityEntity>().Where(it => it.Id == id && it.Site.Id == SiteId && it.Password==password)
                .Select(it => new object[] { it.Id});
            var entities = this.GetEntities<CommodityEntity>(query);
            return entities?.FirstOrDefault() != null;
        }

        #region 详情页
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Detail(IList<CommodityParamModel> ps )
        {
            var model = new CommodityModel
            {
                Params = ps
            };

            return View(GetViewPath("~/Views/Commodity/Detail.cshtml"), model);
        }
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult DetailInfo(IList<CommodityParamModel> ps)
        {
            var model = new CommodityModel();
            if (ps != null && ps.Count > 0)
            {
                var id = ps.Select(it => it.Id.Convert<long>()).ToArray();
                var query = new QueryInfo();
                query.Query<CommodityEntity>().Where(it => id.Contains(it.Id) && it.Site.Id == SiteId & it.Status == CommodityStatusType.Normal)
                    .Select(it => new object[] { it.Id,it.Name, it.FileName, it.Password,it.Price,
                    it.IsShowPrice,it.Description,it.CommodityImages.
                    Select(s=>new object[]{s.FileName,s.Sequence}) });
                model.Commodities = this.GetEntities<CommodityEntity>(query);
                if (model.Commodities != null)
                {
                    for (int i = 0; i < model.Commodities.Count;)
                    {
                        var p = ps.FirstOrDefault(it => it.Id.Convert<long>() == model.Commodities[i].Id);
                        if (p==null || !string.IsNullOrEmpty(model.Commodities[i].Password) && model.Commodities[i].Password!=p.Id)
                        {
                            model.Commodities.RemoveAt(i);
                            continue;
                        }
                        i++;
                    }

                }
            }
            return View(GetViewPath("~/Views/Commodity/_DetailInfo.cshtml"), model);
        }
        /// <summary>
        /// 得到公司信息
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult WeixinQrCode()
        {
            var query = new QueryInfo();
            query.Query<CompanyEntity>().Where(it => it.Site.Id == SiteId).Select(it => it.WeixinQrCodeFileName);
            var entities = this.GetEntities<CompanyEntity>(query);
            var company= entities?.FirstOrDefault();
            if (company == null || string.IsNullOrEmpty(company.WeixinQrCodeFileName))
                return Content("");
            return Content(company.WeixinQrCodeFullFileName);
        }
        #endregion
    }
}
