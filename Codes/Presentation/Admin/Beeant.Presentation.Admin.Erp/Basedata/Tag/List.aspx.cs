using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.Tag
{
    public partial class List : ListPageBase<TagEntity>
    {

        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlTagGroup.LoadData();
            }
            base.Page_Load(sender, e);
        }
    }
}