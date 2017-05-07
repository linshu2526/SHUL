using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SHUL
{
    public class BaseController : Controller
    {
        #region Private

        /// <summary>  
        /// HtmlTextWriter  
        /// </summary>  
        private HtmlTextWriter tw;
        /// <summary>  
        /// StringWriter  
        /// </summary>  
        private StringWriter sw;
        /// <summary>  
        /// StringBuilder  
        /// </summary>  
        private StringBuilder sb;
        /// <summary>  
        /// HttpWriter  
        /// </summary>  
        private HttpWriter output;

        #endregion

        /// <summary>  
        /// 压缩html代码  
        /// </summary>  
        /// <param name="text">html代码</param>  
        /// <returns></returns>  
        private static string Compress(string text)
        {
            Regex reg = new Regex(@"\s*(</?[^\s/>]+[^>]*>)\s+(</?[^\s/>]+[^>]*>)\s*");
            text = reg.Replace(text, m => m.Groups[1].Value + m.Groups[2].Value);

            reg = new Regex(@"(?<=>)\s|\n|\t(?=<)");
            text = reg.Replace(text, string.Empty);

            return text;
        }

        /// <summary>  
        /// 在执行Action的时候，就把需要的Writer存起来  
        /// </summary>  
        /// <param name="filterContext">上下文</param>  
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            sb = new StringBuilder();


            sw = new StringWriter(sb);
            tw = new HtmlTextWriter(sw);
            output = (HttpWriter)filterContext.RequestContext.HttpContext.Response.Output;
            filterContext.RequestContext.HttpContext.Response.Output = tw;

            base.OnActionExecuting(filterContext);


        }
        public static string Replace(string html)
        {
            MatchCollection matchs = Regex.Matches(html, @"\[#([\S\s]*?)#\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (matchs != null && matchs.Count > 0)
            {
                List<string> keys = new List<string>(matchs.Count);//记录已匹配过的

                System.Xml.XmlNode node = SHUL.Lang.GetNode();
                foreach (Match match in matchs)
                {
                    string text = match.Groups[0].Value;
                    string key = match.Groups[1].Value.Trim();
                    key = key.Replace(",", "");
                    if (!keys.Contains(key))
                    {
                        
                        keys.Add(key);
                        //if (dic.ContainsKey(key))
                        //{
                        //html = html.Replace(text, dic[key]);
                        
                        string th_str = key;
                        if (node.SelectSingleNode(key) != null)
                        {
                            th_str = node.SelectSingleNode(key).InnerText;
                        }
                        else
                        {
                            SHUL.Lang.SetCN_Node(key, key);
                            SHUL.Lang.ClearCaches();
                        }
                        html = html.Replace(text,th_str );
                        //}
                    }
                }
                keys = null;
                matchs = null;
            }
            return html;
        }

        /// <summary>  
        /// 在执行完成后，处理得到的HTML，并将他输出到前台  
        /// </summary>  
        /// <param name="filterContext"></param>  
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            string response = Replace(Compress(sb.ToString()));

            output.Write(response);


        }
    }
}
