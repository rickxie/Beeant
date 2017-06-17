using System;

namespace Beeant.Domain.Entities.Supplier
{
    [Serializable]
    public class ContractEntity:BaseEntity<ContractEntity>
    {
        /// <summary>
        /// 供应商标识
        /// </summary>
        public SupplierEntity Supplier { get; set; }
       
        /// <summary>
        /// 结算方式
        /// </summary>
        public string SettlementType { get; set; }

     

        /// <summary>
        /// 支付方式
        /// </summary>
        public ContractPaymentType PaymentType { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentTypeName
        {
            get { return PaymentType.GetName(); }
        }

        /// <summary>
        /// 配送方式
        /// </summary>
        public ContractDispatchType DispatchType { get; set; }

        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DispatchTypeName
        {
            get { return DispatchType.GetName(); }
        }

        /// <summary>
        /// 票据类型
        /// </summary>
        public ContractBillType BillType { get; set; }

        /// <summary>
        /// 票据类型发票
        /// </summary>
        public string BillTypeName 
        {
            get { return BillType.GetName(); }
        }

        /// <summary>
        /// 合同起始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 合同结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 返利条件
        /// </summary>
        public string Rebate { get; set; }

        /// <summary>
        /// 合同附件
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// 合同附件流
        /// </summary>
        public byte[] AttachmentByte { get; set; }

        /// <summary>
        /// 合同附件全名
        /// </summary>
        public string FullAttachment
        {
            get { return this.GetDownLoadUrl(Attachment); }
        }
      
    }
}
