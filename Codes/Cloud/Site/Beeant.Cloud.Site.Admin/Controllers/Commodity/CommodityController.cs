using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Site.Admin.Models.Commodity;
using Beeant.Domain.Entities.Site;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Admin.Controllers.Commodity
{
    [SiteAuthorizeFilter]
    public class CommodityController : SiteAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model=new CommodityListModel();
            model.Catalogs = GetCatalogs();
            model.Tags = GetTags();
            model.Site = this.GetEntityByIdentity<SiteEntity>();
            return View("~/Views/Commodity/index.cshtml",model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string key,string catalogId, string tagId, int? page)
        {
            var model = new CommodityListModel
            {
                Key=key,
                PageIndex = page.HasValue?page.Value:0
            };
            if (!string.IsNullOrEmpty(catalogId))
                model.CatalogId = catalogId.Convert<long>();
            if (!string.IsNullOrEmpty(tagId))
                model.TagId = tagId.Convert<long>();
            model.Commodities = GetCommodities(model);
            if (model.Commodities == null || model.Commodities.Count == 0)
                return Content("");
            return View("~/Views/Commodity/_Commodity.cshtml", model);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CatalogEntity> GetCatalogs()
        {
            var query = new QueryInfo();
            query.Query<CatalogEntity>().Where(it=>it.Site.Id==SiteId)
                .OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Name });
            return this.GetEntities<CatalogEntity>(query);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<TagEntity> GetTags()
        {
            var query = new QueryInfo();
            query.Query<TagEntity>().Where(it => it.Site.Id == SiteId)
                .OrderBy(it => it.Sequence).Select(it => new object[] { it.Id, it.Name });
            return this.GetEntities<TagEntity>(query);
        }
        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CommodityEntity> GetCommodities(CommodityListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<CommodityEntity>()
                .Where(it=>it.Site.Id==SiteId).
                OrderBy(it=>it.Sequence).Select(it => new object[] {it.Id, it.Name,it.FileName,it.Price,it.Cost, it.Sequence});
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
        /// 首页
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Get(long id)
        {
            var query=new QueryInfo();
            query.Query<CommodityEntity>()
                .Where(it => it.Id == id && it.Site.Id == SiteId)
                .Select(it => new object[] {it,
                    it.CommodityImages.Select(s => new object[] {s.Id, s.Sequence,s.FileName})
                    ,it.CommodityTags.Select(s=>new object[] {s.Tag.Id,s.Tag.Name})});
            var entities = this.GetEntities<CommodityEntity>(query);
            var entity = entities?.FirstOrDefault();
            if (entity == null)
                return null;
            var model = new CommodityModel
            {
                Id = entity.Id.ToString(),
                CatalogId = entity.Catalog == null ? "" : entity.Catalog.Id.ToString(),
                Name = entity.Name,
                Cost = entity.Cost,
                Price = entity.Price,
                VenderName = entity.VenderName,
                VenderLinkman = entity.VenderLinkman,
                VenderMobile = entity.VenderMobile,
                VenderAddress = entity.VenderAddress,
                FileName = entity.FullFileName,
                Status=entity.Status,
                IsShowPrice=entity.IsShowPrice,
                Password=entity.Password,
                Description = entity.Description,
                AlbumFileName = entity.AlbumFullFileName,
                IsCreateAlbum=entity.IsCreateAlbum
            };
            if (entity.CommodityImages != null)
            {
                entity.CommodityImages = entity.CommodityImages.OrderBy(it => it.Sequence).ToList();
                model.Images=new List<CommodityImageModel>();
                foreach (var image in entity.CommodityImages)
                {
                    model.Images.Add(new CommodityImageModel
                    {
                        Id = image.Id.ToString(),
                        FileName = image.FullFileName
                    });
                }
            }
            if (entity.CommodityTags != null)
            {
 
                model.Tags = new List<CommodityTagModel>();
                foreach (var tag in entity.CommodityTags)
                {
                    if(tag.Tag==null)
                        continue;
                    model.Tags.Add(new CommodityTagModel
                    {
                        Id = tag.Tag.Id.ToString(),
                        Name = tag.Tag.Name
                    });
                }
            }
            return this.Jsonp(model);
        }

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileValue"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult UpdateImage(string fileName, string fileValue)
        {
            var fileroute = Winner.Creator.Get<Winner.Storage.IFile>();
            fileName = string.Format("Files/Temp/{0}", fileName);
            fileName = fileroute.CreateFileName(fileName);
            var fileByte = Convert.FromBase64String(fileValue);
            var rev = true;
            var product = new CommodityEntity
            {
                FileByte = fileByte,
                FileName = fileName
            };
            var mess = product.FullFileName;
            var errors = Winner.Creator.Get<IValidation>()
                .ValidateInfo(product, ValidationType.Modify, new List<string> { "FileName", "FileByte" });
            if (errors != null && errors.Count > 0)
            {
                rev = false;
                mess = errors[0].Message;
            }
            else
            {
                Ioc.Resolve<IFileApplicationService>().Save(fileName, fileByte);

            }
            var result = new Dictionary<string, object>
            {
                {"Status", rev},
                {"Message", mess}
            };
            return this.Jsonp(result);
        }
        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(CommodityModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Add,null,null);
            entity.Sequence = GetSequence();
            var result = new Dictionary<string, object>();
            entity.Site = new SiteEntity { Id =SiteId };
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status",rev);
            result.Add("Id", entity.Id);
            result.Add("Sequence", entity.Sequence);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }
        /// <summary>
        /// D
        /// </summary>
        /// <returns></returns>
        protected virtual int GetSequence()
        {
            var query=new QueryInfo();
            query.SetPageSize(1).Query<CommodityEntity>().Where(it=>it.Site.Id==SiteId)
                .OrderBy(it => it.Sequence).Select(it => it.Sequence);
            var entities = this.GetEntities<CommodityEntity>(query);
            var entity = entities?.FirstOrDefault();
            if (entity != null)
            {
                if (entity.Sequence - 5000 < -100000000)
                    return -100000000;
                return entity.Sequence - 5000;
            }
            return 100000000;
        }
        #endregion

        #region 修改
        [SiteDataFilter(EntityType = typeof(CommodityEntity))]
        [HttpPost]
        public virtual ActionResult Modify(CommodityModel model)
        {
            if (model == null)
                return null;
            var dataImages = GetCommodityImages(model.Id.Convert<long>());
            var dataTags= GetCommodityTags(model.Id.Convert<long>());
            var entity = model.CreateEntity(SaveType.Modify, dataImages, dataTags);
            var result = new Dictionary<string, object>();
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        protected virtual IList<CommodityImageEntity> GetCommodityImages(long commodityId)
        {
            var query=new QueryInfo();
            query.Query<CommodityImageEntity>()
                .Where(it => it.Commodity.Id == commodityId).Select(it => new object[] {it.Id});
            return this.GetEntities<CommodityImageEntity>(query);
        }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        protected virtual IList<CommodityTagEntity> GetCommodityTags(long commodityId)
        {
            var query = new QueryInfo();
            query.Query<CommodityTagEntity>()
                .Where(it => it.Commodity.Id == commodityId).Select(it => new object[] { it.Id,it.Tag.Id });
            return this.GetEntities<CommodityTagEntity>(query);
        }
        #endregion

        #region 删除
        [SiteDataFilter(EntityType = typeof(CommodityEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<CommodityEntity>();
                foreach (var i in id)
                {
                    var info = new CommodityEntity
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

        #region 修改类目
        [SiteDataFilter(EntityType = typeof(CommodityEntity))]
        public virtual ActionResult UpdataCatalog(string[] id,string catalogId)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<CommodityEntity>();
                foreach (var i in id)
                {
                    var info = new CommodityEntity
                    {
                        Id = i.Convert<long>(),
                        Catalog=new CatalogEntity { Id = catalogId.Convert<long>()},
                        SaveType = SaveType.Modify
                    };
                    info.SetProperty(it => it.Catalog.Id);
                    infos.Add(info);
                }
                rev = this.SaveEntities(infos);
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }
        #endregion

        #region 修改状态
        [SiteDataFilter(EntityType = typeof(CommodityEntity))]
        public virtual ActionResult UpdataStatus(string[] id, int status)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<CommodityEntity>();
                foreach (var i in id)
                {
                    var info = new CommodityEntity
                    {
                        Id = i.Convert<long>(),
                        Status = (CommodityStatusType)status,
                        SaveType = SaveType.Modify
                    };
                    info.SetProperty(it => it.Status);
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
