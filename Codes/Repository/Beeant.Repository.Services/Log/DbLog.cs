using System;
using System.Web;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Log;
using Winner;
using Winner.Log;
using Winner.Persistence;

namespace Beeant.Repository.Services.Log
{
    public class DbLog:ILog
    {
        /// <summary>
        /// 添加错误日志
        /// </summary>
        /// <param name="ex"></param>
        public void AddException(Exception ex)
        {
            Action<Exception> func = Add;
            func.BeginInvoke(ex, null, null);
          
        }
        
        /// <summary>
        /// 添加错误日志
        /// </summary>
        /// <param name="content"></param>
        public void WriteFile(string content)
        {
            Action<string> func = Add;
            func.BeginInvoke(content, null, null);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void Add(Exception ex)
        {
            var info = new ErrorEntity
            {
                Address = HttpContext.Current == null ? "" : HttpContext.Current.Request.Url.ToString(),
                Ip = HttpContext.Current == null ? "" : HttpContext.Current.Request.UserHostAddress,
                Device = HttpContext.Current == null ? "" : HttpContext.Current.Request.UserAgent,
                Account = new AccountEntity { Id = 0 },
                SaveType = SaveType.Add
            };
            info.SetEntity(ex);
            Creator.Get<IContext>().Set(info, info, info.SaveSequence);
            Creator.Get<IContext>().Commit(Creator.Get<IContext>().Save());
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="content"></param>
        protected virtual void Add(string content)
        {
            var info = new ErrorEntity
            {
                Address = HttpContext.Current == null ? "" : HttpContext.Current.Request.Url.ToString(),
                Ip = HttpContext.Current == null ? "" : HttpContext.Current.Request.UserHostAddress,
                Device = HttpContext.Current == null ? "" : HttpContext.Current.Request.UserAgent,
                Account = new AccountEntity { Id =0},
                Message = content,
                Detail = "",
                SaveType = SaveType.Add
            };
            Creator.Get<IContext>().Set(info, info, info.SaveSequence);
            Creator.Get<IContext>().Save();
            Creator.Get<IContext>().Commit(Creator.Get<IContext>().Save());
        }
    }
}
