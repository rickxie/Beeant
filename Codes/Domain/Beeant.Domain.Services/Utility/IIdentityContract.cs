using System.ServiceModel;

namespace Beeant.Domain.Services.Utility
{
     [ServiceContract(Namespace = "http://Beeant.Domain.Services.Utility", ConfigurationName = "Beeant.Domain.Services.Utility.IIdentityContract")]
    public interface IIdentityContract
     {
         /// <summary>
         /// 设置
         /// </summary>
         /// <param name="timeOut"></param>
         /// <param name="info"></param>
         /// <param name="ticket"></param>
         /// <returns></returns>
         [OperationContractAttribute(
             Action = "http://Beeant.Domain.Services.Utility.IIdentityContract/Set",
             ReplyAction = "http://Beeant.Domain.Services.Utility.IIdentityContract/SetResponse")]
         bool Set(string ticket, int timeOut, string info);

         /// <summary>
         /// 得到
         /// </summary>
         /// <returns></returns>
         [OperationContractAttribute(
             Action = "http://Beeant.Domain.Services.Utility.IIdentityContract/Get",
             ReplyAction = "http://Beeant.Domain.Services.Utility.IIdentityContract/GetResponse")]
         string Get(string ticket);
        /// <summary>
        /// 移除
        /// </summary>
        /// <returns></returns>
        [OperationContractAttribute(
        Action = "http://Beeant.Domain.Services.Utility.IIdentityContract/Remove",
        ReplyAction = "http://Beeant.Domain.Services.Utility.IIdentityContract/RemoveResponse")]
         bool Remove(string ticket);
    }
}
