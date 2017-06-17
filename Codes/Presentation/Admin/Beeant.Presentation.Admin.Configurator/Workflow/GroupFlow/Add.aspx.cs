using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Workflow.GroupFlow
{
    public partial class Add : Basic.Services.WebForm.Pages.ListPageBase<GroupEntity>
    {
        public long GroupId
        {
            get { return Request["GroupId"].Convert<long>(); }
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            if(GroupId == 0)
                return;
            query.Query<GroupEntity>().Where(
                it => it.GroupFlows.Count(i => i.Group.Id == GroupId) == 0);

        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            var infos = GetSaveEntities<GroupFlowEntity>(SaveType.Add);
            SaveEntities(infos, "授权成功", "授权失败");
           
        }
        /// <summary>
        /// 重写
        /// </summary>
        /// <typeparam name="TEntityType"></typeparam>
        /// <param name="id"></param>
        /// <param name="saveType"></param>
        /// <returns></returns>
        protected override TEntityType CreateSaveEntity<TEntityType>(long id, SaveType saveType)
        {
            var info = new GroupFlowEntity
            {
                    SaveType=saveType,
                    Group = new GroupEntity { Id = id }, Flow = new FlowEntity { Id = GroupId }
                };
            return info as TEntityType;
        }      

    }
}