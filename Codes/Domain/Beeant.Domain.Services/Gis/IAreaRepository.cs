using Beeant.Domain.Entities.Gis;

namespace Beeant.Domain.Services.Gis
{
    public interface IAreaRepository
    {
        /// <summary>
        /// 匹配区域
        /// </summary>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        MatchEntity Match(string city, string address, string tag);
    }
}
