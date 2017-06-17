using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Ajax.Basedata
{
    public partial class Unit : AjaxPageBase<UnitEntity>
    {
        protected override string GetListItem(UnitEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Name);
        }
       
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Name";
            query.OrderBy("Sequence");

        }
       
    }
}