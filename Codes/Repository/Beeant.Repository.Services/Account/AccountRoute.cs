using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Component.Extension;
using Winner.Persistence;
using Winner.Persistence.Route;

namespace Beeant.Repository.Services.Account
{
    public class AccountRoute
    {
      

        /// <summary>
        /// 写分片
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ShardingInfo GetSaveSharding(object value)
        {
            if (value == null)
                return null;
            var dataBaseIndex = GetSaveDataBaseIndex(value);
            var tableIndex = GetSaveTableIndex(value);
            if (string.IsNullOrWhiteSpace(tableIndex))
                return null;
            var sharding = new ShardingInfo
            {
                SetDataBase = string.Format("BeeantAccountWrite{0}", dataBaseIndex),
                TableIndex = tableIndex
            };
            return sharding;
        }

        /// <summary>
        /// 得到查询分片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IList<ShardingInfo> GetQuerySharding(QueryInfo query)
        {
            var names = query.GetRouteParameters(query.WhereExp);
            if (names == null)
                return null;
            var tableIndexs = GetQueryTableIndex(names);
            if (tableIndexs == null)
            {
                return null;
            }
            var dataBaseIndex = GetQueryDataBaseIndex(names);
            var getDataBase = string.Format("BeeantAccountRead{0}", dataBaseIndex);
            var result=new List<ShardingInfo>();
            foreach (var tableIndex in tableIndexs)
            {
                result.Add(new ShardingInfo
                {
                    GetDataBase = getDataBase,
                    TableIndex = tableIndex
                });
            }
            return result;
        }

  
        /// <summary>
        /// 得到数据库
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        protected virtual string GetQueryDataBaseIndex(IDictionary<string, IList<object>> names)
        {
            return null;
        }
        private static DateTime StartDate = new DateTime(2017, 1, 1);

        /// <summary>
        /// 得到存储索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string GetSaveTableIndex(object value)
        {
            var entity = value as AccountEntity;
            if (entity == null)
                return null;
            if (entity.SaveType == SaveType.Add)
                return ((entity.InsertTime.Date - StartDate).TotalDays%100).ToString();
            return null;
        }

        /// <summary>
        /// 得到存储索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string GetSaveDataBaseIndex(object value)
        {
            return null;
        }
        /// <summary>
        /// 得到存储索引
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        protected virtual IList<string> GetQueryTableIndex(IDictionary<string, IList<object>> names)
        {
            if (names.ContainsKey("InsertTime") && names["InsertTime"] != null && names["InsertTime"].Count != 0)
            {
                var startDate = names["InsertTime"][0].Convert<DateTime>();
                var endDate = names["InsertTime"].Count > 1 ? names["InsertTime"][1].Convert<DateTime>() : startDate;
                if (startDate < DateTime.Now.Date || startDate >= endDate)
                    return null;
                var index = (startDate.Date - StartDate).TotalDays % 100;
                var count = (endDate - startDate).TotalDays;
                var tableIndexs = new List<string>();
                for (int i = 0; i <= count; i++)
                {
                    tableIndexs.Add(index.ToString());
                    index++;
                }
                return tableIndexs;
            }
            return null;
        }
   
    }
}
