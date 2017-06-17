using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Scm.Ajax.Basedata
{
    public partial class Tag : AjaxPageBase<TagEntity>
    {
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["TagGroupId"]))
            {
                base.Page_Load(sender, e);
            }
        }

        protected override string GetListItem(TagEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Value);
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["TagGroupId"]))
            {
                query.Where("TagGroup.Id==@TagGroupId").OrderBy("InsertTime");
                query.SetParameter("TagGroupId", Server.UrlDecode(Request.QueryString["TagGroupId"]));
            }

        }
       
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Value, Name";
        }

    }
}