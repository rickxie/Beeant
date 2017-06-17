using System;

namespace Beeant.Presentation.Admin.Wms.Wms.Shelf
{
    public partial class Edit : System.Web.UI.UserControl
    {

        protected override void  OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStorehouse.LoadData();
            }
            base.OnInit(e);
        }


    }
}