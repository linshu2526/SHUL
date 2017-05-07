using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SHUL
{

    public class LSParse
    {
        public static string DateTimeNow
        {
            get
            {
                return DateTime.Now.ToString(DefaultTimeFormat);
            }
        }
        public static readonly string DefaultTime = "1900-01-01 00:00:00.000";
        public static readonly string DefaultTimeFormat = "yyyy-MM-dd hh:mm:ss.fff";
        public static DateTime ToDateTime(object _obj)
        {
            DateTime _datetime = DateTime.Parse(DefaultTime);//默认时间
            if (_obj != null)
                if (_obj.ToString() != DefaultTime && _obj.ToString() != "")
                    _datetime = Convert.ToDateTime(_obj);
            return _datetime;
        }
        public static Boolean ToBoolean(object _obj)
        {
            if (_obj != null)
                return Convert.ToBoolean(_obj);
            return false;
        }


        public static int ToInt(object _obj)
        {

            if (_obj != null)
            {
                if (_obj.ToString() != "")
                    return int.Parse(_obj.ToString());
                else
                    return 0;
            }
            else
                return 0;



        }
        public static long ToLong(object _obj)
        {

            if (_obj != null && _obj != "")
            {

                return long.Parse(_obj.ToString());
            }
            else
                return 0;



        }
        public static double ToDouble(object _obj)
        {
            if (_obj != null)
            {
                if (_obj.ToString() != "")
                {
                    return double.Parse(_obj.ToString());
                }
            }
            return 0;

        }
        public static string ToString(object _obj)
        {
            if (_obj != null)
                return _obj.ToString();
            else
                return "";
        }

        public static string RequestString(string key)
        {
            return ToString(HttpContext.Current.Request[key]);
        }
        public static int RequestInt(string key)
        {
            return ToInt(HttpContext.Current.Request[key]);
        }
    }
}
