using System;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using PropertyEntity = Beeant.Domain.Entities.Product.PropertyEntity;

namespace Beeant.Presentation.Admin.Erp.Product.PropertyRule
{
    public partial class Edit : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                ddlRule.LoadData();
                LoadPropertyName();
                ckPropertyRuleType.LoadData();
            }
        }

        /// <summary>
        /// 得到属性名称
        /// </summary>
        /// <returns></returns>
        public void LoadPropertyName()
        {
            if (!string.IsNullOrEmpty(Request["Propertyid"]))
            {
                var info = Ioc.Resolve<IApplicationService, PropertyEntity>().GetEntity<PropertyEntity>(Request["Propertyid"].Convert<long>());
                if (info != null)
                    lblPropertyName.Text = info.Name;
            }
        }
     
       
    }
}