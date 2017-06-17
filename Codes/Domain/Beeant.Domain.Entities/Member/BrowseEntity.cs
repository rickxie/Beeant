using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;

namespace Beeant.Domain.Entities.Member
{
    public class BrowseEntity : BaseEntity<BrowseEntity>
    {
        /// <summary>
        /// 所属账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        public ProductEntity Product { get; set; }

 
       
    }
}
