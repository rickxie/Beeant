using System.Web.Mvc;

namespace Beeant.Basic.Services.Mvc.Paging
{
    public static class PagerExtension
    {
        public static Pager Page(this HtmlHelper htmlHelper,
                                             int pageCount, int dataCount, int pageSize = 10,  int linkCount = 10,string pageName="page")
        {
            return new Pager(htmlHelper,pageName)
                {
                    DataCount = dataCount,
                    PageCount = pageCount,
                    LinkCount = linkCount,
                    PageSize = pageSize
                };
       
        }
        public static Pager Page(this HtmlHelper htmlHelper,PagerModel pager, int linkCount = 10, string pageName = "page")
        {
            return new Pager(htmlHelper)
            {
                PageName=pageName,
                DataCount = pager.DataCount,
                PageCount = pager.PageCount,
                LinkCount = linkCount,
                PageSize = pager.PageSize,
                PageIndex=pager.PageIndex,
                IsAjax= pager.IsAjax,
                AjaxFunction=pager.AjaxFunction
            };

        }
        public static Pager AjaxPage(this HtmlHelper htmlHelper, PagerModel pager,string ajaxFunction, int linkCount = 10, string pageName = "page")
        {
            return new Pager(htmlHelper,pageName, ajaxFunction)
            {
                DataCount = pager.DataCount,
                PageCount = pager.PageCount,
                LinkCount = linkCount,
                PageSize = pager.PageSize,
                PageIndex=pager.PageIndex,
                IsAjax=true
            };

        }
    }
}
