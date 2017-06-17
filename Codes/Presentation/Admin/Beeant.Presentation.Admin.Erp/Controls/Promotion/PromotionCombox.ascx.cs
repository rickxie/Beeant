using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Erp.Controls.Promotion
{
    public partial class PromotionCombox : ComboBoxControlBase
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
                    return "/Ajax/Promotion/Promotion.aspx";
                return base.AjaxUrl;
            }
            set { base.AjaxUrl = value; }
        }
    }
}