using System;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using Winner.Persistence.Compiler.Common;
using Winner.Persistence.Declare;

namespace Winner.Persistence.Compiler.SqlServer
{
    public class SqlServer2005QueryCompiler : QueryCompiler
    {
  
        #region 重写分页

        protected override void BuilderPageSql(StringBuilder sql, string selectExp, string fromExp, string whereExp, 
            string groupbyExp, string havingExp, string orderbyExp, QueryInfo query)
        {
            int start = query.PageIndex * query.PageSize + 1;
            int end = start + query.PageSize - 1;
            sql.Append("select * from (");
            if (query.IsDisinct)
            {
                var asName = Regex.Replace(fromExp.Trim(), @".*\s+", "");
                fromExp = string.Format("(select Distinct {0} from {1}) {2}",GetDistinctSelectExp(selectExp),fromExp,asName);
            }
            BuilderSql(sql, string.Format("{0},ROW_NUMBER() OVER (order by {1}) Auto_RowNumberIndex", selectExp, orderbyExp),
                fromExp, whereExp, groupbyExp, havingExp, "", false);
            sql.Append(string.Format(") t  where Auto_RowNumberIndex between {0} and {1} ", start, end));
        }

       
        #endregion
        /// <summary>
        /// 返回去除重复的内容
        /// </summary>
        /// <param name="selectExp"></param>
        /// <returns></returns>
        protected virtual string GetDistinctSelectExp(string selectExp)
        {
            return Regex.Replace(selectExp, @"\s+as\s+[^,]*", "");
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected override void AddParamter(System.Data.Common.DbCommand command, string name, object value)
        {
            if (string.IsNullOrEmpty(name)) return;
            name = string.Format("@{0}", name);
            if (value == null)
            {
                command.Parameters.Add(new SqlParameter(name, DBNull.Value));
            }
            else if (value is Array)
            {
                var array = value as Array;
                if (array.Length > 0)
                {
                    var builder = new StringBuilder();
                    for (int i = 0; i < array.Length; i++)
                    {
                        builder.Append(string.Format("{0},", array.GetValue(i)));
                    }
                    command.Parameters.Add(new SqlParameter(name, builder.ToString())); 
                }
                
            }
            else if (value.GetType().IsEnum)
            {
                var chars = value.GetType().GetCustomAttributes(typeof (CharEnumAttribute), true);
                command.Parameters.Add(chars.Length > 0
                                           ? new SqlParameter(name, Convert.ChangeType(value, typeof (char)))
                                           : new SqlParameter(name, value));
            }
            else
            {
                command.Parameters.Add(new SqlParameter(name, value));
            }

        }


    }
}
