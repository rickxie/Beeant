using System;

namespace Winner.Persistence.Route
{
    [Serializable]
    public class ShardPropertyInfo
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 开始索引
        /// </summary>
        public long StartValue { get; set; }
        /// <summary>
        /// 结束索引
        /// </summary>
        public long EndValue{ get; set; }
        /// <summary>
        /// 固定值
        /// </summary>
        public string FixedValue { get; set; }
        /// <summary>
        /// 固定格式
        /// </summary>
        public string DateFormat { get; set; }
        /// <summary>
        /// 自动类型
        /// </summary>
        public ShardingType ShardingType { get; set; }

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        public virtual string GetTableIndex(object routeValue, ShardingInfo sharding)
        {
            if (string.IsNullOrEmpty(sharding.TableIndex))
                GetFixedTableIndex(routeValue, sharding);
            routeValue = GetRouteValue(routeValue, sharding);
            if (routeValue == null)
                return null;
            var tableIndex= ShardingType == 0 || ShardingType == ShardingType.Fixed?
                GetFixedTableIndex(routeValue, sharding): GetAutoTableIndex(routeValue, sharding);
            return string.Format("{0}{1}", sharding.TableTag, tableIndex);
        }

        /// <summary>
        /// 得到查询数据库
        /// </summary>
        public virtual string GetQueryDataBaseIndex(object routeValue, ShardingInfo sharding)
        {
            if (string.IsNullOrEmpty(sharding.TableIndex))
                GetFixedTableIndex(routeValue, sharding);
            var step = GetTableStep(routeValue, sharding);
            return sharding.GetQueryDataBaseIndex(step);
        }
        /// <summary>
        /// 得到存储数据库
        /// </summary>
        public virtual string GetSaveDataBaseIndex(object routeValue, ShardingInfo sharding)
        {
            if (string.IsNullOrEmpty(sharding.TableIndex))
                GetFixedTableIndex(routeValue, sharding);
            var step = GetTableStep(routeValue, sharding);
            return sharding.GetSaveDataBaseIndex(step);
        }
        /// <summary>
        /// 得到路由值
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual object GetRouteValue(object routeValue, ShardingInfo sharding)
        {
            if (!string.IsNullOrEmpty(Tag) &&
                 !routeValue.ToString().ToLower().Contains(Tag.ToLower()))
                return null;
            routeValue = string.IsNullOrEmpty(Tag)
                ? routeValue
                : routeValue.ToString().ToLower().Replace(Tag.ToLower(), "");
            return routeValue;

        }
        /// <summary>
        /// 得到表索引
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual int GetTableStep(object routeValue, ShardingInfo sharding)
        {
            var step = -1;
            switch (ShardingType)
            {
                case ShardingType.Value:
                    step = GetValueTableStep(routeValue, sharding); break;
                case ShardingType.Remainder:
                    step = GetRemainderTableStep(routeValue, sharding); break;
                case ShardingType.Hour:
                    step = GetHourTableStep(routeValue, sharding); break;
                case ShardingType.Day:
                    step = GetDayTableStep(routeValue, sharding); break;
                case ShardingType.Month:
                    step = GetMonthTableStep(routeValue, sharding); break;
                case ShardingType.Year:
                    step = GetYearTableStep(routeValue, sharding); break;
            }
            return step;
        }
        /// <summary>
        /// 匹配正常
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual string GetAutoTableIndex(object routeValue, ShardingInfo sharding)
        {
            var step = GetTableStep(routeValue,sharding);
            if (step == -1)
                return null;
            switch (sharding.TableStepType)
            {
                case TableStepType.Value:
                    return step.ToString();
                case TableStepType.Hour:
                        return DateTime.Parse(sharding.TableIndex.Insert(4,"-").Insert(7,"-").Insert(10," ")).AddHours(step).ToString("yyyyMMddHH");
                case TableStepType.Day:
                    return DateTime.Parse(sharding.TableIndex.Insert(4, "-").Insert(7, "-")).AddDays(step).ToString("yyyyMMdd");
                case TableStepType.Month:
                    return DateTime.Parse(sharding.TableIndex.Insert(4, "-")).AddMonths(step).ToString("yyyyMM");
                case TableStepType.Year:
                    return DateTime.Parse(sharding.TableIndex).AddYears(step).ToString("yyyy");
            }
            return null;
        }

