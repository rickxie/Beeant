using System;
using Component.Extension;
using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Home.Ajax.Detailrmation
{
    public partial class Content : AjaxPageBase<ContentEntity>
    {
        protected override string GetListItem(ContentEntity info)
        {
            var detail = info.Detail.RemoveHtml();
            return string.Format("Id:'{0}',Title:'{1}',Detail:'{2}'", info.Id, info.Title,
                string.IsNullOrEmpty(detail) || detail.Length <= 80 ? detail : detail.Substring(0, 80));
        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id,Title,Detail";
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            var times = Request.QueryString["times"].Convert<int>();
            query.WhereExp = "InsertTime>=@InsertTime && Class.Tag==@Tag";
            query.SetParameter("InsertTime", DateTime.Now.AddMinutes(-times))
               .SetParameter("Tag",Request.QueryString["Tag"]);
        }
   
    }
}