using System.Text;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.Mvc.Bases;


namespace Beeant.Presentation.Website.Shared.Controllers
{

    public class SharedController : SharedBaseController
    {

        /// <summary>
        /// 得到登录凭据
        /// </summary>
        public virtual ActionResult GetToken(bool? isIgnoreDomain, bool? isSet)
        {
            Response.Clear();
            var token = Ioc.Resolve<IIdentityApplicationService>().GetToken();
            var bulider = new StringBuilder();
            if (token != null)
            {
                if (isSet.HasValue && isSet.Value)
                {
                    bulider.Append("var exp = new Date();");
                    bulider.AppendFormat("exp.setTime(exp.getTime() + {0}*60*1000);", token.TimeOut);
                    foreach (string name in Request.Cookies)
                    {
                        if (name.Equals("ticket") || name.Equals("utime"))
                        {
                            bulider.AppendFormat("document.cookie ='{0}={1}'+';expires='+exp.toGMTString(){2}",
                                                 name, Request.Cookies[name].Value,
                                                 isIgnoreDomain == true
                                                     ? ";"
                                                     : string.Format("+';Path=\"/\";domain=\"{0}\";';",
                                                                     Configuration.ConfigurationManager.GetSetting<string>(
                                                                         "Domain")));
                        }
                    }
                }
                bulider.Append("window.UserCookie ={");
                foreach (string name in Request.Cookies)
                {
                    if (name.Equals("ticket") || name.Equals("utime"))
                        bulider.AppendFormat("{0}:'{1}',", name, Request.Cookies[name].Value);
                }
                if (Request.Cookies.Count > 0)
                    bulider.Remove(bulider.Length - 1, 1);
                bulider.Append("};");

            }
            return Content(bulider.ToString());
        }
    }
}
