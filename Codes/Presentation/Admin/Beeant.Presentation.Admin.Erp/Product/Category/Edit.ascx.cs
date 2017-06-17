using System;

namespace Beeant.Presentation.Admin.Erp.Product.Category
{
    public partial class Edit : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                tvCategoryTree.LoadData();
            }
        }

        protected void tvCategoryTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            lblParentName.Text = tvCategoryTree.TreeView.SelectedNode.Text;
            hfParentId.Value = tvCategoryTree.TreeView.SelectedNode.Value;
            if (!string.IsNullOrEmpty(tvCategoryTree.TreeView.SelectedNode.Value))
                tvCategoryTree.LoadData();
        }
        
    }
}