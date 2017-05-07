using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SHUL
{
    public class RequestHelper
    {
        const string sUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        const string sContentType = "application/x-www-form-urlencoded";
        const string sRequestEncoding = "utf-8";
        const string sResponseEncoding = "utf-8";
        public static string GET = "GET";
        public static string POST = "POST";
        public static string UTF8 = "UTF-8";
        public static string ContentType_urlencoded = "application/x-www-form-urlencoded";
        public static string ContentType_json = "application/json";

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <param name="rp">编码</param>
        ///  <param name="re">编码</param>
        /// <returns>服务器响应</returns>
        public static string PostDataToUrl(string data, string url, string RequestEncoding, string ResponseEncoding)
        {
            Encoding encoding = Encoding.GetEncoding(RequestEncoding);
            byte[] bytesToPost = encoding.GetBytes(data);
            return PostDataToUrl(bytesToPost, url, ResponseEncoding);
        }
        public static string RequestUrl()
        {
            string allkeys = "";
            if (HttpContext.Current.Request.Params != null)
            {
                if (HttpContext.Current.Request.Params.AllKeys != null)
                {
                    for (int i = 0; i < HttpContext.Current.Request.Params.AllKeys.Length; i++)
                    {
                        allkeys += "<br/>" + HttpContext.Current.Request.Params.AllKeys[i] + ":" + HttpContext.Current.Request[HttpContext.Current.Request.Params.AllKeys[i]];
                    }
                }
            }
            
            return HttpContext.Current.Request.Url.OriginalString + "----" + allkeys;
        }

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string PostDataToUrl(byte[] data, string url, string rp)
        {
            #region 创建httpWebRequest对象
            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(
                    string.Format("Invalid url string: {0}", url)
                    );
            }
            #endregion

            #region 填充httpWebRequest的基本信息
            httpRequest.UserAgent = sUserAgent;
            httpRequest.ContentType = sContentType;
            httpRequest.Method = "POST";
            #endregion

            #region 填充要post的内容
            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            #endregion

            #region 发送post请求到服务器并读取服务器返回信息
            Stream responseStream;
            try
            {
                responseStream = httpRequest.GetResponse().GetResponseStream();
            }

            catch (Exception e)
            {
                // log error WinForm调试方式
                //Console.WriteLine(
                //    string.Format("POST操作发生异常：{0}", e.Message)
                //    );
                throw e;
            }
            #endregion

            #region 读取服务器返回信息
            string stringResponse = string.Empty;
            using (StreamReader responseReader =
                new StreamReader(responseStream, Encoding.GetEncoding(rp)))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();
            #endregion
            return stringResponse;
        }

        /// <summary>
        /// 将字符编码为Base64
        /// </summary>
        /// <param name="encodeType">编码方式</param>
        /// <param name="input">明文字符</param>
        /// <returns>字符串</returns>
        public static string EncodeBase64(string encodeType, string input)
        {
            string result = string.Empty;
            byte[] bytes = Encoding.GetEncoding(encodeType).GetBytes(input);
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = input;
            }
            return result;
        }
        /// <summary>
        /// 将字符编码为Base64
        /// </summary>
        /// <param name="encodeType">编码方式</param>
        /// <param name="input">明文字符</param>
        /// <returns>字符串</returns>
        public static string DecodeBase64(string encodeType, string input)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(input);
            try
            {
                decode = Encoding.GetEncoding(encodeType).GetString(bytes);
            }
            catch
            {
                decode = input;
            }
            return decode;
        }

        public static string GetWebException(WebException e, bool IsHTML_BR)
        {
            if (IsHTML_BR)
            {
                string result = "";
                result += ("<br/>出现了一个WebException错误" +
                                         "<br/>错误信息 :" + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    result += string.Format("<br/>错误状态: {0}", ((HttpWebResponse)e.Response).StatusCode);
                    result += string.Format("<br/>错误描述 : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                    using (Stream data = e.Response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        result += ("<br/>反馈:" + text);
                    }
                }
                return result;
            }
            else
            {
                string result = "";
                result += ("出现了一个WebException错误," +
                                         "错误信息 :" + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    result += string.Format("错误状态: {0}", ((HttpWebResponse)e.Response).StatusCode);
                    result += string.Format("错误描述 : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                    using (Stream data = e.Response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        result += ("反馈:" + text);
                    }
                }
                return result;
            }

        }

        public static string GetResponse(string _url, string _ContentType, string method, string postdata,
           WebHeaderCollection headers, string encoding)
        {
            string result = "";
            try
            {
                if (method == GET)
                {
                    if (postdata != null && postdata != "")
                    {
                        _url = _url + "?" + postdata;
                    }

                }
                WebRequest webRequest = WebRequest.Create(_url);
                HttpWebRequest httpRequest = webRequest as HttpWebRequest;
                if (_ContentType != null && _ContentType != "")
                {
                    httpRequest.ContentType = _ContentType;
                }
                if (headers != null)
                {
                    httpRequest.Headers.Add(headers);
                }
                httpRequest.Method = method;
                if (method == POST)
                {
                    if (postdata != null && postdata != "")
                    {
                        using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                        {
                            streamWriter.Write(postdata);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                    }

                }
                Stream responseStream;
                WebResponse response = httpRequest.GetResponse();
                responseStream = response.GetResponseStream();
                using (StreamReader responseReader =
                    new StreamReader(responseStream, Encoding.GetEncoding(encoding)))
                {
                    result = responseReader.ReadToEnd();
                }
                responseStream.Close();
                return result;
            }
            catch (WebException e)
            {
                string exstr = "";
                exstr += RequestHelper.GetWebException(e, true);
                return exstr;
            }
            catch (Exception e)
            {
                return (e.Message);
            }

        }
    }
}
