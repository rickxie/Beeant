using System.Collections.Generic;
using System.Globalization;
using Beeant.Basic.Services.WebForm.Controls;
using Winner.Dislan;

namespace Beeant.Presentation.Admin.Cms.Controls
{
    public partial class MonthsCheckBoxList : CheckBoxListBaseControl
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
 
        public override object GetSource()
        {
            var rev=new List<LanguageInfo>();
            for (int i = 1; i < 32; i++)
            {
                rev.Add(new LanguageInfo{Message = i.ToString(CultureInfo.InvariantCulture),Name=i.ToString(CultureInfo.InvariantCulture)});
            }
            return rev;
        }
    }
}