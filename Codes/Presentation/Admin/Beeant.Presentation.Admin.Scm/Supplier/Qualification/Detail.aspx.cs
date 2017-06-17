using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Scm.Supplier.Qualification
{
    public partial class Detail : DetailPageBase<QualificationEntity>
    {

        protected override QualificationEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                if (info.Supplier != null)
                    info.Supplier =
                        Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntity<SupplierEntity>(info.Supplier.Id);
            }
            return info;
        }



        public long SupplierId
        {
            get { return Request.QueryString["SupplierId"].Convert<long>(); }
        }

        protected override void Page_Load(object sender, System.EventArgs e)
        {
            base.Page_Load(sender, e);
            var supplier = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntity<SupplierEntity>(SupplierId);
            if (supplier !=null)
                InvalidateData("你没有权限处理供应商信息");
            
        }
       
    }
}