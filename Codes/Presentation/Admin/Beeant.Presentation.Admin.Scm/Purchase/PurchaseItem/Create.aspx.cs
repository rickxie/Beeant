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
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Purchase.PurchaseItem
{
    public partial class Create : AddPageBase<PurchaseItemEntity>
    {

        public int Index = 0;
        public override Button SaveButton
        {
            get { return btnSave; }
            set { base.SaveButton = value; }
        }
        protected override PurchaseItemEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Purchase = new PurchaseEntity { Id = Request.QueryString["PurchaseId"].Convert<long>() };
            }
            return info;
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            LoadStorehouse();
            LoadSupplier();
            if (!IsPostBack)
            {
                ddlSearchStatus.LoadData();
                ddlSearchStatus.DropDownList.SelectedIndex = ddlSearchStatus.DropDownList.Items.IndexOf(ddlSearchStatus.DropDownList.Items.FindByValue("Normal"));
            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 库存编号
        /// </summary>
        public StorehouseEntity Storehouse { get; set; }
        /// <summary>
        /// 加载仓库
        /// </summary>
        protected virtual void LoadStorehouse()
        {
            var query = new QueryInfo();
            query.Query<PurchaseEntity>()
                 .Where(it => it.Id == Request.QueryString["PurchaseId"].Convert<long>())
                 .Select(it => new object[] {it.Storehouse.Name});
            var infos = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntities<PurchaseEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                Storehouse = infos.First().Storehouse;
            }
        }
        /// <summary>
        /// 供应商
        /// </summary>
        public SupplierEntity Supplier { get; set; }
        /// <summary>
        /// 重写加载
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadSupplier()
        {
            var query = new QueryInfo();
            query.Query<PurchaseEntity>()
                 .Where(it => it.Id == Request.QueryString["PurchaseId"].Convert<long>())
                 .Select(it => new object[] { it.Account.Id });
            var infos = Ioc.Resolve<IApplicationService, PurchaseEntity>().GetEntities<PurchaseEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                var account = infos.First().Account;
                if (account != null)
                {
                    var querySupplier = new QueryInfo();
                    querySupplier.Query<SupplierEntity>()
                         .Where(it => it.Account.Id == account.Id)
                         .Select(it => new object[] { it.Id });
                    var suppliers = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntities<SupplierEntity>(querySupplier);
                    if (suppliers != null && suppliers.Count > 0)
                    {
                        Supplier = suppliers.First();
                    }
                }
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        protected override void LoadEntity()
        {
            var purchase = this.GetEntity<PurchaseEntity>(Request["PurchaseId"].Convert<long>());
            if (purchase.Status != PurchaseStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("您没有权限在该状态下添加采购单明细");
            base.LoadEntity();
        }

        /// <summary>
        /// 重写保存
        /// </summary>
        protected override void Save()
        {
            var infos = GetPurchaseItems();
            var rev = Ioc.Resolve<IApplicationService, PurchaseItemEntity>().Save(infos);
            var erros = new List<ErrorInfo>();
            if (!rev)
            {
                foreach (var info in infos)
                {
                    erros.AddList(info.Errors);
                }
            }
            SetResult(rev, erros);
        }
     
        /// <summary>
        /// 得到采购明细
        /// </summary>
        protected virtual IList<PurchaseItemEntity> GetPurchaseItems()
        {
          var infos = new List<PurchaseItemEntity>();
            foreach (GridViewRow gvr in gvProduct.Rows)
            {
                if (gvr.RowType != DataControlRowType.DataRow)
                    continue;
                var ckSelect = gvr.FindControl("ckSelect") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                if (ckSelect == null || !ckSelect.Checked)
                    continue;
                var txtCost = gvr.FindControl("txtCost") as System.Web.UI.HtmlControls.HtmlInputText;
                var txtCount = gvr.FindControl("txtCount") as System.Web.UI.HtmlControls.HtmlInputText;
                var hfName = gvr.FindControl("hfName") as System.Web.UI.HtmlControls.HtmlInputHidden;
                if (txtCost == null || txtCount == null || hfName == null)
                    continue;
                var purchaseItem = new PurchaseItemEntity
                {
                    Product = new ProductEntity { Id = ckSelect.Value.Convert<long>() },
                    Price = txtCost.Value.Convert<decimal>(),
                    Count = txtCount.Value.Convert<int>(),
                    Name = hfName.Value,
                    Purchase = new PurchaseEntity { Id = Request.QueryString["PurchaseId"].Convert<long>() },
                    SaveType = SaveType.Add
                };
                infos.Add(purchaseItem);
            }
            return infos;
        }

    
       

        /// <summary>
        /// 重写绑定
        /// </summary>
        /// <param name="container"></param>
        /// <param name="gridView"></param>
        /// <param name="paging"></param>
        /// <param name="isResetPageIndex"></param>
        protected override void BindItemEntities(System.Web.UI.Control container, GridView gridView, Basic.Services.WebForm.Controls.PagerControlBase paging, bool isResetPageIndex)
        {
            if (paging.ID == pgProduct.ID)
            {
                var query = paging.Query;
                if (Storehouse != null && Storehouse.Id > 0)
                {
                    query.SelectExp =
                           string.Format(
                               "{0},Inventories.Count,Inventories.LockCount,Inventories.TransitCount,Inventories.WarningCount,Inventories.Storehouse.Name",
                               paging.Query.SelectExp);
                }
                paging.Query = query;
                base.BindItemEntities(container, gridView, paging, isResetPageIndex);
                return;
            }
            base.BindItemEntities(container, gridView, paging, isResetPageIndex);

        }

        #region 得到库存信息
        /// <summary>
        /// 得到库存数量
        /// </summary>
        /// <param name="inventories"></param>
        /// <returns></returns>
        public virtual int GetInventoryCount(InventoryEntity[] inventories)
        {
            if (inventories == null)
                return 0;
            var count = 0;
            var storehouseName = Storehouse != null ? Storehouse.Name : "";
            foreach (var inventory in inventories)
            {
                if (inventory.Storehouse == null || string.IsNullOrEmpty(inventory.Storehouse.Name))
                    continue;
                var name = inventory.Storehouse.Name.Split('-')[0];
                if (storehouseName == name)
                {
                    count += inventory.Count;
                }
            }
            return count;
        }
        /// <summary>
        /// 得到库存数量
        /// </summary>
        /// <param name="inventories"></param>
        /// <returns></returns>
        public virtual int GetInventoryWarningCount(InventoryEntity[] inventories)
        {
            if (inventories == null)
                return 0;
            var count = 0;
            var storehouseName = Storehouse != null ? Storehouse.Name : "";
            foreach (var inventory in inventories)
            {
                if (inventory.Storehouse == null || string.IsNullOrEmpty(inventory.Storehouse.Name))
                    continue;
                var name = inventory.Storehouse.Name.Split('-')[0];
                if (storehouseName == name)
                {
                    count += inventory.WarningCount;
                }
            }
            return count;
        }
        /// <summary>
        /// 得到在途库存
        /// </summary>
        /// <param name="inventories"></param>
        /// <returns></returns>
        public virtual int GetInventoryTransitCount(InventoryEntity[] inventories)
        {
            if (inventories == null)
                return 0;
            var storehouseName = Storehouse != null ? Storehouse.Name : "";
            var count = 0;
            foreach (var inventory in inventories)
            {
                if (inventory.Storehouse == null || string.IsNullOrEmpty(inventory.Storehouse.Name))
                    continue;
                var name = inventory.Storehouse.Name.Split('-')[0];
                if (storehouseName == name)
                {
                    count += inventory.TransitCount;
                }
            }
            return count;
        }
        /// <summary>
        /// 得到在途库存
        /// </summary>
        /// <param name="inventories"></param>
        /// <returns></returns>
        public virtual int GetInventoryLockCount(InventoryEntity[] inventories)
        {
            if (inventories == null)
                return 0;
            var count = 0;
            var storehouseName = Storehouse != null ? Storehouse.Name : "";
            foreach (var inventory in inventories)
            {
                if (inventory.Storehouse == null || string.IsNullOrEmpty(inventory.Storehouse.Name))
                    continue;
                var name = inventory.Storehouse.Name.Split('-')[0];
                if (storehouseName == name)
                {
                    count += inventory.LockCount;
                }
            }
            return count;
        }
        #endregion

    }
}