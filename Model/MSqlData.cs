using Dapper;

namespace OrangeCloud.Core
{
    public class MSqlData
    {
        /// <summary>
        /// 自增长的流水ID
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 是否使用传入的数据库名,null=不使用
        /// </summary>
        public string UseDatabase { get; set; }

        /// <summary>
        /// Sql语句
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// 传入的参数
        /// </summary>
        public DynamicParameters Param { get; set; }
    }
}
