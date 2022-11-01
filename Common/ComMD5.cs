using System;
using System.Security.Cryptography;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComMD5
    {
        public static string Md5_32_Lower(string str)
        {
            byte[] hashvalue = (new MD5CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(str));

            return BitConverter.ToString(hashvalue).ToLower().Replace("-", "");
        }

        public static string Md5_32_Upper(string str)
        {
            byte[] hashvalue = (new MD5CryptoServiceProvider()).ComputeHash(Encoding.UTF8.GetBytes(str));

            return BitConverter.ToString(hashvalue).ToUpper().Replace("-", "");
        }

        public static string Md5_16_Lower(string str)
        {
            return Md5_32_Lower(str).Substring(8, 16);
        }

        public static string Md5_16_Upper(string str)
        {
            return Md5_32_Upper(str).Substring(8, 16);
        }
    }
}
