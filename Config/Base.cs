using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core
{
    public class Base
    {
        /// <summary>
        /// Cookie - 登录
        /// </summary>
        public static readonly string FR_Cookie_Login = ComConfig.AppSettings["OrangeCloud.Cookie.Login"];

        /// <summary>
        /// KEY
        /// </summary>
        public static readonly string FR_Api_Key = ComConfig.AppSettings["OrangeCloud.Api.Key"];

        /// <summary>
        /// CPKey
        /// </summary>
        public static readonly string FR_Api_CPKey = ComConfig.AppSettings["OrangeCloud.Api.CPKey"];

        /// <summary>
        /// 使用的数据库
        /// </summary>
        public static readonly string FR_Core_DataTable_Type = ComConfig.AppSettings["OrangeCloud.Core.DataTable.Type"];

        /// <summary>
        /// 数据表主键名称
        /// </summary>
        public static readonly string FR_Core_DataTable_IDName = ComConfig.AppSettings["OrangeCloud.Core.DataTable.IDName"];

        /// <summary>
        /// 数据库Key前缀
        /// </summary>
        public static readonly string FR_Core_DatabaseKey_Prefix = ComConfig.AppSettings["OrangeCloud.Core.DatabaseKey.Prefix"];

        /// <summary>
        /// 数据库Key后缀
        /// </summary>
        public static readonly string FR_Core_DatabaseKey_Suffix = ComConfig.AppSettings["OrangeCloud.Core.DatabaseKey.Suffix"];

        #region AppSettings Key名称设置

        /// <summary>
        /// （AppSettings）是否把SQL记录到文本日志
        /// </summary>
        public static readonly bool FR_Log_IsSave = Convert.ToBoolean(ComConfig.AppSettings["OrangeCloud.Log.IsSave"]);

        /// <summary>
        /// （AppSettings）文本日志名称前缀
        /// </summary>
        public static readonly string FR_Log_Prefix = ComConfig.AppSettings["OrangeCloud.Log.Prefix"];

        /// <summary>
        /// （AppSettings）文本日志路径
        /// </summary>
        public static readonly string FR_Log_SavePath = ComConfig.AppSettings["OrangeCloud.Log.SavePath"];

        #endregion

        /// <summary>
        /// 主键名称
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public static string GetPrimaryKey(string tbName)
        {
            if (string.IsNullOrWhiteSpace(FR_Core_DataTable_IDName))
                return tbName + "ID";
            else
                return FR_Core_DataTable_IDName;
        }
    }
}
