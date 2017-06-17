using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Component.Extension;

namespace Beeant.Presentation.Admin.Scm.Supplier.Contract
{
    public partial class Detail : DetailPageBase<ContractEntity>
    {
        protected override ContractEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<ContractEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it, it.Supplier.Name });
            return Ioc.Resolve<IApplicationService, ContractEntity>().GetEntities<ContractEntity>(query).FirstOrDefault();
        }

        public long SupplierId
        {
            get { return Request.QueryString["SupplierId"].Convert<long>(); }
        }
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            var supplier = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntity<SupplierEntity>(SupplierId);
            if (supplier !=null)
            {
                InvalidateData("你没有权限处理供应商信息");
            }
            base.Page_Load(sender,e);
        }
    }
}