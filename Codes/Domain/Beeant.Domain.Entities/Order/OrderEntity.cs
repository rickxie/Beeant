using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Component.Extension;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Order
{


    [Serializable]
    public class OrderEntity : BaseEntity<OrderEntity>
    {
        /// <summary>
        /// 定金
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        ///应用类型
        /// </summary>
        public ChannelType ChannelType { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType Type { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal TotalPayAmount { get; set; }
        /// <summary>
        /// 可开具发票金额
        /// </summary>
        public decimal TotalInvoiceAmount { get; set; }
        /// <summary>
        /// 已开发票金额
        /// </summary>
        public decimal InvoiceAmount { get; set; }
        /// <summary>
        /// 成本金额
        /// </summary>
        public decimal CostAmount { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayTypes { get; set; }
        /// <summary>
        /// 所属账户
        /// </summary>
        public AccountEntity Account { get; set; }
 

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 是否额度账期
        /// </summary>
        public OrderSettleType SettleType { get; set; }

        /// <summary>
        /// 是否额度账期
        /// </summary>
        public string SettleTypeName
        {
            get { return SettleType.GetName(); }
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string[] PayTypeArray
        {
            get
            {
                if (string.IsNullOrEmpty(PayTypes))
                    return null;
                return PayTypes.Split(',');
            }
        }
        /// <summary>
        /// 订单类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }
        /// <summary>
        /// 平台名称
        /// </summary>
        public string ChannelTypeName
        {
            get { return ChannelType.GetName(); }
        }
        /// <summary>
        /// 路由编号
        /// </summary>
        public string RouteId { get; set; }
        #region 配置属性

        private string _variables;

        /// <summary>
        /// 设置
        /// </summary>
        public string Variables
        {
            get
            {
                if (string.IsNullOrEmpty(_variables) && VariablesDictionary != null)
                    _variables = VariablesDictionary.SerializeJson();
                return _variables;
            }
            set
            {
                _variables = value;
                if (string.IsNullOrEmpty(value))
                    return;
                VariablesDictionary = value.DeserializeJson<Dictionary<string, object>>();
            }
        }
        public IDictionary<string, object> VariablesDictionary { get; set; }

        /// <summary>
        /// 得到设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T GetVariable<T>(string key)
        {
            if (VariablesDictionary == null || !VariablesDictionary.ContainsKey(key))
                return default(T);
            return VariablesDictionary[key].Convert<T>();
        }


        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void SetVariable(string key, object value)
        {
            VariablesDictionary = VariablesDictionary ?? new Dictionary<string, object>();
            if (VariablesDictionary.ContainsKey(key))
                VariablesDictionary[key] = value;
            else
                VariablesDictionary.Add(key, value);
        }


        #endregion
        /// <summary>
        /// 订单明细
        /// </summary>
        public IList<OrderItemEntity> OrderItems { get; set; }
 
        /// <summary>
        /// 收款纪录
        /// </summary>
        public IList<OrderPayEntity> OrderPays { get; set; }
        /// <summary>
        /// 维护记录
        /// </summary>
        public IList<OrderNoteEntity> OrderNotes { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public IList<OrderAttachmentEntity> OrderAttachments { get; set; }
        /// <summary>
        /// 快递信息
        /// </summary>
        public IList<OrderExpressEntity> OrderExpresses { get; set; }
        /// <summary>
        /// 订单发票
        /// </summary>
        public IList<OrderInvoiceEntity> OrderInvoices { get; set; }
        /// <summary>
        /// 订单投诉
        /// </summary>
        public IList<OrderComplaintEntity> OrderComplaints { get; set; }
  



        /// <summary>
        /// 原数据
        /// </summary>
        public OrderEntity DataEntity { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public OrderStatusType Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }

        #region 关联订单
        /// <summary>
        /// 订单商品
        /// </summary>
        public IList<OrderProductEntity> OrderProducts { get; set; }

        /// <summary>
        /// 保险信息
        /// </summary>
        public IList<OrderInsuranceEntity> OrderInsurances { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public IList<OrderNumberEntity> OrderNumbers { get; set; }
        #endregion

        #region 业务代码


        #region 添加业务
        /// <summary>
        /// 设置添加业务
        /// </summary>
        protected override void SetAddBusiness()
        {
            SetRelateSaveType(SaveType.Add);
            InvokeItemLoader("Account");
            if (Account == null)
                return;
            SettleType = OrderSettleType.Immediately;
            if (Properties != null)
            {
                SetProperty(it => it.SettleType);
            }
        }
        #endregion

        #region 修改业务
        /// <summary>
        /// 设置修改业务
        /// </summary>
        protected override void SetModifyBusiness()
        {
            if (HasSaveProperty(it => it.Status))
            {
                InvokeItemLoader("DataEntity");
                SetModifyOrderItemsIsCount();
            }
        }



        /// <summary>
        /// 订单明细
        /// </summary>
        protected virtual void SetModifyOrderItemsIsCount()
        {
            InvokeItemLoader("OrderProducts");
            if (OrderProducts != null)
            {
                foreach (var orderProduct in OrderProducts)
                {
                    if (orderProduct.Product == null) continue;
                    orderProduct.Order = this;
                    orderProduct.SetIsCount();
                    if (orderProduct.SaveType == SaveType.None)
                    {
                        orderProduct.SetProperty(it => it.IsCount);
                        orderProduct.SaveType = SaveType.Modify;
                    }

                }
            }
        }

        #endregion

        #region 重新删除
        /// <summary>
        /// 设置删除订单
        /// </summary>
        protected override void SetRemoveBusiness()
        {
            InvokeItemLoader("OrderItems");
            InvokeItemLoader("OrderPays");
            SetRelateSaveType(SaveType.Remove);
        }

        /// <summary>
        /// 设置相关业务存储类型
        /// </summary>
        /// <param name="saveType"></param>
        protected virtual void SetRelateSaveType(SaveType saveType)
        {
            if (OrderItems != null)
            {
                foreach (var orderItem in OrderItems)
                {
                    orderItem.Order = this;
                    orderItem.SaveType = saveType;
                }
            }
            if (OrderPays != null)
            {
                foreach (var orderPay in OrderPays)
                {
                    orderPay.Order = this;
                    orderPay.SaveType = saveType;
                }
            }
            if (OrderInvoices != null)
            {
                foreach (var orderInvoice in OrderInvoices)
                {
                    orderInvoice.Order = this;
                    orderInvoice.SaveType = saveType;
                }
            }
            if (OrderProducts != null)
            {
                foreach (var orderProduct in OrderProducts)
                {
                    orderProduct.Order = this;
                    orderProduct.SaveType = saveType;
                }
            }
        }
        #endregion



        #endregion




    }

}
