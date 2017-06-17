using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Winner.Persistence.Relation;
using Winner.Persistence.Translation;

namespace Winner.Persistence.Compiler.Common
{
    public abstract class SelectCompiler : ISelectCompiler
    {
        #region 声明
        private const string BreakersPattern = @"\([^\(\)]*(((?'Open'\()[^\(\)]*)+((?'-Open'\))[^\(\)]*)+)*(?(Open)(?!))\)";
        /// <summary>
        /// as
        /// </summary>
        private const string AsPattern = @"\s+(AS|as|As|aS)\s+\w+(\s*\.\s*\w+)*(\s*,)?$";
        /// <summary>
        /// 逗号
        /// </summary>
        private const string CommaPatten = @"\s*,$";
        /// <summary>
        /// as
        /// </summary>
        private const string PropertyPattern = @"\w+(\s*\.\s*\w+)*";
        /// <summary>
        /// as
        /// </summary>
        private const string CommonPattern = @"\w+";
        /// <summary>
        /// 结束符号
        /// </summary>
        private const string EndPattern = @"((\s+(AS|as|As|aS)+\s+\w*($|,))|\s*,)?";
  
        /// <summary>
        /// 函数
        /// </summary>
        private const string MethodPattern = @"{0}\s*" + BreakersPattern + EndPattern;

        private const string SubQueryPattern =
            @"Take" + BreakersPattern + "|Skip" + BreakersPattern + "|Where" + BreakersPattern
            + "|OrderBy" + BreakersPattern + "|GroupBy" + BreakersPattern + "|Having" + BreakersPattern;
        /// <summary>
        /// 正则表达式
        /// </summary>
        private static readonly string Pattern =
            PropertyPattern + @"\s*\." + string.Format(MethodPattern, "Select") + @"|" +
            PropertyPattern + @"\s*\.Where\s*\(.*?\)\s*\." + string.Format(MethodPattern, "Select") + @"|" +
            ManyKeyPattern + @"|" +
            string.Format(MethodPattern, "Sum") + @"|" +
            string.Format(MethodPattern, "Count") + @"|" +
            string.Format(MethodPattern, "Min") + @"|" +
            string.Format(MethodPattern, "Max") + @"|" +
            string.Format(MethodPattern, "Average")
            + @"|\s*\*\s*($|,)|" + OperatorKeyPattern + @"|"
            + PropertyPattern + EndPattern;
        /// <summary>
        /// *匹配
        /// </summary>
        private const string AllKeyPattern = @"\s*\*\s*($|,$)";
        /// <summary>
        /// 一对多匹配
        /// </summary>
        private const string ManyKeyPattern = @"\w+.(\w+" + BreakersPattern + ".)*Select" + BreakersPattern;
        /// <summary>
        /// 方法匹配
        /// </summary>
        private const string MethodKeyPattern = @"\w+" + BreakersPattern;
        /// <summary>
        /// 属性匹配
        /// </summary>
        private const string PropertyKeyPattern = @"^" + PropertyPattern;
        /// <summary>
        /// 操作符匹配
        /// </summary>
        private const string OperatorKeyPattern =  @"\+|\-|\*|\/|\%|\(|\)";

        #endregion

        #region 接口的实现

        /// <summary>
        /// 解析查询
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <returns></returns>
        public void Translate(QueryCompilerInfo queryCompiler)
        {
            queryCompiler.Builder=new StringBuilder();
            if (string.IsNullOrEmpty(queryCompiler.Exp) || queryCompiler.Exp.Trim().Equals("*"))//from开始的
            {
                TranslateFromBegin(queryCompiler);
                return;
            }
            TranslateSelectBegin(queryCompiler);
            if (queryCompiler.Builder.Length > 0 && queryCompiler.Builder[queryCompiler.Builder.Length - 1] == ',')
                queryCompiler.Builder.Remove(queryCompiler.Builder.Length - 1, 1);
        }

        #endregion

        #region from开始的查询

        /// <summary>
        /// from开始
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <returns></returns>
        protected virtual void TranslateFromBegin(QueryCompilerInfo queryCompiler)
        {
            AppendFromProperty(queryCompiler,queryCompiler.Object, null, true);
            queryCompiler.Builder.Remove(queryCompiler.Builder.Length - 1, 1);
        }

