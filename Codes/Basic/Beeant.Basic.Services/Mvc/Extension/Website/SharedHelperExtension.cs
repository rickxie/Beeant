using System.Web;
using System.Web.Mvc;

namespace Beeant.Basic.Services.Mvc.Extension.Website
{
    public static class SharedHelperExtension
    {

        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static HtmlString GetTop(this HtmlHelper htmlHelper)
        {
            var html =
                string.Format(
                    "<script type='text/javascript'  src='{0}/Home/Top'></script><script type='text/javascript'>InitTop();</script>",
                    htmlHelper.GetUrl("PresentationWebsiteSharedUrl"));
            return new HtmlString(html);
        }
        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static HtmlString GetHeader(this HtmlHelper htmlHelper)
        {
            var html =
             string.Format(
                 "<script type='text/javascript'  src='{0}/Home/Header'></script><script type='text/javascript'>InitHeader();</script>",
                 htmlHelper.GetUrl("PresentationWebsiteSharedUrl"));
            return new HtmlString(html);
        }


        /// <summary>
        /// 得到底部
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static HtmlString GetFooter(this HtmlHelper htmlHelper)
        {
            var html =
                  string.Format(
                      "<script type='text/javascript'  src='{0}/Home/Header'></script><script type='text/javascript'>InitFooter();</script>",
                      htmlHelper.GetUrl("PresentationWebsiteSharedUrl"));
            return new HtmlString(html);
        }

        /// <summary>
        /// 得到底部
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static HtmlString GetBottom(this HtmlHelper htmlHelper)
        {
            var html =
                  string.Format(
                      "<script type='text/javascript'  src='{0}/Home/Bottom'></script><script type='text/javascript'>InitBottom();</script>",
                      htmlHelper.GetUrl("PresentationWebsiteSharedUrl"));
            return new HtmlString(html);
        }
 
    }
}
