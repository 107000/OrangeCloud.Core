using System;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComHex
    {
        /// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <param name="fenge">是否带%符号</param>
        /// <returns></returns>
        public static string ToHex(string str, string charset, bool isSplit)
        {
            //if ((s.Length % 2) != 0)
            //{
            //    s += " ";//空格
            //    //throw new ArgumentException("s is not valid chinese string!");
            //}

            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);

            byte[] bytes = chs.GetBytes(str);

            StringBuilder sb = new StringBuilder();
            //string str = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                if (isSplit)
                {
                    //str += string.Format("{0}", ",");
                    sb.Append("%");
                }

                //str += string.Format("{0:X}", bytes[i]);
                sb.AppendFormat("{0:X}", bytes[i]);
            }

            //return str.ToLower();
            return sb.ToString();
        }

        /// <summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");

            //hex = hex.Replace(",", "");
            //hex = hex.Replace("\n", "");
            //hex = hex.Replace("\\", "");
            //hex = hex.Replace(" ", "");
            hex = hex.Replace("%", "");

            //if (hex.Length % 2 != 0)
            //{
            //    hex += "20";//空格
            //    //throw new ArgumentException("hex is not a valid number!", "hex");
            //}

            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }

            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);


            return chs.GetString(bytes);
        }


        public static byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
                hex = "0" + hex;

            byte[] arr = new byte[hex.Length >> 1];
            for (int i = 0; i < arr.Length; ++i)
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));

            return arr;
        }

        private static int GetHexVal(char hex)
        {
            int val = hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
