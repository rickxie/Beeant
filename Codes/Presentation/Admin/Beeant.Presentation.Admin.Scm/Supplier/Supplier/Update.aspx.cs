using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Scm.Supplier.Supplier
{
    public partial class Update : UpdatePageBase<SupplierEntity>
    {
        protected override void OnInit(System.EventArgs e)
        {
            if (!IsPostBack)
            {
                var supplier = GetEntity();
                Edit1.DistrictDropDown.LoadData();
                Edit1.DistrictDropDown.ResetByText(supplier.Province, supplier.City, supplier.County);
            }
            base.OnInit(e);
        }

        protected override SupplierEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Status != SupplierStatusType.Invalid)
            {
                InvalidateData("不是未审核供应商信息不能编辑");
            }
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            if (info != null && info.Service != null)
            {
                info.Service = Ioc.Resolve<IApplicationService, UserEntity>().GetEntity<UserEntity>(info.Service.Id);
            }
            return info;
        }

        protected override SupplierEntity FillEntity()
        {
            var info = base.FillEntity();
            Edit1.DistrictDropDown.GetDistrictSelectedValue();
            info.Province = Edit1.DistrictDropDown.ProvinceDropDownList.SelectedItem.Text;
            info.City = Edit1.DistrictDropDown.CityDropDownList.SelectedItem != null ? Edit1.DistrictDropDown.CityDropDownList.SelectedItem.Text : string.Empty;
            info.County = Edit1.DistrictDropDown.CountyDropDownList.SelectedItem != null ? Edit1.DistrictDropDown.CountyDropDownList.SelectedItem.Text : string.Empty; 
            info.Status= SupplierStatusType.Invalid;
            return info;
        }
    }
}