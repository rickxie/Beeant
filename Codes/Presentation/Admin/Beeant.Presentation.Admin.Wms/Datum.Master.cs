using System;
using System.Web.UI;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Presentation.Admin.Wms
{
    public partial class Datum : MasterPage
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