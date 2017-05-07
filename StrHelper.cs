using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SHUL
{
    public class StrHelper
    {
        ///   <summary>
        ///   去除HTML标记
        ///   </summary>
        ///   <param   name=”NoHTML”>包括HTML的源码   </param>
        ///   <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
            RegexOptions.IgnoreCase);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"–>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!–.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
            RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        /// <summary>
        /// 返回根据前后标示返回筛选的string数组
        /// </summary>
        /// <param name="source">元数据</param>
        /// <param name="start">开始标签</param>
        /// <param name="isIncludeStart">返回的数据是否要添加回开始标签</param>
        /// <param name="end">结束标签</param>
        /// <param name="isIncludeEnd">返回的数据是否要添加回结束标签</param>
        /// <param name="fliterStr">过滤条件,例子: .jpg,.png, 将不显示过滤条件的数据</param>
        /// <param name="IncludefliterStr">选择是, 将只显示过滤条件的数据</param>
        /// <param name="RemoveAt0">是否移除根据第一个开始标签标识的前面第一个数据</param>
        /// <param name="isUnescape">是否需要对url转码,针对淘宝</param>
        /// <returns></returns>
        public static List<string> SplitStr(string source,
            string start, bool isIncludeStart, string end, bool isIncludeEnd,
            string fliterStr, bool IncludefliterStr, bool RemoveAt0, bool isUnescape)
        {
            List<string> _SplitStr = new List<string>();

            string tagspiltrp1 = "L--inshu6667778***((225566(";
            string tagspiltrp2 = "$";
            char tagspiltrp2c = '$';
            source = source.Replace(tagspiltrp2, tagspiltrp1);
            source = source.Replace(start, tagspiltrp2);
            string[] sourcelist = source.Split(char.Parse(tagspiltrp2));
            ArrayList alsource = new ArrayList(sourcelist);
            if (RemoveAt0) alsource.RemoveAt(0);
            source = "";
            string[] txtfliter = null;
            if (fliterStr != null)
                txtfliter = fliterStr.TrimEnd(',').Split(',');
            for (int i = 0; i < alsource.Count; i++)
            {
                string temp = ((string)alsource[i]).Replace(end, tagspiltrp2);
                temp = (isIncludeStart ? start : "") +
                    temp.Split(tagspiltrp2c)[0].Replace(tagspiltrp1, tagspiltrp2)
                    + (isIncludeEnd ? end : "");
                if (isUnescape)
                    temp = System.Text.RegularExpressions.Regex.Unescape(temp);


                if (txtfliter == null || txtfliter.Length == 0)
                {
                    _SplitStr.Add(temp);
                }
                else
                {
                    for (int j = 0; j < txtfliter.Length; j++)
                    {

                        if (IncludefliterStr)
                        {
                            if (temp.IndexOf(txtfliter[j]) > -1)
                            {
                                _SplitStr.Add(temp);

                            }
                        }
                        else
                        {
                            if (temp.IndexOf(txtfliter[j]) < 0)
                            {
                                _SplitStr.Add(temp);

                            }
                        }

                    }
                }
            }



            return _SplitStr;
        }
        /// <summary>
        /// 根据前后标示获取中间内容
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string SplitStr(string source, string start, string end, bool RemoveAt0)
        {

            string tagspiltrp1 = "L--inshu6667778***((225566(";
            string tagspiltrp2 = "$";
            source = source.Replace(tagspiltrp2, tagspiltrp1);
            source = source.Replace(start, tagspiltrp2);
            string[] sourcelist = source.Split(char.Parse(tagspiltrp2));
            ArrayList alsource = new ArrayList(sourcelist);
            if (RemoveAt0) alsource.RemoveAt(0);
            source = "";
            for (int i = 0; i < alsource.Count; i++)
            {
                string temp = start;
                if (i == 0)
                    temp = "";
                source += temp + alsource[i].ToString();
            }
            source = source.Replace(end, tagspiltrp2);
            source = source.Split(char.Parse(tagspiltrp2))[0];
            source = source.Replace(tagspiltrp1, tagspiltrp2);
            return source;
        }

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

        public static string JavaScript(string js)
        {
            string result = string.Empty;


            result = string.Format("<script type=\"text/javascript\">{0}</script>", js);


            return result;
        }
        public static string JSRedirect(string url)
        {
            string result = string.Empty;


            result = string.Format("<script type=\"text/javascript\"> top.location = \"{0}\"</script>", url);


            return result;
        }
    }
}
