using System.Text;
using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Presentation.Admin.Finance.Controls.Finance
{
    public partial class BankComboBox : ComboBoxControlBase
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

        public virtual HtmlInputHidden AccountInputHidden { get; set; }
        public virtual HtmlInputText BankNameText { get; set; }
        public virtual HtmlInputText BankHolderText { get; set; }
        public override string AjaxUrl
        {
            get
            {
                if (string.IsNullOrEmpty(base.AjaxUrl))
                    return "/Ajax/Finance/Bank.aspx";
                return base.AjaxUrl;
            }
            set
            {
                base.AjaxUrl = value;
            }
        }

        public override string UrlData
        {
            get
            {
                return string.Format("'AccountId='+document.getElementById('{0}').value+'&number='+value",
                                     AccountInputHidden == null ? "" : AccountInputHidden.ClientID);
            }
            set
            {
                base.UrlData = value;
            }
        }

        protected override void CreateScriptPager()
        {
            base.CreateScriptPager();
            var builder = new StringBuilder();

            builder.Append(ClientID);
            builder.Append(".SelectText=function(item){");
            builder.Append(string.Format("{0}.Input.value = item.innerHTML.split('|')[0];", ClientID));
            if (BankNameText != null)
            {
                builder.AppendFormat("{0}.value = item.innerHTML.split('|')[1];", BankNameText.ClientID);
            }
            if (BankHolderText != null)
            {
                builder.AppendFormat("{0}.value = item.innerHTML.split('|')[2];", BankHolderText.ClientID);
            }
            builder.Append("};");
            Page.ExecuteScript(builder.ToString());
        }
    }
}