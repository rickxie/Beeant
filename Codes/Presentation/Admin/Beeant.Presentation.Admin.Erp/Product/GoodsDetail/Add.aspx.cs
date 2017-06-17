using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.GoodsDetail
{
    public partial class Add : AddPageBase<GoodsDetailEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        public long ProductId
        {
            get { return Request.QueryString["ProductId"].Convert<long>(); }
        }
        public long GoodsId
        {
            get { return Request.QueryString["GoodsId"].Convert<long>(); }
        }
        protected override void LoadData()
        {
            var product = Ioc.Resolve<IApplicationService, ProductEntity>().GetEntity<ProductEntity>(ProductId);
            if (product == null || product.IsSales)
            {
                InvalidateData("上架的商品不能编辑");
            }
          

            base.LoadData();
        }
        /// <summary>
        /// 重新填充
        /// </summary>
        /// <returns></returns>
        protected override GoodsDetailEntity FillEntity()
        {
            var info =base.FillEntity();
            var dataEntity = GoodsDetail();
            info.Product = new ProductEntity {Id = ProductId};
            info.Goods = new GoodsEntity {Id = GoodsId};
            info.Detail = info.Detail == "<br>" ? "" : info.Detail;
            if (dataEntity != null)
            {
                info.SaveType = SaveType.Modify;
                info.SetProperty(it => it.Detail);
                info.Id = dataEntity.Id;
            }
            else
            {
                info.SaveType = SaveType.Add;
            }
            return info;
        }
        /// <summary>
        /// 得到商品详情
        /// </summary>
        /// <returns></returns>
        protected virtual GoodsDetailEntity GoodsDetail()
        {
            var query = new QueryInfo();
            query.Query<GoodsDetailEntity>()
                 .Where(it => it.Goods.Id == GoodsId && it.Product.Id == ProductId);
            var infos = Ioc.Resolve<IApplicationService, GoodsDetailEntity>().GetEntities<GoodsDetailEntity>(query);
            return infos == null ? null : infos.FirstOrDefault();
        }
    }
}