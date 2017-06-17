using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Finance
{

  
    [Serializable]
    public class InvoiceoutEntity : BaseEntity<InvoiceoutEntity>
    {
        /// <summary>
        /// 渠道
        /// </summary>
        public ChannelType ChannelType { get; set; }
        /// <summary>
        /// 发票类型
        /// </summary>
        public InvoiceType Type { get; set; }
        /// <summary>
        /// 开票类型
        /// </summary>
        public InvoiceGeneralType GeneralType { get; set; }
        /// <summary>
        /// 账户信息 
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 是否为冲
        /// </summary>
        public bool IsFlush { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 发票号码
        /// </summary>
        public string InvoiceNumber { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// 接收地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string ExpressName { get; set; }
        /// <summary>
        /// 快递编号
        /// </summary>
        public string ExpressNumber{ get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public UserEntity User { get; set; }
        /// <summary>
        /// 是否为冲
        /// </summary>
        public string IsFlushName
        {
            get { return this.GetStatusName(IsFlush); }
        }
        /// <summary>
        /// 来源
        /// </summary>
        public string ChannelTypeName
        {
            get { return ChannelType.GetName(); }
        }
        /// <summary>
        /// 来源
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }
        /// <summary>
        /// 来源
        /// </summary>
        public string GeneralTypeName
        {
            get { return GeneralType.GetName(); }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public InvoiceoutStatusType Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }
        /// <summary>
        /// 发票付款核销
        /// </summary>
        public IList<OrderInvoiceEntity> OrderInvoices { get; set; }
        /// <summary>
        /// 发票付款核销
        /// </summary>
        public IList<InvoiceoutItemEntity> InvoiceoutItems { get; set; }

        /// <summary>
        /// 原有数据
        /// </summary>
        public InvoiceoutEntity DataEntity { get; set; }

      
        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            if (Status == InvoiceoutStatusType.Finish)
            {
                SetInvoices(true);
            }
               
        }

      
        /// <summary>
        /// 设置删除业务
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("DataEntity");
            if(DataEntity==null)
                return;
            InvoiceNumber = DataEntity.InvoiceNumber;
            if (DataEntity.Status == InvoiceoutStatusType.Finish)
            {
                Amount = 0 - DataEntity.Amount;
                Account = DataEntity.Account;
                SetInvoices(false);
            }
        }

       
        /// <summary>
        /// 设置发票核销
        /// </summary>
        protected virtual void SetInvoices(bool isStatus)
        {
            InvokeItemLoader("InvoiceoutItems");
            if (InvoiceoutItems == null) return;
            foreach (var invoiceoutItem in InvoiceoutItems)
            {
                var invoice = new OrderInvoiceEntity
                {
                    Order = invoiceoutItem.Order,
                    Number=InvoiceNumber,
                    Amount = isStatus ? invoiceoutItem.Amount : 0 - invoiceoutItem.Amount,
                    Remark = "",
                    SaveType = SaveType.Add
                };
                OrderInvoices.Add(invoice);
            }
        }
    }
    
}
