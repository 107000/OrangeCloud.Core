using System;

namespace OrangeCloud.Core
{
    public class SqlCommon
    {
        #region 辅助方法

        //获取数据库字符串的KEY
        public static string GetDBConncationKey(string TypeString, EConnectionMode ConnMode, bool IsOther = false)
        {
            if (!IsOther)
            {
                var database = GetDatabaseName(TypeString);

                //FLog + Read
                if (database == null)
                    return null;

                return Base.FR_Core_DatabaseKey_Prefix
                    + database
                    + Base.FR_Core_DatabaseKey_Suffix
                    + ConnMode.ToString();
            }
            else
                return Base.FR_Core_DatabaseKey_Prefix
                    + TypeString
                    + Base.FR_Core_DatabaseKey_Suffix
                    + ConnMode.ToString();
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <param name="TypeString"></param>
        /// <returns></returns>
        public static string GetDatabaseName(string TypeString)
        {
            var arrType = TypeString.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            //if (arrType.Length >= 2)
            //    return arrType[arrType.Length - 2];
            //else
            //    return null;

            if (arrType.Length < 2)
                return null;

            var arrIndex = Array.IndexOf(arrType, "DataBase");

            if (arrIndex != -1)
                return arrType[arrIndex + 1];

            arrIndex = Array.IndexOf(arrType, "Business");

            if (arrIndex != -1)
                return arrType[arrIndex + 1];

            return null;
        }

        //获取数据表名称
        public static string GetTableName(string TypeString)
        {
            var arrType = TypeString.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            if (arrType.Length <= 0)
                return null;

            return arrType[arrType.Length - 1];
        }

        //获得主键名称
        public static string GetPrimaryKey(string tbName)
        {
            return Base.GetPrimaryKey(tbName);
        }


        #endregion

    }
}
