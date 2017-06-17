using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Models.Certificate
{
    public class CertificateListModel : PagerModel
    {
        public override int PageSize { get; set; } = 24;

        /// <summary>
        /// 类目
        /// </summary>
        public IList<CertificateEntity> Certificates { get; set; }
    }
}