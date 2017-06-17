using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.Style
{
    public partial class List : ListPageBase<StyleEntity>
    {

        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStyleType.LoadData();
            }
            base.Page_Load(sender, e);
        }
    }
}