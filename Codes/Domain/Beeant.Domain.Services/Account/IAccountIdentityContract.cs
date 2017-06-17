using System.ServiceModel;

namespace Beeant.Domain.Services.Account
{
     [ServiceContract(Namespace = "http://Beeant.Domain.Services.Account", ConfigurationName = "Beeant.Domain.Services.Account.IAccountIdentityContract")]
    public interface IAccountIdentityContract
     {
         /// <summary>
         /// 设置
         /// </summary>
         /// <param name="info"></param>
         /// <returns></returns>
         [OperationContract(
             Action = "http://Beeant.Domain.Services.Account.IAccountIdentityContract/Set",
             ReplyAction = "http://Beeant.Domain.Services.Account.IAccountIdentityService/SetResponse")]
         string Save(string info);

    }
}
