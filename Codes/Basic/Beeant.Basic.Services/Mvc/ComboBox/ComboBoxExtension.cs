using System.Web.Mvc;

namespace Beeant.Basic.Services.Mvc.ComboBox
{
    public static class ComboBoxExtension
    {
        public static ComboBox ComboBox(this HtmlHelper htmlHelper)
        {

            return new ComboBox(htmlHelper); 
        }

    }
}
