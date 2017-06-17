using Beeant.Domain.Entities.Gis;
using Beeant.Domain.Entities.Hr;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Gis.Gis.Address
{
    public partial class List : ListPageBase<AddressEntity>
    {
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if(!IsPostBack)
                ddlCity.LoadData();
            base.Page_Load(sender, e);
        }
       
    }
}