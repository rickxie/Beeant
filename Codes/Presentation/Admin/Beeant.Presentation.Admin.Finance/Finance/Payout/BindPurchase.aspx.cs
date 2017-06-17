using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Management;


namespace Beeant.Presentation.Admin.Finance.Finance.Payout
{
    public partial class BindPurchase : ListPageBase<PurchaseEntity>
    {
        public long PayoutId
        {
            get { return Request.QueryString["id"].Convert<long>(); }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
            }
            base.Page_Load(sender, e);
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
            var Payout = Ioc.Resolve<IApplicationService, PayoutEntity>().GetEntity<PayoutEntity>(PayoutId);
            var accountId = Payout == null || Payout.Account == null ? 0 : Payout.Account.Id;
            hfAccountId.Value = accountId.ToString();
            query.Query<PurchaseEntity>().Where(it => it.Account.Id == accountId && it.TotalAmount > it.PayAmount
                && it.PurchasePays.Count(s => s.Payout.Id == PayoutId) == 0);
            base.SetQueryWhere(query);
        }

        protected void btnModifyAmount_Click(object sender, EventArgs e)
        {
            var infos = GetSaveEntities<PurchasePayEntity>(SaveType.Add);
            SaveEntities(infos, "保存成功", "保存失败");
        }


        protected override IList<TEntityType> GetSaveEntities<TEntityType>(SaveType saveType, bool isBindDataKey = true,
                                                                        DropDownList dropDownList = null)
        {
            var infos = new List<PurchasePayEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var txtRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtRemark == null)
                    continue;
                var txtAmount = gvr.FindControl("txtPayout") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtAmount == null)
                    continue;
                var info = new PurchasePayEntity
                    {
                        Amount = txtAmount.Value.Convert<decimal>(),
                        Purchase = new PurchaseEntity {Id = ckSelect.Value.Convert<long>()},
                        Payout = new PayoutEntity { Id = Request.QueryString["id"].Convert<long>() },
                        SaveType = SaveType.Add,
                        Remark = txtRemark.Value,
                    };
                infos.Add(info);
            }
            return infos as IList<TEntityType>;
        }
    }
}