        /// <summary>
        /// 匹配正常
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual int GetRemainderTableStep(object routeValue, ShardingInfo sharding)
        {
            var tableMaxStep = sharding.GetTableMaxStep();
            if (tableMaxStep == -1)
                return -1;
            var value = Math.Abs(long.Parse(routeValue.ToString()));
            var step = (int)(value % tableMaxStep);
            return step;
        }

        /// <summary>
        /// 匹配正常
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual int GetValueTableStep(object routeValue, ShardingInfo sharding)
        {
            var tableMaxStep = sharding.GetTableMaxStep();
            if (tableMaxStep == -1)
                return -1;
            var value = Math.Abs(long.Parse(routeValue.ToString()));
            if (value > StartValue+(EndValue - StartValue + 1)*tableMaxStep)
                return -1;
            var step = (int)(Math.Floor((double)((value- StartValue) / (EndValue - StartValue))));
            if (sharding.TableStep > 0)
                step = step / sharding.TableStep;
            return step;
        }
        /// <summary>
        /// 匹配正常
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual int GetHourTableStep(object routeValue, ShardingInfo sharding)
        {
            var tableMaxStep = sharding.GetTableMaxStep();
            if (tableMaxStep == -1)
                return -1;
            var date = DateTime.Parse(routeValue.ToString());
            var maxDate = DateTime.Parse(sharding.TableIndex.Insert(4, "-").Insert(7, "-").Insert(10, " ")).AddHours(tableMaxStep);
            if (date > maxDate)
                return -1;
            var curdate = DateTime.Parse(FixedValue);
            var step = (date - curdate).TotalHours;
            if (sharding.TableStep > 0)
                step = step / sharding.TableStep;
            return (int)step;
        }
        /// <summary>
        /// 匹配正常
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual int GetDayTableStep(object routeValue, ShardingInfo sharding)
        {
            var tableMaxStep = sharding.GetTableMaxStep();
            if (tableMaxStep == -1)
                return -1;
            var date = DateTime.Parse(routeValue.ToString());
            var maxDate = DateTime.Parse(sharding.TableIndex.Insert(4, "-").Insert(7, "-")).AddDays(tableMaxStep);
            if (date > maxDate)
                return -1;
            var curdate = DateTime.Parse(FixedValue);
            var step = (date - curdate).TotalDays;
            if (sharding.TableStep > 0)
                step = step / sharding.TableStep;
            return (int)step;
        }
        /// <summary>
        /// 匹配正常
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual int GetMonthTableStep(object routeValue, ShardingInfo sharding)
        {
            var tableMaxStep = sharding.GetTableMaxStep();
            if (tableMaxStep == -1)
                return -1;
            var date = DateTime.Parse(routeValue.ToString());
            var maxDate = DateTime.Parse(sharding.TableIndex.Insert(4, "-")).AddMonths(tableMaxStep);
            if (date > maxDate)
                return -1;
            var curdate = DateTime.Parse(FixedValue);
            var step = (date.Year - curdate.Year) * 12 + (date.Month - curdate.Month);
            if (sharding.TableStep > 0)
                step = step / sharding.TableStep;
            return step;
        }
        /// <summary>
        /// 匹配正常
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual int GetYearTableStep(object routeValue, ShardingInfo sharding)
        {
            var tableMaxStep = sharding.GetTableMaxStep();
            if (tableMaxStep == -1)
                return -1;
            var date = DateTime.Parse(routeValue.ToString());
            var maxDate = DateTime.Parse(sharding.TableIndex).AddYears(tableMaxStep);
            if (date > maxDate)
                return -1;
            var curdate = DateTime.Parse(FixedValue);
            var step = date.Year - curdate.Year;
            if (sharding.TableStep > 0)
                step = step / sharding.TableStep;
            return step;
        }
        /// <summary>
        /// 匹配正常
        /// </summary>
        /// <param name="routeValue"></param>
        /// <param name="sharding"></param>
        /// <returns></returns>
        protected virtual string GetFixedTableIndex(object routeValue, ShardingInfo sharding)
        {
            if (StartValue > 0 && EndValue > 0)
            {
                var value = Math.Abs(long.Parse(routeValue.ToString()));
                if (value >= StartValue && value <= EndValue)
                {
                    return sharding.TableIndex;
                }
            }
            else if (!string.IsNullOrEmpty(FixedValue))
            {
                var value = string.IsNullOrEmpty(DateFormat)
                    ? routeValue.ToString()
                    : DateTime.Parse(routeValue.ToString()).ToString(DateFormat);
                if (value == FixedValue)
                    return sharding.TableIndex;
            }
            return null;
        }
    }
}
