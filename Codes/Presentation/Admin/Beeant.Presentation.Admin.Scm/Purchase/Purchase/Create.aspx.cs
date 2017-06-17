using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;
using ScmProduct = Beeant.Domain.Entities.Product.ProductEntity;

namespace Beeant.Presentation.Admin.Scm.Purchase.Purchase
{
    public partial class Create : WorkflowPageBase<PurchaseEntity>
    {


       
        public long OrderId
        {
            get { return Request.Params["OrderId"].Convert<long>(); }
        }

        /// <summary>
        /// 加载消息
        /// </summary>
        protected override void LoadMessageControl()
        {
            if (MessageControl == null)
                MessageControl = Page.FindControl("Message1") as MessageControlBase;
        }
        protected override void SetResult(bool rev, IList<ErrorInfo> errors)
        {
            base.SetResult(rev, errors);
            if (rev)
            {
                Response.Write("<script language=javascript>top.location.reload();</script>");
            }
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
            }
            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                InitlizeOriginalPurchase();
                BindPurchaseItemEntities();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void InitlizeOriginalPurchase()
        {
            if (Request.Params["isSales"] != null)
            {
                if (Request.Params["isSales"].Convert<bool>())
                {
                    trStorehouse.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
            }
       
            ckUser.InputHidden.Value = cbUser.InputHidden.Value;
            ckUser.InputText.Value= cbUser.InputHidden.Value;
            //采购类型 采购日期
            ddlType.DropDownList.SelectedValue = Enum.GetName(typeof(PurchaseType), PurchaseType.Sales);
            txtPurchaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (Request.Params["orderId"] != null)
            {
                var orderId = Request.Params["orderid"].Convert<long>();
                if (orderId > 0)
                {
                    var info = Ioc.Resolve<IApplicationService, OrderEntity>().GetEntity<OrderEntity>(orderId);
                    if (info == null) return;
                    txtOrderId.Value = orderId.ToString();
                }
            }
            //账户
            if (Request.Params["suplierId"] != null)
            {
                var suplierId = Request.Params["suplierId"].Convert<long>();
                var query = new QueryInfo();
                query.Query<SupplierEntity>()
                     .Where(it => it.Id == suplierId)
                     .Select(it => new object[] { it.Account.Id, it.Account.Name });
                var infos = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntities<SupplierEntity>(query);
                var info = infos == null ? null : infos.FirstOrDefault();
                if (info !=null && info.Account != null)
                {
                    AccountComboBox1.InputHidden.Value = info.Account.Id.ToString();
                    AccountComboBox1.InputText.Value = info.Account.Name;
                }

            }
        }
        protected virtual void BindPurchaseItemEntities()
        {
            var rows = Request.Params["rows"].Convert<int>();
            var purchaseItems = new List<PurchaseItemEntity>();
            for (int i = 0; i < rows; i++)
            {
                var productId = Request.Params["ProductId" + i].Convert<long>();
                var count = Request.Params["Count" + i].Convert<int>();
                var price = Request.Params["Price" + i].Convert<decimal>();

                var purchaseItemEntity = new PurchaseItemEntity
                    {
                        Product = Ioc.Resolve<IApplicationService, ScmProduct>().GetEntity<ScmProduct>(productId),
                        Count = count,
                        Price = price,
                    };
                purchaseItems.Add(purchaseItemEntity);
            }

            if (purchaseItems.Count > 0)
            {
                GridView2.DataSource = purchaseItems;
                GridView2.DataBind();
            }
        }

        protected override PurchaseEntity FillEntity()
        {
            var purchaseEntity = base.FillEntity();
            purchaseEntity.OriginalPurchase = new PurchaseEntity { Id = 0 };
            purchaseEntity.Order = new OrderEntity { Id = OrderId };
            purchaseEntity.SaveType = SaveType.Add;
            purchaseEntity.PurchaseItems = new List<PurchaseItemEntity>();
            foreach (GridViewRow gvr in GridView2.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (!ckSelect.Checked)
                    continue;

                var txtPrice = gvr.FindControl("txtPrice") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtPrice == null)
                    continue;
                var txtCount = gvr.FindControl("txtCount") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtCount == null)
                    continue;
                var productId = gvr.FindControl("hidProductId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var txtRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtName = gvr.FindControl("txtName") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (txtName == null)
                    continue;

                var purchaseItem = new PurchaseItemEntity
                {
                    Purchase = purchaseEntity,
                    Product = new ScmProduct { Id = productId.Value.Convert<long>() },
                    Name = txtName.Value.Convert<string>(),
                    Price = txtPrice.Value.Convert<decimal>(),
                    Count = txtCount.Value.Convert<int>(),
                    Remark = txtRemark == null ? "" : txtRemark.Value.Trim(),
                    SaveType = SaveType.Add
                };
                purchaseItem.Amount = purchaseItem.Count * purchaseItem.Price;
                purchaseEntity.PurchaseItems.Add(purchaseItem);
            }
            return purchaseEntity;
        }


    }
}