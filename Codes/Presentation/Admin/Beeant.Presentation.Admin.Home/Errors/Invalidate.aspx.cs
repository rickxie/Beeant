using System;

namespace Beeant.Presentation.Admin.Home.Errors
{
    public partial class Invalidate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["Title"])) return;
            Title = Server.UrlDecode(Request.QueryString["Title"]);
        }
    }
}