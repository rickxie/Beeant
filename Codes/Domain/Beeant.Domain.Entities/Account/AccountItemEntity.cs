using System;
using Winner.Persistence;
namespace Beeant.Domain.Entities.Account
{


    [Serializable]
    public class AccountItemEntity : BaseEntity<AccountItemEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账户信息 
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 单价编号
        /// </summary>
        public long DataId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public AccountItemStatusType Status { get; set; }=AccountItemStatusType.Effective;
        /// <summary>
        /// 原始数据
        /// </summary>
        public AccountItemEntity DataEntity { get; set; }
        /// <summary>
        /// 相关数据
        /// </summary>
        public BaseEntity Data { get; set; }
 

        /// <summary>
        /// 类型名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }

        protected override void SetAddBusiness()
        {
            if(Status == AccountItemStatusType.Effective)
                SetAccount();
        }

        protected override void SetModifyBusiness()
        {
            if(!HasSaveProperty(it=>it.Status))
                return;
            InvokeItemLoader("DataEntity");
            if(DataEntity==null || DataEntity.Status==Status)
                return;
            if (DataEntity.Status != AccountItemStatusType.Effective && Status == AccountItemStatusType.Effective)
            {
                Amount = DataEntity.Amount;
            }
            else if (DataEntity.Status == AccountItemStatusType.Effective && Status != AccountItemStatusType.Effective)
            {
                Amount = 0-DataEntity.Amount;
            }
            SetAccount();
        }

        /// <summary>
        /// 设置账户
        /// </summary>
        /// <returns></returns>
        protected virtual void SetAccount()
        {
            InvokeItemLoader("Account");
            if (Account == null) return;
            Account.Balance += Amount;
            if (Account.SaveType == SaveType.None)
            {
                Account.SetProperty(it => it.Balance);
                Account.SaveType = SaveType.Modify;
            }
            else if (Account.Properties != null)
            {
                Account.SetProperty(it => it.Balance);
            }
        }
    }
    
}
