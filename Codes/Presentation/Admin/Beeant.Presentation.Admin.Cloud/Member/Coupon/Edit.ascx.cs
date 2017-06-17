using System;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Member;
using Beeant.Domain.Entities.Promotion;

namespace Beeant.Presentation.Admin.Cloud.Member.Coupon
{
    public partial class Edit : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                InitByCouponer();
            }
        }

        protected virtual void InitByCouponer()
        {
            if (string.IsNullOrEmpty(Request.QueryString["couponerid"]))
                return;
            var couponer = Ioc.Resolve<IApplicationService, CouponerEntity>().GetEntity<CouponerEntity>(Request.QueryString["couponerid"].Convert<long>());
            if (couponer == null) return;
            txtName.Value = couponer.Name;
            txtAmount.Value = couponer.Amount.ToString();
            txtEndDate.Text = couponer.EndDate.ToString("yyyy-MM-dd");
            txtCouponerId.Value = couponer.Id.ToString();
            txtRemark.Value = couponer.Remark;
        }
    }
}