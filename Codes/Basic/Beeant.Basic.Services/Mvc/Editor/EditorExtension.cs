using System.Web.Mvc;

namespace Beeant.Basic.Services.Mvc.Editor
{
    public static class EditorExtension
    {
        public static Editor Editor(this HtmlHelper htmlHelper)
        {

            return new Editor(htmlHelper); 
        }

    }
}
