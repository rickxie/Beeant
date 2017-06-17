using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Supplier.Qualification
{
    public partial class Add : AddPageBase<QualificationEntity>
    {
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
     
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        protected override QualificationEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<QualificationEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it, it.Supplier.Name });
            return Ioc.Resolve<IApplicationService, QualificationEntity>().GetEntities<QualificationEntity>(query).FirstOrDefault();
        }
    }
}