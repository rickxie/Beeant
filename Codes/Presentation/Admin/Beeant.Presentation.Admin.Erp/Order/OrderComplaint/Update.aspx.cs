using System;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Erp.Order.OrderComplaint
{
    public partial class Update : UpdatePageBase<OrderComplaintEntity>
    {
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
            }
        }
        protected override OrderComplaintEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.IsReply = true;
                info.AnswerTime = DateTime.Now;
                info.Type=OrderComplaintType.None;
                info.SetProperty(it => it.IsReply);
                info.SetProperty(it => it.AnswerTime);
                info.SetProperty(it => it.Type);
            }
            return info;
        }
        protected override OrderComplaintEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.IsReply )
            {
                InvalidateData("该投诉已经处理过");
            }
            return info;
        }
      
    }
}