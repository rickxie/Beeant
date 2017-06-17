using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;

namespace Beeant.Domain.Entities.Member
{
    public class InvoiceEntity : BaseEntity<InvoiceEntity>
    {
        /// <summary>
        /// 所属账户
        /// </summary>
        public AccountEntity Account { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        public InvoiceType Type { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }

        /// <summary>
        /// 发票类型
        /// </summary>
        public InvoiceGeneralType GeneralType { get; set; }

        /// <summary>
        /// 发票类型名称
        /// </summary>
        public string GeneralTypeName
        {
            get { return GeneralType.GetName(); }
        }


        /// <summary>
        /// 发票标题
        /// </summary>
        public string Title { get; set; }

  

        /// <summary>
        /// 发票内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 名称AccountId对应Agent中对象的Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Posctcode { get; set; }
    }
}