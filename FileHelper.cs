using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHUL
{
    public class FileHelper
    {
        string filepath = "";
        public FileHelper(string filepath)
        {
            this.filepath = filepath;
        }
        public void Save(string text, string encoding = "UTF-8")
        {
            File.WriteAllText(filepath, text, Encoding.GetEncoding(encoding));
        }
        public static void SaveTxt(string filepath,string text, string encoding = "UTF-8")
        {
            File.WriteAllText(filepath, text, Encoding.GetEncoding(encoding));
        }
        public static string ReadTxt(string path)
        {
            if (Exists(path))
                return System.IO.File.ReadAllText(path, Encoding.UTF8);
            else
                return "";
        }
        public static void CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static bool Exists(string PathName)
        {
            FileInfo TheFile = new FileInfo(PathName);
            return TheFile.Exists;
        }
    }
}
