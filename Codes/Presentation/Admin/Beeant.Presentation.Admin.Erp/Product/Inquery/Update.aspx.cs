using System;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Product.Inquery
{
    public partial class Update : UpdatePageBase<InqueryEntity>
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
        protected override InqueryEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
              
                info.IsReply = true;
                info.AnswerTime = DateTime.Now;
                info.SetProperty(it => it.IsReply);
                info.SetProperty(it => it.AnswerTime);
            }
            return info;
        }
        protected override InqueryEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.IsReply )
            {
                InvalidateData("该问题已经处理过");
            }
            return info;
        }
    }
}