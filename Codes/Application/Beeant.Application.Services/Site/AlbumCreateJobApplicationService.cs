using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Component.Extension;
using Beeant.Application.Services.Sys;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Site;
using Beeant.Domain.Entities.Sys;
using Beeant.Domain.Services;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Site
{
    public class AlbumCreateJobApplicationService : QueueJobApplicationService, ICreateAlbumApplicationService
    {
        public IDomainService BookDomainService { get; set; }
        public IDomainService CompanyDomainService { get; set; }
        protected override string Name { get; set; } = "CreateSiteAblum";
        public override int PageSize { get; set; } = 5;

        /// <summary>
        /// 处理队列
        /// </summary>
        /// <param name="entity"></param>
        protected override bool Handle(QueueEntity entity)
        {
            var json = entity.Value.DeserializeJson<Dictionary<string, string>>();
            if (json == null || !json.ContainsKey("SiteId") || !json.ContainsKey("AlbumId"))
                return true;
            Create(json["SiteId"].Convert<long>(), json["AlbumId"].Convert<long>());
            return true;
        }

        /// <summary>
        /// 创建 
        /// </summary>
        /// <returns></returns>
        protected virtual void Create(long siteId, long albumId)
        {
            var album = Repository.Get<AlbumEntity>(albumId);
            if (album == null)
                return ;
            SetBooks(siteId, albumId);
            var timespan = DateTime.Now.Ticks.ToString();
            var mark = Winner.Creator.Get<Winner.Base.ISecurity>().EncryptSign(timespan);
            var urls = GetUrls(siteId,album);
            foreach (var url in urls)
            {
                WebsiteImageHelper.Create().Generate(string.Format("{0}&siteId={1}&timespan={2}&mark={3}",
                    url, siteId, timespan, mark), Save, 80L);
            }
        }
        /// <summary>
        /// 得到商品
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CatalogEntity> GetCatalogs(long siteId)
        {
            var query = new QueryInfo();
            query.Query<CatalogEntity>().Where(it => it.Site.Id == siteId).OrderBy(it => it.Sequence)
                .Select(it => new object[] { it.Id, it.Name });
            return Repository.GetEntities<CatalogEntity>(query);
        }
        /// <summary>
        /// 得到商品
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CommodityEntity> GetCommodities(long catalogId, int pageSize, int pageIndex)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(pageSize).SetPageIndex(pageIndex).Query<CommodityEntity>()
                .OrderBy(it => it.Sequence)
                .Where(it => it.Catalog.Id == catalogId && it.IsCreateAlbum)
                .Select(it => new object[] { it.Id });
            return Repository.GetEntities<CommodityEntity>(query);
        }
        /// <summary>
        /// 得到地址
        /// </summary>
        /// <returns></returns>
        protected virtual IList<string> GetUrls(long siteId, AlbumEntity album)
        {
            var domain = Configuration.ConfigurationManager.GetSetting<string>("CloudSiteAdminUrl");
            var result = new List<string>();
            result.Add(string.Format("{0}/Book/CreateFrontCover?albumId={1}", domain, album.Id));
            result.Add(string.Format("{0}/Book/CreateAbout?albumId={1}", domain, album.Id));
            var catalogs = GetCatalogs(siteId);
            if (catalogs != null)
            {
                foreach (var catalog in catalogs)
                {
                    var pageIndex = 0;
                    while (true)
                    {
                        var comdities = GetCommodities(catalog.Id, album.PageSize, pageIndex);
                        if (comdities == null || comdities.Count == 0)
                            break;
                        var url = string.Format("{0}/Book/CreateCommodity?albumId={1}&catalogId={2}&ids={3}", domain,
                            album.Id, pageIndex==0?catalog.Id:0, string.Join(",", comdities.Select(it => it.Id).ToArray()));
                        result.Add(url);
                        pageIndex++;
                    }
                }
            }
            result.Add(string.Format("{0}/Book/CreateBackCover?albumId={1}", domain, album.Id));
            return result;
        }


        /// <summary>
        ///  设置
        /// </summary>
        protected virtual void SetBooks(long siteId,long albumId)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.Query<BookEntity>().Where(it=>it.Site.Id== siteId)
                .Select(it => new object[] { it.Id });
            var entities =  Repository.GetEntities<BookEntity>(query);
            if (entities == null)
                return;
            foreach (var entity in entities)
            {
                entity.IsUsed = false;
                entity.SaveType = SaveType.Modify;
                entity.SetProperty(it => it.IsUsed);
            }
            var unitofworks = BookDomainService.Handle(entities);
            var companyUnitofworks = GetCompanyUnitofworks(siteId, albumId);
            if(companyUnitofworks!=null)
                unitofworks.AddList(companyUnitofworks);
            Commit(unitofworks);
        }

        /// <summary>
        /// 得到公司信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="albumId"></param>
        protected virtual IList<IUnitofwork> GetCompanyUnitofworks(long siteId, long albumId)
        {
            var query = new QueryInfo();
            query.Query<CompanyEntity>().Where(it => it.Site.Id == siteId).Select(it => it.Id);
            var company = Repository.GetEntities<CompanyEntity>(query)?.FirstOrDefault();
            if (company == null)
                return null;
            company.Album = new AlbumEntity {Id = albumId};
            company.SetProperty(it => it.Album.Id);
            company.SaveType = SaveType.Modify;
            return CompanyDomainService.Handle(company);
        }

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileBytes"></param>
        protected virtual void Save(string url, byte[] fileBytes)
        {
            var siteId = HttpUtility.ParseQueryString(url).Get("siteid").Convert<long>();
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(1).Query<BookEntity>().Where(it => it.Site.Id == siteId && it.IsUsed == false)
                .Select(it => new object[] { it.Id, it.FileName });
            var entity = Repository.GetEntities<BookEntity>(query)?.FirstOrDefault();
            if (entity != null)
            {
                entity.FileByte = fileBytes;
                entity.IsUsed = true;
                entity.SetProperty(it => it.IsUsed).SetProperty(it => it.FileName);
                entity.SaveType = SaveType.Modify;
            }
            else
            {
                entity = new BookEntity();
                entity.FileName = "/Files/Images/SiteBook/copy.jpg";
                entity.FileByte = fileBytes;
                entity.IsUsed = true;
                entity.Site = new SiteEntity { Id = siteId };
                entity.SaveType = SaveType.Add;
            }
            var unitofworks = BookDomainService.Handle(entity);
            Commit(unitofworks);
        }
        /// <summary>
        /// 添加队列
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public virtual bool Add(long siteId, long albumId)
        {
            var value=new Dictionary<string,string>();
            value.Add("SiteId",siteId.ToString());
            value.Add("AlbumId", albumId.ToString());
            return Add(value.SerializeJson());

        }
    }
}
