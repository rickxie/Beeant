using System.Web.Mvc;


namespace Beeant.Cloud.Site
{
    public class MobileSiteBaseController : SiteBaseController
    {
        protected override bool CheckAuthorize(ActionExecutingContext filterContext)
        {
            var rev= base.CheckAuthorize(filterContext);
            if (rev)
            {
                return Site.IsOpenMobile;
            }
            return false;
        }
        /// <summary>
        /// 得到路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual string GetViewPath(string path)
        {
            return base.GetViewPath(path, Site.MobileStyle);

        }
    }
}
