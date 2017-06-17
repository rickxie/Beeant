using System;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Purchase.Purchase
{
    public partial class Detail : WorkflowPageBase<PurchaseEntity>
    {
       
        public long OrderId { get; set; }
       
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
                ddlStockType.LoadData();
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 库存编号
        /// </summary>
        public StorehouseEntity Storehouse { get; set; }
        protected override PurchaseEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null&&info.Storehouse!=null)
            {
                OrderId = info.Order.Id;
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
                info.Storehouse=Ioc.Resolve<IApplicationService,StorehouseEntity>().GetEntity<StorehouseEntity>(info.Storehouse.Id);
                Storehouse = info.Storehouse;
            }
        
           
            return info;

        }

        /// <summary>
        /// 重写绑定
        /// </summary>
        /// <param name="container"></param>
        /// <param name="gridView"></param>
        /// <param name="paging"></param>
        /// <param name="isResetPageIndex"></param>
        protected override void BindItemEntities(System.Web.UI.Control container, GridView gridView, PagerControlBase paging, bool isResetPageIndex)
        {
            if (paging.ID == pgPurchaseItem.ID)
            {
                var query = paging.Query;
                query.Query<PurchaseItemEntity>()
                     .Where(
                         it =>
                         it.Purchase.Id == RequestId);
                if (Storehouse != null && Storehouse.Id > 0)
                {
                    query.SelectExp =
                           string.Format(
                               "{0},Product.Inventories.Count,Product.Inventories.LockCount,Product.Inventories.TransitCount,Product.Inventories.WarningCount,Product.Inventories.Storehouse.WarehouseName,Product.Inventories.Storehouse.Name,Product.Inventories.Storehouse.IsWarehouse",
                               paging.Query.SelectExp);
                }
                paging.Query = query;
                base.BindItemEntities(container, gridView, paging, isResetPageIndex);

                return;
            }
            base.BindItemEntities(container, gridView, paging, isResetPageIndex);
         
        }





        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ExecuteScript("Return();");
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ckSelect = e.Row.FindControl("hfSelect") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (ckSelect != null && ckSelect.Attributes["IsWarning"].Convert<bool>())
                {
                    e.Row.Attributes.Add("style", "color:#FF0000");
                }
            }

        }

    }
}