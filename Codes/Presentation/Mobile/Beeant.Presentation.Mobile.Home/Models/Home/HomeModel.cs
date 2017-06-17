using System.Collections.Generic;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Cms;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Mobile.Home.Models.Home
{
    public class HomeModel
    {
        /// <summary>
        /// 内容
        /// </summary>
        public IList<ContentEntity> Contents { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public IList<GoodsEntity> Goodses { get; set; }
        /// <summary>
        /// 代理
        /// </summary>
        public AgentEntity Agent { get; set; }
    }
}
