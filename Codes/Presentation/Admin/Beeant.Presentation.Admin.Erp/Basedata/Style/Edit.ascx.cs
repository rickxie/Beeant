namespace Beeant.Presentation.Admin.Erp.Basedata.Style
{
    public partial class Edit : System.Web.UI.UserControl
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStyleType.LoadData();
            }
            base.OnInit(e);
        }
    }
}