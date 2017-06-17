using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Search;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Search.Similar
{
    public partial class List : Basic.Services.WebForm.Pages.MaintenPageBase<SimilarEntity>
    {

        protected override void SetQueryWhere(QueryInfo query)
        {
            if(!string.IsNullOrEmpty(Request.QueryString["WordId"]))
                query.Query<SimilarEntity>().Where(it => it.Word.Id == Request.QueryString["WordId"].Convert<long>());
            base.SetQueryWhere(query);

        }
    }
}