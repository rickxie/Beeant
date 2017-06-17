using System.ServiceModel;

namespace Beeant.Domain.Services.Product
{
     [ServiceContract(Namespace = "http://Beeant.Domain.Services.Product", ConfigurationName = "Beeant.Domain.Services.Product.IProductContract")]
    public interface IProductContract
    {
         /// <summary>
         /// 设置
         /// </summary>
         /// <param name="info"></param>
         /// <returns></returns>
         [OperationContract(
             Action = "http://Beeant.Domain.Services.Product.IProductService/Set",
             ReplyAction = "http://Beeant.Domain.Services.Product.IProductService/SetResponse")]
         string Save(string info);

    }
}
