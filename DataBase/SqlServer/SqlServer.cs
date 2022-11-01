﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System.Linq.Expressions;
using OrangeCloud.Core.AiExpression;
using System.Transactions;
using System.Text;

namespace OrangeCloud.Core
{
    public class SqlServer : IDataBase
    {
        public override void InsertList<T>(IList<T> listT, string useDatabase = null, bool IsSplitTable = false, MSplitTableConfig config = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            StringBuilder sbSql = new StringBuilder();

            object listParam = null;

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                if (IsSplitTable == true)
                {
                    conn.Execute(SqlMapping.GetCreateTableSql<T>(ref tableName, config));
                }

                int i = 0;

                foreach (var t in listT)
                {
                    FillEntity.Instance.SetInsertSysCols(t);

                    var sqlData = SqlMapping.SqlServerInsert<T>
                    (t
                    , tableName
                    , i);

                    listParam = SqlMapping.AddDynamicParameters(listParam, sqlData.Param);

                    sbSql.Append(sqlData.Sql).Append(";");

                    i++;
                }

                conn.Execute(sbSql.ToString(), listParam);
            }
        }

        public override MSqlData Insert<T>(IList<MSqlData> listSqlData, T t, bool IsIncrement = false, string useDatabase = null, bool IsSplitTable = false, MSplitTableConfig config = null)
        {
            FillEntity.Instance.SetInsertSysCols(t);

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var createTableSql = "";

            //如果设置了分表
            if (IsSplitTable == true)
            {
                //验证是否需要创建数据表
                createTableSql = SqlMapping.GetCreateTableSql<T>(ref tableName, config);
            }

            //获取Insert语句
            var sqlData = SqlMapping.SqlServerInsert<T>
            (t
            , tableName
            , listSqlData.Count
            , IsIncrement);

            //后期生成的变量改短***
            sqlData.DataId = string.Format("@OrangeCloudAutoID_{0}", listSqlData.Count);

            sqlData.DatabaseName = SqlCommon.GetDatabaseName(typeString);

            sqlData.UseDatabase = useDatabase;

            //如果需要创建数据表则拼接Sql语句
            if (!string.IsNullOrWhiteSpace(createTableSql))
                sqlData.Sql = string.Format("{0};{1}", createTableSql, sqlData.Sql);

            //如果需要返回自增流水ID的情况下
            if (IsIncrement)
                sqlData.Sql = string.Format("DECLARE {0} INT; {1}; SELECT {0} = SCOPE_IDENTITY();", sqlData.DataId, sqlData.Sql);

            listSqlData.Add(sqlData);

            return sqlData;
        }

        public override MSqlData Insert64<T>(IList<MSqlData> listSqlData, T t, bool IsIncrement = false, string useDatabase = null, bool IsSplitTable = false, MSplitTableConfig config = null)
        {
            FillEntity.Instance.SetInsertSysCols(t);

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var createTableSql = "";

            //如果设置了分表
            if (IsSplitTable == true)
            {
                //验证是否需要创建数据表
                createTableSql = SqlMapping.GetCreateTableSql<T>(ref tableName, config);
            }

            //获取Insert语句
            var sqlData = SqlMapping.SqlServerInsert<T>
            (t
            , tableName
            , listSqlData.Count
            , IsIncrement);

            //后期生成的变量改短***
            sqlData.DataId = string.Format("@OrangeCloudAutoID_{0}", listSqlData.Count);

            sqlData.DatabaseName = SqlCommon.GetDatabaseName(typeString);

            sqlData.UseDatabase = useDatabase;

            //如果需要创建数据表则拼接Sql语句
            if (!string.IsNullOrWhiteSpace(createTableSql))
                sqlData.Sql = string.Format("{0};{1}", createTableSql, sqlData.Sql);

            //如果需要返回自增流水ID的情况下
            if (IsIncrement)
                sqlData.Sql = string.Format("DECLARE {0} BIGINT; {1}; SELECT {0} = SCOPE_IDENTITY();", sqlData.DataId, sqlData.Sql);

            listSqlData.Add(sqlData);

            return sqlData;
        }