        /// <summary>
        /// from开始
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="ormObject"></param>
        /// <param name="name"></param>
        /// <param name="isfromload"></param>
        protected virtual void AppendFromProperty(QueryCompilerInfo queryCompiler,OrmObjectInfo ormObject, string name, bool isfromload)
        {
            foreach (var p in ormObject.Properties)
            {
                var propertyName = string.IsNullOrEmpty(name) ? p.PropertyName : string.Format("{0}.{1}", name, p.PropertyName);
                SelectFromRead(queryCompiler, p, propertyName, isfromload);
            }
        }
        /// <summary>
        /// 选择From读取方式
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="property"></param>
        /// <param name="propertyName"></param>
        /// <param name="isfromload"></param>
        protected virtual void SelectFromRead(QueryCompilerInfo queryCompiler,OrmPropertyInfo property, 
            string propertyName, bool isfromload)
        {
            if (property.AllowRead && property.Map == null)
            {
                var asName = GetAsName(property, queryCompiler, propertyName);
                if(!string.IsNullOrEmpty(asName))
                    queryCompiler.Builder.Append(asName);
            }
            else if (property.Map != null && !property.Map.CheckRemote() && property.Map.IsGreedyLoad && queryCompiler.Query.IsGreedyLoad)
            {
                queryCompiler.AddJoin(property,propertyName);
                if (property.Map.MapType == OrmMapType.OneToMany)
                {
                    AppendManySql(queryCompiler,null, property,propertyName, null);
                    queryCompiler.Builder.AppendFormat(" as {0}_{1},", propertyName.Replace(".", "_"), queryCompiler.FieldCount);
                }
                else
                {
                    AppendFromProperty(queryCompiler,property.Map.GetMapObject(), string.Format("{0}.{1}", propertyName, property.PropertyName), false);
                }
            }
        }
        /// <summary>
        /// 转换属性到字段
        /// </summary>
        /// <param name="property"></param>
        /// <param name="queryCompiler"></param>
        /// <param name="propertyName"></param>
        protected virtual string GetAsName(OrmPropertyInfo property,
            QueryCompilerInfo queryCompiler, string propertyName)
        {
            if (!property.AllowRead) return null;
            var builder = new StringBuilder();
            builder.Append(queryCompiler.GetFieldName(property, propertyName));
            builder.Append(" as ");
            builder.AppendFormat("{0}_{1}",propertyName.Replace(".", "_"), queryCompiler.FieldCount);
            builder.Append(",");
            return builder.ToString();
        }
        #endregion

        #region select开始的查询

        /// <summary>
        /// select开始
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <returns></returns>
        protected virtual void TranslateSelectBegin(QueryCompilerInfo queryCompiler)
        {
            TranslateSelectBegin(queryCompiler, queryCompiler.Exp);
        }

        /// <summary>
        /// select开始
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected virtual void TranslateSelectBegin(QueryCompilerInfo queryCompiler,string exp)
        {
            var match = Regex.Match(exp, Pattern);
            while (match.Length > 0)
            {
                var rev = AppendSelectAllSql(queryCompiler, match) 
                          || AppendSelectManySql(queryCompiler, match)
                          || AppendSelectMethodSql(queryCompiler, match)
                          || AppendSelectPropertySql(queryCompiler, match);
                if (!rev)
                    queryCompiler.Builder.Append(match.Value);
                match = match.NextMatch();
            }
        }
        /// <summary>
        /// 匹配as
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="match"></param>
        /// <param name="propertyName"></param>
        protected virtual void AppendAsSql(QueryCompilerInfo queryCompiler, Match match,string propertyName)
        {
            var asMatch = Regex.Match(match.Value, AsPattern);
            if (asMatch.Length>0)
            {
                queryCompiler.Builder.Append(asMatch.Value);
                queryCompiler.Builder.AppendFormat("_{0}", queryCompiler.FieldCount);
                return;
            }
            if (!string.IsNullOrEmpty(propertyName) && (Regex.IsMatch(match.Value, CommaPatten) || match.NextMatch().Length == 0))
                queryCompiler.Builder.AppendFormat(" as {0}_{1}", propertyName.Replace(".", "_"), queryCompiler.FieldCount);
            if (Regex.IsMatch(match.Value, CommaPatten)) queryCompiler.Builder.Append(",");
        }

      

