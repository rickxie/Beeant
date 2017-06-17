using System;
using System.Web.UI;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Presentation.Admin.Report
{
    public partial class ChartMain : MasterPage
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