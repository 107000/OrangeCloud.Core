using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComCode
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetCode(int minLength, int maxLength)
        {
            string[] arrCode = { "2", "3", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            StringBuilder sbCode = new StringBuilder();

            Random r = new Random();

            var forLength = r.Next(minLength, maxLength + 1);

            for (int i = 0; i < forLength; i++)
            {
                sbCode.Append(arrCode[r.Next(0, arrCode.Length)]);
            }

            return sbCode.ToString();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string GetCodeZM(int minLength, int maxLength)
        {
            string[] arrCode = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            StringBuilder sbCode = new StringBuilder();

            Random r = new Random();

            var forLength = r.Next(minLength, maxLength + 1);

            for (int i = 0; i < forLength; i++)
            {
                sbCode.Append(arrCode[r.Next(0, arrCode.Length)]);
            }

            return sbCode.ToString();
        }

        public static string GetCodeNum(int minLength, int maxLength)
        {
            string[] arrCode = { "0", "1", "2", "3", "5", "6", "7", "8", "9" };

            StringBuilder sbCode = new StringBuilder();

            Random r = new Random();

            var forLength = r.Next(minLength, maxLength + 1);

            for (int i = 0; i < forLength; i++)
            {
                sbCode.Append(arrCode[r.Next(0, arrCode.Length)]);
            }

            return sbCode.ToString();
        }
    }
}
