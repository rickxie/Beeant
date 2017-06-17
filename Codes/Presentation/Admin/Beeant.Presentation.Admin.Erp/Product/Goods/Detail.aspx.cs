using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Product.Goods
{
    public partial class Detail : DetailPageBase<GoodsEntity>
    {
        private long _requesetId;
        public override long RequestId
        {
            get
            {
                if (_requesetId == 0)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                        _requesetId = Request.QueryString["Id"].Convert<long>();
                    else if (!string.IsNullOrEmpty(Request.QueryString["productid"]))
                    {
                        var product =
                            Ioc.Resolve<IApplicationService, ProductEntity>()
                               .GetEntity<ProductEntity>(Request.QueryString["productid"].Convert<long>());
                        if (product != null && product.Goods != null)
                            _requesetId = product.Goods.Id;
                    }
                }
                return _requesetId;
            }
            set
            {
                base.RequestId = value;
            }
        }
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!IsPostBack)
                LoadGoodsImage();
        }
        protected override GoodsEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                var tagList = new List<string>();
                if (!string.IsNullOrWhiteSpace(info.Tag))
                {
                    tagList = info.Tag.Split(',').ToList();
                }
                var query = new QueryInfo();
                query.Query<TagEntity>().Where(it => tagList.Contains(it.Value));
                var infos = Ioc.Resolve<IApplicationService, TagEntity>().GetEntities<TagEntity>(query);
                lblTag.Text = infos.Count > 0 ? string.Join(",", infos.Select(it => it.Name).ToArray()) : "";

           

                if (info.Category != null)
                    info.Category =
                        Ioc.Resolve<IApplicationService, CategoryEntity>().GetEntity<CategoryEntity>(info.Category.Id);
                if (info.Freight != null)
                    info.Freight = Ioc.Resolve<IApplicationService, FreightEntity>().GetEntity<FreightEntity>(info.Freight.Id);
                if (info.Account != null)
                    info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }

        protected virtual void LoadGoodsImage()
        {
            pgGoodsImage.Query.SetParameter("Id", RequestId);
            var infos = Ioc.Resolve<IApplicationService, GoodsImageEntity>().GetEntities<GoodsImageEntity>(pgGoodsImage.Query);
            if (infos == null) return;
            repGoodsImage.DataSource = infos;
            repGoodsImage.DataBind();
            pgGoodsImage.DataBind();
        }

    }
}