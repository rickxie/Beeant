using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Scm.Supplier.Supplier
{
    public partial class Add : AddPageBase<SupplierEntity>
    {
       
        protected override SupplierEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }

        protected override void OnInit(System.EventArgs e)
        {
            if(!IsPostBack)
                Edit1.DistrictDropDown.LoadData();
            base.OnInit(e);
        }

        protected override SupplierEntity FillEntity()
        {
            var info = base.FillEntity();
            Edit1.DistrictDropDown.GetDistrictSelectedValue();
            info.Province = Edit1.DistrictDropDown.ProvinceDropDownList.SelectedItem.Text;
            if (Edit1.DistrictDropDown.CityDropDownList.SelectedItem != null)
                info.City = Edit1.DistrictDropDown.CityDropDownList.SelectedItem.Text;
            else
                info.City = string.Empty;
            if (Edit1.DistrictDropDown.CountyDropDownList.SelectedItem != null)
                info.County = Edit1.DistrictDropDown.CountyDropDownList.SelectedItem.Text;
            else
                info.County = string.Empty;
            info.Status = SupplierStatusType.Invalid;
            return info;
        }
    }
}