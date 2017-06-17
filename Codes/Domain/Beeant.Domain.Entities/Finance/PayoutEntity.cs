using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;
using Component.Extension;
using Beeant.Domain.Entities.Purchase;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Finance
{

  
    [Serializable]
    public class PayoutEntity : BaseEntity<PayoutEntity>
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
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账户信息 
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 是否为冲洗
        /// </summary>
        public bool IsFlush { get; set; }
        /// <summary>
        /// 货币
        /// </summary>
        public string Currency{ get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 原币种金额
        /// </summary>
        public decimal SourceAmount { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 大写金额
        /// </summary>
        public string Amountinwords { get; set; }
        /// <summary>
        /// 原始编号
        /// </summary>
        public string OriginalNumber { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行账户
        /// </summary>
        public string BankNumber { get; set; }
        /// <summary>
        /// 银行开户人
        /// </summary>
        public string BankHolder { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public UserEntity User { get; set; }
        /// <summary>
        /// 平台类型
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
        /// 是否为冲
        /// </summary>
        public string IsFlushName
        {
            get { return this.GetStatusName(IsFlush); }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public IList<PayoutItemEntity> PayoutItems { get; set; } 
    
        /// <summary>
        /// 数据集
        /// </summary>
        public PayoutEntity DataEntity { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public PayoutStatusType Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }
        /// <summary>
        /// 付款核销
        /// </summary>
        public IList<PurchasePayEntity> Pays { get; set; }
        #region 业务代码
        /// <summary>
        /// 设置总金额
        /// </summary>
        protected virtual void SetAmount()
        {
            if (!HasSaveProperty(it => it.Amount)) return;
            Amountinwords = Amount.ConvertToChineseMoney();
            if (Properties == null) return;
            SetProperty(it => it.Amountinwords);
        }



        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetAmount();
            if (Status == PayoutStatusType.Finish)
            {
                SetPays(true);
            }
          
        }

        /// <summary>
        /// 设置编辑业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            SetAmount();
            if (HasSaveProperty(it => it.Status))
            {
                InvokeItemLoader("DataEntity");
                if (Status == PayoutStatusType.Finish && DataEntity.Status != PayoutStatusType.Finish)
                {
                    ChangeEveryStatusToFinish();
                }
                else if (Status != PayoutStatusType.Finish && DataEntity.Status == PayoutStatusType.Finish)
                {
                    ChangeFinishToEveryStatus();
                }
            }
        }
        /// <summary>
        /// 状态变成完成
        /// </summary>
        protected virtual void ChangeEveryStatusToFinish()
        {
         
            Amount = DataEntity.Amount;
            Account = DataEntity.Account;
            PayType = DataEntity.PayType;
            OriginalNumber = DataEntity.OriginalNumber;
            SetPays(true);
        }
        /// <summary>
        /// 从完成变成其他状态
        /// </summary>
        protected virtual void ChangeFinishToEveryStatus()
        {
            Amount = 0 - DataEntity.Amount;
            Account = DataEntity.Account;
            PayType = DataEntity.PayType;
            OriginalNumber = DataEntity.OriginalNumber;
            SetPays(false);
        }

        /// <summary>
        /// 设置删除业务
        /// </summary>
        protected virtual void SetRemoveBuiness()
        {
            InvokeItemLoader("DataEntity");
            DataEntity.SaveType=SaveType.Remove;
            if (DataEntity.Status == PayoutStatusType.Finish)
            {
                Amount = 0 - DataEntity.Amount;
                Account = DataEntity.Account;
                PayType = DataEntity.PayType;
                OriginalNumber = DataEntity.OriginalNumber;
                SetPays(false);
            }
        }

 




        /// <summary>
        /// 设置付款
        /// </summary>
        protected virtual void SetPays(bool isStatus)
        {
            InvokeItemLoader("PayoutItems");
            if(PayoutItems == null)return;
            foreach (var payoutItem in PayoutItems)
            {
                var pay = new PurchasePayEntity
                {
                    Purchase = payoutItem.Purchase,
                    Amount = isStatus ? payoutItem.Amount : 0 - payoutItem.Amount,
                    Number=OriginalNumber,
                    PayType=PayType,
                    Remark = "",
                    SaveType = SaveType.Add
                };
                Pays.Add(pay);
            }
        }
        #endregion
    }
    
}
