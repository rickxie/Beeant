using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Purchase;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Domain.Entities.Management;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Wms.Wms.Stock
{
    public partial class CreateByPurchase : WorkflowPageBase<StockEntity>
    {
  
        public long PurchaseId
        {
            get { return Request.Params["Purchaseid"].Convert<long>(); }
        }
        public override Button SaveButton
        {
            get { return btnSave; }
            set { base.SaveButton = value; }
        }
        protected PurchaseEntity Purchase;
        protected override void Page_Load(object sender, EventArgs e)
        {
             
            base.Page_Load(sender, e);
            if (PurchaseId > 0)
            {
                var query = new QueryInfo();
                query.Query<PurchaseEntity>().Where(it => it.Id == PurchaseId).Select(it => new object[] {it, it.Account, it.Order});
                Purchase = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntities<PurchaseEntity>(query).FirstOrDefault();
            }
            if (!IsPostBack)
                BindStockItemEntities();
        }
        private void BindStockItemEntities()
        {
            var stockEntity = Ioc.Resolve<IPurchaseApplicationService, PurchaseEntity>().CreateStock(PurchaseId);
            if (stockEntity != null)
            {
                GridView1.DataSource = stockEntity.StockItems;
                GridView1.DataBind();
            }
        }

        protected override void Save()
        {
            if (GridView1.Rows.Count == 0)
            {
                var errs = new List<ErrorInfo>();
                errs.Add(new ErrorInfo { Key = "1", Message = "未选择任何数据" });
                base.SetResult(false, errs);
                return;
            }
            base.Save();
        }
        protected override StockEntity FillEntity()
        {
            var stockEntity = base.FillEntity();
            stockEntity.Purchase = new PurchaseEntity { Id = PurchaseId };
            stockEntity.SaveType = SaveType.Add;
            stockEntity.Type = StockType.PurchaseIn;
            stockEntity.StockItems = new List<StockItemEntity>();
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var txtCount = gvr.FindControl("txtCount") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtCount == null)
                    continue;
                var txtRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtRemark == null)
                    continue;
                var storehouseId = gvr.FindControl("hidStorehouseId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var productId = gvr.FindControl("hidProductEntity") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var name = gvr.FindControl("hidName") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var stockItem = new StockItemEntity
                {
                    Stock = stockEntity,
                    Product = new ProductEntity { Id = productId.Value.Convert<long>() },
                    Storehouse = new StorehouseEntity { Id = storehouseId.Value.Convert<long>() },
                    Name = name.Value,
                    Count = txtCount.Value.Convert<int>(),
                    User = new UserEntity { Id = Identity.Id },
                    Remark = txtRemark.Value,
                    SaveType = SaveType.Add
                };
                stockEntity.StockItems.Add(stockItem);
            }
            return stockEntity;
        }
    }
}