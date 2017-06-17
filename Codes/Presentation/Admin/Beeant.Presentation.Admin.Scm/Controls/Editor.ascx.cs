using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Scm.Controls
{
    public partial class Editor : EditorControlBase
    {
        public override HtmlTextArea TextArea
        {
            get { return txtEditor; }
            set { txtEditor = value; }
        }
    }
}