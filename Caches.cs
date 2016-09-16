using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace SHUL
{
    public class Caches
    {
        // Methods
        public static object Get(string Key)
        {
            return HttpRuntime.Cache[Key];
        }
        public static object Read(string Key)
        {
            return HttpRuntime.Cache[Key];
        }

        public static void Set(string Key, object CacheValue)
        {
            Write(Key, CacheValue);

        }
        public static void Write(string Key, object CacheValue)
        {
            Write(Key, CacheValue, 1);
        }
        public static void Set(string Key, object CacheValue, double cacheTime)
        {
            Write(Key, CacheValue, cacheTime);
        }
        public static void Write(string Key, object CacheValue, double cacheTime)
        {
            if (HttpRuntime.Cache[Key] == null)
            {
                HttpRuntime.Cache.Add(Key, CacheValue, null, DateTime.Now.AddDays(int.Parse(cacheTime.ToString())), TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                HttpRuntime.Cache.Insert(Key, CacheValue, null, DateTime.Now.AddHours(cacheTime), TimeSpan.Zero);
            }
        }
        public static void Set(string Key, object CacheValue, CacheDependency cd)
        {
            Write(Key, CacheValue, cd);
        }
        public static void Write(string Key, object CacheValue, CacheDependency cd)
        {
            if (HttpRuntime.Cache[Key] == null)
            {
                HttpRuntime.Cache.Add(Key, CacheValue, cd, DateTime.Now.AddHours(0.5), TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
            else
            {
                HttpRuntime.Cache.Insert(Key, CacheValue, cd, DateTime.Now.AddHours(0.5), TimeSpan.Zero);
            }
        }

        public static object ReadAndWrite(string Key, object CacheValue, double cacheTime)
        {
            object obj = Read(Key);
            if (obj == null)
            {
                obj = CacheValue;
                Write(Key, CacheValue, cacheTime);
            }
            return obj;
        }

        public static void Clear(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
        public static object ReadAndWrite(string Key, object CacheValue)
        {
            return ReadAndWrite(Key, CacheValue, 0.5);
        }
    }
}
