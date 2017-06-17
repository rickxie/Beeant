using System;
using System.Web;
using System.Web.Mvc;
using Dependent;
using Beeant.Application.Services;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Log;
using Configuration;
using Winner.Persistence;

namespace Beeant.Basic.Services.Mvc.Extension
{
    static public class LogControllerExtension
    {
        #region 异常日志扩展方法
        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="handle"></param>
        static public void AddExceptionLog(this Controller handle)
        {
            AddExceptionLog();
        }
        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="handle"></param>
        static public void AddExceptionLog(this HttpApplication handle)
        {
            AddExceptionLog();
        }
        /// <summary>
        /// 添加异常
        /// </summary>
        /// <param name="handle"></param>
        static public void AddApiErrorLog(this HttpApplication handle)
        {
            try
            {
                var ex = HttpContext.Current.Server.GetLastError();
                var action = CheckSave();
                if (string.IsNullOrEmpty(action))
                {
                    var info = new ErrorEntity
                    {
                        Address = HttpContext.Current.Request.Url.ToString(),
                        Ip = HttpContextHelper.GetClientIp(),
                        Device = HttpContext.Current.Request.UserAgent,
                        SaveType = SaveType.Add
                    };
                    info.SetEntity(ex);
                    info.Account = new AccountEntity {Id = 0};
                    Ioc.Resolve<IApplicationService, ErrorEntity>().Save(info);
                }
                if (ConfigurationManager.GetSetting<bool>("IsDebug"))
                    throw ex;
                HttpContext.Current.Response.Redirect("/Shared/Error");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw ex;
            }
        }
        /// <summary>
        /// 添加错误信息
        /// </summary>
        public static void AddExceptionLog(this Controller handle,Exception ex,IdentityEntity identity=null)
        {
            var info = new ErrorEntity
            {
                Address = HttpContext.Current.Request.Url.ToString(),
                Ip = HttpContextHelper.GetClientIp(),
                Device = HttpContext.Current.Request.UserAgent,
                SaveType = SaveType.Add
            };
            if (identity != null)
            {
                info.Account = new AccountEntity {Id = identity.Id};
            }
            info.SetEntity(ex);
            Ioc.Resolve<IApplicationService, ErrorEntity>().Save(info);
        }
        /// <summary>
        /// 添加异常
        /// </summary>
        private static void AddExceptionLog()
        {
            try
            {
                var url = ConfigurationManager.GetSetting<string>("PresentationWebsiteSharedUrl");
                var action = CheckSave();
                if (string.IsNullOrEmpty(action))
                {
                    var info = new ErrorEntity
                    {
                        Address = HttpContext.Current.Request.Url.ToString(),
                        Ip = HttpContextHelper.GetClientIp(),
                        Device = HttpContext.Current.Request.UserAgent,
                        SaveType = SaveType.Add
                    };
                    info.SetEntity(HttpContext.Current.Server.GetLastError());
                    info.Account = new AccountEntity { Id = 0};
                    Ioc.Resolve<IApplicationService, ErrorEntity>().Save(info);
                    action = "General";
                }
                if (ConfigurationManager.GetSetting<bool>("IsDebug"))
                    throw HttpContext.Current.Server.GetLastError();
                HttpContext.Current.Response.Redirect(string.Format("{0}/Error/{1}", url, action));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw ex;
            }
        }

        /// <summary>
        /// 检查是否存储
        /// </summary>
        /// <returns></returns>
        private static string CheckSave()
        {
            if (HttpContext.Current == null) return null;
            var ex = HttpContext.Current.Server.GetLastError();
            if (ex == null) return null;
            var httpException = ex as HttpException;
            if (httpException != null)
            {
                HttpContext.Current.Response.StatusCode = httpException.GetHttpCode();
                switch (HttpContext.Current.Response.StatusCode)
                {
                    case 403:
                        return  "Http403";
                    case 404:
                        return "Http404";
                    case 413:
                        return "Http413";
                }
                if (ex.Message == "Version Expired")
                {
                    return "VersionExpired";
                }
            }
            else
            {
                var argumentException = ex as ArgumentException;
                if (argumentException != null && argumentException.ParamName == "parameters")
                {
                    return "Http404";
                }
            }
            return null;
        }


        #endregion

        #region 登入日志扩展方法

        /// <summary>
        /// 添加登入日志
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="identity"></param>
        /// <param name="type"></param>
        /// <param name="ip"></param>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <param name="device"></param>
        /// <param name="message"></param>
        public static void AddLoginLog(this Controller controller, IdentityEntity identity,string type, string ip, string city, string address,string device,string message)
        {
            if (identity == null)
                return;
            var info = new LoginEntity
                {
                    Type=string.IsNullOrEmpty(type)?"Account":type,
                    Ip = ip,
                    Address = address,
                    City=city,
                    Device = device,
                    Account = new AccountEntity { Id = identity.Id },
                    Message= message,
                    SaveType = SaveType.Add
                };
            Ioc.Resolve<IApplicationService,LoginEntity>().Save(info);
        }

        /// <summary>
        /// 添加登入日志
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="identity"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void AddLoginLog(this Controller controller, IdentityEntity identity, string type, string message)
        {
            if(identity==null)
                return;
            var info = new LoginEntity
            {
                Type = string.IsNullOrEmpty(type) ? "Account" : type,
                Ip = HttpContextHelper.GetClientIp(),
                Address = controller.Request.Url.ToString(),
                Device = controller.Request.UserAgent,
                Account = new AccountEntity { Id = identity.Id },
                Message = message,
                SaveType = SaveType.Add
            };
            Ioc.Resolve<IApplicationService, LoginEntity>().Save(info);
        }
        #endregion
 

    }
}
