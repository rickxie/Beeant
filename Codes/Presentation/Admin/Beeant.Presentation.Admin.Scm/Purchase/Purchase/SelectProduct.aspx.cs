using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Admin.Scm.Purchase.Purchase
{
    public partial class SelectProduct : SearchPageBase<ProductEntity>
    {
        public override Basic.Services.WebForm.Controls.PagerControlBase Pager
        {
            get
            {
                return Pager1;
            }
            set
            {
                base.Pager = value;
            }
        }
        public override System.Web.UI.HtmlControls.HtmlGenericControl SearchPanel
        {
            get
            {
                return divSearch;
            }
            set
            {
                base.SearchPanel = value;
            }
        }
   
        public override GridView GridView
        {
            get { return GridView1; }
            set
            {
                base.GridView = value;
            }
        }
        public long StorehouseId
        {
            get { return Request.QueryString["StorehouseId"].Convert<long>(); }
        }
        public string StorehouseName
        {
            get { return Request.QueryString["StorehouseName"]; }
        }
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            Page.AddExceptionLog();
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
        /// <summary>
        /// 设置查询
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQuerySelect(Winner.Persistence.QueryInfo query)
        {
            base.SetQuerySelect(query);
            if (StorehouseId != 0)
            {
                query.SelectExp =
                    string.Format(
                        "{0},Inventories.Count,Inventories.LockCount,Inventories.TransitCount,Inventories.WarningCount,Inventories.Storehouse.Name,Inventories.Storehouse.WarehouseName,Inventories.Storehouse.IsWarehouse",
                        query.SelectExp);
            }
        }





    }
}