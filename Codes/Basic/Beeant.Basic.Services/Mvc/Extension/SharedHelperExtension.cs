using System.Web;
using System.Web.Mvc;

namespace Beeant.Basic.Services.Mvc.Extension
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

        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <returns></returns>
        public static string GetNoPicture()
        {
            return string.Format("{0}/Images/None.png", Configuration.ConfigurationManager.GetSetting<string>("PresentationWebsiteSharedUrl"));
        }

        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetNoPicture(this HtmlHelper htmlHelper, string fileName = null)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                return fileName;
            }
            return string.Format("{0}/Images/None.png", htmlHelper.GetUrl("PresentationWebsiteSharedUrl"));
        }
        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string GetNoPicture(this Controller controller)
        {
            return string.Format("{0}/Images/None.png", controller.GetUrl("PresentationWebsiteSharedUrl"));
        }
        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static string GetPreRenderPicture(this HtmlHelper htmlHelper)
        {
            return string.Format("{0}/Images/PreRender.gif", htmlHelper.GetUrl("PresentationWebsiteSharedUrl"));
        }
        /// <summary>
        /// 得到顶部
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string GetPreRenderPicture(this Controller controller)
        {
            return string.Format("{0}/Images/PreRender.gif", controller.GetUrl("PresentationWebsiteSharedUrl"));
        }
    }
}
