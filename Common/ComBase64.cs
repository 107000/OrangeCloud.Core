using System;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComBase64
    {
        public static string StringToBase64(string Text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Text);

            return Convert.ToBase64String(bytes);
        }

        public static string ByteToBase64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] Base64ToByte(string data)
        {
            return Convert.FromBase64String(data);
        }

        public static string Base64ToString(string Text)
        {
            byte[] outputb = Convert.FromBase64String(Text);

            string orgStr = Encoding.UTF8.GetString(outputb);

            return orgStr;
        }

        public static string StringToBase64ToGb2312(string Text)
        {
            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(Text);

            return Convert.ToBase64String(bytes);
        }

        public static string Base64ToStringToGb2312(string Text)
        {
            byte[] outputb = Convert.FromBase64String(Text);

            string orgStr = Encoding.GetEncoding("gb2312").GetString(outputb);

            return orgStr;
        }
    }
}
