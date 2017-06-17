using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Invoicein
{
    public partial class Add : WorkflowPageBase<InvoiceinEntity>
    {
        
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPurchases();
            }
            base.Page_Load(sender, e);
        }
        protected override InvoiceinEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
        /// <summary>
        ///  加载采购单
        /// </summary>
        protected virtual void LoadPurchases()
        {
            IList<PurchaseEntity> infos = null;
            if (!string.IsNullOrEmpty(Request.QueryString["PurchaseIds"]))
            {
                infos = GetPurchasesByPurchaseIds();
            }
            gvInvoice.DataSource = infos;
            gvInvoice.DataBind();
        }
        /// <summary>
        ///  得到商品查询
        /// </summary>
        /// <returns></returns>
        protected virtual IList<PurchaseEntity> GetPurchasesByPurchaseIds()
        {
            var values = Request.QueryString["PurchaseIds"].Split(',');
            var productIds = values.Select(value => value.Convert<long>()).ToArray();
            if (productIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<PurchaseEntity>().Where(it => productIds.Contains(it.Id)).Select(it => new object[]
                    {
                        it.Id, it.Status,it.InsertTime,it.OpenAmount,it.InvoiceAmount
                    });
                return Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntities<PurchaseEntity>(query);

            }
            return null;
        }
        protected override InvoiceinEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.ChannelType= ChannelType.Admin;
                FillInvoices(info);
            }
            return info;
        }
        /// <summary>
        /// 填充发票
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillInvoices(InvoiceinEntity info)
        {
            info.InvoiceinItems = new List<InvoiceinItemEntity>();
            foreach (GridViewRow gvr in gvInvoice.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("hfId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (ckSelect == null)
                    continue;
                var txtInvoiceAmount = gvr.FindControl("txtAmount") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtInvoiceRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtInvoiceAmount == null || txtInvoiceRemark == null)
                    continue;
                var invoiceinItem = new InvoiceinItemEntity
                {
                    Purchase = new PurchaseEntity { Id = ckSelect.Value.Convert<long>() },
                    Amount = txtInvoiceAmount.Value.Convert<decimal>(),
                    Remark = txtInvoiceRemark.Value,
                    Invoicein = info,
                    SaveType = SaveType.Add
                };
                info.InvoiceinItems.Add(invoiceinItem);
            }
            FillInvoicesByPurchaseIds(info);
        }
        /// <summary>
        /// 加载产品明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillInvoicesByPurchaseIds(InvoiceinEntity info)
        {
            if (string.IsNullOrEmpty(hfPurchases.Value))
                return;
            var invoiceItems = hfPurchases.Value.DeserializeJson<List<InvoiceinItemEntity>>();
            if (invoiceItems != null)
            {
                foreach (var subinvoiceItem in invoiceItems)
                {
                    var invoiceItem = new InvoiceinItemEntity
                    {
                        Purchase = new PurchaseEntity { Id = subinvoiceItem.Id },
                        Amount = subinvoiceItem.Amount,
                        Remark = subinvoiceItem.Remark,
                        Invoicein = info,
                        SaveType = SaveType.Add
                    };
                    info.InvoiceinItems.Add(invoiceItem);
                }
            }
        }
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            this.ExecuteScript("registerFunc();");
        }

         
        
    }
}