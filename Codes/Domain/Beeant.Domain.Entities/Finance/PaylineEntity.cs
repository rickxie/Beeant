using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Finance
{


    [Serializable]
    public class PaylineEntity : BaseEntity<PaylineEntity>
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
        /// 流水号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public PaylineType Type { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public PaylineStatusType Status { get; set; }

        /// <summary>
        /// 外部编号
        /// </summary>
        public string OutNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 平台类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }

        /// <summary>
        /// 平台类型
        /// </summary>
        public string ChannelTypeName
        {
            get { return ChannelType.GetName(); }
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public string StatusName
        {
            get { return this.GetStatusName(Status); }
        }
        /// <summary>
        /// 原始数据
        /// </summary>
        public PaylineEntity DataEntity { get; set; }

        /// <summary>
        /// 相关订单
        /// </summary>
        public IList<PaylineItemEntity> PaylineItems { get; set; }
        /// <summary>
        /// 支付流水
        /// </summary>
        public AccountItemEntity AccountItem { get; set; }
        #region 输出结果
        /// <summary>
        /// 输出
        /// </summary>
        public IDictionary<string,string> Forms { get; set; } 
        /// <summary>
        /// 输出流
        /// </summary>
        public string Request { get; set; }
        /// <summary>
        /// 响应流
        /// </summary>
        public string Response { get; set; }
        #endregion
   
        /// <summary>
        /// 设置流水号
        /// </summary>
        public virtual void SetNumber()
        {
            Number = Guid.NewGuid().ToString().Replace("-", "");
        }
        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetPaylineItems();
        }
        /// <summary>
        /// 设置修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            SetPaylineItems();
        }

        /// <summary>
        /// 设置订单
        /// </summary>
        protected virtual void SetPaylineItems()
        {
            if(!HasSaveProperty(it=>it.Status) || Status!= PaylineStatusType.Success)
                return;
            InvokeItemLoader("PaylineItems");
            if (PaylineItems == null)
                return ;
            InvokeItemLoader("DataEntity");
            if(DataEntity!=null &&　(DataEntity.Status== PaylineStatusType.Success || DataEntity.Status== PaylineStatusType.Failure))
                return;
            foreach (var paylineItem in PaylineItems)
            {
                if(paylineItem.Order==null)
                    continue;
                paylineItem.Order.OrderPays = paylineItem.Order.OrderPays ?? new List<OrderPayEntity>();
                paylineItem.Order.OrderPays.Add(new OrderPayEntity
                {
                    Order= paylineItem.Order,
                    Amount=paylineItem.Amount,
                    Remark= DataEntity==null?Remark: DataEntity.Remark,
                    Number= DataEntity == null || HasSaveProperty(it=>it.OutNumber) ? OutNumber : DataEntity.OutNumber,
                    Name = DataEntity == null ? TypeName : DataEntity.TypeName,
                    Tag = DataEntity == null ? Type.ToString() : DataEntity.Type.ToString(),
                    SaveType =SaveType.Add
                });
                if (paylineItem.Order.Status == OrderStatusType.WaitPay &&
                   paylineItem.Order.PayAmount+paylineItem.Amount == paylineItem.Order.TotalPayAmount)
                {
                    paylineItem.Order.Status = OrderStatusType.WaitDelivery;
                    if (paylineItem.Order.SaveType == SaveType.Add)
                        continue;
                    paylineItem.Order.SaveType = SaveType.Modify;
                    paylineItem.Order.SetProperty(it => it.Status);
                }

            }
        }

    }
}
