using System;
using System.Globalization;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using Beeant.Application.Services;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Log;
using Beeant.Domain.Entities.Management;
using Configuration;
using Dependent;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class LogPageExtension
    {
        #region 异常日志扩展方法

        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="ex"></param>
        static public void AddExceptionLog(this Page handle, Exception ex=null)
        {
            ex = ex == null && HttpContext.Current != null ? HttpContext.Current.Server.GetLastError() : ex;
            var info = new ErrorEntity
            {
                Address = HttpContext.Current.Request.Url.ToString(),
                Device = HttpContext.Current.Request.UserAgent,
                Ip = GetClientIp(),
                SaveType = SaveType.Add
            };
            info.SetEntity(ex);
            FillIdentity(info);
            Ioc.Resolve<IApplicationService, ErrorEntity>().Save(info);
        }

        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="e"></param>
        static public void AddExceptionLog(this HttpApplication handle, Exception e=null)
        {
            AddExceptionLog(e);
        }
 
        /// <summary>
        /// 添加异常
        /// </summary>
        private static void AddExceptionLog(Exception e)
        {
            try
            {
                AddErrorEntity(e);
            }
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw ex;
            }
        }


        /// <summary>
        /// 添加错误信息
        /// </summary>
        private static void AddErrorEntity(Exception ex)
        {
            ex = ex==null && HttpContext.Current !=null? HttpContext.Current.Server.GetLastError():ex;
            if (!CheckSave(ex)) return;
            var info = new ErrorEntity
            {
                Address = HttpContext.Current.Request.Url.ToString(),
                Device = HttpContext.Current.Request.UserAgent,
                Ip = GetClientIp(),
                SaveType = SaveType.Add
            };
            info.SetEntity(ex);
            FillIdentity(info);
            Ioc.Resolve<IApplicationService, ErrorEntity>().Save(info);
            if (ConfigurationManager.GetSetting<bool>("IsDebug"))
                throw ex;
            var url = UrlExtension.GetUrl(null, "PresentationAdminHomeUrl");
            HttpContext.Current.Response.Redirect(string.Format("{0}/Errors/Generic.htm", url));
        }

        /// <summary>
        /// 检查是否存储
        /// </summary>
        /// <returns></returns>
        private static bool CheckSave(Exception ex)
        {
            if (HttpContext.Current == null) return false;
            if (ex == null) return false;
            var exception = ex as HttpException;
            if (exception == null) return true;
            var statusCode = exception.GetHttpCode().ToString(CultureInfo.InvariantCulture);
            var url = UrlExtension.GetUrl(null, "PresentationAdminHomeUrl");
            switch (statusCode)
            {
                case "403":
                    HttpContext.Current.Response.Redirect(string.Format("{0}/Errors/403.htm", url));
                    break;
                case "404":
                    HttpContext.Current.Response.Redirect(string.Format("{0}/Errors/404.htm", url));
                    break;
                case "500":
                    var runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
                    if (runTime == null) return true;
                    int maxRequestLength = (runTime.MaxRequestLength) * 1024;
                    if (HttpContext.Current.Request.ContentLength > maxRequestLength)
                    {
                        HttpContext.Current.Response.Redirect(string.Format("{0}/Errors/413.htm", url));
                        return false;
                    }
                    if (ex.Message == "Version Expired")
                    {
                        HttpContext.Current.Response.Redirect(string.Format("{0}/Errors/VersionExpired.htm", url));
                        return false;
                    }
                    return true;
 
            }
            return false;
        }

        /// <summary>
        /// 填充验证信息
        /// </summary>
        /// <param name="info"></param>
        private static void FillIdentity(ErrorEntity info)
        {
            var identity = Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>();
            if (identity != null)
            {
                info.Account = new AccountEntity {Id = identity.Id};
            }
        }
        #endregion

        #region 登入日志扩展方法

        /// <summary>
        /// 添加登入日志
        /// </summary>
        /// <param name="page"></param>
        /// <param name="identity"></param>
        /// <param name="message"></param>
        public static void AddLoginLog(this Page page, IdentityEntity identity,string message)
        {
            var info = new LoginEntity
                {
                    Type="User",
                    Ip = GetClientIp(),
                    Address = page.Request.Url.ToString(),
                    Device=page.Request.UserAgent,
                    Account = new AccountEntity { Id = identity.Id },
                    Message = message,
                    SaveType = SaveType.Add
              };
            Ioc.Resolve<IApplicationService,LoginEntity>().Save(info);
        }
        #endregion

        #region 操作日志

        /// <summary>
        /// 添加登入日志
        /// </summary>
        /// <param name="page"></param>
        /// <param name="identity"></param>
        /// <param name="name"></param>
        /// <param name="control"></param>
        /// <param name="detail"></param>
        /// >
        public static void AddOperationLog(this Page page, IdentityEntity identity, string name, string control,string detail)
        {
            Action<Page, IdentityEntity, string, string, string> action = BeginAddOperationLog;
            action.BeginInvoke(page, identity, name, control, detail, null, null);
        }

        /// <summary>
        /// 异步
        /// </summary>
        /// <param name="page"></param>
        /// <param name="identity"></param>
        /// <param name="name"></param>
        /// <param name="control"></param>
        /// <param name="detail"></param>
        private static void BeginAddOperationLog(Page page, IdentityEntity identity, string name, string control, string detail)
        {
            var info = new OperationEntity
            {
                Type = "WebForm",
                Name = name,
                Ip = GetClientIp(),
                Address = page.Request.Url.ToString(),
                Device = page.Request.UserAgent,
                Account = new AccountEntity {Id= identity.Id },
                Detail = detail,
                Control = control,
                SaveType = SaveType.Add
            };
            Ioc.Resolve<IApplicationService, OperationEntity>().Save(info);
        }
     
        /// <summary>
        /// 得到客户端地址
        /// </summary>
        /// <returns></returns>
        private static string GetClientIp()
        {
            if (HttpContext.Current == null)
                return null;
            return HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null ?
                HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] :
                HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        #endregion

    }
}
