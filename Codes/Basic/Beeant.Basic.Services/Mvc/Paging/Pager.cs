using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Component.Extension;

namespace Beeant.Basic.Services.Mvc.Paging
{
    public class Pager
    {
        /// <summary>
        /// 视图上下文
        /// </summary>
        public HtmlHelper HtmlHelper { get; set; }

        private int _linkCount = 10;
        /// <summary>
        /// 显示按钮数量
        /// </summary>
        public int LinkCount
        {
            get { return _linkCount; }
            set { _linkCount = value; }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大写
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public int DataCount { get; set; }
        /// <summary>
        /// 分页参数名称
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 是否Ajax
        /// </summary>
        public bool IsAjax { get; set; }
        /// <summary>
        /// 请求路径
        /// </summary>
        public string AjaxFunction { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="htmlHelper"></param>
        public Pager(HtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
           
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pageName"></param>
        public Pager(HtmlHelper htmlHelper,string pageName)
        {
            HtmlHelper = htmlHelper;
            PageName = pageName;
            var value = HtmlHelper.ViewContext.RequestContext.RouteData.Values[PageName] ??
                        HtmlHelper.ViewContext.RequestContext.HttpContext.Request.QueryString[PageName];
            PageIndex = value.Convert<int>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pageName"></param>
        /// <param name="ajaxFunction"></param>
        public Pager(HtmlHelper htmlHelper,string pageName,string ajaxFunction)
        {
            HtmlHelper = htmlHelper;
            PageName = pageName;
            AjaxFunction = ajaxFunction;
        }
        private RouteValueDictionary _routeValues=new RouteValueDictionary();
        /// <summary>
        /// 路由
        /// </summary>
        public RouteValueDictionary RouteValues
        {
            get { return _routeValues; }
            set { _routeValues = value; }
        }
        /// <summary>
        /// 呈现分页
        /// </summary>
        /// <param name="partialViewName"></param>
        public virtual MvcHtmlString Partial(string partialViewName)
        {
            return HtmlHelper.Partial(partialViewName, GetModel());
        }
        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Pager AddRouteValue(string name, object value)
        {
            RouteValues[name] = value;
            return this;
        }

        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public Pager AddRouteValues(object routeValues)
        {
            AddRouteValues(new RouteValueDictionary(routeValues));
            return this;
        }

        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public Pager AddRouteValues(RouteValueDictionary routeValues)
        {
            if (routeValues == null) return this;
            RouteValues = routeValues;
            return this;
        }
        /// <summary>
        /// 得到模型
        /// </summary>
        /// <returns></returns>
        protected virtual PagerModel GetModel()
        {
            var model = new PagerModel
                {
                    FirstUrl = GetUrl(0),
                    PreviousUrl = GetUrl(PageIndex - 1),
                    LastUrl = GetUrl(PageCount - 1),
                    NextUrl = GetUrl(PageIndex + 1),
                    Links = new Dictionary<int, string>(),
                    PageIndex=PageIndex,
                    PageCount=PageCount,
                    DataCount=DataCount,
                    IsAjax=IsAjax,
                    VirtualPath = GetVirtualPath(),
                    AjaxFunction=AjaxFunction
                };
            for (int i = 0, index = GetStartIndex(); i < LinkCount; i++, index++)
            {
                var url = GetUrl(index);
                if(string.IsNullOrEmpty(url) && index!=PageIndex)break;
                model.Links.Add(index + 1, url);  
            }
            return model;
        }
        /// <summary>
        /// 得到开始索引
        /// </summary>
        /// <returns></returns>
        protected virtual int GetStartIndex()
        {
            if (PageCount <= LinkCount)
                return 0;
            if (PageIndex <= 0) return 0;
            int index;
            if (PageIndex < LinkCount / 2)
                index = 0;
            else if (PageCount > PageIndex + LinkCount / 2 + 1)
                index = PageIndex - LinkCount / 2;
            else
                index = PageCount - LinkCount;
            return index;
        }

        /// <summary>
        /// 得到地址
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual string GetUrl(int index)
        {
            if (index == PageIndex || index<0 || index>=PageCount) return null;
            var virtualPathForArea = GetVirtualPathForArea(index);
            if (virtualPathForArea == null) return null;
            return virtualPathForArea.VirtualPath;
        }

        /// <summary>
        /// 得到字典
        /// </summary>
        /// <returns></returns>
        protected virtual VirtualPathData GetVirtualPathForArea(int index)
        {
            var pageLinkValueDictionary = new RouteValueDictionary(RouteValues);
            foreach (var routeDataValue in HtmlHelper.ViewContext.RequestContext.RouteData.Values)
            {
                if (!pageLinkValueDictionary.ContainsKey(routeDataValue.Key))
                {
                    pageLinkValueDictionary.Add(routeDataValue.Key, routeDataValue.Value);
                }
            }
            foreach (var qs in HtmlHelper.ViewContext.RequestContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (!pageLinkValueDictionary.ContainsKey(qs))
                {
                    pageLinkValueDictionary.Add(qs,
                                                HtmlHelper.ViewContext.RequestContext.HttpContext.Request.QueryString[qs
                                                    ]);
                }
            }
            if (pageLinkValueDictionary.ContainsKey(PageName))
                pageLinkValueDictionary.Remove(PageName);
            if (!pageLinkValueDictionary.ContainsKey(PageName))
                pageLinkValueDictionary.Add(PageName, index);
            var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(HtmlHelper.ViewContext.RequestContext,
                                                                             pageLinkValueDictionary);
            return virtualPathForArea;
        }

        /// <summary>
        /// 得到字典
        /// </summary>
        /// <returns></returns>
        protected virtual  string GetVirtualPath()
        {
            var pageLinkValueDictionary = new RouteValueDictionary(RouteValues);
            foreach (var routeDataValue in HtmlHelper.ViewContext.RequestContext.RouteData.Values)
            {
                if (!pageLinkValueDictionary.ContainsKey(routeDataValue.Key))
                {
                    pageLinkValueDictionary.Add(routeDataValue.Key, routeDataValue.Value);
                }
            }
            foreach (var qs in HtmlHelper.ViewContext.RequestContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (!pageLinkValueDictionary.ContainsKey(qs))
                {
                    pageLinkValueDictionary.Add(qs,
                                                HtmlHelper.ViewContext.RequestContext.HttpContext.Request.QueryString[qs
                                                    ]);
                }
            }
            if (pageLinkValueDictionary.ContainsKey(PageName))
                pageLinkValueDictionary.Remove(PageName);
            var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(HtmlHelper.ViewContext.RequestContext,
                                                                             pageLinkValueDictionary);

            return virtualPathForArea.VirtualPath;
        }
    }
}
