using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System;
namespace SHUL
{
    public class Router
    {
        Encoding gb2312 = Encoding.GetEncoding(936);//路由器的web管理系统默认编码为gb2312
        /// <summary>
        /// 使用HttpWebRequest对象发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding">编码</param>
        /// <param name="cache">凭证</param>
        /// <returns></returns>
        private static string SendRequest(string url, Encoding encoding, CredentialCache cache)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            if (cache != null)
            {
                request.PreAuthenticate = true;
                request.Credentials = cache;
            }
            string html = null;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader srd = new StreamReader(response.GetResponseStream(), encoding);
                html = srd.ReadToEnd();
                srd.Close();
                response.Close();
            }
            catch (Exception ex) { html = "FALSE" + ex.Message; }
            return html;
        }
        /// <summary>
        /// 获取路由MAC和外网IP地址
        /// </summary>
        /// <param name="RouterIP">路由IP地址，就是网关地址了，默认192.168.1.1</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Passowrd">密码</param>
        /// <returns></returns>
        private string LoadMACWanIP(string RouterIP, string UserName, string Passowrd)
        {
            CredentialCache cache = new CredentialCache();
            string url = "http://" + RouterIP + "/userRpm/StatusRpm.htm";
            cache.Add(new Uri(url), "Basic", new NetworkCredential(UserName, Passowrd));
            return SendRequest(url, gb2312, cache);
        }
    }
}
