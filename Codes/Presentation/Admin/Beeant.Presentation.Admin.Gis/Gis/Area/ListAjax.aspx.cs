using Beeant.Domain.Entities.Gis;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Gis.Gis.Area
{
    public partial class ListAjax : AjaxPageBase<AreaEntity>
    {
        protected override string GetListItem(AreaEntity info)
        {
            var tempEntity = info;
            if (Request["ispublish"] != "true")
                tempEntity = info.GetOrigin();
            return string.Format("Id:{0},Path:'{1}',Name:'{2}',Color:'{3}',Type:'{4}'", tempEntity.Id, tempEntity.Path, tempEntity.Name, tempEntity.Color, tempEntity.Type);
        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id, Name, Path,Color,Type,Origin";
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Where("City==@City").SetParameter("City", Server.UrlDecode(Request.QueryString["City"]));
            query.PageSize = 0;
        }
    }
}