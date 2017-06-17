using System;
using System.Collections.Generic;
using System.Linq;
using Beeant.Application.Services.Workflow;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Services.Finance;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Finance
{
    public class PaylineApplicationService : ApplicationService, IPaylineApplicationService,IJobApplicationService
    {
        /// <summary>
        /// 工作流
        /// </summary>
        public virtual IWorkflowEngineApplicationService WorkflowEngine { get; set; }


        /// <summary>
        /// 平台类型
        /// </summary>
        public IPaylineRepository PaylineRepository { get; set; }

        /// <summary>
        /// 同步接口
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Create(PaylineEntity info)
        {
            if (info == null)
                return false;
            info.SaveType=SaveType.Add;
            info.SetNumber();
            if (!PaylineRepository.Create(info))
                return false;
            Pay(info);
            return true;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        public virtual PaylineEntity Process()
        {
            var info = PaylineRepository.Process();
            if (info == null || info.SaveType == SaveType.None)
                return info;
            return Pay(info);
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Refund(PaylineEntity info)
        {
            if (info == null || info.PaylineItems==null || info.Amount==0)
                return false;
            info.SaveType = SaveType.Add;
            info.SetNumber();
            if (!PaylineRepository.Refund(info))
                return false;
            Pay(info);
            return true;
        }
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="info"></param>
        protected virtual PaylineEntity Pay(PaylineEntity info)
        {
            if (info.SaveType == SaveType.None)
                return info;
            var unitofworks = DomainService.Handle(info);
            if (unitofworks != null   )
            {
                Commit(unitofworks);
            }
            return info;
        }

        private static bool IsExecute { get; set; }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual bool Execute(object[] args)
        {
            if (IsExecute)
                return false;
            IsExecute = true;
            try
            {
                PaylineType payType=(PaylineType)Enum.Parse(typeof(PaylineType), args[0].ToString()) ;
                var infos = GetWaitPaylines(payType);
                Handle(infos);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                IsExecute = false;
            }
            return true;
        }

        protected virtual void Handle(IList<PaylineEntity> infos)
        {
            if(infos==null || infos.Count==0)
                return;
            IList<PaylineEntity> entities=new List<PaylineEntity>();
            foreach (var info in infos)
            {
               if (PaylineRepository.Check(info))
                    entities.Add(info);
            }
            var unitofworks=DomainService.Handle(entities);
            if (unitofworks != null)
            {
                Commit(unitofworks);
            }
        }
        /// <summary>
        /// 得到等待超时的支付
        /// </summary>
        /// <returns></returns>
        protected virtual IList<PaylineEntity> GetWaitPaylines(PaylineType paylineType)
        {
            var query = new QueryInfo {IsReturnCount = false};
            query.Query<PaylineEntity>()
                .Where(
                    it => it.Type== paylineType &&
                        !string.IsNullOrEmpty(it.OutNumber) && it.Status == PaylineStatusType.Waiting &&
                        it.InsertTime <= DateTime.Now.AddMinutes(-2))
                .Select(it => new {it.Id, it.Status, it.Number, it.OutNumber });
            return Repository.GetEntities<PaylineEntity>(query);
        } 
    }
}
