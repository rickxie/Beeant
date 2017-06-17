using System;
using System.Web;
using Configuration;
using Resource;

namespace Beeant.Basic.Services.Common.Extension
{
    static public class GlobalExtension
    {
        static public object Locker = new object();
        public static bool IsLoack;
        public static void Initialize(this HttpApplication httpApplication)
        {
            if(IsLoack)
                return;
            lock (Locker)
            {
                if (IsLoack)
                    return;
                IsLoack = true;
                ConfigurationManager.Initialize(httpApplication.GetType().BaseType.Namespace);
                Action<HttpApplication> compressAction = CompressResource;
                compressAction.BeginInvoke(httpApplication,null, null);
            }
        }

        /// <summary>
        /// 加载语言包
        /// </summary>
        private static void CompressResource(HttpApplication httpApplication)
        {
            if (!ConfigurationManager.GetSetting<bool>("IsDebug"))
            {
                ResourceManager.Compress(httpApplication.Server.MapPath("~/Content/"));
                ResourceManager.Compress(httpApplication.Server.MapPath("~/Styles/"));
                ResourceManager.Compress(httpApplication.Server.MapPath("~/Scripts/"));
            }
        }
 
    }
}
