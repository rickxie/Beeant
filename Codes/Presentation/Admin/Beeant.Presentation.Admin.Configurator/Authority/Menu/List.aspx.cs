using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using System;
using Dependent;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Authority.Menu
{
    public partial class List : MaintenPageBase<MenuEntity>
    {

       


        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlEditSubsystem.LoadData();
                ddlSearchSubsystem.LoadData();
                tvParentMenu.LoadData(ddlEditSubsystem.DropDownList.SelectedValue);
                tvMenuList.TreeView.ShowCheckBoxes = TreeNodeTypes.Leaf;
                LoadData();
            }
        }

        protected override MenuEntity GetEntity()
        {
            if (RequestId==0)
                return null;
            var query = new QueryInfo();
            query.Query<MenuEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Parent.Name, it });
            var infos = Ioc.Resolve<IApplicationService, MenuEntity>().GetEntities<MenuEntity>(query);
            if (infos != null && infos.Count > 0) return infos[0];
            return null;
        }

        protected void tvParentMenu_SelectedNodeChanged(object sender, EventArgs e)
        {
            lblParentName.Text = tvParentMenu.TreeView.SelectedNode.Text;
            hfParentId.Value = tvParentMenu.TreeView.SelectedNode.Value;
            if (!string.IsNullOrEmpty(tvParentMenu.TreeView.SelectedNode.Value))
                tvParentMenu.LoadData(ddlEditSubsystem.DropDownList.SelectedValue);
            IsShowEditContainer = true;
        }

        protected void tvMenuList_SelectedNodeChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tvMenuList.TreeView.CheckedNodes.Count; i++)
                tvMenuList.TreeView.CheckedNodes[i].Checked = false;
            tvMenuList.TreeView.SelectedNode.Checked = true;
            NoClearAndLoadData();
            hfMenuId.Value = tvMenuList.TreeView.SelectedNode.Value;
            btnModify.Visible = true;

        }

        /// <summary>
        /// 不清除数据
        /// </summary>
        protected virtual void NoClearAndLoadData()
        {
            tvMenuList.LoadData(ddlSearchSubsystem.DropDownList.SelectedValue);
        }

        protected override void LoadEntities()
        {
            while (tvMenuList.TreeView.Nodes.Count>0)
            {
                tvMenuList.TreeView.Nodes.RemoveAt(0);
            }
            NoClearAndLoadData();
        }
        protected override void SetResult(MenuEntity info)
        {
            base.SetResult(info);
            if (info.Errors.Count == 0)
            {
                tvParentMenu.TreeView.Nodes.Clear();
                tvParentMenu.LoadData(ddlEditSubsystem.DropDownList.SelectedValue);
                lblParentName.Text = "";
                hfParentId.Value = "";
            }    
        }
        protected override IList<TEntityType> GetSaveEntities<TEntityType>(SaveType saveType, bool isBindDataKey = true, DropDownList dropDownList = null)
        {
            return (from TreeNode node in tvMenuList.TreeView.CheckedNodes where node.Checked select CreateSaveEntity<TEntityType>(node.Value.Convert<long>(), SaveType.Remove)).ToList();
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            RequestId = tvMenuList.TreeView.SelectedValue.Convert<long>();
            LoadEntity();
        }

        public virtual void Subsystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsShowEditContainer = true;
            tvParentMenu.TreeView.Nodes.Clear();
            tvParentMenu.LoadData(ddlEditSubsystem.DropDownList.SelectedValue);
        }
    }
}