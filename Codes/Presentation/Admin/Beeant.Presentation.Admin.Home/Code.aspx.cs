using System;
using System.Web.UI;
using Beeant.Basic.Services.Common.Extension;

namespace Beeant.Presentation.Admin.Home
{
    public partial class Code : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            CodeHelper.CreateCode("AdminCode");
        }
    }
}