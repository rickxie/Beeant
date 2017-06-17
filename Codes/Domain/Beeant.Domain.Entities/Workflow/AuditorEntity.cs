using System.Collections.Generic;

namespace Beeant.Domain.Entities.Workflow
{
    /// <summary>
    /// 组
    /// </summary>
    public class AuditorEntity : BaseEntity<AuditorEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 授权用户
        /// </summary>
        public IList<AuditorAccountEntity> AuditorAccounts { get; set; }

    }
}
