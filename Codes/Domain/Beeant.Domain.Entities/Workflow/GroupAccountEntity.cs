using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Workflow
{
    public class GroupAccountEntity : BaseEntity<GroupAccountEntity>
    {
        /// <summary>
        /// 用户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 组
        /// </summary>
        public GroupEntity Group { get; set; }
    }
}
