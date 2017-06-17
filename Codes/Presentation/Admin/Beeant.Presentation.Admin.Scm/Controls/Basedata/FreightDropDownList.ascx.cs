using System.Linq;
using System.Web.UI.WebControls;
using Beeant.Domain.Entities.Basedata;

using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Controls;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Controls.Basedata
{
    public partial class FreightDropDownList : DropDownListTemplateBaseControl<FreightEntity>
    {
        /// <summary>
        /// 具体控件
        /// </summary>
        public override DropDownList DropDownList
        {
            get { return DropDownList1; }
        }
        /// <summary>
        /// 根据账户加载
        /// </summary>
        /// <param name="accountId"></param>
        public virtual void LoadData(long accountId)
        {
            Query = new QueryInfo();
            Query.Query<FreightEntity>().Where(it => it.Account.Id == accountId);
            LoadData();
        }
    }
}