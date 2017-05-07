using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SHUL
{
    public class Json
    {
        string str;
        List<string> keys;
        List<string> values;
        public Json()
        {
            str = "";
            keys = new List<string>();
            values = new List<string>();
        }
        public void Add(string key, object value)
        {
            keys.Add(key);
            values.Add(LSParse.ToString(value));
        }
        public string JsonString
        {
            get
            {
                str = "{";
                for (int i = 0; i < keys.Count; i++)
                {
                    string key = keys[i].Replace("\"","'");
                    string value = values[i].Replace("\"", "'");
                    str += string.Format("\"{0}\":\"{1}\",", key, value);
                }
                str = str.TrimEnd(',');
                str += "}";
                return str;
            }
        }


        // 从一个Json串生成对象信息
        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serializer.ReadObject(mStream);
        }
        // 从一个对象信息生成Json串
        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        public static string DataTableToJson(DataTable dataRowList)
        {
            return JsonConvert.SerializeObject(dataRowList);
            
                   
        }
    }
}
