using System.Text;
using System.Web.UI;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class ImageRecoverExtension
    {
        public static void ImageRecover(this Page page)
        {
            var builder = new StringBuilder();
            var addresses = Winner.Creator.Get<Winner.Storage.Address.IAddress>().GetAddresses();
            if (addresses == null || addresses.Count == 0)
                return ;
            page.RegisterScript("/Scripts/Winner/ImageRecover/Winner.ImageRecover.js");
            builder.Append("$(document).ready(function(){var imageRecover = new Winner.ImageRecover({Nodes:[");
            foreach (var address in addresses)
            {
                builder.Append("{");
                builder.AppendFormat("Url:'{0}',IsNormal:true,GroupName:'{1}'", address.Url, address.GroupName);
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.AppendFormat("],DefaultUrl:'{0}/Images/NoPic.jpg'", page.GetUrl("PresentationAdminHomeUrl"));
            builder.Append("});imageRecover.Initialize();});");
            page.ExecuteScript(builder.ToString(),true,false);
        }

        
 
    }
}
