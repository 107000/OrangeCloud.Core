using Dapper;
using OrangeCloud.Core.AiExpression;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OrangeCloud.Core
{
    /// <summary>
    /// ORM链式工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ORMFactory<T>
    {
        /// <summary>
        /// 1 = 执行 ORM查询
        /// 2 = 执行 SQL语句
        /// 3 = 执行 存储过程
        /// 6 = 执行 ORM插入
        /// 7 = 执行 ORM修改
        /// 8 = 执行 ORM删除
        /// </summary>
        private int ORMType { get; set; }

        /// <summary>
        /// 映射类
        /// </summary>
        private string TypeString { get; set; }

        /// <summary>
        /// 服务器
        /// </summary>
        private string Server { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        private string Database { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        private string TableName { get; set; }

        /// <summary>
        /// 查询字段
        /// </summary>
        private string SelectColumns { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        private string Where { get; set; }

        /// <summary>
        /// SQL语句
        /// </summary>
        private string Sql { get; set; }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        private string ProcedureName { get; set; }

        /// <summary>
        /// SQL参数
        /// </summary>
        private object SqlParameter { get; set; }

        /// <summary>
        /// 是否分表
        /// </summary>
        private bool IsSplitTable { get; set; }

        /// <summary>
        /// 插入时 是否 返回增量ID
        /// </summary>
        private bool IsIncrement { get; set; }

        /// <summary>
        /// 表实体
        /// </summary>
        private T TableEntity { get; set; }

        /// <summary>
        /// 分表配置
        /// </summary>
        private MSplitTableConfig SplitTableConfig { get; set; }

        /// <summary>
        /// 排序条件
        /// </summary>
        private string OrderBy { get; set; }

        /// <summary>
        /// 是否添加nolock
        /// </summary>
        private bool Nolock { get; set; }

        /// <summary>
        /// 指定读写库
        /// </summary>
        private EConnectionMode ConnectionMode { get; set; }

        /// <summary>
        /// 字符串链接
        /// </summary>
        private string ConnectionKey { get; set; }

        /// <summary>
        /// 初始化：使用 T 映射的数据库配置
        /// </summary>
        public ORMFactory()
        {
            TypeString = typeof(T).ToString();

            TableName = SqlCommon.GetTableName(TypeString);

            SelectColumns = "*";
        }

        /// <summary>
        /// 初始化：使用动态指向数据库
        /// </summary>
        /// <param name="database">数据库配置KEY</param>
        public ORMFactory(string database)
        {
            TypeString = typeof(T).ToString();

            TableName = SqlCommon.GetTableName(TypeString);

            SelectColumns = "*";

            Database = database;
        }

        /// <summary>
        /// 初始化：使用动态指向服务器和数据库
        /// </summary>
        /// <param name="server">服务器IP</param>
        /// <param name="database">数据库配置KEY</param>
        public ORMFactory(string server, string database)
        {
            TypeString = typeof(T).ToString();

            TableName = SqlCommon.GetTableName(TypeString);

            SelectColumns = "*";

            Server = server;

            Database = database;
        }

        private void ResetValue()
        {
            Where = null;

            OrderBy = null;

            Sql = null;

            SqlParameter = null;

            ProcedureName = null;

            IsSplitTable = false;

            SplitTableConfig = null;

            SelectColumns = "*";

            IsIncrement = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizExp"></param>
        /// <param name="connMode"></param>
        /// <returns></returns>
        public ORMFactory<T> Get(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, EConnectionMode connMode = EConnectionMode.Read)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Get(expc, connMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expc"></param>
        /// <param name="connMode"></param>
        /// <returns></returns>
        public ORMFactory<T> Get(AiExpConditions<T> expc, EConnectionMode connMode = EConnectionMode.Read)
        {
            ResetValue();

            ORMType = 1;

            ConnectionMode = connMode;

            // 读库加nolock，写库不加nolock
            Nolock = (connMode == EConnectionMode.Read ? true : false);

            Where = expc.Where();

            OrderBy = expc.OrderBy();

            ConnectionKey = SqlCommon.GetDBConncationKey(TypeString, ConnectionMode);

            Sql = null;

            SqlParameter = null;

            return this;
        }

        /// <summary>
        /// 配置存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public ORMFactory<T> RunProc(string procName, object param = null)
        {
            ResetValue();

            ORMType = 3;

            ConnectionMode = EConnectionMode.Write;

            ConnectionKey = SqlCommon.GetDBConncationKey(TypeString, ConnectionMode);

            ProcedureName = procName;

            SqlParameter = SqlMapping.XParameters(param);

            return this;
        }

        /// <summary>
        /// 配置SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns></returns>
        public ORMFactory<T> RunSql(string sql, object param = null)
        {
            ResetValue();

            ORMType = 2;

            ConnectionMode = EConnectionMode.Write;

            ConnectionKey = SqlCommon.GetDBConncationKey(TypeString, ConnectionMode);

            Sql = sql;

            SqlParameter = SqlMapping.XParameters(param);

            return this;
        }

        /// <summary>
        /// 配置SQL语句【分页用法】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="orderby">分页时必须传入order by语句</param>
        /// <param name="param">SQL参数</param>
        /// <returns></returns>
        public ORMFactory<T> RunSql(string sql, string orderby, object param = null)
        {
            OrderBy = orderby;

            return RunSql(sql, param);
        }

        /// <summary>
        /// ORM 插入
        /// </summary>
        /// <param name="t">DataTable实体类</param>
        /// <param name="isIncrement">true = 返回自增流水号、false = 返回影响行数</param>
        /// <returns></returns>
        public ORMFactory<T> Insert(T t, bool isIncrement = true)
        {
            ResetValue();

            ORMType = 6;

            ConnectionMode = EConnectionMode.Write;

            ConnectionKey = SqlCommon.GetDBConncationKey(TypeString, ConnectionMode);

            IsIncrement = isIncrement;

            FillEntity.Instance.SetInsertSysCols(t);

            TableEntity = t;

            return this;
        }

        /// <summary>
        /// ORM 修改
        /// </summary>
        /// <param name="t">DataTable实体类</param>
        /// <returns></returns>
        public ORMFactory<T> Update(T t)
        {
            ResetValue();

            ORMType = 7;

            ConnectionMode = EConnectionMode.Write;

            ConnectionKey = SqlCommon.GetDBConncationKey(TypeString, ConnectionMode);

            FillEntity.Instance.SetUpdateSysCols(t);

            TableEntity = t;

            var sqlData = SqlMapping.SqlServerUpdate(t, TableName, 0);

            Sql = sqlData.Sql;

            SqlParameter = sqlData.Param;

            return this;
        }

        /// <summary>
        /// ORM 修改
        /// </summary>
        /// <param name="t">DataTable实体类</param>
        /// <param name="bizExp"></param>
        /// <returns></returns>
        public ORMFactory<T> Update(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Update(t, expc);
        }

        /// <summary>
        /// ORM 修改
        /// </summary>
        /// <param name="t">DataTable实体类</param>
        /// <param name="expc"></param>
        /// <returns></returns>
        public ORMFactory<T> Update(T t, AiExpConditions<T> expc)
        {
            ResetValue();

            ORMType = 7;

            ConnectionMode = EConnectionMode.Write;

            ConnectionKey = SqlCommon.GetDBConncationKey(TypeString, ConnectionMode);

            FillEntity.Instance.SetUpdateSysCols(t);

            TableEntity = t;

            var sqlData = SqlMapping.SqlServerUpdate(t, TableName, expc.Where(), 0);

            Sql = sqlData.Sql;

            SqlParameter = sqlData.Param;

            return this;
        }

        /// <summary>
        /// ORM 删除
        /// </summary>
        /// <param name="t">DataTable实体类</param>
        /// <returns></returns>
        public ORMFactory<T> Delete(T t)
        {
            ResetValue();

            ORMType = 8;

            ConnectionMode = EConnectionMode.Write;

            ConnectionKey = SqlCommon.GetDBConncationKey(TypeString, ConnectionMode);

            FillEntity.Instance.SetUpdateSysCols(t);

            TableEntity = t;

            var sqlData = SqlMapping.SqlServerDelete(t, TableName, 0);

            Sql = sqlData.Sql;

            SqlParameter = sqlData.Param;

            return this;
        }

        /// <summary>
        /// ORM 删除
        /// </summary>
        /// <param name="t"></param>
        /// <param name="bizExp"></param>
        /// <returns></returns>
        public ORMFactory<T> Delete(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Delete(t, expc);
        }

        /// <summary>
        /// ORM 删除
        /// </summary>
        /// <param name="t"></param>
        /// <param name="expc"></param>
        /// <returns></returns>
        public ORMFactory<T> Delete(T t, AiExpConditions<T> expc)
        {
            ResetValue();

            ORMType = 8;

            ConnectionMode = EConnectionMode.Write;

            ConnectionKey = SqlCommon.GetDBConncationKey(TypeString, ConnectionMode);

            FillEntity.Instance.SetUpdateSysCols(t);

            var sqlData = SqlMapping.SqlServerDelete(t, TableName, expc.Where(), 0);

            Sql = sqlData.Sql;

            SqlParameter = sqlData.Param;

            return this;
        }

        /// <summary>
        /// 设置查询字段
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selectColumns"></param>
        /// <returns></returns>
        public ORMFactory<T> Select<TResult>(Func<T, TResult> selectColumns)
        {
            SelectColumns = selectColumns.Select();

            return this;
        }

        /// <summary>
        /// 设置查询字段
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selectColumns"></param>
        /// <returns></returns>
        public ORMFactory<T> Select<TResult>(string selectColumns)
        {
            SelectColumns = selectColumns;

            return this;
        }

        /// <summary>
        /// 设置分表插入
        /// </summary>
        /// <param name="splitTableConfig"></param>
        /// <returns></returns>
        public ORMFactory<T> SetSplitTable(MSplitTableConfig splitTableConfig)
        {
            IsSplitTable = true;

            SplitTableConfig = splitTableConfig;

            return this;
        }

        /// <summary>
        /// 设置服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public ORMFactory<T> SetServer(string server)
        {
            Server = server;

            return this;
        }

        /// <summary>
        /// 设置数据库
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public ORMFactory<T> SetDatabase(string database)
        {
            Database = database;

            return this;
        }

        /// <summary>
        /// 追加 Where SQL
        /// </summary>
        /// <param name="sqlParam">语句和参数</param>
        /// <returns></returns>
        public ORMFactory<T> AppendWhere(MSqlParam sqlParam)
        {
            Where += " and " + sqlParam.Sql;

            SqlParameter = SqlMapping.XParameters(SqlMapping.AddDynamicParameters(SqlParameter, sqlParam.Param));

            return this;
        }

        /// <summary>
        /// 返回第一行数据
        /// </summary>
        /// <returns></returns>
        public T FirstOrDefault()
        {
            if (ORMType == 1)
                Sql = SqlMapping.SqlServerGetList<T>(TableName, Where, OrderBy, SelectColumns, Nolock);

            using (IDbConnection conn = SqlMapping.GetConnection(ConnectionKey, Database, Server))
            {
                if (ORMType == 3)
                    return conn.Query<T>(ProcedureName, SqlParameter, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                else
                    return conn.Query<T>(Sql, SqlParameter).FirstOrDefault();
            }
        }

        /// <summary>
        /// 返回第一行数据
        /// </summary>
        /// <typeparam name="NT">需要映射填充的业务实体类</typeparam>
        /// <returns></returns>
        public NT FirstOrDefault<NT>()
        {
            if (ORMType == 1)
                Sql = SqlMapping.SqlServerGetList<T>(TableName, Where, OrderBy, SelectColumns, Nolock);

            using (IDbConnection conn = SqlMapping.GetConnection(ConnectionKey, Database, Server))
            {
                if (ORMType == 3)
                    return conn.Query<NT>(ProcedureName, SqlParameter, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
                else
                    return conn.Query<NT>(Sql, SqlParameter).FirstOrDefault();
            }
        }

        /// <summary>
        /// 返回数据列表
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            if (ORMType == 1)
                Sql = SqlMapping.SqlServerGetList<T>(TableName, Where, OrderBy, SelectColumns, Nolock);

            using (IDbConnection conn = SqlMapping.GetConnection(ConnectionKey, Database, Server))
            {
                if (ORMType == 3)
                    return conn.Query<T>(ProcedureName, SqlParameter, null, true, null, CommandType.StoredProcedure).ToList();
                else
                    return conn.Query<T>(Sql, SqlParameter).ToList();
            }
        }

        /// <summary>
        /// 返回数据列表
        /// </summary>
        /// <typeparam name="NT">需要映射填充的业务实体类</typeparam>
        /// <returns></returns>
        public List<NT> ToList<NT>()
        {
            if (ORMType == 1)
                Sql = SqlMapping.SqlServerGetList<T>(TableName, Where, OrderBy, SelectColumns, Nolock);

            using (IDbConnection conn = SqlMapping.GetConnection(ConnectionKey, Database, Server))
            {
                if (ORMType == 3)
                    return conn.Query<NT>(ProcedureName, SqlParameter, null, true, null, CommandType.StoredProcedure).ToList();
                else
                    return conn.Query<NT>(Sql, SqlParameter).ToList();
            }
        }

        /// <summary>
        /// 返回分页数据
        /// </summary>
        /// <param name="pageIndex">页码：从 1 开始</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="sqlVersion">SQL版本：影响性能</param>
        /// <returns></returns>
        public MPageData<T> ToPageList(int pageIndex, int pageSize, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            string sqlCount = null;

            if (ORMType == 1)
                Sql = SqlMapping.SqlServerGetList<T>(TableName, Where, OrderBy, pageIndex, out sqlCount, pageSize, SelectColumns, sqlVersion, Nolock);
            else if (ORMType == 2)
                Sql = SqlMapping.ExecuteSqlPage(Sql, "", OrderBy, pageIndex, out sqlCount, pageSize, sqlVersion);

            MPageData<T> m = new MPageData<T>();

            using (IDbConnection conn = SqlMapping.GetConnection(ConnectionKey, Database, Server))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, SqlParameter).SingleOrDefault();

                m.Data = conn.Query<T>(Sql, SqlParameter).ToList();
            }

            return m;
        }

        /// <summary>
        /// 返回分页数据
        /// </summary>
        /// <typeparam name="NT">需要映射填充的业务实体类</typeparam>
        /// <param name="pageIndex">页码：从 1 开始</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="sqlVersion">SQL版本：影响性能</param>
        /// <returns></returns>
        public MPageData<NT> ToPageList<NT>(int pageIndex, int pageSize, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            string sqlCount = null;

            if (ORMType == 1)
                Sql = SqlMapping.SqlServerGetList<T>(TableName, Where, OrderBy, pageIndex, out sqlCount, pageSize, SelectColumns, sqlVersion, Nolock);
            else if (ORMType == 2)
                Sql = SqlMapping.ExecuteSqlPage(Sql, "", OrderBy, pageIndex, out sqlCount, pageSize, sqlVersion);

            MPageData<NT> m = new MPageData<NT>();

            using (IDbConnection conn = SqlMapping.GetConnection(ConnectionKey, Database, Server))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, SqlParameter).SingleOrDefault();

                m.Data = conn.Query<NT>(Sql, SqlParameter).ToList();
            }

            return m;
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <returns></returns>
        public long Execute()
        {
            using (IDbConnection conn = SqlMapping.GetConnection(ConnectionKey, Database, Server))
            {
                // 执行 存储过程
                if (ORMType == 3)
                    return conn.Execute(ProcedureName, SqlParameter, null, null, CommandType.StoredProcedure);
                // 执行 ORM插入
                else if (ORMType == 6)
                {
                    string tableName = TableName;

                    // 如果需要分表
                    if (IsSplitTable == true)
                    {
                        conn.Execute(SqlMapping.GetCreateTableSql<T>(ref tableName, SplitTableConfig));
                    }

                    // 生成插入语句
                    var sqlData = SqlMapping.SqlServerInsert<T>(TableEntity, tableName, 0, IsIncrement);

                    // 如果不需要返回流水
                    if (!IsIncrement)
                        return conn.Execute(sqlData.Sql, sqlData.Param);

                    // 如果需要返回流水ID
                    sqlData.Param.Add("@OrangeCloudAutoID", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    conn.Execute(sqlData.Sql + ";SELECT @OrangeCloudAutoID=SCOPE_IDENTITY();", sqlData.Param);

                    return sqlData.Param.Get<long>("OrangeCloudAutoID");
                }
                else
                    return conn.Execute(Sql, SqlParameter);
            }
        }

    }
}
