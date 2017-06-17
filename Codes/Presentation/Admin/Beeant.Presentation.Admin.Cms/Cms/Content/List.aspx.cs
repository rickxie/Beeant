using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Cms.Cms.Content
{
    public partial class List : ListPageBase<ContentEntity>
    {

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ClassId"]))
                query.Query<ContentEntity>().Where(it => it.Class.Id == Request.QueryString["ClassId"].Convert<long>());
            base.SetQueryWhere(query);
        }
     
 
     
    }
}