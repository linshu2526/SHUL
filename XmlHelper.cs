using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SHUL
{
    public class XmlHelper
    {
        XmlDocument lsXmlDoc;
        
        public XmlHelper(string xmlpath)
        {
            lsXmlDoc = new XmlDocument();
            lsXmlDoc.Load(xmlpath);
        }

        #region 反序列化
        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type">类型</param>  
        /// <param name="xml">XML字符串</param>  
        /// <returns></returns>  
        //public static object Deserialize(Type type, string xml)
        //{
        //    try
        //    {
        //        using (StringReader sr = new StringReader(xml))
        //        {
        //            XmlSerializer xmldes = new XmlSerializer(type);
        //            return xmldes.Deserialize(sr);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        throw;
        //    }
        //}
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string filepath)
        {
            object obj;
            try
            {
                using (StreamReader stream = new StreamReader(filepath))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    obj = xmldes.Deserialize(stream.BaseStream);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }

        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type"></param>  
        /// <param name="xml"></param>  
        /// <returns></returns>  
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化XML文件
        /// <summary>  
        /// 序列化XML文件  
        /// </summary>  
        /// <param name="type">类型</param>  
        /// <param name="obj">对象</param>  
        /// <returns></returns>  
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            //创建序列化对象  
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象  
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();
            return str;
        }
        #endregion

        #region 将XML转换为DATATABLE
        /// <summary>  
        /// 将XML转换为DATATABLE  
        /// </summary>  
        /// <param name="FileURL"></param>  
        /// <returns></returns>  
        public static DataTable XmlAnalysisArray()
        {
            try
            {
                string FileURL = System.Configuration.ConfigurationManager.AppSettings["Client"].ToString();
                DataSet ds = new DataSet();
                ds.ReadXml(FileURL);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>  
        /// 将XML转换为DATATABLE  
        /// </summary>  
        /// <param name="FileURL"></param>  
        /// <returns></returns>  
        public static DataSet XmlAnalysisArray(string FileURL)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(FileURL);
                return ds;
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message.ToString());
                return null;
            }
        }
        #endregion
       
        #region 获取对应XML节点的值
        /// <summary>  
        /// 摘要:获取对应XML节点的值  
        /// </summary>  
        /// <param name="stringRoot">XML节点的标记</param>  
        /// <returns>返回获取对应XML节点的值</returns>  
        public static string XmlAnalysis(string stringRoot, string xml)
        {
            if (stringRoot.Equals("") == false)
            {
                try
                {
                    XmlDocument XmlLoad = new XmlDocument();
                    XmlLoad.LoadXml(xml);
                    return XmlLoad.DocumentElement.SelectSingleNode(stringRoot).InnerXml.Trim();
                }
                catch (Exception ex)
                {

                }
            }
            return "";
        }

        /// <summary>
        /// 根据xml的路径获取节点的值
        /// </summary>
        /// <param name="root"></param>
        /// <param name="xmlpath"></param>
        /// <returns></returns>
        public string GetNode(string root)
        {

            XmlNode oldChild = lsXmlDoc.SelectSingleNode(root);
            if (oldChild != null)
            {
                return oldChild.InnerText;
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="childRoot"></param>
        /// <returns></returns>
        public List<string> GetNodeList(string root,string childRoot)
        {
            List<string> strs = new List<string>();
            XmlNodeList oldChilds = lsXmlDoc.SelectNodes(root);
            if (oldChilds != null)
            {
                foreach (XmlNode item in oldChilds)
                {
                    if (item != null)
                    {
                        XmlNode node = item.SelectSingleNode(childRoot);
                        if (node != null)
                        {
                            strs.Add(node.InnerText);
                        }
                    }
                }
            }
            if (strs == null)
                return new List<string>();
            return strs;
        }
        #endregion


    }  
}
