using System.Text;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.Extension.Mobile;

namespace Beeant.Basic.Services.Mvc.Script
{
    static public class ImageRecoverExtension
    {
        public static MvcHtmlString ImageRecover(this HtmlHelper htmlHelper)
        {
            var builder = new StringBuilder();
            var addresses = Winner.Creator.Get<Winner.Storage.Address.IAddress>().GetAddresses();
            if (addresses == null || addresses.Count == 0)
                return new MvcHtmlString(builder.ToString());
            builder.Insert(0, string.Format("<script src=\"/Scripts/Winner/ImageRecover/Winner.ImageRecover.js\" type=\"text/javascript\"></script>"));
            builder.Append("<script type=\"text/javascript\">$(document).ready(function(){window.ImageRecover = new Winner.ImageRecover({Nodes:[");
            foreach (var address in addresses)
            {
                builder.Append("{");
                builder.AppendFormat("Url:\"{0}\",IsNormal:true,GroupName:\"{1}\"", address.Url, address.GroupName);
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.AppendFormat("],DefaultUrl:\"{0}\"", htmlHelper.GetNoPicture());
            builder.Append("});window.ImageRecover.Initialize();});</script>");
            return new MvcHtmlString(builder.ToString());
        }

        
 
    }
}
