using System;
using System.Collections.Generic;

namespace Winner.Persistence.Route
{
    [Serializable]
    public class ShardingInfo
    {
        /// <summary>
        /// 读取数据库表数量
        /// </summary>
        public int GetDataBaseTableCount { get; set; }
        /// <summary>
        /// 写取数据库表数量
        /// </summary>
        public int SetDataBaseTableCount { get; set; }
        /// <summary>
        /// 表标签
        /// </summary>
        public string TableTag { get; set; }
        /// <summary>
        /// 表的数量
        /// </summary>
        public string TableIndex { get; set; }
        /// <summary>
        /// 步长
        /// </summary>
        public string MaxTableIndex { get; set; }
        /// <summary>
        /// 读库
        /// </summary>
        public string GetDataBase { get; set; }
        /// <summary>
        /// 写库
        /// </summary>
        public string SetDataBase { get; set; }
        /// <summary>
        /// 是否可以写
        /// </summary>
        public bool IsWrite { get; set; }
        /// <summary>
        /// 步长
        /// </summary>
        public int TableStep { get; set; }
        /// <summary>
        ///   
        /// </summary>
        public TableStepType TableStepType { get; set; }
        /// <summary>
        /// 规则
        /// </summary>
        public IList<ShardPropertyInfo> ShardProperties { get; set; }
        /// <summary>
        /// 得到最大步长
        /// </summary>
        /// <returns></returns>
        public virtual int GetTableMaxStep()
        {
            var maxTableIndex = GetMaxTableIndex();
            switch (TableStepType)
            {
                case TableStepType.Value:
                    return int.Parse(maxTableIndex);
                case TableStepType.Hour:
                    {
                        var maxDate = DateTime.Parse(maxTableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
                        var curDate = DateTime.Parse(TableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
                        return (int)(maxDate - curDate).TotalHours+1;
                    }
                case TableStepType.Day:
                    {
                        var maxDate = DateTime.Parse(maxTableIndex.Insert(4, "-").Insert(7, "-"));
                        var curDate = DateTime.Parse(TableIndex.Insert(4, "-").Insert(7, "-"));
                        return (int)(maxDate - curDate).TotalDays;
                    }
                case TableStepType.Month:
                    {
                        var maxDate = DateTime.Parse(maxTableIndex.Insert(4, "-"));
                        var curDate = DateTime.Parse(TableIndex.Insert(4, "-"));
                        var step = (maxDate.Year - curDate.Year) * 12 + (maxDate.Month - curDate.Month);
                        return step+1;
                    }
                case TableStepType.Year:
                    {
                        var maxDate = DateTime.Parse(maxTableIndex.Insert(4, "-"));
                        var curDate = DateTime.Parse(TableIndex.Insert(4, "-"));
                        return maxDate.Year - curDate.Year+1;
                    }
            }
            return -1;
        }
        /// <summary>
        ///  得到表最大索引
        /// </summary>
        /// <returns></returns>
        public virtual string GetMaxTableIndex()
        {
            if (!string.IsNullOrEmpty(MaxTableIndex))
                return MaxTableIndex;
            switch (TableStepType)
            {
                case TableStepType.Hour:
                    return DateTime.Now.ToString("yyyyMMddHH");
                case TableStepType.Day:
                    return DateTime.Now.ToString("yyyyMMdd");
                case TableStepType.Month:
                    return DateTime.Now.ToString("yyyyMM");
                case TableStepType.Year:
                    return DateTime.Now.ToString("yyyy");
            }
            return MaxTableIndex;
        }

        #region 得到所有分片
        /// <summary>
        ///  得到所有分片
        /// </summary>
        /// <returns></returns>
        public virtual IList<ShardingInfo> GetAllShardings()
        {
            if (string.IsNullOrEmpty(TableIndex))
            {
                var sharding = new ShardingInfo
                {
                    GetDataBase = GetDataBase,
                    SetDataBase = SetDataBase,
                    IsWrite = IsWrite,
                    MaxTableIndex = MaxTableIndex,
                    TableStep = TableStep,
                    TableIndex="",
                    TableStepType = TableStepType
                };
                return new List<ShardingInfo> { sharding };
            }
            switch (TableStepType)
            {
                case TableStepType.Value:
                    return GetAllValueShardings();
                case TableStepType.Hour:
                    return GetAllHourShardings();
                case TableStepType.Day:
                    return GetAllDayShardings();
                case TableStepType.Month:
                    return GetAllMonthShardings();
                case TableStepType.Year:
                    return GetAllYearShardings();
            }
            return null;
        }

        /// <summary>
        /// 得到值得所以分片
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ShardingInfo> GetAllValueShardings()
        {
            var maxTableIndex = GetMaxTableIndex();
            var shardings = new List<ShardingInfo>();
            var max = int.Parse(maxTableIndex);
            for (int i = int.Parse(TableIndex); i < max;)
            {
                var sharding = new ShardingInfo
                {
                    GetDataBase = GetQueryDataBaseIndex(i),
                    SetDataBase = GetSaveDataBaseIndex(i),
                    IsWrite = IsWrite,
                    MaxTableIndex = MaxTableIndex,
                    TableStep = TableStep,
                    TableStepType = TableStepType
                };
                sharding.TableIndex = i.ToString();
                i += TableStep;
                shardings.Add(sharding);
            }
            return shardings;
        }
        /// <summary>
        /// 得到 小时得所以分片
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ShardingInfo> GetAllHourShardings()
        {
            var maxTableIndex = GetMaxTableIndex();
            var shardings = new List<ShardingInfo>();
            var maxDate = DateTime.Parse(maxTableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
            var curDate = DateTime.Parse(TableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
            var i = 1;
            while (curDate < maxDate)
            {
                var sharding = new ShardingInfo
                {
                    GetDataBase = GetQueryDataBaseIndex(i),
                    SetDataBase = GetSaveDataBaseIndex(i),
                    IsWrite = IsWrite,
                    MaxTableIndex = MaxTableIndex,
                    TableStep = TableStep,
                    TableStepType = TableStepType
                };
                sharding.TableIndex = curDate.ToString("yyyyMMddHH");
                curDate = curDate.AddHours(TableStep);
                shardings.Add(sharding);
                i++;
            }
            return shardings;
        }
        /// <summary>
        /// 得到天 得所以分片
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ShardingInfo> GetAllDayShardings()
        {
            var maxTableIndex = GetMaxTableIndex();
            var shardings = new List<ShardingInfo>();
            var maxDate = DateTime.Parse(maxTableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
            var curDate = DateTime.Parse(TableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
            var i = 1;
            while (curDate < maxDate)
            {
                var sharding = new ShardingInfo
                {
                    GetDataBase = GetQueryDataBaseIndex(i),
                    SetDataBase = GetSaveDataBaseIndex(i),
                    IsWrite = IsWrite,
                    MaxTableIndex = MaxTableIndex,
                    TableStep = TableStep,
                    TableStepType = TableStepType
                };
                sharding.TableIndex = curDate.ToString("yyyyMMdd");
                curDate = curDate.AddDays(TableStep);
                shardings.Add(sharding);
                i++;
            }
            return shardings;
        }
        /// <summary>
        /// 得到月得所以分片
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ShardingInfo> GetAllMonthShardings()
        {
            var maxTableIndex = GetMaxTableIndex();
            var shardings = new List<ShardingInfo>();
            var maxDate = DateTime.Parse(maxTableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
            var curDate = DateTime.Parse(TableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
            var i = 1;
            while (curDate < maxDate)
            {
                var sharding = new ShardingInfo
                {
                    GetDataBase = GetQueryDataBaseIndex(i),
                    SetDataBase = GetSaveDataBaseIndex(i),
                    IsWrite = IsWrite,
                    MaxTableIndex = MaxTableIndex,
                    TableStep = TableStep,
                    TableStepType = TableStepType
                };
                sharding.TableIndex = curDate.ToString("yyyyMM");
                curDate = curDate.AddMonths(TableStep);
                shardings.Add(sharding);
                i++;
            }
            return shardings;
        }
        /// <summary>
        /// 得到年得所以分片
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ShardingInfo> GetAllYearShardings()
        {
            var maxTableIndex = GetMaxTableIndex();
            var shardings = new List<ShardingInfo>();
            var maxDate = DateTime.Parse(maxTableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
            var curDate = DateTime.Parse(TableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " "));
            var i = 1;
            while (curDate < maxDate)
            {
                var sharding = new ShardingInfo
                {
                    GetDataBase = GetQueryDataBaseIndex(i),
                    SetDataBase = GetSaveDataBaseIndex(i),
                    IsWrite = IsWrite,
                    MaxTableIndex = MaxTableIndex,
                    TableStep = TableStep,
                    TableStepType = TableStepType
                };
                sharding.TableIndex = curDate.ToString("yyyy");
                curDate = curDate.AddYears(TableStep);
                shardings.Add(sharding);
                i++;
            }
            return shardings;
        }

        /// <summary>
        /// 得到查询数据库
        /// </summary>
        public virtual string GetQueryDataBaseIndex(int step)
        {
            if (GetDataBaseTableCount == 0)
                return GetDataBase;
            if (step == -1 || step < GetDataBaseTableCount)
                return GetDataBase;
            var value = (double)(step / GetDataBaseTableCount);
            return string.Format("{0}{1}", GetDataBase, (int)Math.Floor(value));
        }
        /// <summary>
        /// 得到存储数据库
        /// </summary>
        public virtual string GetSaveDataBaseIndex(int step)
        {
            if (SetDataBaseTableCount == 0)
                return SetDataBase;
            if (step == -1 || step < SetDataBaseTableCount)
                return SetDataBase;
            var value = (double)(step / SetDataBaseTableCount);
            return string.Format("{0}{1}", SetDataBase, (int)Math.Floor(value));
        }
        #endregion
    }
}
