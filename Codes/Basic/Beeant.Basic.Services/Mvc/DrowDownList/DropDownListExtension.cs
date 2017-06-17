using System.Web.Mvc;

namespace Beeant.Basic.Services.Mvc.DrowDownList
{
    public static class DropDownListExtension
    {
        public static DropDownList DropDownList(this HtmlHelper htmlHelper)
        {
        
            return new DropDownList(htmlHelper); 
        }

    }
}
