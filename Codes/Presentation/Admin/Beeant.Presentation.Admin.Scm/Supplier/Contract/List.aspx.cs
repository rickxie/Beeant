using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Dependent;
using Beeant.Application.Services;

namespace Beeant.Presentation.Admin.Scm.Supplier.Contract
{
    public partial class List : ListPageBase<ContractEntity>
    {
        /// <summary>
        /// 设置权限，防止UI地址强行登录
        /// </summary>
        public long SupplierId
        {
            get { return Request.QueryString["SupplierId"].Convert<long>(); }
        }
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            var supplier = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntity<SupplierEntity>(SupplierId);
            if (supplier.Status != SupplierStatusType.Invalid)
            {
                InvalidateData("你没有权限处理供应商信息");
            }
            base.Page_Load(sender, e);
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<ContractEntity>().Where(it => it.Supplier.Id == Request.QueryString["SupplierId"].Convert<long>());
            base.SetQueryWhere(query);
        }

        protected override void OnInit(System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlPaymentType.LoadData();
                ddlDispatchType.LoadData();
                ddlBillType.LoadData();
            }

            base.OnInit(e);
        }
    }
}