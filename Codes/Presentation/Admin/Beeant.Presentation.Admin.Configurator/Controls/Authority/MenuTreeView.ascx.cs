using System.Collections.Generic;
using System.Web.UI.WebControls;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Authority;
using Dependent;
using Beeant.Application.Services;
using Beeant.Basic.Services.WebForm.Controls;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Configurator.Controls.Authority
{
    public partial class MenuTreeView : TreeViewTemplateBaseControl<MenuEntity>
    {

        public override TreeView TreeView
        {
            get { return TreeView1; }
            set
            {
                base.TreeView = value;
            }
        }

        public virtual void LoadData(string subsystemId)
        {
            var infos = GetEntities(subsystemId);
            BindTree(infos);
        }


        /// <summary>
        /// 得到记录集
        /// </summary>
        /// <returns></returns>
        protected virtual IList<BaseEntity> GetEntities(string subsystemId)
        {
            var query = new QueryInfo {SelectExp = "Id,Name", FromExp = "MenuEntity", WhereExp = "Parent.Id==@ParentId",OrderByExp = "Sequence asc"};
            if (!string.IsNullOrEmpty(subsystemId))
            {
                query.WhereExp = string.Format("{0} && Subsystem.Id==@SubsystemId", query.WhereExp);
                query.SetParameter("SubsystemId", subsystemId);
            }
            query.SetParameter("ParentId", TreeView1.SelectedValue ?? "");
            return Ioc.Resolve<IApplicationService, BaseEntity>().GetEntities<BaseEntity>(query);
        }

      
    }
}