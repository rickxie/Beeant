using Beeant.Domain.Entities.Member;

namespace Beeant.Presentation.Mobile.Member.Models.Home
{
    public class HomeModel
    {
        /// <summary>
        /// 会员信息
        /// </summary>
        public  MemberEntity Member { get; set; }
        /// <summary>
        /// 消息数量
        /// </summary>
        public int MessageCount { get; set; }
    }
}