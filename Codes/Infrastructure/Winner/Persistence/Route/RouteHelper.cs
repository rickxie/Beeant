using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Winner.Persistence.Route
{
    public static class RouteHelper 
    {

        #region 得到路由参数
        /// <summary>
        /// 连接
        /// </summary>
        private const string ConnectorPattern = @"\&\&|\|\|";
        /// <summary>
        /// as
        /// </summary>
        private const string PropertyPattern = @"@?\w+(\s*\.\s*\w+)*";
        /// <summary>
        /// 双引号
        /// </summary>
        private const string QuotationPattern = "\"[^\"]+\\\"[^\\\"]*(((?'Open'\\\")[^\\\"]*)+((?'-Open'\\\")[^\\\"]*)+)*(?(Open)(?!))\\\"+\"|\"[^\"]+\"";
        /// <summary>
        /// 数字
        /// </summary>
        private const string NumberPattern = @"[^a-zA-Z_]\d+";
        /// <summary>
        /// 数字
        /// </summary>
        private const string Pattern = "(" + PropertyPattern + ")|(" + ConnectorPattern+")";
        /// <summary>
        /// 得到路由参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="whereExp"></param>
        /// <returns></returns>
        static public IDictionary<string,IList<object>> GetRouteParameters(this ParameterInfo parameter, string whereExp)
        {
            var result = new Dictionary<string, IList<object>>();
            var tempParamter = new Dictionary<string, object>();
            whereExp = FillterQuotation(whereExp, tempParamter);
            whereExp = FillterNumber(whereExp, tempParamter);
            string propertyName =null;
            string parameterName = null;
            var match = Regex.Match(whereExp, Pattern);
            while (match.Length > 0)
            {
                if (Regex.IsMatch(match.Value, ConnectorPattern))
                {
                    AppendResult(result, parameter, tempParamter, propertyName, parameterName);
                    propertyName = null;
                    parameterName = null;
                }
                else
                {
                    if (match.Value.Contains("@"))
                    {
                        parameterName = match.Value;
                        if (parameterName.IndexOf(".") > -1)
                        {
                            parameterName = parameterName.Substring(0, parameterName.IndexOf("."));
                        }
                    }
                    else
                        propertyName = match.Value;
                }
                match = match.NextMatch();
            }
            AppendResult(result, parameter, tempParamter, propertyName, parameterName);
            return result;
        }

        /// <summary>
        /// 添加结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parameter"></param>
        /// <param name="tempParamter"></param>
        /// <param name="propertyName"></param>
        /// <param name="parameterName"></param>
        private static void AppendResult(IDictionary<string, IList<object>> result, ParameterInfo parameter, IDictionary<string, object> tempParamter,
                                         string propertyName, string parameterName)
        {
            if (!string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(parameterName))
            {
                var pName = parameterName.Replace("@", "");
                if (!result.ContainsKey(propertyName))
                    result.Add(propertyName,new List<object>());
                if (tempParamter.ContainsKey(pName))
                    result[propertyName].Add(tempParamter[pName]);
                else if (parameter.Parameters != null && parameter.Parameters.ContainsKey(pName))
                    result[propertyName].Add(parameter.Parameters[pName]);
            }
        }
        /// <summary>
        /// 替换引号
        /// </summary>
        /// <param name="whereExp"></param>
        /// <param name="tempPamater"></param>
        /// <returns></returns>
        private static string FillterQuotation(string whereExp, IDictionary<string,object> tempPamater )
        {
            var builder = new StringBuilder();
            var match = Regex.Match(whereExp, QuotationPattern);
            while (match.Length > 0)
            {
                var name = string.Format("_RouteTempP{0}", tempPamater.Count + 1);
                tempPamater.Add(name, match.Value);
                builder.Append(whereExp.Substring(0, match.Index));
                builder.AppendFormat("@{0}", name);
                builder.Append(whereExp.Substring(match.Index + match.Length + 1,whereExp.Length - match.Index - match.Length - 1));
                match = match.NextMatch();
            }
            if (builder.Length > 0) return builder.ToString();
            return whereExp;
        }
        /// <summary>
        /// 替换数字
        /// </summary>
        /// <param name="whereExp"></param>
        /// <param name="tempPamater"></param>
        /// <returns></returns>
        private static string FillterNumber(string whereExp, IDictionary<string, object> tempPamater)
        {
            var builder = new StringBuilder();
            var match = Regex.Match(whereExp, NumberPattern);
            while (match.Length > 0)
            {
                var name = string.Format("P{0}", tempPamater.Count + 1);
                tempPamater.Add(name, match.Value);
                builder.Append(whereExp.Substring(0, match.Index));
                builder.AppendFormat("@{0}", name);
                builder.Append(whereExp.Substring(match.Index + match.Length + 1, whereExp.Length - match.Index - match.Length - 1));
                match = match.NextMatch();
            }
            if (builder.Length > 0) return builder.ToString();
            return whereExp;
        }
        #endregion


    }
}
