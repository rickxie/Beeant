using System.Linq;
using Beeant.Domain.Entities.Workflow;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Workflow.Flow
{
    public partial class List : MaintenPageBase<FlowEntity>
    {
        protected override FlowEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Account = new AccountEntity {Id = 0};
            }
            return info;
        }
        /// <summary>
        /// 设置查询
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<FlowEntity>().Where(it => it.Account.Id == 0);
            base.SetQueryWhere(query);
        }
    }
}