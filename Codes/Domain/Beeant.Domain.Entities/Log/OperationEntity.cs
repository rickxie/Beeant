using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Log
{
    [Serializable]
    public class OperationEntity : BaseEntity<OperationEntity>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 控件
        /// </summary>
        public string Control { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 错误页面
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string Device { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public AccountEntity Account { get; set; }

    }
}
