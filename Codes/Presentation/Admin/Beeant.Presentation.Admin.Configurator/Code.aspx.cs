using System;
using System.Web.UI;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Presentation.Admin.Configurator
{
    public partial class Code : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            CodeHelper.CreateCode("ConfiguratorCode");
        }
    }
}