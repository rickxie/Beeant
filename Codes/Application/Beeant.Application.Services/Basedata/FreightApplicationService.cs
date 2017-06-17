using System.Collections.Generic;
using Beeant.Domain.Entities.Basedata;
using Winner.Persistence;

namespace Beeant.Application.Services.Basedata
{
    public class FreightApplicationService : RealizeApplicationService<FreightEntity> 
    {
        static protected readonly object Locker=new object();


        /// <summary>
        /// 得到事务接口
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override IList<IUnitofwork> Handle(IList<FreightEntity> infos)
        {
            foreach (var info in infos)
            {
                SetFreightCarries(info);
            }
            return base.Handle(infos);
        }

        #region 设置类型


        /// <summary>
        /// 设置价格
        /// </summary>
        protected virtual void SetFreightCarries(FreightEntity info)
        {
            if (info.Carries == null || info.Carries.Count == 0)
                return;
            foreach (var carry in info.Carries)
            {
                if (info.SaveType == SaveType.Add)
                {
                    carry.SaveType = SaveType.Add;
                    carry.Id = 0;
                }
                else if(carry.Id>0)
                {
                    carry.SaveType = SaveType.Modify;
                }
            }
        }

        

        #endregion
    }
}
