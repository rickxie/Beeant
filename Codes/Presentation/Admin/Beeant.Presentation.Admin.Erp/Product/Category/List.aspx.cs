using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System;
using Component.Extension;
using Beeant.Domain.Entities.Product;

using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Product.Category
{
    public partial class List : ListPageBase<CategoryEntity>
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tvCategoryTree.TreeView.ShowCheckBoxes = TreeNodeTypes.Leaf;
                LoadData();
            }
        }

   
        protected void tvCategoryTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tvCategoryTree.TreeView.CheckedNodes.Count; i++)
                tvCategoryTree.TreeView.CheckedNodes[i].Checked = false;
            tvCategoryTree.TreeView.SelectedNode.Checked = true;
            NoClearAndLoadData();
            hfCategoryId.Value = tvCategoryTree.TreeView.SelectedNode.Value;
            spName.InnerHtml = tvCategoryTree.TreeView.SelectedNode.Text;
        }
        /// <summary>
        /// 不清除数据
        /// </summary>
        protected virtual void NoClearAndLoadData()
        {
            tvCategoryTree.LoadData();
        }

        protected override void LoadEntities()
        {
            while (tvCategoryTree.TreeView.Nodes.Count > 0)
            {
                tvCategoryTree.TreeView.Nodes.RemoveAt(0);
            }
            NoClearAndLoadData();
        }
    
        protected override IList<TEntityType> GetSaveEntities<TEntityType>(SaveType saveType, bool isBindDataKey = true, DropDownList dropDownList = null)
        {
            return (from TreeNode node in tvCategoryTree.TreeView.CheckedNodes where node.Checked select CreateSaveEntity<TEntityType>(node.Value.Convert<long>(), SaveType.Remove)).ToList();
        }

        
    }
}