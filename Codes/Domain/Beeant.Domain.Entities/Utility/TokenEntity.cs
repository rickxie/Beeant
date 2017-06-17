namespace Beeant.Domain.Entities.Utility
{
    public class TokenEntity
    {

        /// <summary>
        /// 凭据
        /// </summary>
        public string Ticket { get; set; }
        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public int TimeOut { get; set; }
    }
}
