using System;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cms.Cms.Content
{
    public partial class Edit : System.Web.UI.UserControl
    {
     

        public virtual SaveType UploaderSaveType
        {
            get { return Uploader1.SaveType; }
            set { Uploader1.SaveType = value;
                Uploader2.SaveType = value;
            }
        }

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
            lblClassName.Text = tvClassTree.TreeView.SelectedNode.Text;
            hfClassId.Value = tvClassTree.TreeView.SelectedNode.Value;
            if (!string.IsNullOrEmpty(tvClassTree.TreeView.SelectedNode.Value))
                tvClassTree.LoadData();
        }
        public virtual void ddlPlatformType_SelectedIndexChanged(object sender, EventArgs e)
        {

            tvClassTree.TreeView.Nodes.Clear();
            

        }
    }
}