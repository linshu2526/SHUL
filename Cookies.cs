using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SHUL
{
    public class Cookies
    {
        public static void Add(string key, string value, int day = 1)
        {

            if (!String.IsNullOrEmpty(Get(key)))
            {
                HttpContext.Current.Request.Cookies.Remove(key);
            }
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Expires = DateTime.Now.AddDays(day);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void Remove(string key)
        {
            HttpContext.Current.Request.Cookies.Remove(key);
        }
        public static string Get(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                return HttpContext.Current.Request.Cookies[key].Value;
            }
            return "";
        }

        public static string Get_Set(string key, string value)
        {
            if (String.IsNullOrEmpty(Get(key)))
            {
                Add(key, value);
            }
            return HttpContext.Current.Request.Cookies[key].Value;
        }
        public static void Clear()
        {
            HttpContext.Current.Request.Cookies.Clear();
            HttpContext.Current.Response.Cookies.Clear();
        }
    }
}
