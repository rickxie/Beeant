using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Purchase.Purchase
{
    public partial class Add : WorkflowPageBase<PurchaseEntity>
    {
         
        public long ProductId
        {
            get { return Request.QueryString["ProductId"].Convert<long>(); }
        }

        public PurchaseEntity Purchase { get; set; }
        /// <summary>
        /// 重写加载
        /// </summary>
        /// <returns></returns>
        protected override PurchaseEntity GetEntity()
        {
            var query = new QueryInfo();
            query.Query<PurchaseEntity>()
                 .Where(it => it.Id == RequestId)
                 .Select(it => new object[] { it, it.Account.Name, it.PurchaseItems.Select(s => s.Product.Id) });
            var infos = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntities<PurchaseEntity>(query);
            if (infos == null)
                return null;
            var info = infos.FirstOrDefault();
            Purchase = info;
            return info;

        }

        /// <summary>
        /// 原始订单
        /// </summary>
        public PurchaseEntity OriginalPurchase { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void InitlizeDefautValue()
        {
       
            if (!string.IsNullOrEmpty(Request.QueryString["OriginalPurchaseId"]))
            {
                txtOriginalPurchase.Text = Request.QueryString["OriginalPurchaseId"];
                ddlType.DropDownList.SelectedIndex = ddlType.DropDownList.Items.IndexOf(ddlType.DropDownList.Items.FindByValue("Return"));
                ddlType.DropDownList.Enabled = false;
                txtOriginalPurchase.Enabled = false;
                var query = new QueryInfo();
                query.Query<PurchaseEntity>()
                     .Where(it => it.Id == Request.QueryString["OriginalPurchaseId"].Convert<long>())
                     .Select(it => new object[] { it, it.Account.Name, it.PurchaseItems.Select(s => new object[] { s.Price, s.Product.Id }) });
                var infos = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntities<PurchaseEntity>(query);
                var info = infos == null ? null : infos.FirstOrDefault();
                OriginalPurchase = info;
                if (info != null && info.Account != null)
                {
                    SupplierAccountComboBox1.InputHidden.Value = info.Account.Id.ToString();
                    var suplier = GetSupplier();
                    if (suplier != null)
                    {
                        SupplierAccountComboBox1.InputText.Value = suplier.Name;
                    }
                }
            }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
                txtDeliveryDate.Text = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
                ckUser.InputHidden.Value = Identity.Id.ToString();
                ckUser.InputText.Value = Identity.Name;
                txtPurchaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ddlType.DropDownList.SelectedIndex = ddlType.DropDownList.Items.IndexOf(ddlType.DropDownList.Items.FindByValue("Wms"));
                LoadDefaultWms();
                LoadProducts();

            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 加载默认仓库
        /// </summary>
        protected virtual void LoadDefaultWms()
        {
            var query = new QueryInfo();
            query.SetPageSize(1)
                 .Query<StorehouseEntity>()
                 .Where(it => it.IsUsed )
                 .Select(it => new object[] {it.Id, it.Name});
            var infos = Ioc.Resolve<IApplicationService, StorehouseEntity>().GetEntities<StorehouseEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                StorehouseComboBox1.InputHidden.Value = infos.First().Id.ToString();
                StorehouseComboBox1.InputText.Value = infos.First().Name;
            }
        }
 
        /// <summary>
        ///  加载商品
        /// </summary>
        protected virtual void LoadProducts()
        {
            IList<ProductEntity> infos = null;
            if (Purchase != null && Purchase.PurchaseItems != null)
            {
                infos = GetProductsByOrders();
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["productIds"]))
            {
                infos = GetProductsByProductIds();
            }
            SetProductPrices(infos);
            gvProduct.DataSource = infos;
            gvProduct.DataBind();
        }

        /// <summary>
        ///  得到商品查询
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ProductEntity> GetProductsByOrders()
        {
            var productIds =
                Purchase.PurchaseItems.Where(it => it.Product != null && it.Product.Id > 0)
                     .Select(it => it.Product.Id)
                     .ToArray();
            if (productIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<ProductEntity>().Where(it => productIds.Contains(it.Id)).Select(it => new object[]
                    {
                       it.Id,it.Name,it.Goods.Category.Name,it.Price,it.IsSales,it.Cost
                       ,it.Inventories.Count,it.Inventories.Select(s=>new object[]{s.LockCount,s.TransitCount,s.WarningCount,s.Storehouse.Name})
                    });
                return Ioc.Resolve<IApplicationService, ProductEntity>().GetEntities<ProductEntity>(query);

            }
            return null;
        }

        /// <summary>
        ///  得到商品查询
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ProductEntity> GetProductsByProductIds()
        {

            var values = Request.QueryString["productIds"].Split(',');
            var productIds = values.Select(value => value.Convert<long>()).ToArray();
            if (productIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<ProductEntity>().Where(it => productIds.Contains(it.Id)).Select(it => new object[]
                    {
                       it.Id,it.Name,it.Goods.Category.Name,it.Price,it.IsSales,it.Cost
                       ,it.Inventories.Count,it.Inventories.Select(s=>new object[]{s.LockCount,s.TransitCount,s.WarningCount,s.Storehouse.Name})
                    });
                return Ioc.Resolve<IApplicationService, ProductEntity>().GetEntities<ProductEntity>(query);

            }
            return null;
        }


        /// <summary>
        /// 设置产品价格
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void SetProductPrices(IList<ProductEntity> infos)
        {
            if (infos == null)
                return;
            foreach (var info in infos)
            {
                if(OriginalPurchase==null)continue;
                var orderItem = OriginalPurchase.PurchaseItems == null
                                    ? null
                                    : OriginalPurchase.PurchaseItems.FirstOrDefault(
                                        it => it.Product != null && it.Product.Id == info.Id);
                if (orderItem == null) continue;
                info.Price = orderItem.Price;
                info.Cost = orderItem.Price;
                info.Count = orderItem.Count;
            }
        }

    
        /// <summary>
        /// 得到供应商
        /// </summary>
        /// <returns></returns>
        protected virtual SupplierEntity GetSupplier()
        {
            if (SupplierAccountComboBox1.InputHidden.Value == "")
                return null;
            var query = new QueryInfo();
            query.Query<SupplierEntity>()
                 .Where(it => it.Account.Id == SupplierAccountComboBox1.InputHidden.Value.Convert<long>())
                 .Select(it => new object[]{it.Id,it.Name});
            var infos = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntities<SupplierEntity>(query);
            return infos == null ? null : infos.FirstOrDefault();
        }

        protected override PurchaseEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                FillPurchaseItems(info);
            }
            return info;
        }
        /// <summary>
        /// 填充发票
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillPurchaseItems(PurchaseEntity info)
        {
            info.PurchaseItems = new List<PurchaseItemEntity>();
            foreach (GridViewRow gvr in gvProduct.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("hfId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (ckSelect == null)
                    continue;
                var txtCost = gvr.FindControl("txtCost") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtCount = gvr.FindControl("txtCount") as System.Web.UI.HtmlControls.HtmlInputText;
                var hfName = gvr.FindControl("hfName") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (txtCost == null  || txtCount == null || hfName == null)
                    continue;
                var purchaseItem = new PurchaseItemEntity
                {
                    Product = new ProductEntity { Id = ckSelect.Value.Convert<long>() },
                    Price = txtCost.Value.Convert<decimal>(),
                    Count = txtCount.Value.Convert<int>(),
                    Name=hfName.Value,
                    Purchase = info,
                    SaveType = SaveType.Add
                };
                info.PurchaseItems.Add(purchaseItem);
            }
            FillPurchaseItemsByProductIds(info);
        }
        /// <summary>
        /// 加载产品明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillPurchaseItemsByProductIds(PurchaseEntity info)
        {
            if (string.IsNullOrEmpty(hfProducts.Value))
                return;
            var purchaseItems = hfProducts.Value.DeserializeJson<List<PurchaseItemEntity>>();
            if (purchaseItems != null)
            {
                foreach (var subPurchaseItem in purchaseItems)
                {
                    var purchaseItem = new PurchaseItemEntity
                    {
                        Product = new ProductEntity { Id = subPurchaseItem.Id },
                        Price = subPurchaseItem.Price,
                        Count = subPurchaseItem.Count,
                        Name = subPurchaseItem.Name,
                        Purchase = info,
                        Remark = subPurchaseItem.Remark,
                        SaveType = SaveType.Add
                    };
                    info.PurchaseItems.Add(purchaseItem);
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