using Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OrangeCloud.Core
{
    public static class ComPublicStatic
    {
        public static string Safe(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
                return str.Replace("'", "");

            return str;
        }

        public static string Save(string sql)
        {
            var saveLog = Base.FR_Log_IsSave;

            var saveLogPrefix = Base.FR_Log_Prefix;

            var savePath = Base.FR_Log_SavePath;

            if (saveLog == true)
            {
                if (string.IsNullOrWhiteSpace(savePath))
                    savePath = "d:\\Log\\";

                //System.IO.File.AppendAllText(savePath + DateTime.Now.ToString("yyyyMMdd") + ".txt", DateTime.Now.ToString() + ":\r\n" + sql + "\r\n");

                using (FileStream fs = new FileStream(savePath + saveLogPrefix + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    fs.Write(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString() + ":\r\n" + sql + "\r\n"));
                }
            }

            return sql;
        }

        public static MSqlData Save(string sql, DynamicParameters param)
        {
            Save(sql + "\r\n" + param.JsonSerialize());

            return new MSqlData() { Sql = sql, Param = param };
        }

        public static string GetSign(EMath sign)
        {
            if (sign == EMath.ADD)
                return "+";
            else if (sign == EMath.SUBTRACT)
                return "-";
            else if (sign == EMath.MULTIPLY)
                return "*";
            else if (sign == EMath.DIVIDE)
                return "/";

            return "";
        }

        public static bool IsXEnum(this Type t)
        {
            if (t == null)
                return false;

            if (t.IsEnum == true)
                return true;

            Type u = Nullable.GetUnderlyingType(t);

            return (u != null) && u.IsEnum;
        }

    }
}
