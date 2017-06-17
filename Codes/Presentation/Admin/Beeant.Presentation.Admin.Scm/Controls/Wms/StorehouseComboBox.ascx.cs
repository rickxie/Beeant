using System.Text;
using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Scm.Controls.Wms
{
    public partial class StorehouseComboBox : ComboBoxControlBase
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
        /// 是否使用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 是否仓
        /// </summary>
        public bool IsWarehouse { get; set; }
        public override string AjaxUrl
        {
            get
            {
                if (string.IsNullOrEmpty(base.AjaxUrl))
                {
                    var builder = new StringBuilder("/Ajax/Wms/Storehouse.aspx");
                    var flag = "?";
                    if (IsUsed)
                    {
                        builder.AppendFormat("{0}isUsed={1}", flag,IsUsed);
                        flag = "&";
                    }
                    if (IsWarehouse)
                    {
                        builder.AppendFormat("{0}IsWarehouse={1}", flag, IsWarehouse);
                    }
                    return builder.ToString();
                }
                    
                return base.AjaxUrl;
            }
            set
            {
                base.AjaxUrl = value;
            }
        }
       
    }
}