using System;
using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Product.Inquery
{
    public partial class Edit : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var query = new QueryInfo();
            query.Query<InqueryEntity>().Where(it => it.Id == Request.QueryString["Id"].Convert<long>()).Select(it=>new {it,it.Goods.Name});
            var infos = Ioc.Resolve<IApplicationService, InqueryEntity>().GetEntities<InqueryEntity>(query);
            if (infos != null&&infos.Count>0)
            {
                var info = Ioc.Resolve<IApplicationService, GoodsEntity>().GetEntity<GoodsEntity>(infos.First().Goods.Id.Convert<long>());
                if (info != null)
                    lblGoodsName.Text = info.Name;
            }

        }
        
    }
}