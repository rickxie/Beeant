using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Scm.Controls
{
    public partial class GeneralCheckBoxList : CheckBoxListBaseControl
    {
        public override System.Web.UI.WebControls.CheckBoxList CheckBoxList
        {
            get
            {
                return CheckBoxList1;
            }
            set
            {
                base.CheckBoxList = value;
            }
        }
      
    }
}