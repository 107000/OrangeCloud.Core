using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComCookie
    {
        public static void SetCookie(string CookieName, string CookieValue, int Minute = 0, string Domain = null)
        {
            if (Domain == null)
                Domain = ComConfig.AppSettings["RootDomain"];

            var cookieOption = new Microsoft.AspNetCore.Http.CookieOptions();

            if (Minute > 0)
                cookieOption.Expires = DateTime.Now.AddMinutes(Minute);

            cookieOption.Path = "/";

            cookieOption.Domain = Domain;

            HttpContext.Current.Response.Cookies.Append(CookieName, CookieValue, cookieOption);
        }

        /// <summary>
        /// 取Cookie
        /// </summary>
        /// <param name="CookieName">cookie名称</param>
        /// <param name="strKey">cookie键</param>
        /// <returns></returns>
        public static string GetCookie(string CookieName)
        {
            string CookieValue = "";

            HttpContext.Current.Request.Cookies.TryGetValue(CookieName, out CookieValue);

            return CookieValue;
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <returns></returns>
        public static void DelCookie(string CookieName)
        {
            string Domain = ComConfig.AppSettings["RootDomain"];

            var cookieOption = new Microsoft.AspNetCore.Http.CookieOptions();

            cookieOption.Domain = Domain;

            HttpContext.Current.Response.Cookies.Delete(CookieName, cookieOption);
        }
    }
}
