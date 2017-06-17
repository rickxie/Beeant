
using Beeant.Application.Dtos.Account;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Security;

namespace Beeant.Application.Services.Account
{
    public interface IPasswordApplicationService
    {
        /// <summary>
        /// 检查密码是否正确
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool CheckPassword(string name, string password,out AccountEntity entity);
        /// <summary>
        /// 检查密码是否正确
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool CheckPassword(long accountId,string password);
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AccountEntity CheckAccount(string name);

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        PasswordDto SendCode(string name, CodeEntity code);
        /// <summary>
        /// 检查验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        PasswordDto CheckCode(string name, string code, CodeType codeType);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        PasswordDto Reset(string name,string password);
    }
}
