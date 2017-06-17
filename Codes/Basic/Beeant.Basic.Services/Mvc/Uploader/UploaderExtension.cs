using System.Web.Mvc;

namespace Beeant.Basic.Services.Mvc.Uploader
{
    public static class UploaderExtension
    {
        public static Uploader Uploader(this HtmlHelper htmlHelper)
        {

            return new Uploader(htmlHelper); 
        }

    }
}
