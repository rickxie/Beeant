using Beeant.Domain.Entities.Promotion;

namespace Beeant.Presentation.Mobile.Detail.Models.Home
{
    public class PromotionModel
    {

        /// <summary>
        /// 活动
        /// </summary>
        public PromotionEntity Promotion { set; get; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public long ProductId { set; get; }
    }
}