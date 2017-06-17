using System;
using System.Linq;
using System.Web.Mvc;
using Beeant.Application.Services;
using Dependent;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Winner.Persistence;
using Beeant.Domain.Entities.Log;

namespace Beeant.Distributed.Outside.Api.Controllers.Log
{
    [TokenFilter]
    public class ErrorController : ApiBaseController
    {
        #region 注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public virtual ActionResult Add(string message,string address,string device,string ip,string detail)
        {
            try
            {
                var info = new ErrorEntity
                {
                    Address = string.IsNullOrWhiteSpace(address) ? Request.Url.ToString(): address,
                    Device =string.IsNullOrWhiteSpace(device) ? Request.UserAgent: device,
                    Ip = string.IsNullOrWhiteSpace(ip) ? Request.UserHostAddress : ip,
                    Message = message,
                    Detail = string.IsNullOrWhiteSpace(detail) ? "": detail,
                    SaveType = SaveType.Add
                };
                var rev=Ioc.Resolve<IApplicationService, ErrorEntity>().Save(info);
                return rev
                    ? ReturnSuccessResult("添加成功")
                    : ReturnFailureResult(info.Errors != null && info.Errors.Count > 0
                        ? string.Join(",", info.Errors.Select(it => it.Message).ToArray())
                        : "添加失败");

            }
            catch (Exception ex)
            {
                return ReturnExceptionResult(ex);
            }
        }

     
        #endregion
    }
}
