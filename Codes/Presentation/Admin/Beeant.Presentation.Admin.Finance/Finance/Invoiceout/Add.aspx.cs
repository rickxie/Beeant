using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Invoiceout
{
    public partial class Add : WorkflowPageBase<InvoiceoutEntity>
    {
        
        protected override InvoiceoutEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        ///  加载订单
        /// </summary>
        protected virtual void LoadOrders()
        {
            IList<OrderEntity> infos = null;
            if (!string.IsNullOrEmpty(Request.QueryString["OrderIds"]))
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
        protected virtual IList<OrderEntity> GetPurchasesByPurchaseIds()
        {
            var values = Request.QueryString["OrderIds"].Split(',');
            var productIds = values.Select(value => value.Convert<long>()).ToArray();
            if (productIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<OrderEntity>().Where(it => productIds.Contains(it.Id)).Select(it => new object[]
                    {
                        it.Id, it.Status,it.InsertTime,it.TotalInvoiceAmount,it.InvoiceAmount
                    });
                return Ioc.Resolve<IApplicationService, OrderEntity>().GetEntities<OrderEntity>(query);

            }
            return null;
        }
        protected override InvoiceoutEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.ChannelType = ChannelType.Admin;
                FillInvoices(info);
            }
            return info;
        }
        /// <summary>
        /// 填充发票
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillInvoices(InvoiceoutEntity info)
        {
            info.InvoiceoutItems = new List<InvoiceoutItemEntity>();
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
                var invoiceoutItem = new InvoiceoutItemEntity
                {
                        Order = new OrderEntity {Id = ckSelect.Value.Convert<long>()},
                        Amount = txtInvoiceAmount.Value.Convert<decimal>(),
                        Remark = txtInvoiceAmount.Value,
                        Invoiceout = info,
                        SaveType = SaveType.Add
                    };
                info.InvoiceoutItems.Add(invoiceoutItem);
            }
            FillInvoicesByPurchaseIds(info);
        }
        /// <summary>
        /// 加载产品明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillInvoicesByPurchaseIds(InvoiceoutEntity info)
        {
            if (string.IsNullOrEmpty(hfOrders.Value))
                return;
            var invoiceoutItems = hfOrders.Value.DeserializeJson<List<InvoiceoutItemEntity>>();
            if (invoiceoutItems != null)
            {
                foreach (var subinvoiceoutItem in invoiceoutItems)
                {
                    var invoiceoutItem= new InvoiceoutItemEntity
                    {
                        Order = new OrderEntity { Id = subinvoiceoutItem.Id },
                        Amount = subinvoiceoutItem.Amount,
                        Remark = subinvoiceoutItem.Remark,
                        Invoiceout = info,
                        SaveType = SaveType.Add
                    };
                    info.InvoiceoutItems.Add(invoiceoutItem);
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