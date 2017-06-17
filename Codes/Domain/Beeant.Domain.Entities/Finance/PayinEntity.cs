using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Order;
using Component.Extension;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Finance
{


    [Serializable]
    public class PayinEntity : BaseEntity<PayinEntity>
    {
        /// <summary>
        /// 渠道
        /// </summary>
        public ChannelType ChannelType { get; set; }
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
        public string Currency { get; set; }

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
        /// 收款核销
        /// </summary>
        public IList<PayinItemEntity> PayinItems { get; set; }

        /// <summary>
        /// 是否为冲
        /// </summary>
        public string IsFlushName
        {
            get { return this.GetStatusName(IsFlush); }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public PayinStatusType Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }
        /// <summary>
        /// 数据集
        /// </summary>
        public PayinEntity DataEntity { get; set; }

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
            if (Status == PayinStatusType.Finish)
            {
                SetPayinItems(true);
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
                if (Status == PayinStatusType.Finish && DataEntity.Status != PayinStatusType.Finish)
                {
                    ChangeEveryStatusToFinish();
                }
                else if (Status != PayinStatusType.Finish && DataEntity.Status == PayinStatusType.Finish)
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
            SetPayinItems(true);
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
            SetPayinItems(false);
        }

        /// <summary>
        /// 设置删除业务
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("DataEntity");
            DataEntity.SaveType = SaveType.Remove;
            if (DataEntity.Status == PayinStatusType.Finish)
            {
                Amount = 0 - DataEntity.Amount;
                Account = DataEntity.Account;
                PayType = DataEntity.PayType;
                OriginalNumber = DataEntity.OriginalNumber;
                SetPayinItems(false);
            }
        }

        /// <summary>
        /// 流水账
        /// </summary>
        public IList<OrderPayEntity> Pays { get; set; }

       
        /// <summary>
        /// 设置付款
        /// </summary>
        protected virtual void SetPayinItems(bool isStatus)
        {
            InvokeItemLoader("PayinItems");
            if (PayinItems == null) return;
            Pays=new List<OrderPayEntity>();
            foreach (var payinItem in PayinItems)
            {
                var pay = new OrderPayEntity
                {
                    Order = payinItem.Order,
                    Amount = isStatus ? payinItem.Amount : 0 - payinItem.Amount,
                    Number=OriginalNumber,
                    Name = PayType,
                    Remark = "",
                    SaveType = SaveType.Add
                };
                Pays.Add(pay);
            }
        }

    }
    
}
