using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Erp.Product.PropertyRule
{
    public partial class List : ListPageBase<PropertyRuleEntity>
    {

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (string.IsNullOrEmpty(Request["Propertyid"])) return;
            query.Query<PropertyRuleEntity>().Where(
                it => it.Property.Id == Request["Propertyid"].Convert<long>());

        }

        public override void Remove_Click(object sender, System.EventArgs e)
        {
            SaveEntities(SaveType.Remove, "回收成功", "回收失败");
        }
        /// <summary>
        /// 得到属性名称
        /// </summary>
        /// <returns></returns>
        public string GetPropertyName()
        {
            if (!string.IsNullOrEmpty(Request["Propertyid"]))
            {
                var info = Ioc.Resolve<IApplicationService, PropertyEntity>().GetEntity<PropertyEntity>(Request["Propertyid"].Convert<long>());
                if (info != null) return string.Format("{0}-", info.Name);
            }
            return "";
        }
    }
}