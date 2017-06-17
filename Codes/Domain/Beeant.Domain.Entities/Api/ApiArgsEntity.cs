namespace Beeant.Domain.Entities.Api
{
    public class ApiArgsEntity
    {
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 凭证
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 协议
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 请求值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 加密
        /// </summary>
        public string Sign { get; set; }
    }
}
