using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Finance.Finance.Invoiceout
{
    public partial class BindOrder : ListPageBase<OrderEntity>
    {
        public long InvoiceoutId
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
            var entity = Ioc.Resolve<IApplicationService, InvoiceoutEntity>().GetEntity<InvoiceoutEntity>(InvoiceoutId);
            if (entity == null || entity.Status != InvoiceoutStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("您没有权限在该状态下操作收款核销");
            base.LoadData();
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            var received = Ioc.Resolve<IApplicationService, InvoiceoutEntity>().GetEntity<InvoiceoutEntity>(InvoiceoutId);
            var accountId = received == null || received.Account == null ? 0 : received.Account.Id;
            hfAccountId.Value = accountId.ToString();
            query.Query<OrderEntity>()
                 .Where(
                     it =>
                     it.Account.Id == received.Account.Id 
                     && it.TotalInvoiceAmount > it.InvoiceAmount);
            base.SetQueryWhere(query);
        }

        /// <summary>
        /// 批量修改金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBindOrder_Click(object sender, EventArgs e)
        {
            SaveEntities(GetPriceProductEntities(), "绑定订单成功", "绑定订单失败");
        }


        /// <summary>
        /// 得到价格实体
        /// </summary>
        protected virtual IList<InvoiceoutItemEntity> GetPriceProductEntities()
        {
            var infos = new List<InvoiceoutItemEntity>();
            foreach (GridViewRow gvr in GridView.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect==null || !ckSelect.Checked)
                    continue;
                var txtPrice = gvr.FindControl("txtPrice") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtPrice == null || txtRemark==null)
                    continue;
                var info = new InvoiceoutItemEntity
                    {
                        Id = ckSelect.Value.Convert<long>(),
                        Order = new OrderEntity { Id = ckSelect.Value.Convert<long>()},
                        Amount = txtPrice.Value.Convert<decimal>(),
                    
                        Invoiceout = new InvoiceoutEntity { Id = InvoiceoutId },
                        Remark = txtRemark.Value,
                        SaveType = SaveType.Add
                    };
                infos.Add(info);
            }
            return infos;
        }
    }
}