        public override int Insert<T>(T t, bool IsIncrement = false, string useDatabase = null, bool IsSplitTable = false, MSplitTableConfig config = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            FillEntity.Instance.SetInsertSysCols(t);

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                if (IsSplitTable == true)
                {
                    conn.Execute(SqlMapping.GetCreateTableSql<T>(ref tableName, config));
                }

                var sqlData = SqlMapping.SqlServerInsert<T>
                (t
                , tableName
                , 0
                , IsIncrement);


                if (!IsIncrement)
                    return conn.Execute(sqlData.Sql, sqlData.Param);
                else
                {
                    sqlData.Param.Add("@OrangeCloudAutoID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    conn.Execute(sqlData.Sql + ";SELECT @OrangeCloudAutoID=SCOPE_IDENTITY();", sqlData.Param);

                    return sqlData.Param.Get<int>("@OrangeCloudAutoID");
                }
            }
        }

        public override long Insert64<T>(T t, bool IsIncrement = false, string useDatabase = null, bool IsSplitTable = false, MSplitTableConfig config = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            FillEntity.Instance.SetInsertSysCols(t);

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                if (IsSplitTable == true)
                {
                    conn.Execute(SqlMapping.GetCreateTableSql<T>(ref tableName, config));
                }

                var sqlData = SqlMapping.SqlServerInsert<T>
                (t
                , tableName
                , 0
                , IsIncrement);

                if (!IsIncrement)
                    return conn.Execute(sqlData.Sql, sqlData.Param);
                else
                {
                    sqlData.Param.Add("@OrangeCloudAutoID", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    conn.Execute(sqlData.Sql + ";SELECT @OrangeCloudAutoID=SCOPE_IDENTITY();", sqlData.Param);

                    return sqlData.Param.Get<long>("@OrangeCloudAutoID");
                }
            }
        }

