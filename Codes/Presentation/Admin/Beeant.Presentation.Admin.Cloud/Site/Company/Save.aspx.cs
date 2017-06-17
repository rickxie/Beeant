using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Cloud.Site.Company
{
    public partial class Save : DatumPageBase<CompanyEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set { base.IsUpdatePanel = true; }
        }

        /// <summary>
        /// 得到信息
        /// </summary>
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
                
            }
            set { base.IsFillAllEntity = value; }
        }

        public long SiteId
        {
            get {return Request.QueryString["SiteId"].Convert<long>(); }
        }
  
        /// <summary>
        /// 得到信息
        /// </summary>
        /// <returns></returns>
        protected override CompanyEntity GetEntity()
        {
            return GetCompany();
        }

        /// <summary>
        /// 得到公司信息
        /// </summary>
        /// <returns></returns>
        protected virtual CompanyEntity GetCompany()
        {
          
            var query=new QueryInfo();
            query.Query<CompanyEntity>().Where(it => it.Site.Id == SiteId);
            var infos = Ioc.Resolve<IApplicationService, CompanyEntity>().GetEntities<CompanyEntity>(query);
            return infos?.FirstOrDefault();
        }
   
        /// <summary>
        /// 填充
        /// </summary>
        /// <returns></returns>
        protected override CompanyEntity FillEntity()
        {
            var entity= base.FillEntity();
            var company = GetCompany();
            if (company != null)
            {
                entity.SaveType = SaveType.Modify;
                entity.Id = company.Id;
            }
            else
            {
                entity.Properties = null;
                entity.SaveType=SaveType.Add;
                entity.Site = new SiteEntity {Id = SiteId};
                entity.Name = "";
                entity.Address = "";
                entity.Detail = "";
                entity.Email = "";
                entity.Fax = "";
                entity.Linkman = "";
                entity.Mobile = "";
                entity.Qq = "";
                entity.Album=new AlbumEntity();
            }
            return entity;
        }
    }
}