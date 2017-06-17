using System;

namespace Beeant.Presentation.Admin.Cms.Cms.Class
{
    public partial class Edit : System.Web.UI.UserControl
    {


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                tvClassTree.LoadData();
            }
        }

        protected void tvClassTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            lblParentName.Text = tvClassTree.TreeView.SelectedNode.Text;
            hfParentId.Value = tvClassTree.TreeView.SelectedNode.Value;
            if (!string.IsNullOrEmpty(tvClassTree.TreeView.SelectedNode.Value))
                tvClassTree.LoadData();
        }
        public virtual void ddlPlatformType_SelectedIndexChanged(object sender, EventArgs e)
        {

            tvClassTree.TreeView.Nodes.Clear();
           
        }
    }
}