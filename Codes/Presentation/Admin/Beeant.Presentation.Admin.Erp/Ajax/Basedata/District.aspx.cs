using System;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Ajax.Basedata
{
    public partial class District : AjaxPageBase<DistrictEntity>
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["DistrictId"]))
            {
                base.Page_Load(sender, e);
            }
        }

        protected override string GetListItem(DistrictEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Id);
        }

        protected override System.Collections.Generic.IList<DistrictEntity> GetEntities()
        {
            var query = new QueryInfo { FromExp = typeof(DistrictEntity).FullName };
            SetQuery(query);
            return Ioc.Resolve<IApplicationService, DistrictEntity>().GetEntities<DistrictEntity>(query);
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["DistrictId"]))
            {
                query.Where("Parent.Id==@DistrictId&&IsUsed==1");
                query.SetParameter("DistrictId", Server.UrlDecode(Request.QueryString["DistrictId"]));
            }

        }

        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id, Name";
        }
    }
}