using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SHUL
{
    public class ModelInfo
    {
        public string rec_str;
        int xz;
        int count_add;
        public ModelInfo()
        {
            rec_str = ""; 
            xz = 0; 
            count_add = 0;
        }
        public void Add(string t_e_name, string t_pro, string t_pro_name, string t_namespace)
        {
            string sql = @"INSERT INTO  [CTableByE]
           ([t_e_name]
           ,[t_pro]
           ,[t_pro_name]
,[t_namespace])
     VALUES
           (@t_e_name
           ,@t_pro
           ,@t_pro_name
,@t_namespace)";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("t_e_name", t_e_name));
            ps.Add(new SqlParameter("t_pro_name", t_pro_name));

            bool check = SHUL.LSParse.ToInt(SHUL.SqlHelper.ExecuteScalar(@"select count(*) from CTableByE where t_e_name=@t_e_name and t_pro_name=@t_pro_name ", ps.ToArray())) == 0;

            ps.Add(new SqlParameter("t_pro", t_pro));
            ps.Add(new SqlParameter("t_namespace", t_namespace));
            try
            {
                if (check)
                    SHUL.SqlHelper.ExecuteNonQuery(sql, ps.ToArray());
            }
            catch (Exception)
            {
                throw;
            }

        }
       
        public void rec(object str) { rec_str += "<br/>" + str.ToString(); }

       
        public void AddTypeToData(Type t, string name_space = "")
        {
            count_add++;
            rec("xz   " + xz.ToString());
            rec("count_add   " + count_add.ToString());
            foreach (PropertyInfo pi in t.GetProperties())
            {
                string t_namespace = RemoveLast(pi.PropertyType.FullName)
                    .Replace(", mscorlib, Version=4.0.0", "")
                      .Replace("System.Nullable`1[[", "")
                      .Replace(", MvcApplication1, Version=1.0.0", "");
                string t_pro = pi.PropertyType.Name;
                if (t_pro == "Nullable`1")
                {
                    t_pro = t_namespace.Replace("System.", "");
                }
                Add(pi.ReflectedType.FullName, t_pro, pi.Name, t_namespace);

                if (name_space != "")
                {
                    if (t_namespace == name_space && name_space + "." + t_pro.Replace("[", "").Replace("]", "") != pi.ReflectedType.FullName)
                    {
                        rec("name_space    " + name_space);
                        rec("pi.PropertyType.FullName   " + pi.PropertyType.FullName);
                        xz++;
                        //System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(name_space);
                        Type type = Type.GetType(pi.PropertyType.FullName.Replace("s[", "").Replace("[", "").Replace("]", ""));
                        if (type.BaseType.FullName != "System.Enum")
                        {
                            AddTypeToData(type, name_space);
                        }

                    }
                }


            }
        }
        public void AddTypeToDataNoRepeat(Type t, string name_space = "")
        {
            count_add++;

            rec("count_add   " + count_add.ToString());
            if (t != null)
            {
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    string t_namespace = RemoveLast(pi.PropertyType.FullName)
                        .Replace(", mscorlib, Version=4.0.0", "")
                          .Replace("System.Nullable`1[[", "")
                          .Replace(", MvcApplication1, Version=1.0.0", "");
                    string t_pro = pi.PropertyType.Name;
                    if (t_pro == "Nullable`1")
                    {
                        t_pro = t_namespace.Replace("System.", "");
                    }
                    Add(pi.ReflectedType.FullName, t_pro, pi.Name, t_namespace);

                }
            }

        }
        public void AddTypeListToData(string typenamelist, string name_space)
        {
            string[] typelist = (typenamelist.TrimEnd(',') + ",").Split(',');
            for (int i = 0; i < typelist.Length - 1; i++)
            {
                Type type = Type.GetType(string.Format("{0}.{1}", name_space, typelist[i]));
                AddTypeToData(type, name_space);
            }


        }
        public List<string> GetClasses(string nameSpace)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            List<string> namespacelist = new List<string>();
            List<string> classlist = new List<string>();

            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == nameSpace)
                    namespacelist.Add(type.Name);
            }

            foreach (string classname in namespacelist)
                classlist.Add(classname);

            return classlist;
        }
        public string RemoveLast(string str)
        {
            string r_str = "";
            string[] strs = str.Split('.');
            for (int i = 0; i < strs.Length - 1; i++)
            {
                r_str += strs[i] + ".";
            }
            return r_str.TrimEnd('.');
        }

    }
}
