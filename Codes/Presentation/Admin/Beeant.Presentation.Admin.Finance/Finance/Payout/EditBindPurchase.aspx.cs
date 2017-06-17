using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Finance.Finance.Payout
{
    public partial class EditBindPurchase : ListPageBase<PurchasePayEntity>
    {
        public long PayoutId
        {
            get { return Request.QueryString["id"].Convert<long>(); }
        }

        protected override void LoadData()
        {
            var entity = Ioc.Resolve<IApplicationService, PayoutEntity>().GetEntity<PayoutEntity>(PayoutId);
            if (entity == null || entity.Status != PayoutStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("您没有权限在该状态下操作付款核销");
            base.LoadData();
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<PurchasePayEntity>().Where(it => it.Payout.Id == Request.QueryString["id"].Convert<long>()).Select(s=>s);
            base.SetQueryWhere(query);
        }

        protected void btnModifyAmount_Click(object sender, EventArgs e)
        {
            SaveEntities(GetChangeAmountEntities(), "修改金额成功", "修改金额失败");
        }

        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<PurchasePayEntity> GetChangeAmountEntities()
        {
            var infos = new List<PurchasePayEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect != null && !ckSelect.Checked)
                    continue;
                var txtAmount = gvr.FindControl("txtAmount") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtAmount == null)
                    continue;
                var info = new PurchasePayEntity
                    {
                        Id = ckSelect.Value.Convert<long>(),
                        Amount = txtAmount.Value.Convert<decimal>(),
                        Remark = txtRemark.Value,
                        SaveType = SaveType.Modify,
                    };
                info.SetProperty(it => it.Amount);
                info.SetProperty(it => it.Remark);
                infos.Add(info);
            }
            return infos;
        }
    }
}