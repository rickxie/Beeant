using System.Collections.Generic;
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
    public partial class List : ListPageBase<QualificationEntity>
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
         //<summary>
         //重写
         //</summary>
         //<param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<QualificationEntity>().Where(it => it.Supplier.Id == SupplierId);
            base.SetQueryWhere(query);
        }
    }
}