using System;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Admin.Wms.Wms.StockItem
{
    public partial class Edit : System.Web.UI.UserControl
    {

        public bool IsLoadStorehouse { get; set; }
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsLoadStorehouse)
                    tvStorehouseTree.LoadData();
                InitByProduct();
            }
            base.OnInit(e);
        }
   

        protected void tvStorehouseTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            lblStorehouseName.Text = tvStorehouseTree.TreeView.SelectedNode.Text;
            hfStorehouseId.Value = tvStorehouseTree.TreeView.SelectedNode.Value;
            if (!string.IsNullOrEmpty(tvStorehouseTree.TreeView.SelectedNode.Value))
                tvStorehouseTree.LoadData();
        }
        
        /// <summary>
        /// 根据商品初始化
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitByProduct()
        {
            if (string.IsNullOrEmpty(Request.QueryString["productid"]))
                return false;
            var product = Ioc.Resolve<IApplicationService, ProductEntity>().GetEntity<ProductEntity>(Request.QueryString["productid"].Convert<long>());
            if (product == null) return false;
            txtName.Value = product.Name;
            txtCount.Value = "1";
            txtProductId.Value = product.Id.ToString();
            return true;
        }
     

    }
}