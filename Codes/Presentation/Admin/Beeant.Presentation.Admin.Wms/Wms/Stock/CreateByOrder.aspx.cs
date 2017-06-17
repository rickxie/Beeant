
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Wms.Wms.Stock
{
    public partial class CreateByOrder : WorkflowPageBase<StockEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return true; }
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

        public override long RequestId
        {
            get
            {
                return OrderId;
            }
        }
       
        public long OrderId
        {
            get { return Request.Params["OrderId"].Convert<long>(); }
        }

        protected OrderEntity Order;

        public override Button SaveButton
        {
            get { return btnSave; }
            set { base.SaveButton = value; }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
           
            if (!IsPostBack)
            {
                BindItemEntities();

            }
        }

        protected virtual long GetOrderId(long productId)
        {
            var order = Ioc.Resolve<IApplicationService, ProductEntity>().GetEntity<ProductEntity>(productId);
            return order.Goods.Id;
        }
        private void BindItemEntities()
        {
            var rows = Request.Params["rows"].Convert<int>();
            var stockItems = new List<StockItemEntity>();
            for (int i = 0; i < rows; i++)
            {
                var productId = Request.Params["ProductId" + i].Convert<long>();
                var count = Request.Params["Count" + i].Convert<int>();
                var storehouseId = Request.Params["StorehouseId" + i].Convert<long>();
                var name = Request.Params["Name" + i].Convert<string>();
                var stockItemEntity = new StockItemEntity
                    {
                        Product = Ioc.Resolve<IApplicationService, ProductEntity>().GetEntity<ProductEntity>(productId),
                        Count = count,
                        Storehouse = Ioc.Resolve<IApplicationService, StorehouseEntity>().GetEntity<StorehouseEntity>(storehouseId),
                        Name = name,
                    };
                stockItems.Add(stockItemEntity);
            }
            if (stockItems.Count > 0)
            {
                GridView1.DataSource = stockItems;
                GridView1.DataBind();
            }
        }


        protected override StockEntity FillEntity()
        {
            var stockEntity = base.FillEntity();
            stockEntity.Order = new OrderEntity { Id = OrderId };

            stockEntity.SaveType = SaveType.Add;
            stockEntity.Type = StockType.SalesOut;
            stockEntity.StockItems = new List<StockItemEntity>();
            foreach (GridViewRow gvr in GridView1.Rows)
            {

                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var txtCount = gvr.FindControl("txtCount") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtCount == null)
                    continue;
                var txtsubRemark = gvr.FindControl("txtRemark") as System.Web.UI.HtmlControls.HtmlInputText;
                if (txtsubRemark == null)
                    continue;
                var storehouseId = gvr.FindControl("hidStorehouseId") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var productId = gvr.FindControl("hidProductEntity") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var name = gvr.FindControl("hidName") as System.Web.UI.HtmlControls.HtmlInputHidden;
                var stockItem = new StockItemEntity
                {
                    Stock = stockEntity,
                    Storehouse = new StorehouseEntity { Id = storehouseId.Value.Convert<long>() },
                    Product = new ProductEntity { Id = productId.Value.Convert<long>() },
                    Name = name.Value,
                    Count = -(Math.Abs(txtCount.Value.Convert<int>())),
                    User = new UserEntity { Id = Identity.Id },
                    Remark = txtsubRemark.Value,
                    SaveType = SaveType.Add
                };
                stockEntity.StockItems.Add(stockItem);
            }
            return stockEntity;
        }
    }
}