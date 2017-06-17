using System.Linq;
using System.Text;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Finance.Finance.Payline
{
    public partial class List : ListPageBase<PaylineEntity>
    {

        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlPaylineType.LoadData();
            }
            base.Page_Load(sender, e);
        }

        public virtual string GetOrderList(string orderIds)
        {
            var href = new StringBuilder();
            if (orderIds.Length > 0)
            {
                orderIds.Split(',').ToList().ForEach( order => href.Append(string.Format("<a href='/Order/Order/Detail.aspx?id={0}' target='_blank'>{0}</a>&nbsp;",
                                                                                         order)));
            }
            return href.ToString();
        }

        
      
    }
}