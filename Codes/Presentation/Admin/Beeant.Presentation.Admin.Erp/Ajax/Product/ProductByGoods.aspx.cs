using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Product;

using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Ajax.Product
{
    public partial class ProductByGoods : AjaxPageBase<ProductEntity>
    {
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["GoodsId"]))
            {
                base.Page_Load(sender, e);
            }
        }

        protected override string GetListItem(ProductEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Id);
        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id,Name";
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (string.IsNullOrEmpty(Request.QueryString["GoodsId"]))
                return;
            query.Query<ProductEntity>().Where(it => it.Goods.Id == Request.QueryString["GoodsId"].Convert<long>());
        }
    }
}