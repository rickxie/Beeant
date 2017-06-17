using System;
using System.Text;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Presentation.Admin.Erp.Basedata.Delivery
{
    public partial class Edit : System.Web.UI.UserControl
    {
       
        
       
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
               ddlCity.LoadData();
            }
            base.OnInit(e);
        }
    }
}