        /// <summary>
        /// 操作符
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        protected virtual bool AppendSelectAllSql(QueryCompilerInfo queryCompiler, Match match)
        {
            if (!Regex.IsMatch(match.Value, AllKeyPattern) || !(Regex.IsMatch(match.Value, CommaPatten) || match.NextMatch().Length == 0)) return false;
            AppendFromProperty(queryCompiler,queryCompiler.Object, null, true);
            if (queryCompiler.Builder.Length > 0)
                queryCompiler.Builder.Remove(queryCompiler.Builder.Length - 1, 1);
            AppendAsSql(queryCompiler, match, null);
            return true;
        }




        /// <summary>
        /// 得到属性开始的索引
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        protected virtual bool AppendSelectPropertySql(QueryCompilerInfo queryCompiler, Match match)
        {
            var m = Regex.Match(match.Value, PropertyKeyPattern);
            if (!m.Success) return false;
            var propertyName = m.Value.Trim().Trim(',');
            var chainProperties = queryCompiler.Object.GetChainProperties(propertyName);
            var property = chainProperties[chainProperties.Count - 1];
            if (property.Map != null)
            {
                var subSelect = propertyName;
                if (property.PropertyName.Equals(propertyName))
                {
                    subSelect = "*";
                }
                else
                {
                    var name = string.Format("{0}.", property.PropertyName);
                    var index = propertyName.IndexOf(name);
                    if (index >= 0)
                    {
                        subSelect = propertyName.Substring(index + name.Length);
                    }
                }
                var realPropertyName = propertyName.Replace(string.Format(".{0}", subSelect), "");
                if (FillRemoteQuery(queryCompiler, realPropertyName, property, subSelect,match))
                {
                    queryCompiler.AddJoins(chainProperties);
                    var builder = new StringBuilder();
                    for (int i = 0; i < chainProperties.Count-1; i++)
                    {
                        builder.AppendFormat("{0}.", chainProperties[i].PropertyName);
                    }
                    builder.Append(property.Map.ObjectProperty.PropertyName);
                    var name = queryCompiler.GetFieldName(property.Map.ObjectProperty, builder.ToString());
                    if (!queryCompiler.Builder.ToString().Contains(name))
                    {
                        queryCompiler.Builder.Append(name);
                        var bd=new StringBuilder();
                        for (int i = 0; i < chainProperties.Count-1; i++)
                        {
                            bd.AppendFormat("{0}_", chainProperties[i].PropertyName);
                        }
                        queryCompiler.Builder.AppendFormat(" as {0}{1}_{2},", bd,
                                                           property.Map.ObjectProperty.PropertyName.Replace(".", "_"),
                                                           queryCompiler.FieldCount);
                    }
                    return true;
                }
            }
            queryCompiler.AddJoins(chainProperties);
            if (property.Map != null)
            {
                AppendFromProperty(queryCompiler, property.Map.GetMapObject(), propertyName, true);
                if (queryCompiler.Builder.Length > 0)
                    queryCompiler.Builder.Remove(queryCompiler.Builder.Length - 1, 1);
                AppendAsSql(queryCompiler, match, null);
                return true;
            }
            queryCompiler.Builder.Append(queryCompiler.GetFieldName(property, propertyName));
            AppendAsSql(queryCompiler, match, propertyName);
            return true;
        }

