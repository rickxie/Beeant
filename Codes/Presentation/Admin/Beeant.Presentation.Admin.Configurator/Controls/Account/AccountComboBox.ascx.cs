using System.Text;
using System.Web.UI.HtmlControls;

using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Presentation.Admin.Configurator.Controls.Account
{
    public partial class AccountComboBox : ComboBoxControlBase
    {
        public override HtmlInputHidden InputHidden
        {
            get { return hfInputHidden; }
            set
            {
                base.InputHidden = value;
            }
        }

        public override HtmlInputText InputText
        {
            get { return txtInputText; }
            set
            {
                base.InputText = value;
            }
        }

        public override string AjaxUrl
        {
            get
            {
                if (string.IsNullOrEmpty(base.AjaxUrl))
                    return  "/Ajax/Account/Account.aspx";
                return base.AjaxUrl;
            }
            set
            {
                base.AjaxUrl = value;
            }
        }
 
        protected override void CreateScriptPager()
        {
            base.CreateScriptPager();
            var builder = new StringBuilder();

            builder.Append(ClientID);
            builder.Append(".SelectText=function(item){");
            builder.Append(string.Format("{0}.Input.value = item.innerHTML.split('|')[0];", ClientID));
            builder.Append("};");
            Page.ExecuteScript(builder.ToString());
        }
    }
}