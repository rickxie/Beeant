using System;
using System.Web.UI;
using Beeant.Application.Services.Utility;
using Dependent;

namespace Beeant.Presentation.Admin.Configurator
{
    public partial class Quit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Ioc.Resolve<IIdentityApplicationService>().Remove();
            Response.Redirect("/Default.aspx");
        }
    }
}