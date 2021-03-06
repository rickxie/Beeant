﻿using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Component.Extension;

namespace Beeant.Presentation.Admin.Scm.Supplier.Contract
{
    public partial class Update : UpdatePageBase<ContractEntity>
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
        protected override ContractEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Supplier != null)
                info.Supplier = Ioc.Resolve<IApplicationService, SupplierEntity>().GetEntity<SupplierEntity>(info.Supplier.Id);
            if (info == null || info.Supplier == null || info.Supplier.Status != SupplierStatusType.Invalid)
                InvalidateData("你没有权限处理供应商信息");
            return info;
        }

    }
}