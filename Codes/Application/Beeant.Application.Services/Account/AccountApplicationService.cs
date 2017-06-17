using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Application.Services.Account
{
    public class AccountApplicationService : RealizeApplicationService<AccountEntity>
    {
       
        /// <summary>
        /// 重新异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool HandleException<T>(Exception ex, T info)
        {
            var rev = base.HandleException(ex, info);
            if (rev)
                return true;
            switch (ex.Message)
            {
                case "ResiterAccountError":
                    return true;
                default:
                    return false;
            }
        }
    }
}
