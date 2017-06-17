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
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Payout
{
    public partial class Add : WorkflowPageBase<PayoutEntity>
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
                SetPayName();
                LoadPurchases();
            }
            base.Page_Load(sender, e);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            cbBank.AccountInputHidden = cbAccount.InputHidden;
            cbBank.BankHolderText =txtBankHolder;
            cbBank.BankNameText = txtBankName;
        }
        protected override PayoutEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
  

        protected override PayoutEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.ChannelType=ChannelType.Admin;
                FillPaids(info);
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
            gvPaid.DataSource = infos;
            gvPaid.DataBind();
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
                        it.Id, it.Status,it.InsertTime,it.TotalAmount,it.PayAmount
                    });
                return Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntities<PurchaseEntity>(query);

            }
            return null;
        }
        /// <summary>
        /// 填充发票
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillPaids(PayoutEntity info)
        {
            info.PayoutItems = new List<PayoutItemEntity>();
            foreach (GridViewRow gvr in gvPaid.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("hfId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (ckSelect == null)
                    continue;
                var txtPaidAmount = gvr.FindControl("txtAmount") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtPaidRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtPaidAmount == null || txtPaidRemark == null)
                    continue;
                var payoutItem = new PayoutItemEntity
                {
                    Purchase = new PurchaseEntity { Id = ckSelect.Value.Convert<long>() },
                    Amount = txtPaidAmount.Value.Convert<decimal>(),
                    Remark = txtPaidRemark.Value,
                    Payout = info,
                   
                    SaveType = SaveType.Add
                };
                info.PayoutItems.Add(payoutItem);
            }
            FillPaidsByPurchaseIds(info);
        }
        /// <summary>
        /// 加载产品明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillPaidsByPurchaseIds(PayoutEntity info)
        {
            if (string.IsNullOrEmpty(hfPurchases.Value))
                return;
            var paids = hfPurchases.Value.DeserializeJson<List<PayoutItemEntity>>();
            if (paids != null)
            {
                foreach (var subpaid in paids)
                {
                    var paid = new PurchasePayEntity
                    {
                        Purchase = new PurchaseEntity { Id = subpaid.Id },
                        Amount = subpaid.Amount,
                        Remark = subpaid.Remark,
                        Payout = info,
                        SaveType = SaveType.Add
                    };
                    info.Pays.Add(paid);
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
            var supplier = GetSupplier();
            if (supplier != null && !string.IsNullOrEmpty(supplier.Name))
            {
                name = supplier.Name;
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
            txtName.Value = string.Format("{0}/{1}/付款单",name,DateTime.Now.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// 得到供应商
        /// </summary>
        /// <returns></returns>
        protected virtual SupplierEntity GetSupplier()
        {
            var query = new QueryInfo();
            query.Query<SupplierEntity>()
                 .Where(it => it.Account.Id == cbAccount.InputHidden.Value.Convert<long>())
                 .Select(it => it.Name);
            var infos = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntities<SupplierEntity>(query);
            return infos == null ? null : infos.FirstOrDefault();
        }
    }
}