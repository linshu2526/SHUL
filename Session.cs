using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SHUL
{
    public class Session
    {
        public static void Add(string key, object value)
        {
            Remove(key);
            HttpContext.Current.Session[key] = value;
        }
 
        public static object Get(string key)
        {
            return HttpContext.Current.Session[key];
        }
        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
        public static object Get_Set(string key, string value)
        {
            if (Get(key) == null)
            {
                Add(key, value);
            }
            return HttpContext.Current.Session[key];
        }
    }
}
