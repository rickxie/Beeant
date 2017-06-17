using System.Text;
using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Presentation.Admin.Scm.Controls.Supplier
{
    public partial class SupplierComBox : ComboBoxControlBase
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
 
        /// <summary>
        /// 绑定用户
        /// </summary>
        public long ServiceId { get; set; }

        public override string AjaxUrl
        {
            get
            {
                if (string.IsNullOrEmpty(base.AjaxUrl))
                    return "/Ajax/Supplier/Supplier.aspx?ServiceId=" + ServiceId.ToString();
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