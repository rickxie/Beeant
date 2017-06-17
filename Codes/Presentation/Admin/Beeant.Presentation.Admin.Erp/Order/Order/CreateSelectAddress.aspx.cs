using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Member;
using Component.Extension;

namespace Beeant.Presentation.Admin.Erp.Order.Order
{
    public partial class CreateSelectAddress : AddPageBase<AddressEntity>
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        public override System.Web.UI.WebControls.Button SaveButton
        {
            get { return btnSave; }
            set
            {
                btnSave = value;
            }
        }
        public override MessageControlBase MessageControl
        {
            get { return Message1 as MessageControlBase; }
            set
            {
                base.MessageControl = value;
            }
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (!IsPostBack)
                DistrictDropDownList1.LoadData();
            base.OnInit(e);
        }
        protected override AddressEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.Account = new AccountEntity { Id = Request.QueryString["AccountId"].Convert<long>() };
                DistrictDropDownList1.GetDistrictSelectedValue();
                info.Country = "中国";
                info.Province = DistrictDropDownList1.ProvinceDropDownList.SelectedValue;
                info.City = DistrictDropDownList1.CityDropDownList.SelectedValue;
                info.County = DistrictDropDownList1.CountyDropDownList.SelectedValue;
            }
            return info;
        }
        protected override void SetResult(bool rev, System.Collections.Generic.IList<Winner.Filter.ErrorInfo> errors)
        {
            base.SetResult(rev, errors);
            if (rev)
            {
                this.ExecuteScript(string.Format("parent.window.note.CloseButton.click();parent.document.getElementById('{0}').click();", Request["btn"]));
            }
        }
    }
}
