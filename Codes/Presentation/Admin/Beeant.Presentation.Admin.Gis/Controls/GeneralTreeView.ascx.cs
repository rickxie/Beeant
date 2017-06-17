using Beeant.Domain.Entities;
using System.Web.UI.WebControls;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Gis.Controls
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