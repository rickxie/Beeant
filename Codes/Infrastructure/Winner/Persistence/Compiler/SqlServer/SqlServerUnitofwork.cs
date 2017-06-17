using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Winner.Persistence.Compiler.Common;
using Winner.Persistence.Data;
using Winner.Persistence.Declare;
using Winner.Persistence.Relation;
using Winner.Persistence.Translation;

namespace Winner.Persistence.Compiler.SqlServer
{
    public class SqlServerUnitofwork : UnitofworkBase
    {
        #region 属性
        /// <summary>
        /// 执行的命令
        /// </summary>
        public ISaveCompiler SaveCompiler { get; set; }
        /// <summary>
        /// 连接对象集合
        /// </summary>
        public SqlConnection Connection { get; set; }
        /// <summary>
        /// 事务对象集合
        /// </summary>
        public SqlTransaction Transaction { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 连接字符串，xql命令集合,对象
        /// </summary>
        /// <param name="ormDataBase"></param>
        /// <param name="infos"></param>
        /// <param name="saveCompiler"></param>
        public SqlServerUnitofwork(OrmDataBaseInfo ormDataBase, IList<SaveInfo> infos, ISaveCompiler saveCompiler)
        {
            Connection = new SqlConnection(ormDataBase.ConnnectString);
            OrmDataBase = ormDataBase;
            Infos = infos;
            SaveCompiler = saveCompiler;
        }
        #endregion

        #region 接口的实现

        /// <summary>
        /// 执行
        /// </summary>
        public override void Execute()
        {
            try
            {
                Begin();
                var bulkCopys=new Dictionary<string,IList<SaveInfo>>();
                SqlCommand cmd = GetSqlCommand();
                Infos = Infos.OrderByDescending(it => it.Sequence).ToList();
                var contentEntities = Infos.Select(it => it.Entity).ToList();
                foreach (var info in Infos)
                {

                    if (info.IsBulkCopy && info.Information.SaveType == SaveType.Add)
                    {
                        if (!bulkCopys.ContainsKey(info.SetTableName))
                        {
                            bulkCopys.Add(info.SetTableName, new List<SaveInfo>());
                        }
                        bulkCopys[info.SetTableName].Add(info);
                        continue;
                    }
                    var saveCompiler = new SaveCompilerInfo
                    {
                        Command = cmd,
                        ContentEntities = contentEntities,
                        SaveInfo = info,
                        OrmDataBase = OrmDataBase,
                        IsSaveParameters = false
                    };
                    SaveCompiler.Save(saveCompiler);
                }
                BulkCopy(bulkCopys);
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }

        }
     
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="bulkCopys"></param>
        protected virtual void BulkCopy(IDictionary<string, IList<SaveInfo>> bulkCopys)
        {
            if (bulkCopys == null || bulkCopys.Count == 0)
                return;
            foreach (var bulkCopy in bulkCopys)
            {
                var sqlBulkCopy = new SqlBulkCopy(Connection, SqlBulkCopyOptions.CheckConstraints, Transaction);
                var firstSaveInfo = bulkCopy.Value.First();
                var dataTable = new DataTable();
                var properties = new List<OrmPropertyInfo>();
                sqlBulkCopy.DestinationTableName = firstSaveInfo.SetTableName;
                foreach (OrmPropertyInfo p in firstSaveInfo.Object.Properties)
                {
                    if (!p.AllowAdd || p.IsIdentityKey || p.Map!=null)
                        continue;
                    properties.Add(p);
                    dataTable.Columns.Add(new DataColumn(p.FieldName));
                    sqlBulkCopy.ColumnMappings.Add(p.FieldName, p.FieldName);
                }
                foreach (var saveInfo in bulkCopy.Value)
                {
                    var dr = dataTable.NewRow();
                    foreach (var property in properties)
                    {
                        object pValue = saveInfo.Entity.GetProperty(property.PropertyName);
                        if (pValue!=null && pValue.GetType().IsEnum)
                        {
                            var chars = pValue.GetType().GetCustomAttributes(typeof(CharEnumAttribute), true);
                            pValue = chars.Length > 0 ? Convert.ChangeType(pValue, typeof(char)) : Convert.ChangeType(pValue, typeof(int));
                        }
                        dr[property.FieldName] = pValue;
                    }
                    dataTable.Rows.Add(dr);
                }
                sqlBulkCopy.WriteToServer(dataTable);
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        public override void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            finally
            {
                Close();
            }
        

        }
        /// <summary>
        /// 回滚
        /// </summary>
        public override void Rollback()
        {
            try
            {
                if (Transaction != null)
                    Transaction.Rollback();
            }
            finally
            {
                Close();
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 开启事务
        /// </summary>
        protected virtual void Begin()
        {
            SetConnnection();
            Transaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// 得到链接
        /// </summary>
        /// <returns></returns>
        protected virtual void SetConnnection()
        {
            Connection = GetConnnection<SqlConnection>(OrmDataBase.GetAllSetOrmDataBase());
        }
        /// <summary>
        /// 重写创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ormDataBase"></param>
        /// <returns></returns>
        protected override T CreateTryConnection<T>(OrmDataBaseInfo ormDataBase)
        {
            return new SqlConnection(ormDataBase.ConnnectString) as T;
        }

        /// <summary>
        /// 得到SqlCommand
        /// </summary>
        /// <returns></returns>
        protected virtual SqlCommand GetSqlCommand()
        {
            var sqlcmd = new SqlCommand {Connection = Connection, Transaction = Transaction};
            return sqlcmd;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        protected virtual void Close()
        {
            if (Connection != null && Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        #endregion
    }
}
