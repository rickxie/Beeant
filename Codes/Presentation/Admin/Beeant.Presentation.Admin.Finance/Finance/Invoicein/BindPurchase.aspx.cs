using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Management;


namespace Beeant.Presentation.Admin.Finance.Finance.Invoicein
{
    public partial class BindPurchase : ListPageBase<PurchaseEntity>
    {
        public long InvoiceinId
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
            var entity = Ioc.Resolve<IApplicationService, InvoiceinEntity>().GetEntity<InvoiceinEntity>(InvoiceinId);
            if (entity == null || entity.Status != InvoiceinStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("您没有权限在该状态下操作发票核销");
            base.LoadData();
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            var invoicein = Ioc.Resolve<IApplicationService, InvoiceinEntity>().GetEntity<InvoiceinEntity>(InvoiceinId);
            var accountId = invoicein == null || invoicein.Account == null ? 0 : invoicein.Account.Id;
            hfAccountId.Value = accountId.ToString();
            query.Query<PurchaseEntity>().Where(it => it.Account.Id == accountId && it.OpenAmount > it.InvoiceAmount);
            base.SetQueryWhere(query);
        }

        protected void btnModifyAmount_Click(object sender, EventArgs e)
        {
            var infos = GetSaveEntities<PurchaseInvoiceEntity>(SaveType.Add);
            SaveEntities(infos, "保存成功", "保存失败");
        }


        protected override IList<TEntityType> GetSaveEntities<TEntityType>(SaveType saveType, bool isBindDataKey = true,
                                                                        DropDownList dropDownList = null)
        {
            var infos = new List<InvoiceinItemEntity>();
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
                var txtAmount = gvr.FindControl("txtPayment") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtAmount == null)
                    continue;
                var info = new InvoiceinItemEntity
                    {
                        Amount = txtAmount.Value.Convert<decimal>(),
                        Purchase = new PurchaseEntity {Id = ckSelect.Value.Convert<long>()},
                        Invoicein = new InvoiceinEntity { Id = InvoiceinId },
                        SaveType = SaveType.Add,
                        Remark = txtRemark.Value,
                    };
                infos.Add(info);
            }
            return infos as IList<TEntityType>;
        }
    }
}