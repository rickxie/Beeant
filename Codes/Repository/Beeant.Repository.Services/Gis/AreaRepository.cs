using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Configuration;
using Beeant.Domain.Entities.Gis;
using Beeant.Domain.Services.Gis;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Repository.Services.Gis
{
    public class AreaRepository : IAreaRepository
    {
        /// <summary>
        /// 匹配区域
        /// </summary>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public virtual MatchEntity Match(string city, string address,string tag)
        {
            if (string.IsNullOrWhiteSpace(address))
                return GetMatch("",city, tag,0,0);
            var result = GetAddressResult(city, address, tag);
            if (result == null)
               return GetBaiduResult(city, address, tag);
            return result;

        }


        #region 得到百度结果
        /// <summary>
        /// 输出百度结果
        /// </summary>
        protected virtual MatchEntity GetBaiduResult(string city, string address, string tag)
        {
            try
            {
                var json = GetBaiduApiResult(city, address);
                if (json != null && json.Count != 0)
                {
                    return GetMatch(city, "Baidu", tag, json["lng"], json["lat"]);
                }
                return null;
            }
            catch(Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return null;

        }

        /// <summary>
        /// 得到百度接口结果
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, double> GetBaiduApiResult(string city, string address)
        {
            var map = ConfigurationManager.GetSetting<string>("BaiduMap").DeserializeJson<dynamic>();
            var result = new Dictionary<string, double>();
            string searchUrl =
                string.Format("{0}/place/v2/search?ak={1}&output=json&q={2}&region={3}", map.Url.ToString(), map.Ak.ToString(), address, city);
            string value = Winner.Creator.Get<Winner.Base.IComponent>().GetPageContent(searchUrl, "utf-8");
            var json = value.DeserializeJson<dynamic>();
            if (json == null || json.results == null || json.results.Count == 0)
            {
                string geocoderUrl =
                    string.Format("{0}/geocoder/v2/?ak={1}&output=json&address={2}&city={3}", map.Url.ToString(), map.Ak.ToString(), address, city);
                value = Winner.Creator.Get<Winner.Base.IComponent>().GetPageContent(geocoderUrl, "utf-8");
                json = value.DeserializeJson<dynamic>();
                if (json != null && json.result != null && json.result.location != null &&
                    json.result.location.lat != null && json.result.location.lng != null)
                {
                    result.Add("lat", (double)json.result.location.lat);
                    result.Add("lng", (double)json.result.location.lng);
                }

            }
            else if (json.results[0].location != null && json.results[0].location.lat != null && json.results[0].location.lng != null)
            {

                result.Add("lat", (double)json.results[0].location.lat);
                result.Add("lng", (double)json.results[0].location.lng);
            }
            return result;
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 得到区域
        /// </summary>
        /// <returns></returns>
        protected virtual IList<AreaEntity> GetEntities()
        {
            var query = new QueryInfo();
            query.SetCacheName("地图缓存").SetCacheTime(DateTime.Now.AddHours(5)).Query<AreaEntity>().Where(it => it.IsUsed).Select(it => new object[] { it.Name, it.Path,it.City, it.Tag, it.Type, it.Value });
            var infos = Winner.Creator.Get<IContext>().GetInfos<List<AreaEntity>>(query);
            return infos;
        }

        /// <summary>
        /// 得到区域
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <param name="city"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual MatchEntity GetMatch(string city,string type, string tag, double lng, double lat)
        {
            var match = new MatchEntity
            {
                Areas = new List<AreaEntity>(),
                Lat = lat,
                Lng = lng
            };
            var infos = GetEntities();
            if (infos != null)
            {
                foreach (var info in infos)
                {
                    if ((string.IsNullOrEmpty(tag) || info.Tag == tag) && 
                        (string.IsNullOrEmpty(type) || info.Type == type) &&
                        (string.IsNullOrEmpty(city) || info.City == city) &&
                        (lng==0 || lat==0 || info.CheckPoint(lng, lat)))
                    {
                        match.Areas.Add(info);
                    }
                }

            }
            return match;
        }
        /// <summary>
        /// 得到私有地址库
        /// </summary>
        /// <returns></returns>
        protected virtual MatchEntity GetAddressResult(string city, string address, string tag)
        {
            var info = GetAddress(address);
            if (info == null)
                return null;
            var json = info.Point.DeserializeJson<dynamic>(); 
            return GetMatch(city, "", tag, (double)json.lng, (double)json.lat);
        }
        /// <summary>
        /// 得到地址信息
        /// </summary>
        /// <returns></returns>
        protected virtual AddressEntity GetAddress(string address)
        {
            var query = new QueryInfo();
            query.SetPageSize(1).Query<AddressEntity>()
                .Where(it => it.Name == address || (it.IsStartWith && address.StartsWith(it.Name)))
                .Select(it => it.Point);
            var infos = Winner.Creator.Get<IContext>().GetInfos<List<AddressEntity>>(query);
            return infos?.FirstOrDefault();
        }
  

        #endregion
    }
}
