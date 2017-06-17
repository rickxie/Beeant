using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;

using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Ajax.Product
{
    public partial class Category : AjaxPageBase<CategoryEntity>
    {
      
        protected override string GetListItem(CategoryEntity info)
        {
            return string.Format("Id: '{0}', Name: '{1}', Pinyin: '{2}', Initial: '{3}', IsPublish: {4}, HasChild: {5},ImageCount:{6} "
                , info.Id, info.Name, info.Pinyin, info.Initial,
                info.IsPublish.ToString().ToLower()
                ,(info.Children!=null && info.Children.Count>0).ToString().ToLower(),info.ImageCount);

        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.Query<CategoryEntity>().Select(it => new object[] {it.Id, it.Name,it.Pinyin,it.Initial,it.IsPublish,it.ImageCount, it.Children.Select(s=>s.Id)});
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
           
            query.Query<CategoryEntity>().Where(it => it.Parent.Id == (Request.QueryString["ParentId"].Convert<long>()));
        }
        protected override System.Collections.Generic.IList<CategoryEntity> GetEntities()
        {
            var query = new QueryInfo { FromExp = typeof(CategoryEntity).FullName};
            SetQuery(query);
            return Ioc.Resolve<IApplicationService, CategoryEntity>().GetEntities<CategoryEntity>(query);
        }
    }
}