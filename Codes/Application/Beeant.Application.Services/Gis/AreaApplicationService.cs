using Beeant.Domain.Entities.Gis;
using Beeant.Domain.Services.Gis;
 

namespace Beeant.Application.Services.Gis
{
    public class DepositApplicationService : IAreaApplicationService
    {
        public IAreaRepository AreaRepository { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public virtual MatchEntity Match(string city, string address, string tag)
        {
            return AreaRepository.Match(city, address, tag);
        }
    }
}
