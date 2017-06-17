using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Admin.Scm.Purchase.PurchaseItem
{
    public partial class Edit : System.Web.UI.UserControl
    {
        

        protected override void OnInit(System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
                InitByProduct();
            }
            
            base.OnInit(e);
        }
        /// <summary>
        /// 根据商品初始化
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitByProduct()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]) && string.IsNullOrEmpty(Request.QueryString["productid"]))
                return false;
            var product = Ioc.Resolve<IApplicationService, ProductEntity>().GetEntity<ProductEntity>(Request.QueryString["productid"].Convert<long>());
            if (product == null) return false;
            txtName.Value = product.Name;
            txtCount.Value = "1";
            txtProductId.Value = product.Id.ToString();
            txtPrice.Value = product.Cost.ToString();
            return true;
        }
    
    }
}