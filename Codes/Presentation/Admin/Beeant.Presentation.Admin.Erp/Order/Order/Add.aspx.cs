using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Promotion;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Order.Order
{
    public partial class Add : WorkflowPageBase<OrderEntity>
    {

        

        public long ProductId
        {
            get { return Request.QueryString["productid"].Convert<long>(); }
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
                txtOrderDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ddlType.DropDownList.SelectedIndex = ddlType.DropDownList.Items.IndexOf(ddlType.DropDownList.Items.FindByValue("Standard"));
                txtDeposit.Text = "0";
                txtOriginalOrder.Text = Request.QueryString["OriginalOrderId"];
                InitlizeOriginalOrder();
                LoadProducts();
            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 原始订单
        /// </summary>
        public OrderEntity OriginalOrder { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void InitlizeOriginalOrder()
        {
            if (!string.IsNullOrEmpty(txtOriginalOrder.Text))
            {
                ddlType.DropDownList.SelectedIndex = ddlType.DropDownList.Items.IndexOf(ddlType.DropDownList.Items.FindByValue("Return"));
                var query = new QueryInfo();
                query.Query<OrderEntity>()
                     .Where(it => it.Id == txtOriginalOrder.Text.Convert<long>())
                     .Select(it => new object[] { it, it.Account.Name,it.OrderProducts.Select(s=>new object[]{s.Price,s.Product.Id})});
                var infos = Ioc.Resolve<IApplicationService, OrderEntity>().GetEntities<OrderEntity>(query);
                var info = infos == null ? null : infos.FirstOrDefault();
                OriginalOrder = info;
                if (info != null && info.Account!=null)
                {
                    AccountComboBox1.InputHidden.Value = info.Account.Id.ToString();
                    AccountComboBox1.InputText.Value = info.Account.Name;
                    AccountComboBox1.InputText.Disabled = false;
                }
            }
        }

        public OrderEntity Order { get; set; }
        /// <summary>
        /// 重写加载
        /// </summary>
        /// <returns></returns>
        protected override OrderEntity GetEntity()
        {
            var query = new QueryInfo();
            query.Query<OrderEntity>()
                 .Where(it => it.Id == RequestId)
                 .Select(it => new object[] {it, it.Account.Name, it.OrderProducts.Select(s => s.Product.Id)});
            var infos = Ioc.Resolve<IApplicationService, OrderEntity>().GetEntities<OrderEntity>(query);
            if (infos == null)
                return null;
            var info = infos.FirstOrDefault();
            Order = info;
            return info;

        }
        /// <summary>
        ///  加载商品
        /// </summary>
        protected virtual void LoadProducts()
        {
            IList<ProductEntity> infos=null;
            if (Order != null && Order.OrderProducts != null)
            {
               infos=GetProductsByOrders();
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
                Order.OrderProducts.Where(it => it.Product != null && it.Product.Id > 0)
                     .Select(it => it.Product.Id)
                     .ToArray();
            if (productIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<ProductEntity>().Where(it => productIds.Contains(it.Id)).Select(it => new object[]
                    {
                        it.Id, it.Name, it.Price, it.Cost, it.Count, it.IsReturn,
                        it.Goods.GoodsDetails.Select(s=>new object[] {s.Name,s.Detail}), it.OrderMinCount
                    });
                return Ioc.Resolve<IApplicationService, ProductEntity>().GetEntities<ProductEntity>(query);
               
            }
            return null;
        }

        /// <summary>
        /// 得到包装清单
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual string GetDescription(GoodsEntity goods,long productId)
        {
            if (goods == null || goods.GoodsDetails == null)
                return "";
            var goodsDetail = goods.GoodsDetails.FirstOrDefault(it => it.Name == "PackageDescription" && it.Product != null && it.Product.Id == productId);
            if (goodsDetail != null)
                return goodsDetail.Detail;
            goodsDetail = goods.GoodsDetails.FirstOrDefault(it => it.Name == "PackageDescription" && it.Product != null && it.Product.Id == 0);
            if (goodsDetail != null)
                return goodsDetail.Detail;
            return "";
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
                        it.Id, it.Name, it.Price, it.Cost, it.Count, it.Goods.IsReturn,
                        it.Goods.GoodsDetails.Select(s=>new object[] {s.Name,s.Detail}), it.OrderMinCount
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
            if(infos==null)
                return;
            foreach (var info in infos)
            {
                var orderProduct = OriginalOrder.OrderProducts == null
                                    ? null
                                    : OriginalOrder.OrderProducts.FirstOrDefault(
                                        it => it.Product != null && it.Product.Id == info.Id);
                if (orderProduct == null) continue;
                info.Price = orderProduct.Price;
                info.Cost = orderProduct.Price;
                info.Count = orderProduct.Count;
            }
        }
 

        protected override OrderEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.ChannelType = ChannelType.Admin;
                FillOrderProducts(info);
            }
            return info;
        }

        /// <summary>
        /// 填充发票
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillOrderProducts(OrderEntity info)
        {
            info.OrderProducts = new List<OrderProductEntity>();
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
                var hfIsReturn = gvr.FindControl("hfIsReturn") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var hfDescription = gvr.FindControl("hfDescription") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var txtSubRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtCost == null || txtCount == null || hfName == null || hfIsReturn == null || hfDescription == null || txtSubRemark==null)
                    continue;
                var orderProduct = new OrderProductEntity
                {
                    Product = new ProductEntity { Id = ckSelect.Value.Convert<long>() },
                    Price = txtCost.Value.Convert<decimal>(),
                    Count = ddlType.DropDownList.SelectedValue == "Return" ? 0 - txtCount.Value.Convert<int>() : txtCount.Value.Convert<int>(),
                    Name = hfName.Value,
                    Order = info,
                    IsReturn =hfIsReturn.Convert<bool>(),
                    Description=hfDescription.Value,
                    Promotion=new PromotionEntity{Id=0},
                    Remark=txtSubRemark.Value,
                    SaveType = SaveType.Add
                };
                info.OrderProducts.Add(orderProduct);
            }
            FillOrderProductsByProductIds(info);
        }
        /// <summary>
        /// 加载产品明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillOrderProductsByProductIds(OrderEntity info)
        {
            if (string.IsNullOrEmpty(hfProducts.Value))
                return;
            var orderProducts = hfProducts.Value.DeserializeJson<List<OrderProductEntity>>();
            if (orderProducts != null)
            {
                foreach (var subOrderProduct in orderProducts)
                {
                    var orderProduct = new OrderProductEntity
                    {
                        Product = new ProductEntity{Id=subOrderProduct.Id},
                        Price = subOrderProduct.Price,
                        Count = subOrderProduct.Count,
                        Name = subOrderProduct.Name,
                        Order = info,
                        IsReturn =subOrderProduct.IsReturn ,
                        Description = subOrderProduct.Description,
                        Remark = subOrderProduct.Remark,
                        SaveType = SaveType.Add
                    };
                    info.OrderProducts.Add(orderProduct);
                }
            }
        }
        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ExecuteScript("Init();");
        }
    }
}