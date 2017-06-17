using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using System.Linq;

namespace Beeant.Presentation.Admin.Erp.Basedata.District
{
    public partial class List : ListPageBase<DistrictEntity>
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tvDistrictTree.TreeView.ShowCheckBoxes = TreeNodeTypes.Leaf;
                LoadData();
            }
        }


        protected void tvDistrictTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tvDistrictTree.TreeView.CheckedNodes.Count; i++)
                tvDistrictTree.TreeView.CheckedNodes[i].Checked = false;
            tvDistrictTree.TreeView.SelectedNode.Checked = true;
            NoClearAndLoadData();
            hfClassId.Value = tvDistrictTree.TreeView.SelectedNode.Value;
            spName.InnerHtml = tvDistrictTree.TreeView.SelectedNode.Text;
        }
        /// <summary>
        /// 不清除数据
        /// </summary>
        protected virtual void NoClearAndLoadData()
        {
            tvDistrictTree.LoadData();
        }

        protected override void LoadEntities()
        {
            while (tvDistrictTree.TreeView.Nodes.Count > 0)
            {
                tvDistrictTree.TreeView.Nodes.RemoveAt(0);
            }
            NoClearAndLoadData();
        }

        protected override IList<TEntityType> GetSaveEntities<TEntityType>(SaveType saveType, bool isBindDataKey = true, DropDownList dropDownList = null)
        {
            return (from TreeNode node in tvDistrictTree.TreeView.CheckedNodes where node.Checked select CreateSaveEntity<TEntityType>(node.Value.Convert<long>(), SaveType.Remove)).ToList();
        }
       
    }
}