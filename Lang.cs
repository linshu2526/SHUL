using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace SHUL
{
    public enum LangType
    {
        cn,
        fr,
        en
    }
    public class Lang
    {
        private static XmlNode Node(string path, string cachename)
        {
            string xmlFilePath = path;

            XmlNode _node = (XmlNode)SHUL.Caches.Get(cachename);
            if (_node == null)
            {

                XmlDocument doc = new XmlDocument();
                doc.Load(HttpContext.Current.Server.MapPath(xmlFilePath));
                XmlNodeList nodeList = doc.SelectNodes("/lang");
                _node = nodeList[0];
                SHUL.Caches.Set(cachename, _node);
            }
            return _node;
        }
        public static void ClearCaches()
        {
            Caches.Clear("~/Resource/fr.xml");
            Caches.Clear("~/Resource/cn.xml");
            Caches.Clear("~/Resource/en.xml");
            
        }
        public static string CurrLang
        {
            get
            {
                switch (LSParse.ToString(HttpContext.Current.Session["lang"]))
                {
                    case "fr":
                        return "~/Resource/fr.xml";
                    case "cn":
                        return "~/Resource/cn.xml";
                    case "en":
                        return "~/Resource/en.xml";
                    default:
                        return "~/Resource/cn.xml";
                }
               
            }
        }
        public static void SetLange(string langType)
        {
            HttpContext.Current.Session["lang"] = langType;
        }
        public static void SetLange(LangType langType)
        {
            HttpContext.Current.Session["lang"] = langType.ToString();
        }
        public static XmlNode GetNode()
        {
            return Node(CurrLang, CurrLang);
        }
        
        public static string GetCurrPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath(CurrLang);
            }
        }
        public static void SetCN_Node(string key, string name)
        {
            XmlDocument doc = new XmlDocument();
            string path = GetCurrPath;
            doc.Load(path);
            XmlNodeList nodeList = doc.SelectNodes("/lang");
            try
            {
                XmlElement xe= doc.CreateElement(key);//创建一个节点 
                xe.InnerText = name;
                nodeList[0].AppendChild(xe);
                doc.Save(path);
            }
            catch (Exception er)
            {
                //throw new Exception(er.ToString());
            }

        }

        public static string Get(string key)
        {

            XmlNode node = GetNode();

            if (node == null)
            {
                return key + "<----未匹配!!";
            }
            return node.SelectSingleNode(key).InnerText;

        }
    }
}
