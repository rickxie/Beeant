using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Purchase;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Finance
{


    [Serializable]
    public class InvoiceinEntity : BaseEntity<InvoiceinEntity>
    {
        /// <summary>
        /// 渠道
        /// </summary>
        public ChannelType ChannelType { get; set; }
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
        public string ExpressNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
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
        /// 状态
        /// </summary>
        public InvoiceinStatusType Status { get; set; }

        /// <summary>
        /// 发票付款核销
        /// </summary>
        public IList<InvoiceinItemEntity> InvoiceinItems { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }

        public InvoiceinEntity DataEntity;

        /// <summary>
        /// 发票付款核销
        /// </summary>
        public IList<PurchaseInvoiceEntity> PurchaseInvoices { get; set; }
        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            if (Status == InvoiceinStatusType.Finish)
            {
                SetAccountItem();
                SetInvoices(true);
            }
               
        }

        /// <summary>
        /// 设置编辑业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            if (HasSaveProperty(it => it.Status))
            {
                InvokeItemLoader("DataEntity");
                if (Status == InvoiceinStatusType.Finish && DataEntity.Status != InvoiceinStatusType.Finish)
                {
                    Amount = DataEntity.Amount;
                    Account = DataEntity.Account;
                    SetAccountItem();
                    SetInvoices(true);
                }
                else if (Status != InvoiceinStatusType.Finish && DataEntity.Status == InvoiceinStatusType.Finish)
                {
                    Amount = 0 - DataEntity.Amount;
                    Account = DataEntity.Account;
                    SetAccountItem();
                    SetInvoices(false);
                }
            }
        }

        /// <summary>
        /// 设置删除业务
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("DataEntity");
            if (DataEntity == null)
                return;
            InvoiceNumber = DataEntity.InvoiceNumber;
            if (DataEntity.Status == InvoiceinStatusType.Finish)
            {
                Amount = 0 - DataEntity.Amount;
                Account = DataEntity.Account;
                SetAccountItem();
                SetInvoices(false);
            }
        }

        /// <summary>
        /// 流水账
        /// </summary>
        public AccountItemEntity AccountItem { get; set; }

        /// <summary>
        /// 设置AccountItem
        /// </summary>
        /// <returns></returns>
        protected virtual bool SetAccountItem()
        {
            InvokeItemLoader("Account");
            if (Account == null) return false;
            AccountItem = new AccountItemEntity
            {
                Amount = 0- Amount,
                Account = Account,
                Data = this,
                Status = AccountItemStatusType.Effective,
                SaveType = SaveType.Add
            };
            return true;
        }
        /// <summary>
        /// 设置发票核销
        /// </summary>
        protected virtual void SetInvoices(bool isStatus)
        {
            InvokeItemLoader("InvoiceinItems");
            if (InvoiceinItems == null) return;
            foreach (var invoiceinItem in InvoiceinItems)
            {
                var invoice = new PurchaseInvoiceEntity
                {
                    Purchase = invoiceinItem.Purchase,
                    Amount = isStatus?invoiceinItem.Amount:0- invoiceinItem.Amount,
                    Number = InvoiceNumber,
                    Remark = "",
                    SaveType = SaveType.Add
                };
                PurchaseInvoices.Add(invoice);
            }
        }
    }
    
}
