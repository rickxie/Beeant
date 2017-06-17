namespace Beeant.Presentation.Website.Buy.Models.Member
{
    public class AddressModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string County { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// 接收地址
        /// </summary>
        public string Address { get; set; }
    }
}