        public override int Update<T>(T t, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            FillEntity.Instance.SetUpdateSysCols(t);

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerUpdate<T>
                (t
                , tableName
                , 0);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Execute(sqlData.Sql, sqlData.Param);
            }
        }

        public override int Update<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Update(t, expc, useDatabase, ConnMode);
        }

        public override int Update<T>(T t, AiExpConditions<T> expc, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            FillEntity.Instance.SetUpdateSysCols(t);

            var where = expc.Where();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerUpdate<T>
                (t
                , tableName
                , where
                , 0);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Execute(sqlData.Sql, sqlData.Param);
            }
        }

        public override MSqlData Update<T>(IList<MSqlData> listSqlData, T t, string useDatabase = null)
        {
            FillEntity.Instance.SetUpdateSysCols(t);

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerUpdate<T>
                (t
                , tableName
                , listSqlData.Count);

            sqlData.DatabaseName = SqlCommon.GetDatabaseName(typeString);

            sqlData.UseDatabase = useDatabase;

            listSqlData.Add(sqlData);

            return sqlData;
        }

        public override MSqlData Update<T>(IList<MSqlData> listSqlData, T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Update(listSqlData, t, expc, useDatabase);
        }

        public override MSqlData Update<T>(IList<MSqlData> listSqlData, T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            FillEntity.Instance.SetUpdateSysCols(t);

            var where = expc.Where();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerUpdate<T>
                (t
                , tableName
                , where
                , listSqlData.Count);

            sqlData.DatabaseName = SqlCommon.GetDatabaseName(typeString);

            sqlData.UseDatabase = useDatabase;

            listSqlData.Add(sqlData);

            return sqlData;
        }

        public override int LogicalDelete<T>(T t, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            FillEntity.Instance.SetDeleteSysCols(t);

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerUpdate<T>
                (t
                , tableName
                , 0);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Execute(sqlData.Sql, sqlData.Param);
            }
        }

        public override int LogicalDelete<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return LogicalDelete(t, bizExp, useDatabase, ConnMode);
        }

        public override int LogicalDelete<T>(T t, AiExpConditions<T> expc, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            FillEntity.Instance.SetDeleteSysCols(t);

            var where = expc.Where();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerUpdate<T>
                (t
                , tableName
                , where
                , 0);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Execute(sqlData.Sql, sqlData.Param);
            }
        }

        public override MSqlData LogicalDelete<T>(IList<MSqlData> listSqlData, T t, string useDatabase = null)
        {
            FillEntity.Instance.SetDeleteSysCols(t);

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerUpdate<T>
                (t
                , tableName
                , listSqlData.Count);

            sqlData.DatabaseName = SqlCommon.GetDatabaseName(typeString);

            sqlData.UseDatabase = useDatabase;

            listSqlData.Add(sqlData);

            return sqlData;
        }

        public override MSqlData LogicalDelete<T>(IList<MSqlData> listSqlData, T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return LogicalDelete(listSqlData, expc, useDatabase);
        }

        public override MSqlData LogicalDelete<T>(IList<MSqlData> listSqlData, T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            FillEntity.Instance.SetDeleteSysCols(t);

            var where = expc.Where();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerUpdate<T>
                (t
                , tableName
                , where
                , listSqlData.Count);

            sqlData.DatabaseName = SqlCommon.GetDatabaseName(typeString);

            sqlData.UseDatabase = useDatabase;

            listSqlData.Add(sqlData);

            return sqlData;
        }

        public override int Delete<T>(T t, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerDelete<T>
                (t
                , tableName
                , 0);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Execute(sqlData.Sql);
            }
        }

        public override int Delete<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Delete(t, expc, useDatabase, ConnMode);
        }

        public override int Delete<T>(T t, AiExpConditions<T> expc, string useDatabase = null, EConnectionMode ConnMode = EConnectionMode.Write)
        {
            var where = expc.Where();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerDelete<T>
                (t
                , tableName
                , where
                , 0);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Execute(sqlData.Sql);
            }
        }

        public override MSqlData Delete<T>(IList<MSqlData> listSqlData, T t, string useDatabase = null)
        {
            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerDelete<T>
                (t
                , tableName
                , listSqlData.Count);

            sqlData.DatabaseName = SqlCommon.GetDatabaseName(typeString);

            sqlData.UseDatabase = useDatabase;

            listSqlData.Add(sqlData);

            return sqlData;
        }

        public override MSqlData Delete<T>(IList<MSqlData> listSqlData, T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Delete(listSqlData, t, expc, useDatabase);
        }

        public override MSqlData Delete<T>(IList<MSqlData> listSqlData, T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            var where = expc.Where();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var sqlData = SqlMapping.SqlServerDelete<T>
                (t
                , tableName
                , where
                , listSqlData.Count);

            sqlData.DatabaseName = SqlCommon.GetDatabaseName(typeString);

            sqlData.UseDatabase = useDatabase;

            listSqlData.Add(sqlData);

            return sqlData;
        }

        public override T Get<T>(string id, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            //DapperHelper da = new DapperHelper(GetDBConncationKey(typeString));

            var sql = SqlMapping.SqlServerGet<T>
                (tableName
                , id
                , showCols
                , nolock);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Query<T>(sql).FirstOrDefault();
            }

            //throw new NotImplementedException(typeString + "," + GetDBConncationKey(typeString));
        }

        public override NT Get<T, NT>(string id, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            //DapperHelper da = new DapperHelper(GetDBConncationKey(typeString));

            var sql = SqlMapping.SqlServerGet<T>
                (tableName
                , id
                , showCols
                , nolock);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Query<NT>(sql).FirstOrDefault();
            }

            //throw new NotImplementedException(typeString + "," + GetDBConncationKey(typeString));
        }

        public override IList<T> Get<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Get<T>(expc, showCols, useDatabase, nolock, ConnMode);
        }

        public override IList<T> Get<T>(AiExpConditions<T> expc, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var where = expc.Where();

            var orderBy = expc.OrderBy();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            //DapperHelper da = new DapperHelper(GetDBConncationKey(typeString));

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, orderBy, showCols);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Query<T>(sql).ToList();
            }

            //throw new NotImplementedException();
        }

        public override IList<NT> Get<T, NT>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Get<T, NT>(expc, showCols, useDatabase, nolock, ConnMode);
        }

        public override IList<NT> Get<T, NT>(AiExpConditions<T> expc, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var where = expc.Where();

            var orderBy = expc.OrderBy();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            //DapperHelper da = new DapperHelper(GetDBConncationKey(typeString));

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, orderBy, showCols);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Query<NT>(sql).ToList();
            }

            //throw new NotImplementedException();
        }

        public override IList<T> GetWhere<T>(string where, string orderBy, object param = null, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            param = SqlMapping.XParameters(param);

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, orderBy, showCols);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Query<T>(sql, param).ToList();
            }

            //throw new NotImplementedException();
        }

        public override IList<NT> GetWhere<T, NT>(string where, string orderBy, object param = null, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            param = SqlMapping.XParameters(param);

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, orderBy, showCols);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Query<NT>(sql, param).ToList();
            }

            //throw new NotImplementedException();
        }

        public override MPageData<T> Get<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, int pageIndex, int pageSize, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Get<T>(expc, pageIndex, pageSize, showCols, useDatabase, nolock, ConnMode, sqlVersion);
        }

        public override MPageData<T> Get<T>(AiExpConditions<T> expc, int pageIndex, int pageSize, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var where = expc.Where();

            var orderBy = expc.OrderBy();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            string sqlCount = "";

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, orderBy, pageIndex, out sqlCount, pageSize, showCols, sqlVersion, nolock);

            MPageData<T> m = new MPageData<T>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                m.TotalNumber = conn.Query<int>(sqlCount).SingleOrDefault();

                m.Data = conn.Query<T>(sql).ToList();
            }

            return m;
        }

        public override MPageData<NT> Get<T, NT>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, int pageIndex, int pageSize, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Get<T, NT>(expc, pageIndex, pageSize, showCols, useDatabase, nolock, ConnMode, sqlVersion);
        }

        public override MPageData<NT> Get<T, NT>(AiExpConditions<T> expc, int pageIndex, int pageSize, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var where = expc.Where();

            var orderBy = expc.OrderBy();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            string sqlCount = "";

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, orderBy, pageIndex, out sqlCount, pageSize, showCols, sqlVersion, nolock);

            MPageData<NT> m = new MPageData<NT>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                m.TotalNumber = conn.Query<int>(sqlCount).SingleOrDefault();

                m.Data = conn.Query<NT>(sql).ToList();
            }

            return m;
        }

        public override MPageData<T> GetWhere<T>(string where, string orderBy, int pageIndex, int pageSize, object param = null, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            string sqlCount = "";

            param = SqlMapping.XParameters(param);

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, orderBy, pageIndex, out sqlCount, pageSize, showCols, sqlVersion, nolock);

            MPageData<T> m = new MPageData<T>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, param).SingleOrDefault();

                m.Data = conn.Query<T>(sql, param).ToList();
            }

            return m;
        }

        public override MPageData<NT> GetWhere<T, NT>(string where, string orderBy, int pageIndex, int pageSize, object param = null, string showCols = "*", string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            if (ConnMode == EConnectionMode.Write)
                nolock = false;

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            string sqlCount = "";

            param = SqlMapping.XParameters(param);

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, orderBy, pageIndex, out sqlCount, pageSize, showCols, sqlVersion, nolock);

            MPageData<NT> m = new MPageData<NT>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, param).SingleOrDefault();

                m.Data = conn.Query<NT>(sql, param).ToList();
            }

            return m;
        }

        public override MLeftJoin LeftJoin<TLeft, TRight>(IList<MLeftJoin> list, string On, string leftCols = "*", string rightCols = "*", bool nolock = true)
        {
            var typeLeftString = typeof(TLeft).ToString();

            var leftTableName = SqlCommon.GetTableName(typeLeftString);

            var typeRightString = typeof(TRight).ToString();

            var rightTableName = SqlCommon.GetTableName(typeRightString);

            //多段拆分
            var ArrOn = On.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

            string leftByName = "", rightByName = "";

            var ArrLeft = ArrOn[0].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            var ArrRight = ArrOn[1].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            leftByName = ArrLeft[0].Safe().Trim();

            rightByName = ArrRight[0].Safe().Trim();

            return SqlMapping.SqlServerGetLeftJoinSql<TLeft, TRight>(list, leftTableName, rightTableName, On, leftByName, rightByName, leftCols, rightCols, nolock);
        }

        public override string GetSql(IList<MLeftJoin> list, string wheres, string orderBy)
        {
            return SqlMapping.GetSql(list, wheres, orderBy);
        }

        public override MGetSql GetSql(IList<MLeftJoin> list, string wheres, string orderBy, int pageIndex, int pageSize)
        {
            return new MGetSql()
            {
                list = list,
                wheres = wheres,
                orderBy = orderBy,
                pageIndex = pageIndex,
                pageSize = pageSize
            };
        }

        public override IList<TReturn> Get<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = SqlMapping.GetPrimaryKey<TSecond>().KeyName;

            param = SqlMapping.XParameters(param);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                return conn.Query(sql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }
        }

        public override MPageData<TReturn> Get<TFirst, TSecond, TReturn>(MGetSql sql, Func<TFirst, TSecond, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = SqlMapping.GetPrimaryKey<TSecond>().KeyName;

            param = SqlMapping.XParameters(param);

            string sqlCount = "";

            var rtnSql = SqlMapping.SqlServerGetLeftJoinList(SqlMapping.GetSql(sql.list), sql.wheres, sql.orderBy, sql.pageIndex, sql.pageSize, out sqlCount, sqlVersion);

            MPageData<TReturn> m = new MPageData<TReturn>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, param).SingleOrDefault();

                m.Data = conn.Query(rtnSql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }

            return m;
        }

        public override IList<TReturn> Get<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName);

            param = SqlMapping.XParameters(param);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                return conn.Query(sql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }
        }

        public override MPageData<TReturn> Get<TFirst, TSecond, TThird, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName);

            param = SqlMapping.XParameters(param);

            string sqlCount = "";

            var rtnSql = SqlMapping.SqlServerGetLeftJoinList(SqlMapping.GetSql(sql.list), sql.wheres, sql.orderBy, sql.pageIndex, sql.pageSize, out sqlCount, sqlVersion);

            MPageData<TReturn> m = new MPageData<TReturn>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, param).SingleOrDefault();

                m.Data = conn.Query(rtnSql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }

            return m;
        }

        public override IList<TReturn> Get<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1},{2}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName
                , SqlMapping.GetPrimaryKey<TFourth>().KeyName);

            param = SqlMapping.XParameters(param);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                return conn.Query(sql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }
        }

        public override MPageData<TReturn> Get<TFirst, TSecond, TThird, TFourth, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1},{2}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName
                , SqlMapping.GetPrimaryKey<TFourth>().KeyName);

            param = SqlMapping.XParameters(param);

            string sqlCount = "";

            var rtnSql = SqlMapping.SqlServerGetLeftJoinList(SqlMapping.GetSql(sql.list), sql.wheres, sql.orderBy, sql.pageIndex, sql.pageSize, out sqlCount, sqlVersion);

            MPageData<TReturn> m = new MPageData<TReturn>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, param).SingleOrDefault();

                m.Data = conn.Query(rtnSql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }

            return m;
        }

        public override IList<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1},{2},{3}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName
                , SqlMapping.GetPrimaryKey<TFourth>().KeyName
                , SqlMapping.GetPrimaryKey<TFifth>().KeyName);

            param = SqlMapping.XParameters(param);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                return conn.Query(sql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }
        }

        public override MPageData<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1},{2},{3}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName
                , SqlMapping.GetPrimaryKey<TFourth>().KeyName
                , SqlMapping.GetPrimaryKey<TFifth>().KeyName);

            param = SqlMapping.XParameters(param);

            string sqlCount = "";

            var rtnSql = SqlMapping.SqlServerGetLeftJoinList(SqlMapping.GetSql(sql.list), sql.wheres, sql.orderBy, sql.pageIndex, sql.pageSize, out sqlCount, sqlVersion);

            MPageData<TReturn> m = new MPageData<TReturn>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, param).SingleOrDefault();

                m.Data = conn.Query(rtnSql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }

            return m;
        }

        public override IList<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1},{2},{3},{4}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName
                , SqlMapping.GetPrimaryKey<TFourth>().KeyName
                , SqlMapping.GetPrimaryKey<TFifth>().KeyName
                , SqlMapping.GetPrimaryKey<TSixth>().KeyName);

            param = SqlMapping.XParameters(param);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                return conn.Query(sql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }
        }

        public override MPageData<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1},{2},{3},{4}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName
                , SqlMapping.GetPrimaryKey<TFourth>().KeyName
                , SqlMapping.GetPrimaryKey<TFifth>().KeyName
                , SqlMapping.GetPrimaryKey<TSixth>().KeyName);

            param = SqlMapping.XParameters(param);

            string sqlCount = "";

            var rtnSql = SqlMapping.SqlServerGetLeftJoinList(SqlMapping.GetSql(sql.list), sql.wheres, sql.orderBy, sql.pageIndex, sql.pageSize, out sqlCount, sqlVersion);

            MPageData<TReturn> m = new MPageData<TReturn>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, param).SingleOrDefault();

                m.Data = conn.Query(rtnSql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }

            return m;
        }

        public override IList<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1},{2},{3},{4},{5}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName
                , SqlMapping.GetPrimaryKey<TFourth>().KeyName
                , SqlMapping.GetPrimaryKey<TFifth>().KeyName
                , SqlMapping.GetPrimaryKey<TSixth>().KeyName
                , SqlMapping.GetPrimaryKey<TSeventh>().KeyName);

            param = SqlMapping.XParameters(param);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                return conn.Query(sql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }
        }

        public override MPageData<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            var typeString = typeof(TSecond).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            var splitOn = string.Format("{0},{1},{2},{3},{4},{5}"
                , SqlMapping.GetPrimaryKey<TSecond>().KeyName
                , SqlMapping.GetPrimaryKey<TThird>().KeyName
                , SqlMapping.GetPrimaryKey<TFourth>().KeyName
                , SqlMapping.GetPrimaryKey<TFifth>().KeyName
                , SqlMapping.GetPrimaryKey<TSixth>().KeyName
                , SqlMapping.GetPrimaryKey<TSeventh>().KeyName);

            param = SqlMapping.XParameters(param);

            string sqlCount = "";

            var rtnSql = SqlMapping.SqlServerGetLeftJoinList(SqlMapping.GetSql(sql.list), sql.wheres, sql.orderBy, sql.pageIndex, sql.pageSize, out sqlCount, sqlVersion);

            MPageData<TReturn> m = new MPageData<TReturn>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode)))
            {
                m.TotalNumber = conn.Query<int>(sqlCount, param).SingleOrDefault();

                m.Data = conn.Query(rtnSql,
                                                                map,
                                                                param,
                                                                null,
                                                                true,
                                                                splitOn,
                                                                null,
                                                                null).ToList();
            }

            return m;
        }

        //public override MTrans InsertByTrans<T>(T t, bool IsIncrement = false, string ParaName = null, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    FillEntity.Instance.SetInsertSysCols(t);

        //    MTrans m = new MTrans();

        //    var typeString = typeof(T).ToString();

        //    var tableName = GetTableName(typeString);

        //    m.type = GetDBConncationKey(typeString, ConnMode);

        //    if (IsIncrement && !string.IsNullOrWhiteSpace(ParaName))
        //        m.sql = SqlMapping.SqlServerInsert<T>
        //            (t
        //            , tableName
        //            , string.IsNullOrWhiteSpace(IDName) ? GetPrimaryKey(tableName) : IDName
        //            , IsIncrement)
        //            + string.Format(";SELECT {0}=SCOPE_IDENTITY();", ParaName.safe());
        //    else
        //        m.sql = SqlMapping.SqlServerInsert<T>
        //            (t
        //            , tableName
        //            , string.IsNullOrWhiteSpace(IDName) ? GetPrimaryKey(tableName) : IDName
        //            , IsIncrement);

        //    m.sqlType = EType.插入;

        //    m.isIncrement = IsIncrement;

        //    m.ParaName = ParaName;

        //    return m;
        //}

        //public override MTrans UpdateByTrans<T>(T t, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    FillEntity.Instance.SetUpdateSysCols(t);

        //    MTrans m = new MTrans();

        //    var typeString = typeof(T).ToString();

        //    var tableName = GetTableName(typeString);

        //    m.type = GetDBConncationKey(typeString, ConnMode);

        //    m.sql = SqlMapping.SqlServerUpdate<T>
        //        (t
        //        , tableName
        //        , string.IsNullOrWhiteSpace(IDName) ? GetPrimaryKey(tableName) : IDName);

        //    m.sqlType = EType.修改;

        //    return m;
        //}

        //public override MTrans UpdateByTrans<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    FillEntity.Instance.SetUpdateSysCols(t);

        //    MTrans m = new MTrans();

        //    AiExpConditions<T> expc = new AiExpConditions<T>();

        //    expc.Add(bizExp);

        //    var where = expc.Where();

        //    var typeString = typeof(T).ToString();

        //    var tableName = GetTableName(typeString);

        //    m.type = GetDBConncationKey(typeString, ConnMode);

        //    m.sql = SqlMapping.SqlServerUpdate<T>
        //        (t
        //        , tableName
        //        , string.IsNullOrWhiteSpace(IDName) ? GetPrimaryKey(tableName) : IDName
        //        , where);

        //    m.sqlType = EType.修改;

        //    return m;
        //}

        //public override MTrans LogicalDeleteByTrans<T>(T t, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    FillEntity.Instance.SetDeleteSysCols(t);

        //    MTrans m = new MTrans();

        //    var typeString = typeof(T).ToString();

        //    var tableName = GetTableName(typeString);

        //    m.type = GetDBConncationKey(typeString, ConnMode);

        //    m.sql = SqlMapping.SqlServerUpdate<T>
        //        (t
        //        , tableName
        //        , string.IsNullOrWhiteSpace(IDName) ? GetPrimaryKey(tableName) : IDName);

        //    m.sqlType = EType.修改;

        //    return m;
        //}

        //public override MTrans LogicalDeleteByTrans<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    FillEntity.Instance.SetDeleteSysCols(t);

        //    MTrans m = new MTrans();

        //    AiExpConditions<T> expc = new AiExpConditions<T>();

        //    expc.Add(bizExp);

        //    var where = expc.Where();

        //    var typeString = typeof(T).ToString();

        //    var tableName = GetTableName(typeString);

        //    m.type = GetDBConncationKey(typeString, ConnMode);

        //    m.sql = SqlMapping.SqlServerUpdate<T>
        //        (t
        //        , tableName
        //        , string.IsNullOrWhiteSpace(IDName) ? GetPrimaryKey(tableName) : IDName
        //        , where);

        //    m.sqlType = EType.修改;

        //    return m;
        //}

        //public override MTrans DeleteByTrans<T>(T t, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    MTrans m = new MTrans();

        //    var typeString = typeof(T).ToString();

        //    var tableName = GetTableName(typeString);

        //    m.type = GetDBConncationKey(typeString, ConnMode);

        //    m.sql = SqlMapping.SqlServerDelete<T>
        //        (t
        //        , tableName
        //        , string.IsNullOrWhiteSpace(IDName) ? GetPrimaryKey(tableName) : IDName);

        //    m.sqlType = EType.删除;

        //    return m;
        //}

        //public override MTrans DeleteByTrans<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    MTrans m = new MTrans();

        //    AiExpConditions<T> expc = new AiExpConditions<T>();

        //    expc.Add(bizExp);

        //    var where = expc.Where();

        //    var typeString = typeof(T).ToString();

        //    var tableName = GetTableName(typeString);

        //    m.type = GetDBConncationKey(typeString, ConnMode);

        //    m.sql = SqlMapping.SqlServerDelete<T>
        //        (t
        //        , tableName
        //        , string.IsNullOrWhiteSpace(IDName) ? GetPrimaryKey(tableName) : IDName
        //        , where);

        //    m.sqlType = EType.删除;

        //    return m;
        //}

        public override TransactionScope TransMaster(System.Transactions.IsolationLevel isolationLevel = System.Transactions.IsolationLevel.Serializable)
        {
            TransactionOptions option = new TransactionOptions();

            option.IsolationLevel = isolationLevel;//System.Transactions.IsolationLevel.ReadCommitted;

            return new TransactionScope(TransactionScopeOption.Required, option);
        }

        //public override bool ExecuteWithTrans(List<MTrans> sqlList)
        //{
        //    bool isOK = false;

        //    TransactionOptions option = new TransactionOptions();

        //    option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

        //    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, option))
        //    {
        //        try
        //        {
        //            var keys = from o in sqlList group o by o.type into t select t.Key;

        //            foreach (var k in keys)
        //            {
        //                using (IDbConnection conn = SqlMapping.GetConnection(GetDBConncationKey(k, EConnectionMode.Write)))
        //                {
        //                    var list = from o in sqlList where o.type == k select o;

        //                    var p = new DynamicParameters();

        //                    var str = new StringBuilder();

        //                    foreach (var o in list)
        //                    {
        //                        if (o.isIncrement && o.sqlType == EType.插入 && !string.IsNullOrWhiteSpace(o.ParaName))
        //                        {
        //                            p.Add(o.ParaName, dbType: DbType.Int32, direction: ParameterDirection.Output);

        //                            //str.Append(o.sql + string.Format(";SELECT {0}=SCOPE_IDENTITY();", o.ParaName));
        //                        }

        //                        str.Append(o.sql);
        //                    }

        //                    conn.Execute(str.ToString(), p);

        //                }
        //            }

        //            // 没有错误,提交事务
        //            ts.Complete();

        //            isOK = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            isOK = false;

        //            throw new Exception("发送信息异常,原因:" + ex.Message);
        //        }
        //        finally
        //        {
        //            //释放资源
        //            ts.Dispose();
        //        }
        //    }

        //    return isOK;
        //}

        public override IList<T> RunProc<T>(string procName, object param, string dbName, string useDatabase = null, string useServer = null)
        {
            var typeString = typeof(T).ToString();

            param = SqlMapping.XParameters(param);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(dbName, EConnectionMode.Write, true), useDatabase, useServer))
            {
                return conn.Query<T>(procName,
                                        param,
                                        null,
                                        true,
                                        null,
                                        CommandType.StoredProcedure).ToList();
            }
        }

        public override IList<T> RunSql<T>(string sql, object param, string dbName, string useDatabase = null, string useServer = null)
        {
            var typeString = typeof(T).ToString();

            param = SqlMapping.XParameters(param);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(dbName, EConnectionMode.Write, true), useDatabase, useServer))
            {
                return conn.Query<T>(sql, param).ToList();
            }
        }

        public override MPageData<T> RunSql<T>(string sql, string orderBy, object param, int pageIndex, int pageSize, string dbName, string useDatabase = null, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            string strSqlCount = "";

            string strSql = SqlMapping.ExecuteSqlPage(sql, "", orderBy, pageIndex, out strSqlCount, pageSize, sqlVersion);

            param = SqlMapping.XParameters(param);

            MPageData<T> m = new MPageData<T>();

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(dbName, EConnectionMode.Write, true), useDatabase))
            {
                m.TotalNumber = conn.Query<int>(strSqlCount, param).SingleOrDefault();

                m.Data = conn.Query<T>(strSql, param).ToList();
            }

            return m;
        }

        public override decimal Sum<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string SumCol, string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Sum<T>(expc, SumCol, useDatabase, nolock, ConnMode);
        }

        public override decimal Sum<T>(AiExpConditions<T> expc, string SumCol, string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            var where = expc.Where();

            var orderBy = expc.OrderBy();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            //DapperHelper da = new DapperHelper(GetDBConncationKey(typeString));

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, "", string.Format("ISNULL(sum({0}),0)", SumCol), nolock);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Query<decimal>(sql).SingleOrDefault();
            }
        }

        public override int Submit(IList<MSqlData> list, bool isTransactionScope = false)
        {
            StringBuilder sbSql;

            object listParam;

            var listDatabase = list.Select(o => o.DatabaseName).Distinct();

            int OKCount = 0;

            if (isTransactionScope == false)
            {
                foreach (var db in listDatabase)
                {
                    var listSqlData = list.Where(o => o.DatabaseName == db).ToList();

                    sbSql = new StringBuilder();

                    listParam = null;

                    foreach (var item in listSqlData)
                    {
                        listParam = SqlMapping.AddDynamicParameters(listParam, item.Param);

                        sbSql.Append(item.Sql);
                    }

                    using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(db, EConnectionMode.Write, true), listSqlData[0].UseDatabase))
                    {
                        OKCount += conn.Execute(sbSql.ToString(), listParam);
                    }
                }
            }
            else
            {
                using (var t = ORM.BeginTransaction())
                {
                    foreach (var db in listDatabase)
                    {
                        var listSqlData = list.Where(o => o.DatabaseName == db).ToList();

                        sbSql = new StringBuilder();

                        listParam = null;

                        foreach (var item in listSqlData)
                        {
                            listParam = SqlMapping.AddDynamicParameters(listParam, item.Param);

                            sbSql.Append(item.Sql);
                        }

                        using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(db, EConnectionMode.Write, true), listSqlData[0].UseDatabase))
                        {
                            OKCount += conn.Execute(sbSql.ToString(), listParam);
                        }
                    }

                    t.Complete();
                }
            }

            return OKCount;
        }

        public override int Count<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            AiExpConditions<T> expc = new AiExpConditions<T>();

            expc.Add(bizExp);

            return Count<T>(expc, useDatabase, nolock, ConnMode);
        }

        public override int Count<T>(AiExpConditions<T> expc, string useDatabase = null, bool nolock = true, EConnectionMode ConnMode = EConnectionMode.Read)
        {
            var where = expc.Where();

            var orderBy = expc.OrderBy();

            var typeString = typeof(T).ToString();

            var tableName = SqlCommon.GetTableName(typeString);

            //DapperHelper da = new DapperHelper(GetDBConncationKey(typeString));

            var sql = SqlMapping.SqlServerGetList<T>(tableName, where, "", "count(1)", nolock);

            using (IDbConnection conn = SqlMapping.GetConnection(SqlCommon.GetDBConncationKey(typeString, ConnMode), useDatabase))
            {
                return conn.Query<int>(sql).SingleOrDefault();
            }
        }


    }
}
