using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Wms.Controls.Basedata
{
    public partial class PackUnitComboBox : ComboBoxControlBase
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
                    return "/Ajax/Basedata/Unit.aspx";
                return base.AjaxUrl;
            }
            set
            {
                base.AjaxUrl = value;
            }
        }
    }
}