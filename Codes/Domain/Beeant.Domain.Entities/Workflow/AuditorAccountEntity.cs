using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Workflow
{
    public class AuditorAccountEntity : BaseEntity<AuditorAccountEntity>
    {
        /// <summary>
        /// 用户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public AuditorEntity Auditor { get; set; }
    }
}
