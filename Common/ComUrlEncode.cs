using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComUrlEncode
    {
        //编码
        public static string UrlEncode(string str, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            if (str != null)
            {
                str = System.Web.HttpUtility.UrlEncode(str, encoding);

                return str;
            }
            else
            {
                return string.Empty;
            }
        }

        //解码
        public static string UrlDecode(string str, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            if (str != null)
            {
                str = System.Web.HttpUtility.UrlDecode(str, encoding);

                return str;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
