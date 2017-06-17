using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Workflow;
using Dependent;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Workflow.Condition
{
    public partial class List : MaintenPageBase<ConditionEntity>
    {
        public string NodeName { get; set; }
        public long NodeId
        {
            get { return Request["NodeId"].Convert<long>(); }
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<ConditionEntity>().Where(it => it.Node.Id == NodeId);
            base.SetQueryWhere(query);
         

        }
 
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlNode.LoadData();
            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 加载状态信息
        /// </summary>
        /// <returns></returns>
        protected virtual long LoadStatusEntity()
        {
            var info = Ioc.Resolve<IApplicationService, NodeEntity>().GetEntity<NodeEntity>(NodeId);
            if (info != null) 
            {
                NodeName= string.Format("{0}-", info.Name);
                return info.Flow==null?0:info.Flow.Id;
            }
            return 0;
        }
 
        /// <summary>
        /// 得到状态集合
        /// </summary>
        /// <param name="flowId"></param>
        /// <returns></returns>
        protected virtual IList<NodeEntity> GetStatusEntities(long flowId)
        {
             if( flowId==0)
                return null;
            var query = new QueryInfo();
            query.Query<NodeEntity>().Where(it => it.Flow.Id == flowId)
                .Select(it => new object[] {it.Id, it.Name});
               
            return Ioc.Resolve<IApplicationService, NodeEntity>().GetEntities<NodeEntity>(query);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        protected override ConditionEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info.SaveType == SaveType.Add)
                info.Node = new NodeEntity { Id = NodeId };
            return info;
        }
     
    }
}