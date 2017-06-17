using System;
using Beeant.Domain.Entities.Api;
using Winner.Persistence;


namespace Beeant.Presentation.Admin.Configurator.Api.Voucher
{
    public partial class List : Basic.Services.WebForm.Pages.MaintenPageBase<VoucherEntity>
    {
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
            }
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
                ddlSearchType.LoadData();
            }
            base.Page_Load(sender, e);
        }

        protected override VoucherEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info.SaveType == SaveType.Add)
            {
                info.Token =string.IsNullOrWhiteSpace(txtToken.Value)? Guid.NewGuid().ToString().Replace("-", ""): txtToken.Value;
                info.SetProperty(it => it.Token);
            }
            info.SetProperty(it => it.TypeName);
            return info;
        }

    }
}