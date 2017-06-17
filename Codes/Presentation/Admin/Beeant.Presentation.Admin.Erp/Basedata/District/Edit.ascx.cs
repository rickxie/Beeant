using System;

namespace Beeant.Presentation.Admin.Erp.Basedata.District
{
    public partial class Edit : System.Web.UI.UserControl
    {


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                tvDistrictTree.LoadData();
            }
        }

        protected void tvDistrictTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            lblParentName.Text = tvDistrictTree.TreeView.SelectedNode.Text;
            hfParentId.Value = tvDistrictTree.TreeView.SelectedNode.Value;
            if (!string.IsNullOrEmpty(tvDistrictTree.TreeView.SelectedNode.Value))
                tvDistrictTree.LoadData();
        }
        
    }
}