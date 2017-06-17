using System.Collections.Generic;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;

namespace Beeant.Domain.Services.Workflow
{
    public class NodeDomainService : RealizeDomainService<NodeEntity>
    {
        /// <summary>
        /// 条件
        /// </summary>
        public IDomainService ConditionDomainService { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        public IDomainService PropertyDomainService { get; set; }

        private IDictionary<string, IUnitofworkHandle> _itemHandles;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, IUnitofworkHandle> ItemHandles
        {
            get
            {
                return _itemHandles ?? (_itemHandles = new Dictionary<string, IUnitofworkHandle>
                    {
                        {"Conditions", new UnitofworkHandle<ConditionEntity>{DomainService= ConditionDomainService}},
                        {"NodeProperties", new UnitofworkHandle<PropertyEntity>{DomainService= PropertyDomainService}}

                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        #region 重新验证


        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(NodeEntity info)
        {
            return ValidateFlow(info) && ValidateAuditor(info); 
        }

        /// <summary>
        /// 验证类型
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateFlow(NodeEntity info)
        {
            if (!info.HasSaveProperty(it => it.Flow.Id))
                return true;
            if (info.Flow != null && info.Flow.SaveType == SaveType.Add)
                return true;
            if (info.Flow != null && info.Flow.Id!=0)
            {
                if (Repository.Get<FlowEntity>(info.Flow.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(FlowEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证类型
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateAuditor(NodeEntity info)
        {
            if (!info.HasSaveProperty(it => it.Auditor.Id))
                return true;
            if (info.Auditor != null && info.Auditor.SaveType == SaveType.Add)
                return true;
            if (info.Auditor != null && info.Auditor.Id != 0)
            {
                if (Repository.Get<AuditorEntity>(info.Auditor.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(AuditorEntity).FullName, "NoExist");
            return false;
        }
        #endregion
    }
}
