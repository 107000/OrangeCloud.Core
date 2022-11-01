using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComGUID
    {
        /// <summary>
        /// 生成36位大写GUID
        /// </summary>
        public static string GetGuid
        {
            get
            {
                return Guid.NewGuid().ToString().ToUpper();
            }
        }

        /// <summary>
        /// 生成16位唯一字符串
        /// </summary>
        public static string GetGuidByNum16
        {
            get
            {
                long i = 1;
                foreach (byte b in Guid.NewGuid().ToByteArray())
                {
                    i *= ((int)b + 1);
                }
                return string.Format("{0:x}", i - DateTime.Now.Ticks).ToLower();
            }
        }

        /// <summary>
        /// 生成19位唯一数字字符串
        /// </summary>
        public static string GetGuidInt
        {
            get
            {
                return BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0).ToString();
            }
        }
    }
}
