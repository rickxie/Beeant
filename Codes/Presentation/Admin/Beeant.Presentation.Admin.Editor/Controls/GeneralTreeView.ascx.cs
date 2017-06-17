using System.Web.UI.WebControls;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Editor.Controls
{
    public partial class GeneralTreeView : TreeViewBaseControl
    {
        public override TreeView TreeView
        {
            get { return TreeView1; }
            set
            {
                base.TreeView = value;
            }
        }
    }
}