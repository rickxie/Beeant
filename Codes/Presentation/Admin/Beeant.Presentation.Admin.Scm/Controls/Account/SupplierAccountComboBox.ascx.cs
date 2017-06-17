using System.Text;
using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;


namespace Beeant.Presentation.Admin.Scm.Controls.Account
{
    public partial class SupplierAccountComboBox : ComboBoxControlBase
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

        public SupplierAccountComboBox()
        {
            UserId = 0;
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        public long UserId { get; set; }

        private string _statusFlag = "Effective";
        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return _statusFlag; }
            set { _statusFlag = value; }
        }
        public override string AjaxUrl
        {
            get
            {
                if (string.IsNullOrEmpty(base.AjaxUrl))
                    return string.Format("/Ajax/Supplier/SupplierAccount.aspx?User.Id={0}&Status={1}" , UserId,Status);
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