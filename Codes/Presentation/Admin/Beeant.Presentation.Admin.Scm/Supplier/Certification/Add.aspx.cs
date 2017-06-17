using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
namespace Beeant.Presentation.Admin.Scm.Supplier.Certification
{
    public partial class Add : AddPageBase<CertificationEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }

        public long SupplierId
        {
            get { return Request.QueryString["SupplierId"].Convert<long>(); }
        }

        protected override CertificationEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
                info.Supplier = new SupplierEntity {Id = SupplierId};
            return info;
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