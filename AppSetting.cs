using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;

namespace SHUL
{
    public class AppSetting
    {
        /// <summary>
        /// AppSetting配置项
        /// </summary>
        public static string GetConfig(string key)
        {
            string returnValue = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(returnValue))
            {
                return returnValue;
            }
            return string.Empty;
        }
        /// <summary>
        /// 管理员会话名称
        /// </summary>
        public static string AdminSessionName
        {
            get
            {
                string _AdminSessionName = GetConfig("AdminSessionName");
                if (!string.IsNullOrEmpty(_AdminSessionName))
                {
                    return Convert.ToString(_AdminSessionName);
                }
                return null;
            }
        }
        /// <summary>
        /// 是否Mssql 默认true
        /// </summary>
        public static bool IsMsSql
        {
            get
            {
                string _IsMsSql = GetConfig("IsMsSql");
                if (!string.IsNullOrEmpty(_IsMsSql))
                {
                    return Convert.ToBoolean(_IsMsSql);
                }
                return true;
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public static string Web_index_typeid
        {
            get
            {
                string _Web_index_typeid = "";
                _Web_index_typeid = Convert.ToString(Caches.Read("web_index_typeid"));
                if (string.IsNullOrEmpty(_Web_index_typeid))
                {
                    _Web_index_typeid = AppSetting.GetConfig("web_index_typeid");
                    Caches.Write("web_index_typeid", _Web_index_typeid);
                }
                return _Web_index_typeid;
            }
        }  /// <summary>
        ///  
        /// </summary>
        public static string WebChannel
        {
            get
            {
                string _WebChannel = "";
                _WebChannel = Convert.ToString(Caches.Read("web_channel"));
                if (string.IsNullOrEmpty(_WebChannel))
                {
                    _WebChannel = AppSetting.GetConfig("web_channel");
                    Caches.Write("web_channel", _WebChannel);
                }
                return _WebChannel;
            }
        }
        /// <summary>
        /// 域名后缀，为了重写
        /// </summary>
        public static string webHouZhui
        {
            get
            {
                string _webHouZhui = "";
                _webHouZhui = Convert.ToString(Caches.Read("webhouzhui"));
                if (string.IsNullOrEmpty(_webHouZhui))
                {
                    _webHouZhui = AppSetting.GetConfig("webhouzhui");
                    Caches.Write("webhouzhui", _webHouZhui);
                }
                return _webHouZhui;
            }
        }

        /// <summary>
        /// 前台menu
        /// </summary>
        public static string webMenuSetting
        {
            get
            {
                string _webMenuSetting = "";
                _webMenuSetting = Convert.ToString(Caches.Read("webMenuSetting"));
                if (string.IsNullOrEmpty(_webMenuSetting))
                {
                    _webMenuSetting = AppSetting.GetConfig("webMenuSetting");
                    Caches.Write("webMenuSetting", _webMenuSetting);
                }
                return _webMenuSetting;
            }
        }
 

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public static string DbConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            }
        }
        public static string TemplatePath
        {
            get
            {
                string path = "";

                path = Convert.ToString(Caches.Read("TemplatePath"));
                if (string.IsNullOrEmpty(path))
                {
                    path = AppSetting.GetConfig("TemplatePath");
                    Caches.Write("TemplatePath", path);
                }
                return path;
            }
        }
        /// <summary>
        /// 数据库项目位置
        /// </summary>
        public static string App_DataSPath
        {
            get
            {
                return MapPath("App_Data");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string MapPath(string name)
        {

            return HttpContext.Current.Server.MapPath(name);

        }
    }
}
