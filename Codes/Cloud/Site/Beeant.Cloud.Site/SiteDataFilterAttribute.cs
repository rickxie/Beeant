using Beeant.Basic.Services.Mvc.FilterAttribute;
using Component.Extension;

namespace Beeant.Cloud.Site
{
    public class SiteDataFilterAttribute : DataFilterAttribute
    {

      
        /// <summary>
        /// 值名称
        /// </summary>
        public override string IdentityName
        {
            get { return "Site.Id"; }
            set { base.IdentityName = value; }
        }

        public override long IdentityId
        {
            get
            {
                return Identity.Numbers != null && Identity.Numbers.ContainsKey("SiteId") ? Identity.Numbers["SiteId"].Convert<long>() : 0;
            }
            set { base.IdentityId = value; }
        }

         
     
    }
}
