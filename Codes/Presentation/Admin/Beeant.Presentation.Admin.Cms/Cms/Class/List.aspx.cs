using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cms.Cms.Class
{
    public partial class List : ListPageBase<ClassEntity>
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tvClassTree.TreeView.ShowCheckBoxes = TreeNodeTypes.Leaf;
                LoadData();
            }
        }


        protected void tvClassTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tvClassTree.TreeView.CheckedNodes.Count; i++)
                tvClassTree.TreeView.CheckedNodes[i].Checked = false;
            tvClassTree.TreeView.SelectedNode.Checked = true;
            NoClearAndLoadData();
            hfClassId.Value = tvClassTree.TreeView.SelectedNode.Value;
            spName.InnerHtml = tvClassTree.TreeView.SelectedNode.Text;
        }
        /// <summary>
        /// 不清除数据
        /// </summary>
        protected virtual void NoClearAndLoadData()
        {
            tvClassTree.LoadData();
        }

        protected override void LoadEntities()
        {
            while (tvClassTree.TreeView.Nodes.Count > 0)
            {
                tvClassTree.TreeView.Nodes.RemoveAt(0);
            }
            NoClearAndLoadData();
        }

        protected override IList<TEntityType> GetSaveEntities<TEntityType>(SaveType saveType, bool isBindDataKey = true, DropDownList dropDownList = null)
        {
            return (from TreeNode node in tvClassTree.TreeView.CheckedNodes where node.Checked select CreateSaveEntity<TEntityType>(node.Value.Convert<long>(), SaveType.Remove)).ToList();
        }
       
    }
}