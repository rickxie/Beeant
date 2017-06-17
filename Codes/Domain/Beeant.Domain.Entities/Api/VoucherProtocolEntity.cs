namespace Beeant.Domain.Entities.Api
{
    public class VoucherProtocolEntity : BaseEntity<VoucherProtocolEntity>
    {
        /// <summary>
        /// 凭证 
        /// </summary>
        public VoucherEntity Voucher { get; set; }
        /// <summary>
        /// 协议
        /// </summary>
        public ProtocolEntity Protocol { get; set; }
        /// <summary>
        /// 单秒请求数 
        /// </summary>
        public int SecondCount { get; set; }
        /// <summary>
        /// 单天请求数 
        /// </summary>
        public long DayCount { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Args { get; set; }
        /// <summary>
        /// 是否禁止 
        /// </summary>
        public bool IsForbid { get; set; }
        /// <summary>
        /// 是否签名
        /// </summary>
        public bool IsSign { get; set; }
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsLog { get; set; }
        /// <summary>
        /// 是否禁止名称
        /// </summary>
        public string IsForbidName
        {
            get { return this.GetStatusName(IsForbid); }
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
