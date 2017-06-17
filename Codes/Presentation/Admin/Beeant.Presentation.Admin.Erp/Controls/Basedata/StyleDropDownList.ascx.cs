using System.Linq;
using System.Web.UI.WebControls;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Controls;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Controls.Basedata
{
    public partial class StyleDropDownList : DropDownListTemplateBaseControl<StyleEntity>
    {
        /// <summary>
        /// 具体控件
        /// </summary>
        public override DropDownList DropDownList
        {
            get { return DropDownList1; }
        }

        /// <summary>
        /// 根据类型加载
        /// </summary>
        /// <param name="type"></param>
        public virtual void LoadData(StyleType type)
        {
            Query = new QueryInfo();
            Query.Query<StyleEntity>().Where(it => it.Type == type);
            LoadData();
        }
    }
}