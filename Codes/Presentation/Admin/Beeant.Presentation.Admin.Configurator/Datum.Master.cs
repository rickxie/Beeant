using System;
using System.Web.UI;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Presentation.Admin.Configurator
{
    public partial class Datum : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            Page.AddExceptionLog();
        }
    }
}