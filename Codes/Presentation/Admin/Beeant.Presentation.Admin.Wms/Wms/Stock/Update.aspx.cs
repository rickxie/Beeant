using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Wms.Wms.Stock
{
    public partial class Update : WorkflowPageBase<StockEntity>
    {
        
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
            }

            base.Page_Load(sender, e);
        }
       
    }
}