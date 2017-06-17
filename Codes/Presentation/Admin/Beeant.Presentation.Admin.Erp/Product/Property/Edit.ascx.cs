using System;

namespace Beeant.Presentation.Admin.Erp.Product.Property
{
    public partial class Edit : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                tvCategoryTree.LoadData();
                ddlType.LoadData();
                ddlSearchType.LoadData();
            }
        }
 

        protected void tvCategoryTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            lblCategoryName.Text = tvCategoryTree.TreeView.SelectedNode.Text;
            hfCategoryId.Value = tvCategoryTree.TreeView.SelectedNode.Value;
            if (!string.IsNullOrEmpty(tvCategoryTree.TreeView.SelectedNode.Value))
                tvCategoryTree.LoadData();
        }
        
    }
}