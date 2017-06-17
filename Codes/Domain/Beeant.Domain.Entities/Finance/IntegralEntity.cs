using System;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Finance
{
    [Serializable]
    public class IntegralEntity : BaseEntity<IntegralEntity>
    {
        /// <summary>
        /// 账户信息
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 积分增减值
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public IntegralEntity DataEntity { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public UserEntity User { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public IntegralStatusType Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }

        #region 业务代码




        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            if (Status == IntegralStatusType.Finish)
            {
                SetIntegralItem();
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
                if (Status == IntegralStatusType.Finish && DataEntity.Status != IntegralStatusType.Finish)
                {
                    ChangeEveryStatusToFinish();
                }
                else if (Status != IntegralStatusType.Finish && DataEntity.Status == IntegralStatusType.Finish)
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
            Title = DataEntity.Title;
            Amount = DataEntity.Amount;
            Account = DataEntity.Account;
            Remark = DataEntity.Remark;
            SetIntegralItem();
        }
        /// <summary>
        /// 从完成变成其他状态
        /// </summary>
        protected virtual void ChangeFinishToEveryStatus()
        {
            Title = DataEntity.Title;
            Amount = 0 - DataEntity.Amount;
            Account = DataEntity.Account;
            Remark = DataEntity.Remark;
            SetIntegralItem();
        }

        /// <summary>
        /// 设置删除业务
        /// </summary>
        protected virtual void SetRemoveBuiness()
        {
            InvokeItemLoader("DataEntity");
            DataEntity.SaveType = SaveType.Remove;
            if (DataEntity.Status == IntegralStatusType.Finish)
            {
                Title = DataEntity.Title;
                Amount = 0 - DataEntity.Amount;
                Account = DataEntity.Account;
                SetIntegralItem();
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
        protected virtual bool SetIntegralItem()
        {
            InvokeItemLoader("Account");
            if (Account == null || Amount == 0) return false;
            AccountItem = new AccountItemEntity
            {
                Name=Title,
                Amount = Amount,
                Account = Account,
                Data = this,
                Remark = Remark,
                SaveType = SaveType.Add
            };
            return true;
        }

        #endregion
    }
}
