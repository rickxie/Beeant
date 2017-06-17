using System.Collections.Generic;

namespace Beeant.Domain.Entities.Api
{
    public class ProtocolEntity : BaseEntity<ProtocolEntity>
    {
        /// <summary>
        /// 接口名称 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 别名 
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 描述 
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 是否验证 
        /// </summary>
        public bool IsVerify { get; set; }
        /// <summary>
        /// 是否启用 
        /// </summary>
        public bool IsStart { get; set; }
        /// <summary>
        /// 单秒请求数 
        /// </summary>
        public int SecondCount { get; set; }
        /// <summary>
        /// 单天请求数 
        /// </summary>
        public long DayCount { get; set; }
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsLog { get; set; }
        /// <summary>
        /// 是否签名
        /// </summary>
        public bool IsSign { get; set; }
        /// <summary>
        /// 限制协议
        /// </summary>
        public IList<VoucherProtocolEntity> VoucherProtocols { get; set; }
        /// <summary>
        /// 是否验证名称
        /// </summary>
        public string IsVerifyName
        {
            get { return this.GetVerifyName(IsVerify); }
        }
        /// <summary>
        /// 是否禁止名称
        /// </summary>
        public string IsStartName
        {
            get { return this.GetStatusName(IsStart); }
        }
        /// <summary>
        /// 是否禁止名称
        /// </summary>
        public string IsLogName
        {
            get { return this.GetStatusName(IsLog); }
        }
    }
}
