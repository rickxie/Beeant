using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Erp.Controls
{
    public partial class GeneralDropDownList : DropDownListBaseControl
    {
        /// <summary>
        /// 具体控件
        /// </summary>
        public override System.Web.UI.WebControls.DropDownList DropDownList
        {
            get { return DropDownList1; }
        }
      
    }
}