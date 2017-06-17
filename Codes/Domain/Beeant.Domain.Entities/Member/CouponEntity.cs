using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Promotion;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Member
{
    [Serializable]
    public class CouponEntity : BaseEntity<CouponEntity>
    {
  
      
        /// <summary>
        ///账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 优惠券模板
        /// </summary>
        public CouponerEntity Couponer { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime UsedTime { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime CollectTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUsed { get; set; }
 
        /// <summary>
        /// 相关订单
        /// </summary>
        public string OrderIds { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
  
        /// <summary>
        /// 是否使用
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetStatusName(IsUsed); }
        }
        public CouponEntity DataEntity { get; set; }
 
        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetUsedTime();
            SetCouponer();
        }

 

        /// <summary>
        /// 设置优惠券模板
        /// </summary>
        protected virtual void SetCouponer()
        {
            InvokeItemLoader("Couponer");
            if(Couponer==null || Couponer.Id==0)
                return;
            Couponer.Count--;
            if (Couponer.SaveType == SaveType.None)
            {
                Couponer.SetProperty(it => it.Count);
                Couponer.SaveType = SaveType.Modify;
            }
            else if (Couponer.Properties != null)
            {
                Couponer.SetProperty(it => it.Count);
            }
        }

        /// <summary>
        /// 设置使用时间
        /// </summary>
        protected virtual void SetUsedTime()
        {
            if (!HasSaveProperty(it => it.IsUsed) || !IsUsed)
                return;
            UsedTime = DateTime.Now;
            if (Properties == null) return;
            SetProperty(it => it.UsedTime);
        }

    }
}
