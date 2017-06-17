using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Beeant.Basic.Services.Mvc.Uploader
{
    public class Uploader
    {
        /// <summary>
        /// 视图上下文
        /// </summary>
        public HtmlHelper HtmlHelper { get; set; }
 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="htmlHelper"></param>
        public Uploader(HtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
           
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="partialViewName"></param>
        /// <param name="model"></param>
        public virtual MvcHtmlString Partial(string partialViewName, UploaderModel model)
        {
           
            return HtmlHelper.Partial(partialViewName, model);
        }

       
    }
}
