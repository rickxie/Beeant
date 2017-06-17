using System.Collections.Generic;
using WordEntity = Beeant.Domain.Entities.Search.WordEntity;

namespace Beeant.Application.Services.Search
{
    public class WordApplicationService : RealizeApplicationService<WordEntity> 
    {
        static protected readonly object Locker=new object();

        /// <summary>
        /// 重写Save
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<WordEntity> infos)
        {
            lock (Locker)
            {
                return base.Save(infos);
            }
        }

 

      
    }
}
