using System;

namespace Beeant.Domain.Entities.Account
{
    [Serializable]
    public class CardEntity : BaseEntity<CardEntity>
    {
        /// <summary>
        /// 账户信息 
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 日充值限额
        /// </summary>
        public decimal DayRechargeAmount { get; set; }
        /// <summary>
        /// 日提现限额
        /// </summary>
        public decimal DayWithdrawAmount { get; set; }
        /// <summary>
        /// 日消费限额
        /// </summary>
        public decimal DayConsumeAmount { get; set; }
    }
}
