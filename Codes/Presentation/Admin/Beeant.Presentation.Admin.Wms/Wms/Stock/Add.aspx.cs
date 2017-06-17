using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Wms.Wms.Stock
{
    public partial class Add : WorkflowPageBase<StockEntity>
    {
        

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
                LoadProducts();
            }
            base.Page_Load(sender, e);
           
        }
        /// <summary>
        ///  加载商品
        /// </summary>
        protected virtual void LoadProducts()
        {
            IList<ProductEntity> infos = null;
            if (!string.IsNullOrEmpty(Request.QueryString["productIds"]))
            {
                infos = GetProductsByProductIds();
            }
            gvProduct.DataSource = infos;
            gvProduct.DataBind();
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
                       it.Id,it.Name
                    });
                return Ioc.Resolve<IApplicationService, ProductEntity>().GetEntities<ProductEntity>(query);

            }
            return null;
        }

        /// <summary>
        /// 填充出库单
        /// </summary>
        /// <returns></returns>
        protected override StockEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                FillStockItems(info);
            }
            return info;
        }
        /// <summary>
        /// 填充发票
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillStockItems(StockEntity info)
        {
            info.StockItems = new List<StockItemEntity>();
            foreach (GridViewRow gvr in gvProduct.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("hfId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (ckSelect == null)
                    continue;
                var txtsubCount = gvr.FindControl("txtCount") as System.Web.UI.HtmlControls.HtmlInputText;
                var hfName = gvr.FindControl("hfName") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var hfStorehouseId = gvr.FindControl("hfStorehouseId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var txtsubRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (hfStorehouseId == null || txtsubCount == null || txtCount == null || hfName == null || txtsubRemark == null)
                    continue;
                var purchaseItem = new StockItemEntity
                {
                    Product = new ProductEntity { Id = ckSelect.Value.Convert<long>() },
                    Count = txtsubCount.Value.Convert<int>(),
                    Name = hfName.Value,
                    Stock = info,
                    Storehouse = new StorehouseEntity { Id = hfStorehouseId.Value.Convert<long>() },
                    User = new UserEntity { Id = Identity.Id },
                    Remark = txtsubRemark.Value,
                    SaveType = SaveType.Add
                };
                info.StockItems.Add(purchaseItem);
            }
            FillStockItemsByProductIds(info);
        }
        /// <summary>
        /// 加载产品明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void FillStockItemsByProductIds(StockEntity info)
        {
            if (string.IsNullOrEmpty(hfProducts.Value))
                return;
            var stockItems = hfProducts.Value.DeserializeJson<List<Dictionary<string,object>>>();
            if (stockItems != null)
            {
                foreach (var subStockItem in stockItems)
                {
                    var values = subStockItem as Dictionary<string, object>;
                    if (values==null)continue;
                    var stockItem = new StockItemEntity{Stock=info,User=new UserEntity{Id=Identity.Id},SaveType = SaveType.Add};
                    foreach (var value in values)
                    {
                        switch (value.Key)
                        {
                            case "Id":
                                stockItem.Product = new ProductEntity { Id = value.Value.Convert<long>() };
                                break;
                            case "Count":
                                stockItem.Count = value.Value.Convert<int>();
                                break;
                            case "Name":
                                stockItem.Name = value.Value.ToString();
                                break;
                            case "Remark":
                                stockItem.Remark = value.Value.ToString();
                                break;
                            case "StorehouseId":
                                stockItem.Storehouse = new StorehouseEntity { Id = value.Value.Convert<long>() };
                                break;
                        }
                    }
                    info.StockItems.Add(stockItem);
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