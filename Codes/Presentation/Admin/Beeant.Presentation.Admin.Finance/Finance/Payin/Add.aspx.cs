using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Payin
{
    public partial class Add : WorkflowPageBase<PayinEntity>
    {
       
        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ExecuteScript(string.Format("validator.BindControlValidateEvent(document.getElementById('{0}'), 'change');",txtPayTime.ClientID));
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtPayTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ddlCurrency.LoadData();
                ddlPayType.LoadData();
                if (ddlCurrency.DropDownList.Items.Count > 1)
                {
                    ddlCurrency.DropDownList.Items[1].Selected = true;
                }
                if (ddlPayType.DropDownList.Items.Count > 1)
                {
                    ddlPayType.DropDownList.Items[1].Selected = true;
                }
                LoadOrders();
                SetPayName();
            }
            base.Page_Load(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            cbBank.AccountInputHidden = cbAccount.InputHidden;
            cbBank.BankHolderText = txtBankHolder;
            cbBank.BankNameText = txtBankName;
        }

        protected override PayinEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
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
            gvReceived.DataSource = infos;
            gvReceived.DataBind();
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
                        it.Id, it.Status,it.InsertTime,it.TotalAmount,it.TotalInvoiceAmount, it.InvoiceAmount
                    });
                return Ioc.Resolve<IApplicationService, OrderEntity>().GetEntities<OrderEntity>(query);

            }
            return null;
        }

        protected override PayinEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.ChannelType = ChannelType.Admin;
                FillReceiveds(info);
            }
            return info;
        }
        /// <summary>
        /// 填充发票
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillReceiveds(PayinEntity info)
        {
            info.PayinItems = new List<PayinItemEntity>();
            foreach (GridViewRow gvr in gvReceived.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("hfId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (ckSelect == null)
                    continue;
                var txtReceivedAmount = gvr.FindControl("txtAmount") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtReceivedRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtReceivedAmount == null || txtReceivedRemark == null)
                    continue;
                var PayinItem = new PayinItemEntity
                {
                    Order = new OrderEntity { Id = ckSelect.Value.Convert<long>() },
                    Amount = txtReceivedAmount.Value.Convert<decimal>(),
                    Remark = txtReceivedRemark.Value,
                    Payin = info,
                    SaveType = SaveType.Add
                };
                info.PayinItems.Add(PayinItem);
            }
            FillReceivedsByOrderIds(info);
        }
        /// <summary>
        /// 加载产品明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillReceivedsByOrderIds(PayinEntity info)
        {
            if (string.IsNullOrEmpty(hfOrders.Value))
                return;
            var payinItems = hfOrders.Value.DeserializeJson<List<PayinItemEntity>>();
            if (payinItems != null)
            {
                foreach (var subreceived in payinItems)
                {
                    var payinItem = new PayinItemEntity
                    {
                        Order = new OrderEntity { Id = subreceived.Id },
                        Amount = subreceived.Amount,
                        Remark = subreceived.Remark,
                        Payin = info,
                        SaveType = SaveType.Add
                    };
                    info.PayinItems.Add(payinItem);
                }
            }
        }
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
            this.ExecuteScript("registerFunc();");
        }
        public override void Search_Click(object sender, EventArgs e)
        {
            base.Search_Click(sender, e);
            SetPayName();
        }
        /// <summary>
        /// 得到名称
        /// </summary>
        /// <returns></returns>
        protected virtual void SetPayName()
        {
            if (cbAccount.InputHidden.Value == "")
            {
                return;
            }
            var name = "";
            var agent = GetAgent();
            if (agent != null && !string.IsNullOrEmpty(agent.Name))
            {
                name = agent.Name;
            }
            if (string.IsNullOrEmpty(name))
            {
                var account =
               Ioc.Resolve<IApplicationService, AccountEntity>()
                  .GetEntity<AccountEntity>(cbAccount.InputHidden.Value.Convert<long>());
                if (account != null)
                {
                    name = !string.IsNullOrEmpty(account.RealName) ? account.RealName : account.Name;
                }
            }
            txtName.Value = string.Format("{0}/{1}/收款单", name, DateTime.Now.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// 得到供应商
        /// </summary>
        /// <returns></returns>
        protected virtual AgentEntity GetAgent()
        {
            var query = new QueryInfo();
            query.Query<AgentEntity>()
                 .Where(it => it.Account.Id == cbAccount.InputHidden.Value.Convert<long>())
                 .Select(it => it.Name);
            var infos = Ioc.Resolve<IApplicationService, AgentEntity>().GetEntities<AgentEntity>(query);
            return infos == null ? null : infos.FirstOrDefault();
        }
    }
}