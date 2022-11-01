using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComSHA
    {
        public static string SHA256Encrypt(string str)
        {
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(str);
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] retVal = sha256.ComputeHash(bytValue);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetSHA256HashFromString() fail,error:" + ex.Message);
            }
        }
    }
}
