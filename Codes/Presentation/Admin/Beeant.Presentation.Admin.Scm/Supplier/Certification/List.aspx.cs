using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Component.Extension;

namespace Beeant.Presentation.Admin.Scm.Supplier.Certification
{
    public partial class List : ListPageBase<CertificationEntity>
    {
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<CertificationEntity>().Where(it => it.Supplier.Id == Request.QueryString["SupplierId"].Convert<long>());
            base.SetQueryWhere(query);
        }

        public long SupplierId
        {
            get { return Request.QueryString["SupplierId"].Convert<long>(); }
        }

        protected override void Page_Load(object sender, System.EventArgs e)
        {
            var supplier = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntity<SupplierEntity>(SupplierId);
            if (supplier.Status != SupplierStatusType.Invalid)
                InvalidateData("你没有权限处理供应商信息");

            base.Page_Load(sender, e);
        }
    }
}