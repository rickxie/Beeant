using System;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Services;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Order
{
    public class ComplaintApplicationService : RealizeApplicationService<OrderComplaintEntity>
    {
        public IDomainService MessageDomainService { set; get; }

        public override bool Save(OrderComplaintEntity info)
        {
            var rev = base.Save(info);
            Action<OrderComplaintEntity> action = SendMessage;
            action.BeginInvoke(info, null, null);
            return rev;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        protected virtual void SendMessage(OrderComplaintEntity info)
        {
            var query = new QueryInfo();
            query.Query<OrderComplaintEntity>()
                .Where(it => it.Id == info.Id)
                .Select(it => new object[] {it.Id, it.Order.Id, it.Order.Account.Id});
            var datas = Repository.GetEntities<OrderComplaintEntity>(query);
            var dataInfo = datas?.FirstOrDefault();
            if (dataInfo == null)
                return;
            var lang = Winner.Creator.Get<Winner.Dislan.ILanguage>();
            var msg = new MessageEntity
            {
                SaveType = SaveType.Add,
                Account = new AccountEntity {Id = dataInfo.Order.Account.Id},
                IsRead = false,
                Title = lang.GetName("Beeant.Domain.Entities.Complaint.Message", "Title"),
                Detail =string.Format(lang.GetName("Beeant.Domain.Entities.Complaint.Message", "Detail"),dataInfo.Order.Id)
            };
            var unitofworks = MessageDomainService.Handle(msg);
            if (unitofworks != null)
            {
                Commit(unitofworks);
            }
        }
    }
}
