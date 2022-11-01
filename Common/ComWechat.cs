using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core.Common
{
    public class ComWechat
    {
        /// <summary>
        /// access_token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string access_token(string appId, string appSecret)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appSecret;

            var json = ComRequest.GetWeb(url);

            return JsonConvert.DeserializeObject<MAccess_Token>(json).access_token;
        }

        public static MUserInfo userInfo(string appId, string appSecret)
        {
            string access_Token = access_token(appId, appSecret);

            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_Token + "&openid=OPENID";

            var json = ComRequest.GetWeb(url);

            return JsonConvert.DeserializeObject<MUserInfo>(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static MUserInfo userInfo(string access_token)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=OPENID";

            var json = ComRequest.GetWeb(url);

            return JsonConvert.DeserializeObject<MUserInfo>(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string SendTextMessage(string access_token, string openId, string content)
        {
            string data = "{\"touser\":\"" + openId + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + content + "\"}}";

            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + access_token;

            return ComRequest.PostWeb(url, data);
        }

        /// <summary>
        /// 网页授权 - 第一步 - 返回获取Code的验证地址,通过地址跳转获取Code,如取消验证则无Code，所以要判断是否为NULL
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="toUrl"></param>
        public static string OAuthUrl(string appId, string toUrl, string tgId, string scope = "snsapi_base")
        {
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appId + "&redirect_uri=" + ComUrlEncode.UrlEncode(toUrl) + "&response_type=code&scope=" + scope + "&state=" + tgId + "#wechat_redirect";

            return url;
        }

        /// <summary>
        /// 网页授权 - 第二步 - 返回用户基本信息（OpenId, AccessToken）
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static MOAuthAccessToken OAuthAccess_token(string appId, string appSecret, string code = null)
        {
            if (code == null)
                code = HttpContext.Current.Request.Query["code"];

            if (code == null)
                return null;

            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appId + "&secret=" + appSecret + "&code=" + code + "&grant_type=authorization_code";

            var json = ComRequest.GetWeb(url);

            return JsonConvert.DeserializeObject<MOAuthAccessToken>(json);
        }

        public static MUserInfo OAuthUserInfo(string access_token, string openid, string lang = "zh_CN")
        {
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=" + lang;

            var json = ComRequest.GetWeb(url);

            return JsonConvert.DeserializeObject<MUserInfo>(json);
        }

        public static MShortUrl ShortUrl(string access_token, string long_url)
        {
            string data = "{\"action\":\"long2short\",\"long_url\":\"" + long_url + "\"}";

            string url = string.Format("https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}", access_token);

            var Html = ComRequest.PostWeb(url, data, "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", null, "", Encoding.GetEncoding("utf-8"));

            return JsonConvert.DeserializeObject<MShortUrl>(Html);
        }

        /// <summary>
        /// 获取微信自定义 + 个性化菜单
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetMenu(string access_token)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token);

            return ComRequest.GetWeb(url);
        }

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="menuJson"></param>
        /// <returns></returns>
        public static string CreateMenu(string access_token, string menuJson)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token);

            return ComRequest.PostWeb(url, menuJson);
        }

        /// <summary>
        /// 删除全部菜单（包含自定义菜单和个性化菜单）
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string DeleteMenu(string access_token)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", access_token);

            return ComRequest.GetWeb(url);
        }

        /// <summary>
        /// 创建个性化菜单（必须要先创建自定义菜单）
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="menuJson"></param>
        /// <returns></returns>
        public static string CreateConditionalMenu(string access_token, string menuJson)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token={0}", access_token);

            return ComRequest.PostWeb(url, menuJson);
        }

        /// <summary>
        /// 删除指定ID的个性化菜单
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public static string DeleteConditionalMenu(string access_token, string menuJson)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delconditional?access_token={0}", access_token);

            return ComRequest.PostWeb(url, menuJson);
        }

        /// <summary>
        /// 测试个性化菜单匹配结果
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string GetConditionalMenu(string access_token, string menuJson)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/trymatch?access_token={0}", access_token);

            return ComRequest.PostWeb(url, menuJson);
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string CreateTags(string access_token, string tag)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/create?access_token={0}", access_token);

            return ComRequest.PostWeb(url, tag);
        }

        /// <summary>
        /// 获取公众号已创建的标签
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetTags(string access_token)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/get?access_token={0}", access_token);

            return ComRequest.GetWeb(url);
        }

        /// <summary>
        /// 编辑标签
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string UpdateTags(string access_token, string tag)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/update?access_token={0}", access_token);

            return ComRequest.PostWeb(url, tag);
        }

        /// <summary>
        /// 根据标签Id删除标签
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public static string DeleteTags(string access_token, string tagJson)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/delete?access_token={0}", access_token);

            return ComRequest.PostWeb(url, tagJson);
        }

        /// <summary>
        /// 获取标签下粉丝列表
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="tagJson"></param>
        /// <returns></returns>
        public static string GetTagUserList(string access_token, string tagJson)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/tag/get?access_token={0}", access_token);

            return ComRequest.PostWeb(url, tagJson);
        }


        /// <summary>
        /// 批量为用户打标签
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid_list"></param>
        /// <returns></returns>
        public static string BatchTagging(string access_token, string openIdList)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?access_token={0}", access_token);

            return ComRequest.PostWeb(url, openIdList);
        }

        /// <summary>
        /// 批量为用户取消标签
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openIdList"></param>
        /// <returns></returns>
        public static string BatchUnTagging(string access_token, string openIdList)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/members/batchuntagging?access_token={0}", access_token);

            return ComRequest.PostWeb(url, openIdList);
        }

        /// <summary>
        /// 获取用户身上的标签列表
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static string GetUserTagList(string access_token, string openId)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/getidlist?access_token={0}", access_token);

            return ComRequest.PostWeb(url, openId);
        }
    }

}
