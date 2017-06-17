using System;
using System.Text;
using Beeant.Domain.Entities.Product;

using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.Property
{
    public partial class List : ListPageBase<PropertyEntity>
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
            }
            base.Page_Load(sender,e);
        }


        protected override void AddClientScript()
        {
            base.AddClientScript();
            var builder = new StringBuilder("$(\"#btnImport\").click(function () {");
            builder.AppendFormat("var checkboxs = $(\"#{0}\").find(\"input[SubCheckName='selectall']:checked\");",
                                 GridView.ClientID);
            builder.Append("if (checkboxs.length == 0) {");
            builder.Append("alert(\"请选择要导入的属性\");");
            builder.Append("return;}");
            builder.Append(" var id = [];");
            builder.Append("for (var i = 0; i < checkboxs.length; i++) {");
            builder.Append(" id.push(checkboxs[i].value);");
            builder.Append("  }window.open(\"import.aspx?id=\" + id.join(','));});");
            this.ExecuteScript(builder.ToString());
        }

    }
}