using System.Linq;
using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Home.Desktop.Notice
{
    public partial class List : ListPageBase<ContentEntity>
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        protected override void SetQueryWhere(Winner.Persistence.QueryInfo query)
        {
            query.Query<ContentEntity>()
                 .Where(it => it.Class.Tag == "ERP_Message_Notice");
            base.SetQueryWhere(query);
        }
    }
}