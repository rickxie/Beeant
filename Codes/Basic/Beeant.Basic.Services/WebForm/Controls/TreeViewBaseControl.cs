using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Controls
{
    public class TreeViewBaseControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// 事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SelectedNodeChangedHandler(object sender, EventArgs e);
        /// <summary>
        /// 事件
        /// </summary>
        public event SelectedNodeChangedHandler SelectedNodeChanged;
        /// <summary>
        /// 具体控件
        /// </summary>
        public virtual TreeView TreeView { get; set; }

        /// <summary>
        /// 是否显示无
        /// </summary>
        public bool IsShowNone { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        public virtual string EntityName { get; set; }

        private string _dataTextField = "Name";
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string DataTextField
        {
            get { return _dataTextField; }
            set { _dataTextField = value; }
        }
        private string _dataValueField = "Id";
        /// <summary>
        /// 值
        /// </summary>
        public virtual string DataValueField
        {
            get { return _dataValueField; }
            set { _dataValueField = value; }
        }
        private string _parentPropertyName = "Parent.Id";
        /// <summary>
        /// 父属性名称
        /// </summary>
        public virtual string ParentPropertyName
        {
            get { return _parentPropertyName; }
            set { _parentPropertyName = value; }
        }
        private string _sequencePropertyName = "Sequence";
        /// <summary>
        /// 排序名称
        /// </summary>
        public virtual string SequencePropertyName
        {
            get { return _sequencePropertyName; }
            set { _sequencePropertyName = value; }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData()
        {
            var infos = GetEntities();
            BindTree(infos);
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData(QueryInfo query)
        {
            var infos =  Ioc.Resolve<IApplicationService, BaseEntity>().GetEntities<BaseEntity>(query);
            BindTree(infos);
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void BindTree(IList<BaseEntity> infos)
        {
            var nodes = TreeView.SelectedNode == null ? TreeView.Nodes : TreeView.SelectedNode.ChildNodes;
            BindTree(infos, nodes);
            if (IsShowNone && TreeView.SelectedNode == null)
                TreeView.Nodes.Add(new TreeNode("无", ""));
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="nodes"></param>
        protected virtual void BindTree(IList<BaseEntity> infos, TreeNodeCollection nodes)
        {
            if (nodes.Count > 0)
                return;
            foreach (var info in infos)
            {
                var name = Winner.Creator.Get<Winner.Base.IProperty>().GetValue<string>(info, "Name");
                var id = Winner.Creator.Get<Winner.Base.IProperty>().GetValue<string>(info, "Id");
                nodes.Add(new TreeNode(name, id));
            }
        }

        /// <summary>
        /// 获取选中值
        /// </summary>
        /// <returns></returns>
        public virtual string GetSelectedValue()
        {
            return TreeView.SelectedValue;
        }

        /// <summary>
        /// 得到记录集
        /// </summary>
        /// <returns></returns>
        protected virtual IList<BaseEntity> GetEntities()
        {
            var query = new QueryInfo();
            query.From(EntityName).Where(string.Format("{0}==@ParentId", ParentPropertyName))
                .Select(string.Format("{0},{1}", DataTextField, DataValueField)).SetParameter("ParentId", TreeView.SelectedValue.Convert<long>());
            if (!string.IsNullOrEmpty(SequencePropertyName))
            {
                query.OrderBy(SequencePropertyName);
            }
            return Ioc.Resolve<IApplicationService, BaseEntity>().GetEntities<BaseEntity>(query);
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (SelectedNodeChanged != null)
                SelectedNodeChanged(sender, e);
            TreeView.SelectedNode.Expand();
        }
    }
}
