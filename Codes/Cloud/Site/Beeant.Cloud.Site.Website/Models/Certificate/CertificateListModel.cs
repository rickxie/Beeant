using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;


namespace Beeant.Cloud.Site.Website.Models.Certificate
{
    public class CertificateListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 24; }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public IList<CertificateEntity> Certificaties { get; set; }
    }
}
