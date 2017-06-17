using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Erp.Controls.Product
{
    public partial class RuleDropDownList : DropDownListTemplateBaseControl<RuleEntity>
    {
        /// <summary>
        /// 具体控件
        /// </summary>
        public override DropDownList DropDownList
        {
            get { return DropDownList1; }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadRuleRemark();
        }

        protected virtual void LoadRuleRemark()
        {
            if (DropDownList.SelectedIndex == 0)
            {
                spRemark.InnerHtml = "";
                return;
            }
            var rule = Ioc.Resolve<IApplicationService, RuleEntity>().GetEntity<RuleEntity>(DropDownList.SelectedValue.Convert<long>());
            if (rule != null)
            {
                spRemark.InnerHtml = rule.Remark;
            }
        }
    }
}