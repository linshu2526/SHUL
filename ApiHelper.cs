using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
namespace SHUL
{
    public class SlApi
    {
        public int state { get; set; }
        public string msg { get; set; }
    }
    public class MobileUser
    {
        public string name { get; set; }
        public int count { get; set; }
    }
    /// <summary>  
    /// SetOrder 的摘要说明  
    /// </summary>  
    public class ApiHelper
    {
        public static int Curr
        {
            get
            {
                string name = "MobileUser";
                MobileUser mu = new MobileUser();
                MobileUser obj = (MobileUser)Caches.Get(name);
                if (obj != null)
                {
                    mu = obj;
                }
                mu.count = mu.count++;
                Caches.Set(name, mu);
                return mu.count;
                
            }
        }
        public static bool IsPass()
        {
            string name = "slispass";
            object obj = Caches.Get(name);
            if (obj == null)
            {
                 Caches.Set(name, "isget");
                 return true;
            }
            return false;
        }

        public static string Authorized(string paramStr, string time, string sign)
        {
            SlApi slapi = new SlApi();

            slapi.msg = "验证安全通过";
            slapi.state = 0;

            //如果客户端传入的sign与服务器重写加密以后的前面不相等，那么就说明这个url一个非法的请求  
            if (string.Equals(sign, ApiHelper.Encrypt(paramStr), StringComparison.OrdinalIgnoreCase) == false)
            {
                slapi.msg = "参数有篡改";
                slapi.state = 1;
            };
            //将当客户端传递过来的时间字符串转换成DateTime格式。然后与服务器获取的当前时间比对，如果大于10秒，表示url过期了  
            DateTime clientTime = DateTime.Parse(time);

            //将服务器的获取到的当前时间减去客户端请求的时候传递过来的时间  
            double timespan = (DateTime.Now - clientTime).TotalSeconds;
            if (timespan > 5 || timespan < 0)
            {
                slapi.msg = "url已经过期";
                slapi.state = 2;
            }
            return Json.ObjectToJson(slapi);
        }
        public static string Encrypt(string str)
        {
            return Encryption.SL_SHA1_Encrypt(str);
        }
 
    }
}
