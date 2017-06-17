using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Promotion
{
    [Serializable]
    public class CouponerEntity : BaseEntity<CouponerEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 优惠券面值
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 优惠券截止日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 优惠券数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 每个用户领取数量
        /// </summary>
        public int CollectCount { get; set; }
        /// <summary>
        /// 领取截止时间
        /// </summary>
        public DateTime CollectEndDate { get; set; }
        /// <summary>
        /// 是否需要密码
        /// </summary>
        public bool IsCode { get; set; }
        /// <summary>
        /// 优惠券备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetStatusName(IsUsed); }
        }
        /// <summary>
        /// 是否需要密码
        /// </summary>
        public string IsCodeName
        {
            get { return this.GetStatusName(IsCode); }
        }
        /// <summary>
        /// 是否需要密码
        /// </summary>
        public string IsShowName
        {
            get { return this.GetShowName(IsShow); }
        }
    }
}
