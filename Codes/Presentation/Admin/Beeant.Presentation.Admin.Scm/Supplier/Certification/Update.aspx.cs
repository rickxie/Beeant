using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Dependent;
using Beeant.Application.Services;
using Component.Extension;

namespace Beeant.Presentation.Admin.Scm.Supplier.Certification
{
    public partial class Update : UpdatePageBase<CertificationEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set { base.IsUpdatePanel = value; }
        }

        public override bool IsFillAllEntity
        {
            get { return false; }
            set
            {
                base.IsFillAllEntity = value;
            }
        }

        public long SupplierId
        {
            get { return Request.QueryString["SupplierId"].Convert<long>(); }
        }

    
        protected override CertificationEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Supplier != null)
                info.Supplier = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntity<SupplierEntity>(info.Supplier.Id);
            if (info == null || info.Supplier == null ||  info.Supplier.Status != SupplierStatusType.Invalid)
                InvalidateData("你没有权限处理供应商信息");
            return info;
        }
    }
}