using OrangeCloud.Core.AiExpression;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace OrangeCloud.Core
{
    /// <summary>
    /// OrangeCloud ORM
    /// </summary>
    public static class ORM
    {
        /// <summary>
        /// SQL插入 - 立即执行
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="isReturnId">是否返回Id</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="isSplitTable">是否需要分表</param>
        /// <param name="config">分表配置</param>
        /// <returns></returns>
        public static int Insert<T>(T t, bool isReturnId = false, string useDatabase = null, bool isSplitTable = false, MSplitTableConfig config = null)
        {
            return DataBase.Instance.Insert(t, isReturnId, useDatabase, isSplitTable, config, EConnectionMode.Write);
        }

        /// <summary>
        /// SQL插入 - 立即执行
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="isReturnId">是否返回Id</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="isSplitTable">是否需要分表</param>
        /// <param name="config">分表配置</param>
        /// <returns></returns>
        public static long Insert64<T>(T t, bool isReturnId = false, string useDatabase = null, bool isSplitTable = false, MSplitTableConfig config = null)
        {
            return DataBase.Instance.Insert64(t, isReturnId, useDatabase, isSplitTable, config, EConnectionMode.Write);
        }

        /// <summary>
        /// SQL插入 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="listSqlData">工作集</param>
        /// <param name="t">实体</param>
        /// <param name="isReturnId">是否返回Id</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="isSplitTable">是否需要分表</param>
        /// <param name="config">分表配置</param>
        /// <returns></returns>
        public static MSqlData Insert<T>(IList<MSqlData> listSqlData, T t, bool isReturnId = false, string useDatabase = null, bool isSplitTable = false, MSplitTableConfig config = null)
        {
            return DataBase.Instance.Insert(listSqlData, t, isReturnId, useDatabase, isSplitTable, config);
        }

        /// <summary>
        /// SQL插入 - 批处理 - 传入List Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listT"></param>
        /// <param name="useDatabase"></param>
        /// <param name="isSplitTable"></param>
        /// <param name="config"></param>
        public static void InsertList<T>(IList<T> listT, string useDatabase = null, bool isSplitTable = false, MSplitTableConfig config = null)
        {
            DataBase.Instance.InsertList(listT, useDatabase, isSplitTable, config);
        }

        /// <summary>
        /// SQL插入 - 批处理 - 传入List Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listSqlData"></param>
        /// <param name="listT"></param>
        /// <param name="isReturnId"></param>
        /// <param name="useDatabase"></param>
        /// <param name="isSplitTable"></param>
        /// <param name="config"></param>
        public static void InsertList<T>(IList<MSqlData> listSqlData, IList<T> listT, bool isReturnId = false, string useDatabase = null, bool isSplitTable = false, MSplitTableConfig config = null)
        {
            foreach (var t in listT)
            {
                DataBase.Instance.Insert(listSqlData, t, isReturnId, useDatabase, isSplitTable, config);
            }
        }

        /// <summary>
        /// SQL插入 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="listSqlData">工作集</param>
        /// <param name="t">实体</param>
        /// <param name="isReturnId">是否返回Id</param>
        /// <param name="useDatabase"></param>
        /// <param name="isSplitTable">是否需要分表</param>
        /// <param name="config">分表配置</param>
        /// <returns></returns>
        public static MSqlData Insert64<T>(IList<MSqlData> listSqlData, T t, bool isReturnId = false, string useDatabase = null, bool isSplitTable = false, MSplitTableConfig config = null)
        {
            return DataBase.Instance.Insert64(listSqlData, t, isReturnId, useDatabase, isSplitTable, config);
        }

        /// <summary>
        /// SQL插入 - 批处理 - 传入List Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listSqlData"></param>
        /// <param name="listT"></param>
        /// <param name="isReturnId"></param>
        /// <param name="useDatabase"></param>
        /// <param name="isSplitTable"></param>
        /// <param name="config"></param>
        public static void Insert64List<T>(IList<MSqlData> listSqlData, IList<T> listT, bool isReturnId = false, string useDatabase = null, bool isSplitTable = false, MSplitTableConfig config = null)
        {
            foreach (var t in listT)
            {
                DataBase.Instance.Insert64(listSqlData, t, isReturnId, useDatabase, isSplitTable, config);
            }
        }

        /// <summary>
        /// SQL修改 - 立即执行
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static int Update<T>(T t, string useDatabase = null)
        {
            return DataBase.Instance.Update(t, useDatabase, EConnectionMode.Write);
        }

        /// <summary>
        /// SQL修改 - 根据搜索条件 - 立即执行
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static int Update<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            return DataBase.Instance.Update(t, bizExp, useDatabase, EConnectionMode.Write);
        }

        public static int Update<T>(T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            return DataBase.Instance.Update(t, expc, useDatabase, EConnectionMode.Write);
        }

        /// <summary>
        /// SQL修改 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="listSqlData">工作集</param>
        /// <param name="t">实体</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static MSqlData Update<T>(IList<MSqlData> listSqlData, T t, string useDatabase = null)
        {
            return DataBase.Instance.Update(listSqlData, t, useDatabase);
        }

        /// <summary>
        /// SQL修改 - 批处理 - 传入List Model
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="listSqlData">工作集</param>
        /// <param name="t">实体</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static void UpdateList<T>(IList<MSqlData> listSqlData, IList<T> listT, string useDatabase = null)
        {
            foreach (var t in listT)
            {
                DataBase.Instance.Update(listSqlData, t, useDatabase);
            }
        }

        /// <summary>
        /// SQL修改 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="listSqlData">工作集</param>
        /// <param name="t">实体</param>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static MSqlData Update<T>(IList<MSqlData> listSqlData, T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            return DataBase.Instance.Update(listSqlData, t, bizExp, useDatabase);
        }

        public static MSqlData Update<T>(IList<MSqlData> listSqlData, T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            return DataBase.Instance.Update(listSqlData, t, expc, useDatabase);
        }

        /// <summary>
        /// SQL物理删除 - 立即执行
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static int Delete<T>(T t, string useDatabase = null)
        {
            return DataBase.Instance.Delete(t, useDatabase, EConnectionMode.Write);
        }

        /// <summary>
        /// SQL物理删除 - 根据搜索条件 - 立即执行
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static int Delete<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            return DataBase.Instance.Delete(t, bizExp, useDatabase, EConnectionMode.Write);
        }

        public static int Delete<T>(T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            return DataBase.Instance.Delete(t, expc, useDatabase, EConnectionMode.Write);
        }

        /// <summary>
        /// SQL物理删除 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="listSqlData">工作集</param>
        /// <param name="t">实体</param>
        /// <param name="useDatabase"></param>
        /// <returns></returns>
        public static MSqlData Delete<T>(IList<MSqlData> listSqlData, T t, string useDatabase = null)
        {
            return DataBase.Instance.Delete(listSqlData, t, useDatabase);
        }

        /// <summary>
        /// SQL物理删除 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="listSqlData">工作集</param>
        /// <param name="listT">实体</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static void Delete<T>(IList<MSqlData> listSqlData, List<T> listT, string useDatabase = null)
        {
            foreach (var t in listT)
            {
                DataBase.Instance.Delete(listSqlData, t, useDatabase);
            }
        }

        /// <summary>
        /// SQL物理删除 - 根据搜索条件 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="listSqlData">工作集</param>
        /// <param name="t">实体</param>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static MSqlData Delete<T>(IList<MSqlData> listSqlData, T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            return DataBase.Instance.Delete(listSqlData, t, bizExp, useDatabase);
        }

        public static MSqlData Delete<T>(IList<MSqlData> listSqlData, T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            return DataBase.Instance.Delete(listSqlData, t, expc, useDatabase);
        }

        /// <summary>
        /// SQL逻辑删除 - 立即执行
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static int LogicalDelete<T>(T t, string useDatabase = null)
        {
            return DataBase.Instance.LogicalDelete(t, useDatabase, EConnectionMode.Write);
        }

        /// <summary>
        /// SQL逻辑删除 - 根据搜索条件 - 立即执行
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static int LogicalDelete<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            return DataBase.Instance.LogicalDelete(t, bizExp, useDatabase, EConnectionMode.Write);
        }

        public static int LogicalDelete<T>(T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            return DataBase.Instance.LogicalDelete(t, expc, useDatabase, EConnectionMode.Write);
        }

        /// <summary>
        /// SQL逻辑删除 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static MSqlData LogicalDelete<T>(IList<MSqlData> listSqlData, T t, string useDatabase = null)
        {
            return DataBase.Instance.LogicalDelete(listSqlData, t, useDatabase);
        }

        /// <summary>
        /// SQL逻辑删除 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static void LogicalDeleteList<T>(IList<MSqlData> listSqlData, List<T> listT, string useDatabase = null)
        {
            foreach (var t in listT)
            {
                DataBase.Instance.LogicalDelete(listSqlData, t, useDatabase);
            }
        }

        /// <summary>
        /// SQL逻辑删除 - 根据搜索条件 - 批处理
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="t">实体</param>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static MSqlData LogicalDelete<T>(IList<MSqlData> listSqlData, T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null)
        {
            return DataBase.Instance.LogicalDelete(listSqlData, t, bizExp, useDatabase);
        }

        public static MSqlData LogicalDelete<T>(IList<MSqlData> listSqlData, T t, AiExpConditions<T> expc, string useDatabase = null)
        {
            return DataBase.Instance.LogicalDelete(listSqlData, t, expc, useDatabase);
        }

        //public static MTrans InsertByTrans<T>(T t, bool IsIncrement = false, string ParaName = null, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    return DataBase.Instance.InsertByTrans(t, IsIncrement, ParaName, IDName, ConnMode);
        //}

        //public static MTrans UpdateByTrans<T>(T t, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    return DataBase.Instance.UpdateByTrans(t, IDName, ConnMode);
        //}

        //public static MTrans UpdateByTrans<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    return DataBase.Instance.UpdateByTrans(t, bizExp, IDName, ConnMode);
        //}

        //public static MTrans LogicalDeleteByTrans<T>(T t, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    return DataBase.Instance.LogicalDeleteByTrans(t, IDName, ConnMode);
        //}

        //public static MTrans LogicalDeleteByTrans<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    return DataBase.Instance.LogicalDeleteByTrans(t, bizExp, IDName, ConnMode);
        //}

        //public static MTrans DeleteByTrans<T>(T t, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    return DataBase.Instance.DeleteByTrans(t, IDName, ConnMode);
        //}

        //public static MTrans DeleteByTrans<T>(T t, Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string IDName = null, EConnectionMode ConnMode = EConnectionMode.Write)
        //{
        //    return DataBase.Instance.DeleteByTrans(t, bizExp, IDName, ConnMode);
        //}

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        /// <returns></returns>
        public static TransactionScope BeginTransaction(System.Transactions.IsolationLevel isolationLevel = System.Transactions.IsolationLevel.Serializable)
        {
            return DataBase.Instance.TransMaster(isolationLevel);
        }

        //public static bool ExecuteWithTrans(List<MTrans> sqlList)
        //{
        //    return DataBase.Instance.ExecuteWithTrans(sqlList);
        //}

        /// <summary>
        /// SQL查询 - 按Id查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="id">主键Id</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <returns></returns>
        public static T Get<T>(string id, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get<T>(id, showCols, useDatabase, nolock, connMode);
        }

        /// <summary>
        /// SQL查询 - 按Id查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <typeparam name="NT">映射的实体</typeparam>
        /// <param name="id">主键Id</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <returns></returns>
        public static NT Get<T, NT>(string id, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get<T, NT>(id, showCols, useDatabase, nolock, connMode);
        }

        /// <summary>
        /// SQL查询 - 按条件查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <returns></returns>
        public static IList<T> Get<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get(bizExp, showCols, useDatabase, nolock, connMode);
        }

        public static IList<T> Get<T>(AiExpConditions<T> expc, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get(expc, showCols, useDatabase, nolock, connMode);
        }

        /// <summary>
        /// SQL查询 - 按条件查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <typeparam name="NT">映射的实体</typeparam>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <returns></returns>
        public static IList<NT> Get<T, NT>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get<T, NT>(bizExp, showCols, useDatabase, nolock, connMode);
        }

        public static IList<NT> Get<T, NT>(AiExpConditions<T> expc, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get<T, NT>(expc, showCols, useDatabase, nolock, connMode);
        }

        /// <summary>
        /// SQL查询 - 按条件查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="where">搜索条件 - Sql Where</param>
        /// <param name="orderBy">排序 - Sql Order By</param>
        /// <param name="param">参数（传入的参数必须参数化）</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <returns></returns>
        public static IList<T> GetWhere<T>(string where, string orderBy, object param = null, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.GetWhere<T>(where, orderBy, param, showCols, useDatabase, nolock, connMode);
        }

        /// <summary>
        /// SQL查询 - 按条件查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <typeparam name="NT">映射的实体</typeparam>
        /// <param name="where">搜索条件 - Sql Where</param>
        /// <param name="orderBy">排序 - Sql Order By</param>
        /// <param name="param">参数（传入的参数必须参数化）</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="ConnMode">读库/写库（默认为读库）</param>
        /// <returns></returns>
        public static IList<NT> GetWhere<T, NT>(string where, string orderBy, object param = null, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.GetWhere<T, NT>(where, orderBy, param, showCols, useDatabase, nolock, connMode);
        }

        /// <summary>
        /// SQL分页查询 - 按条件查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <param name="sqlVersion">Sql版本（影响分页性能）</param>
        /// <returns></returns>
        public static MPageData<T> Get<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, int pageIndex, int pageSize, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get(bizExp, pageIndex, pageSize, showCols, useDatabase, nolock, connMode, sqlVersion);
        }

        public static MPageData<T> Get<T>(AiExpConditions<T> expc, int pageIndex, int pageSize, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get(expc, pageIndex, pageSize, showCols, useDatabase, nolock, connMode, sqlVersion);
        }

        /// <summary>
        /// SQL分页查询 - 按条件查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <typeparam name="NT">映射的实体</typeparam>
        /// <param name="bizExp">搜索条件 - Lambda表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <param name="sqlVersion">Sql版本（影响分页性能）</param>
        /// <returns></returns>
        public static MPageData<NT> Get<T, NT>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, int pageIndex, int pageSize, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get<T, NT>(bizExp, pageIndex, pageSize, showCols, useDatabase, nolock, connMode, sqlVersion);
        }

        public static MPageData<NT> Get<T, NT>(AiExpConditions<T> expc, int pageIndex, int pageSize, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Get<T, NT>(expc, pageIndex, pageSize, showCols, useDatabase, nolock, connMode, sqlVersion);
        }

        /// <summary>
        /// SQL分页查询 - 按条件查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="where">搜索条件 - Sql Where</param>
        /// <param name="orderBy">排序 - Sql Order By</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="param">参数（传入的参数必须参数化）</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <param name="sqlVersion">Sql版本（影响分页性能）</param>
        /// <returns></returns>
        public static MPageData<T> GetWhere<T>(string where, string orderBy, int pageIndex, int pageSize, object param = null, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.GetWhere<T>(where, orderBy, pageIndex, pageSize, param, showCols, useDatabase, nolock, connMode, sqlVersion);
        }

        /// <summary>
        /// SQL分页查询 - 按条件查询
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <typeparam name="NT">映射的实体</typeparam>
        /// <param name="where">搜索条件 - Sql Where</param>
        /// <param name="orderBy">排序 - Sql Order By</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="param">参数（传入的参数必须参数化）</param>
        /// <param name="showCols">需查询的字段，默认为（*）全部</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <param name="connMode">读库/写库（默认为读库）</param>
        /// <param name="sqlVersion">Sql版本（影响分页性能）</param>
        /// <returns></returns>
        public static MPageData<NT> GetWhere<T, NT>(string where, string orderBy, int pageIndex, int pageSize, object param = null, string showCols = "*", string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.GetWhere<T, NT>(where, orderBy, pageIndex, pageSize, param, showCols, useDatabase, nolock, connMode, sqlVersion);
        }

        public static IList<MLeftJoin> LeftJoin<TLeft, TRight>(string On, string leftCols = "*", string rightCols = "*", bool nolock = false)
        {
            List<MLeftJoin> list = new List<MLeftJoin>();

            list.Add(DataBase.Instance.LeftJoin<TLeft, TRight>(null, On, leftCols, rightCols, nolock));

            return list;
        }

        public static IList<MLeftJoin> LeftJoin<TLeft, TRight>(this IList<MLeftJoin> list, string On, string rightCols = "*", bool nolock = false)
        {
            list.Add(DataBase.Instance.LeftJoin<TLeft, TRight>(list, On, "*", rightCols, nolock));

            return list;
        }

        public static string GetSql(this IList<MLeftJoin> list, string wheres, string orderBy)
        {
            return DataBase.Instance.GetSql(list, wheres, orderBy);
        }

        public static MGetSql GetSql(this IList<MLeftJoin> list, string wheres, string orderBy, int pageIndex, int pageSize)
        {
            return DataBase.Instance.GetSql(list, wheres, orderBy, pageIndex, pageSize);
        }

        public static IList<TReturn> Get<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
            where TSecond : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode);
        }

        public static IList<TReturn> Get<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
            where TSecond : new()
            where TThird : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode);
        }

        public static IList<TReturn> Get<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
            where TSecond : new()
            where TThird : new()
            where TFourth : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode);
        }

        public static IList<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
            where TSecond : new()
            where TThird : new()
            where TFourth : new()
            where TFifth : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode);
        }

        public static IList<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
            where TSecond : new()
            where TThird : new()
            where TFourth : new()
            where TFifth : new()
            where TSixth : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode);
        }

        public static IList<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read)
            where TSecond : new()
            where TThird : new()
            where TFourth : new()
            where TFifth : new()
            where TSixth : new()
            where TSeventh : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode);
        }

        public static MPageData<TReturn> Get<TFirst, TSecond, TReturn>(MGetSql sql, Func<TFirst, TSecond, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
            where TSecond : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode, sqlVersion);
        }

        public static MPageData<TReturn> Get<TFirst, TSecond, TThird, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
            where TSecond : new()
            where TThird : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode, sqlVersion);
        }

        public static MPageData<TReturn> Get<TFirst, TSecond, TThird, TFourth, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
            where TSecond : new()
            where TThird : new()
            where TFourth : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode, sqlVersion);
        }

        public static MPageData<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
            where TSecond : new()
            where TThird : new()
            where TFourth : new()
            where TFifth : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode, sqlVersion);
        }

        public static MPageData<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
            where TSecond : new()
            where TThird : new()
            where TFourth : new()
            where TFifth : new()
            where TSixth : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode, sqlVersion);
        }

        public static MPageData<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(MGetSql sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, EConnectionMode ConnMode = EConnectionMode.Read, ESqlVersion sqlVersion = ESqlVersion.SqlServer2012)
            where TSecond : new()
            where TThird : new()
            where TFourth : new()
            where TFifth : new()
            where TSixth : new()
            where TSeventh : new()
        {
            return DataBase.Instance.Get(sql, map, param, ConnMode, sqlVersion);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="procName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <param name="configDBName">配置文件中的数据库字符串对应的KEY，去除Read或Write</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static IList<T> RunProc<T>(string procName, object param, string configDBName, string useDatabase = null, string useServer = null)
        {
            return DataBase.Instance.RunProc<T>(procName, param, configDBName, useDatabase, useServer);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="configDBName">配置文件中的数据库字符串对应的KEY，去除Read或Write</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static IList<T> RunSql<T>(string sql, object param, string configDBName, string useDatabase = null, string useServer = null)
        {
            return DataBase.Instance.RunSql<T>(sql, param, configDBName, useDatabase, useServer);
        }

        /// <summary>
        /// 执行SQL语句 - 带分页
        /// </summary>
        /// <typeparam name="T">表名</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="orderBy">排序</param>
        /// <param name="param">参数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">行数</param>
        /// <param name="configDBName">配置文件中的数据库字符串对应的KEY，去除Read或Write</param>
        /// <param name="useDatabase">使用指定数据库名称</param>
        /// <returns></returns>
        public static MPageData<T> RunSql<T>(string sql, string orderBy, object param, int pageIndex, int pageSize, string configDBName, string useDatabase = null)
        {
            return DataBase.Instance.RunSql<T>(sql, orderBy, param, pageIndex, pageSize, configDBName, useDatabase);
        }

        /// <summary>
        /// SQL批提交
        /// </summary>
        /// <param name="list">工作单元</param>
        /// <param name="isTransactionScope">是否开启跨库事务</param>
        public static int Submit(this IList<MSqlData> list, bool isTransactionScope = false)
        {
            return DataBase.Instance.Submit(list);
        }

        public static decimal Sum<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string sumCol, string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Sum(bizExp, sumCol, useDatabase, nolock, connMode);
        }

        public static decimal Sum<T>(AiExpConditions<T> expc, string sumCol, string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Sum(expc, sumCol, useDatabase, nolock, connMode);
        }

        public static int Count<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp, string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Count(bizExp, useDatabase, nolock, connMode);
        }

        public static int Count<T>(AiExpConditions<T> expc, string useDatabase = null, EConnectionMode connMode = EConnectionMode.Read)
        {
            //读库加nolock，写库不加nolock
            bool nolock = (connMode == EConnectionMode.Read ? true : false);

            return DataBase.Instance.Count(expc, useDatabase, nolock, connMode);
        }

        public static IList<MKeyValue> Add(this IList<MKeyValue> list, string Key, object Value)
        {
            list.Add(new MKeyValue() { Key = Key, Value = Value });

            return list;
        }

        public static string Join<TLeft, TRight>(
            Expression<Func<TLeft, TRight, bool>> on
            , Expression<Func<TLeft, TRight, bool>> where
            , Expression<Func<IQueryable<TLeft>, IQueryable<TLeft>>> bizExp
            )
        //where TRight : new()
        {
            // Get<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> bizExp

            return on.ToString() + "," + where.ToString() + "," + bizExp.ToString();

            //return list;
        }

        public static IQueryable<T1> Where<T1, T2>(this IQueryable<T1> source, Expression<Func<T1, T2, bool>> predicate)
        {
            return null;
        }

        public static IOrderedQueryable<T1> OrderBy2<T1, T2, TKey>(this IQueryable<T1> source, Expression<Func<T1, T2, TKey>> keySelector)
        {
            return null;
        }

        //public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector);

        public static IList<MDynamicParameter> Add(this IList<MDynamicParameter> list, string name, object value, DbType? dbType = null, int? size = null)
        {
            list.Add(new MDynamicParameter() { name = name, value = value, dbType = dbType, size = size });

            return list;
        }

        /// <summary>
        /// 实体类Mapping
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="NT"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static NT MapTo<T, NT>(this T source)
            where T : class
            where NT : class
        {
            if (source == null) return default(NT);
            var config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<T, NT>());
            var mapper = config.CreateMapper();
            return mapper.Map<NT>(source);
        }

        /// <summary>
        /// 实体类Mapping
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="NT"></typeparam>
        /// <param name="source"></param>
        /// <param name="configExp"></param>
        /// <returns></returns>
        public static NT MapTo<T, NT>(this T source, Action<AutoMapper.IMapperConfigurationExpression> configExp)
            where T : class
            where NT : class
        {
            if (source == null) return default(NT);
            var config = new AutoMapper.MapperConfiguration(configExp != null ? configExp : cfg => cfg.CreateMap<T, NT>());
            var mapper = config.CreateMapper();
            return mapper.Map<NT>(source);
        }

        /// <summary>
        /// 实体类列表Mapping
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="NT"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<NT> MapTo<T, NT>(this List<T> source)
            where T : class
            where NT : class
        {
            if (source == null) return new List<NT>();
            var config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<T, NT>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<NT>>(source);
        }

        /// <summary>
        /// 实体类列表Mapping
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="NT"></typeparam>
        /// <param name="source"></param>
        /// <param name="configExp"></param>
        /// <returns></returns>
        public static List<NT> MapTo<T, NT>(this List<T> source, Action<AutoMapper.IMapperConfigurationExpression> configExp)
            where T : class
            where NT : class
        {
            if (source == null) return new List<NT>();
            var config = new AutoMapper.MapperConfiguration(configExp != null ? configExp : cfg => cfg.CreateMap<T, NT>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<NT>>(source);
        }

        /// <summary>
        /// 创建 ORMFactory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbKey">强制指定链接字符串的KEY【不动态获取DBKEY，后续赋值的都无效】</param>
        /// <returns></returns>
        public static ORMFactory<T> Build<T>(object dbKey = null)
        {
            Type type = typeof(ORMFactory<T>);
            object[] parameters = new object[] { dbKey };
            object obj = Activator.CreateInstance(type, parameters);
            return obj as ORMFactory<T>;
        }

        /// <summary>
        /// 创建 ORMFactory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">数据库名称</param>
        /// <param name="dbKey">强制指定链接字符串的KEY【不动态获取DBKEY，后续赋值的都无效】</param>
        /// <returns></returns>
        public static ORMFactory<T> Build<T>(string database, object dbKey = null)
        {
            Type type = typeof(ORMFactory<T>);
            object[] parameters = new object[] { database, dbKey };
            object obj = Activator.CreateInstance(type, parameters);
            return obj as ORMFactory<T>;
        }

        /// <summary>
        /// 创建 ORMFactory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="server">服务器IP</param>
        /// <param name="database">数据库名称</param>
        /// <param name="dbKey">强制指定链接字符串的KEY【不动态获取DBKEY，后续赋值的都无效】</param>
        /// <returns></returns>
        public static ORMFactory<T> Build<T>(string server, string database, object dbKey = null)
        {
            Type type = typeof(ORMFactory<T>);
            object[] parameters = new object[] { server, database, dbKey };
            object obj = Activator.CreateInstance(type, parameters);
            return obj as ORMFactory<T>;
        }

    }
}
