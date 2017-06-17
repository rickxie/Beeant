using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Promotion;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Promotion.Promotion
{
    public partial class Detail : DetailPageBase<PromotionEntity>
    {
        public int Index = 0;


        protected override PromotionEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<PromotionEntity>().Where(it => it.Id == RequestId);
                
            return Ioc.Resolve<IApplicationService, PromotionEntity>().GetEntities<PromotionEntity>(query).FirstOrDefault();

        }
    }
}