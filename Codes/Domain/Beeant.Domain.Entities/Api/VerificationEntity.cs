using Winner;
using Winner.Filter;

namespace Beeant.Domain.Entities.Api
{
    public class VerificationEntity
    {
        /// <summary>
        /// 凭证用户
        /// </summary>
        public VoucherEntity Voucher { get; set; }
        /// <summary>
        /// 得到协议名称
        /// </summary>
        public ProtocolEntity Protocol { get; set; }
        /// <summary>
        /// 得到协议名称
        /// </summary>
        public VoucherProtocolEntity VoucherProtocol { get; set; }
        /// <summary>
        /// 错误
        /// </summary>
        public ErrorInfo Error { get; set; }
        /// <summary>
        /// 是否验证
        /// </summary>
        public bool IsPass
        {
            get { return Error == null; }
        }
        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsLog
        {
            get
            {
                if (VoucherProtocol != null)
                    return VoucherProtocol.IsLog;
                if (Voucher != null)
                    return Voucher.IsLog;
                if (Protocol != null)
                    return Protocol.IsLog;
                return false;
            }
        }
        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="args"></param>
        public virtual void SetError(string code, params object[] args)
        {
            Error = Creator.Get<IValidation>().GetErrorInfo(typeof (VerificationEntity).FullName, code);
            if (args != null && args.Length > 0)
                Error.Message = string.Format(Error.Message, args);
        }
    }
}