        /// <summary>
        /// 得到属性开始的索引
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        protected virtual bool AppendSelectMethodSql(QueryCompilerInfo queryCompiler, Match match)
        {
            var m = Regex.Match(match.Value, MethodKeyPattern);
            if (!m.Success) return false;
            var methodName = Regex.Match(match.Value, CommonPattern).Value;
            if (!m.Success) return false;
            var propertyName = Regex.Match(match.Value, BreakersPattern).Value.Trim('(').Trim(')').Trim();
            queryCompiler.Builder.Append(methodName.Equals("Average")?"Avg":methodName);
            queryCompiler.Builder.Append("(");
            if (methodName.Equals("Count"))
                queryCompiler.Builder.Append("1)");
            else
                TranslateSelectBegin(queryCompiler, string.Format("{0})", propertyName));
            AppendAsSql(queryCompiler, match, propertyName);
            return true;
        }
        /// <summary>
        /// 得到属性开始的索引
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        protected virtual bool AppendSelectManySql(QueryCompilerInfo queryCompiler, Match match)
        {
            var m = Regex.Match(match.Value,ManyKeyPattern);
            if (!m.Success) return false;
            var lastIndex = Regex.Match(match.Value, PropertyPattern).Value.LastIndexOf('.');
            string subSelect;
            if (Regex.IsMatch(match.Value, SubQueryPattern))
            {
                var index = match.Value.IndexOf(".Select");
                if(index == -1)
                    index =match.Value.LastIndexOf(".");
                else
                    index = match.Value.LastIndexOf(".",index,index);
                var value = match.Value.Substring(index,
                    match.Value.Length - index);
                subSelect = Regex.Match(value, BreakersPattern).Value;
            }
            else
            {
                subSelect = Regex.Match(match.Value, BreakersPattern).Value;
            }
            subSelect = subSelect.Substring(1, subSelect.Length - 2);
            var propertyName = match.Value.Substring(0, lastIndex);
            var chainProperties = queryCompiler.Object.GetChainProperties(propertyName,true);
            var nameBuilder = new StringBuilder();
            for (int i = 0; i < chainProperties.Count; i++)
            {
                if (nameBuilder.Length > 0)
                    nameBuilder.AppendFormat(".{0}", chainProperties[i].PropertyName);
                else
                    nameBuilder.AppendFormat(chainProperties[i].PropertyName);
                if (chainProperties[i].Map != null && chainProperties[i].Map.CheckRemote())
                {
                    var subText = match.Value.Substring(nameBuilder.Length + 1,
                                                        match.Value.Length - nameBuilder.Length - 1).Trim();
                    if (subText.StartsWith("Select("))
                    {
                        subText = subSelect;
                    }
                    if (FillRemoteQuery(queryCompiler, nameBuilder.ToString(), chainProperties[i], subText,match))
                    {
                        var tps = new List<OrmPropertyInfo>();
                        var builder = new StringBuilder();
                        for (int j = 0; j <= i; j++)
                        {
                            tps.Add(chainProperties[j]);
                            if (j < i)
                                builder.AppendFormat("{0}.", chainProperties[j].PropertyName);
                        }
                        builder.AppendFormat("{0}", chainProperties[i].Map.ObjectProperty.PropertyName);
                        queryCompiler.AddJoins(tps);
                        var name = queryCompiler.GetFieldName(chainProperties[i].Map.ObjectProperty, builder.ToString());
                        if (!queryCompiler.Builder.ToString().Contains(name))
                        {
                            queryCompiler.Builder.Append(name);
                            queryCompiler.Builder.AppendFormat(" as {0}_{1},", builder.ToString().Replace(".", "_"),queryCompiler.FieldCount);
                        }
                        return true;
                    }
                }
            }
            queryCompiler.AddJoins(chainProperties);
            var property = chainProperties[chainProperties.Count - 1];
            AppendManySql(queryCompiler, match, property,propertyName, subSelect);
            AppendAsSql(queryCompiler, match, propertyName);
            return true;
        }

        /// <summary>
        /// 得到查询结果
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        protected virtual QueryInfo GetSelectManyJoinQuery(QueryCompilerInfo queryCompiler, Match match)
        {
            if (match == null)
                return null;
            var m = Regex.Match(match.Value, SubQueryPattern);
            if (!m.Success) return null;
            var query = new QueryInfo { IsReturnCount = false };
            do
            {
                var methodName = Regex.Match(m.Value, CommonPattern).Value;
                var value = Regex.Match(m.Value, BreakersPattern).Value;
                value = value.Substring(1, value.Length - 2);
                switch (methodName)
                {
                    case "Take":
                        query.SetPageSize(int.Parse(value));
                        break;
                    case "Skip":
                        query.SetPageIndex(int.Parse(value));
                        break;
                    case "Where":
                        query.WhereExp = value;
                        break;
                    case "GroupBy":
                        query.GroupByExp = value;
                        break;
                    case "OrderBy":
                        query.OrderByExp = value;
                        break;
                    case "Having":
                        query.HavingExp = value;
                        break;
                }
                m = m.NextMatch();
            } while (m.Success);
            if ((!string.IsNullOrEmpty(query.WhereExp) || !string.IsNullOrEmpty(query.HavingExp)) && queryCompiler.Query.Parameters!=null)
            {
                foreach (var parameter in queryCompiler.Query.Parameters)
                {
                    var name = string.Format("@{0}", parameter.Key);
                    if (!string.IsNullOrEmpty(query.WhereExp) && query.WhereExp.Contains(name) ||
                        !string.IsNullOrEmpty(query.HavingExp) && query.HavingExp.Contains(name))
                    {
                        query.SetParameter(parameter.Key, parameter.Value);
                    }
                }
            }
            return query;
        }

