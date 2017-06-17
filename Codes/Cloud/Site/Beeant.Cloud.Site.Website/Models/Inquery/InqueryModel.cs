namespace Beeant.Cloud.Site.Website.Models.Inquery
{
    public class InqueryModel
    {
        /// <summary>
        /// 询盘信息
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 联系号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// 是否显示验证码
        /// </summary>
        public bool IsShowCode { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

    }
}
