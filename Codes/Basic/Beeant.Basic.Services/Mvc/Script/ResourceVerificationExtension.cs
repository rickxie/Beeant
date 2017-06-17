using System.Text;
using System.Web.Mvc;
using Beeant.Domain.Entities.Authority;

namespace Beeant.Basic.Services.Mvc.Script
{
    static public class ResourceVerificationExtension
    {
        public static MvcHtmlString RemoveNoAuthorizeResource(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewBag.Verification == null)
                return null;
            VerificationEntity verification = htmlHelper.ViewBag.Verification;
            if (verification.Controls == null)
                return null;
            var builder = new StringBuilder();    
            builder.Append("<script type=\"text/javascript\">$(document).ready(function(){");
            foreach (var control in verification.Controls)
            {
                if (control.Value)
                    continue;
                builder.AppendFormat("$(document).find(\"*[name={0}]\").remove();", control.Key);
            }
            builder.Append("</script>");
            return new MvcHtmlString(builder.ToString());
        }

        
 
    }
}
