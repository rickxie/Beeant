using System;
using System.IO;
using System.Web;

namespace Winner.Storage
{
    public class DocumentHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Output(context, context.Request.Url.AbsolutePath.ToLower());
        }

        public bool IsReusable { get; }
        /// <summary>
        /// 签名地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SignUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            var timespan = DateTime.Now.Ticks.ToString();
            var mark = Creator.Get<Base.ISecurity>().EncryptSign(timespan);
            url = string.Format("{0}{1}timespan={2}&mark={3}", url, url.Contains("?")?"&":"?", timespan, mark);
            return url;
        }
        /// <summary>
        /// 检查
        /// </summary>
        /// <returns></returns>
        protected virtual bool Check(HttpContext context)
        {
            var url = context.Request.Url.ToString();
            var query = url.Split('?');
            var ps = query.Length == 2 ? query[1] : url;
            var timespan = HttpUtility.ParseQueryString(ps).Get("timespan");
            var mark = HttpUtility.ParseQueryString(ps).Get("mark");
            if (string.IsNullOrEmpty(mark) || string.IsNullOrEmpty(timespan))
                return false;
            var mk = Creator.Get<Base.ISecurity>().EncryptSign(timespan);
            if (mark.ToLower() != mk.ToLower())
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        protected virtual void Output(HttpContext context,string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                context.Response.Write("");
                return;
            }
            if (!Check(context))
            {
                context.Response.Write("");
            }
            fileName = context.Server.MapPath(fileName);
            var exe = Path.GetExtension(fileName);
            if (File.Exists(fileName))
            {
                context.Response.Charset = "UTF-8"; // 或UTF-7 以防乱码
                context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}{1}",string.IsNullOrEmpty(context.Request["name"])? Guid.NewGuid().ToString(): context.Request["name"], exe));
                context.Response.ContentType = "text/octet-stream";
                context.Response.Clear();
                context.Response.Flush();
                context.Response.WriteFile(fileName);
            }
          
        }
 

    }
}
