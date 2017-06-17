using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Beeant.Domain.Entities.Cms;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Presentation.Mvc.Shared.Models.Product;

using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Shared.Controllers.Product
{
    public class CategoryController : BaseController
    {
       
        #region 类目视图

        /// <summary>
        /// 类目视图
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        public virtual ActionResult Children(long categoryId,string viewPath)
        {
            var query = new QueryInfo();
            query.SetCacheName("类目缓存").SetCacheTime(DateTime.Now.AddDays(1)).Query<CategoryEntity>().Where(it => it.Parent.Id == categoryId && it.IsShow).OrderByDescending(it => it.Sequence)
                 .Select(it => new object[] { it.Name, it.Id});
            var categories = this.GetEntities<CategoryEntity>(query);
            return View(viewPath, categories);
        }

        /// <summary>
        /// 类目视图
        /// </summary>
        /// <param name="viewPath"></param>
        /// <returns></returns>
        public virtual ActionResult Partial(string viewPath)
        {
            var query = new QueryInfo();
            query.SetCacheName("类目缓存").SetCacheTime(DateTime.Now.AddDays(1)).Query<CategoryEntity>().Where(it => it.Parent.Id == 0 &&  it.IsShow).OrderByDescending(it => it.Sequence)
                 .Select(it => new object[] { it.Name, it.Id, it.Children.Select(s => new object[] { s.Id, s.Name, s.IsShow, s.Children.Select(n => new object[] { n.Id, n.Name, n.IsShow }) }) });
            var categories = this.GetEntities<CategoryEntity>(query);
            var model = GetCategoryModels(categories);
            return View(viewPath, model);
        }
   
        /// <summary>
        /// 设置类目
        /// </summary>
        /// <param name="categories"></param>
        protected virtual IList<CategoryModel> GetCategoryModels(IList<CategoryEntity> categories)
        {
            if (categories == null) return null;
            var result = new List<CategoryModel>();
            var contents = GetCategoryContents();
            foreach (var category in categories)
            {
                result.Add(
                    new CategoryModel
                    {
                        Category = category,
                        Contents = contents == null
                                       ? null
                                       : contents.Where(
                                           it => it.Tag == category.Id.ToString()).ToList()
                    });
                if (category.Children == null) continue;
                category.Children = category.Children.Where(it => it.IsShow).ToList();
                foreach (var subC in category.Children)
                {
                    if (subC.Children == null) continue;
                    subC.Children = subC.Children.Where(it => it.IsShow).ToList();
                }

            }
            return result;
        }

        /// <summary>
        /// 得到广告列表
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ContentEntity> GetCategoryContents()
        {
            var query = new QueryInfo();
            query.SetCacheName("类目广告缓存").SetCacheTime(DateTime.Now.AddDays(1))
                .Query<ContentEntity>().Where(it => it.IsShow && it.Class.Tag == "B2C_Advertisement_Category").OrderByDescending(it => it.Sequence);
            return this.GetEntities<ContentEntity>(query);
        }
        #endregion
    }
}