        /// <summary>
        /// 添加集合属性
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="match"></param>
        /// <param name="property"></param>
        /// <param name="propertyName"></param>
        /// <param name="subSelect"></param>
        protected virtual void AppendManySql(QueryCompilerInfo queryCompiler,Match match, OrmPropertyInfo property,
                                             string propertyName, string subSelect)
        {
            var subQueryCompiler = new QueryCompilerInfo(property.Map.GetMapObject(), subSelect,
                                                        new TableInfo {Joins= new Dictionary<string, JoinInfo>()},
                                                         new StringBuilder())
                {
                    Query = queryCompiler.Query,
                    RemotePropertyName = propertyName,
                    Parent = queryCompiler,
                    TranslateQuery = queryCompiler.TranslateQuery
            };
            subQueryCompiler.SubQuery = GetSelectManyJoinQuery(subQueryCompiler, match);
            if (string.IsNullOrEmpty(subSelect) || subSelect.Equals("*"))
                TranslateFromBegin(subQueryCompiler);
            else
                TranslateSelectBegin(subQueryCompiler);
            if (subQueryCompiler.Builder.Length > 0)
            {
                if (subQueryCompiler.Builder[subQueryCompiler.Builder.Length - 1] == ',')
                    subQueryCompiler.Builder.Remove(subQueryCompiler.Builder.Length - 1, 1);
                var sql = GetManyFieldName(property, propertyName, queryCompiler, subQueryCompiler);
                queryCompiler.Builder.Append(sql);
            }
        }


        /// <summary>
        /// 填充远程加载
        /// </summary>
        /// <param name="queryCompiler"></param>
        /// <param name="propertyName"></param>
        /// <param name="property"></param>
        /// <param name="subSelect"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        protected virtual bool FillRemoteQuery(QueryCompilerInfo queryCompiler, string propertyName, OrmPropertyInfo property,
                                               string subSelect,Match match)
        {
            if (property.Map == null || !property.Map.CheckRemote())
                return false;
            var builder = new StringBuilder(propertyName);
            var tempQueryCompiler = queryCompiler;
            var remotePropertyName = queryCompiler.RemotePropertyName;
            while (tempQueryCompiler != null && !string.IsNullOrEmpty(remotePropertyName))
            {
                builder.Insert(0, string.Format("{0}.", remotePropertyName));
                tempQueryCompiler = tempQueryCompiler.Parent;
                if (tempQueryCompiler != null) remotePropertyName = tempQueryCompiler.RemotePropertyName;
            }
            queryCompiler.Query.RemoteQueries = queryCompiler.Query.RemoteQueries ??
                                                new Dictionary<string, RemoteQueryInfo>();
            var key = builder.ToString();
            if (!queryCompiler.Query.RemoteQueries.ContainsKey(key))
            {
                var remoteQuery = new RemoteQueryInfo(key);
                remoteQuery.SelectExp = string.IsNullOrEmpty(subSelect) ? "*" : subSelect;
                remoteQuery.IsLazyLoad = queryCompiler.Query.IsLazyLoad;
                var subQuery = GetSelectManyJoinQuery(queryCompiler, match);
                if (subQuery != null)
                {
                    remoteQuery.PageSize = subQuery.PageSize;
                    remoteQuery.PageIndex = subQuery.PageIndex;
                    remoteQuery.WhereExp = subQuery.WhereExp;
                    remoteQuery.Parameters = subQuery.Parameters;
                }
                queryCompiler.Query.RemoteQueries.Add(key, remoteQuery);
            }
            else
            {
                queryCompiler.Query.RemoteQueries[key].SelectExp = string.Format("{0},{1}",
                                                                                queryCompiler.Query.RemoteQueries[
                                                                                    key].SelectExp, subSelect);
            }
            return true;
        }

        /// <summary>
        /// 得到一对多查询字段
        /// </summary>
        /// <param name="property"></param>
        /// <param name="propertName"></param>
        /// <param name="selectComplier"></param>
        /// <param name="subSelectComplier"></param>
        /// <returns></returns>
        protected abstract string GetManyFieldName(OrmPropertyInfo property, string propertName,
                                                   QueryCompilerInfo selectComplier,QueryCompilerInfo subSelectComplier);



        #endregion

    }
}
