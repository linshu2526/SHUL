using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SHUL
{

    public class SqlHelperAddParameter
    {
        public SqlType sqltype { get; set; }
        public string key { get; set; }
        public object value { get; set; }
        public bool IsPRIMARY { get; set; }

    }
    public class SqlHelperDeleteParameter
    {
        public SqlType sqltype { get; set; }
        public string key { get; set; }
        public object value { get; set; }
        public bool IsCondtion { get; set; }

    }
    public class SHULParameter
    {
        public SqlType sqltype { get; set; }
        public string key { get; set; }
        public object value { get; set; }
        public bool IsCondtionOrPRIMARY { get; set; }

    }
    public class Parameters
    {
        List<SqlHelperAddParameter> _SqlHelperAddParameters;
        List<SqlHelperDeleteParameter> _SqlHelperDeleteParameter;
        List<SHULParameter> _SHULParameter;
        public string TableName { get; set; }
        public Parameters(string tablename)
        {
            TableName = tablename;
            _SqlHelperAddParameters = new List<SqlHelperAddParameter>();
            _SqlHelperDeleteParameter = new List<SqlHelperDeleteParameter>();
            _SHULParameter = new List<SHULParameter>();
        }
        public void Add(string key, object value, SqlType sqltype, bool IsCondtionOrPRIMARY = false)
        {
            _SqlHelperAddParameters.Add(new SqlHelperAddParameter
            {
                sqltype = sqltype,
                key = key,
                value = value,
                IsPRIMARY = IsCondtionOrPRIMARY

            });
            _SqlHelperDeleteParameter.Add(new SqlHelperDeleteParameter
            {
                sqltype = sqltype,
                key = key,
                value = value,
                IsCondtion = IsCondtionOrPRIMARY
            });

            _SHULParameter.Add(new SHULParameter
            {
                sqltype = sqltype,
                key = key,
                value = value,
                IsCondtionOrPRIMARY = IsCondtionOrPRIMARY
            });
        }
        public List<SqlHelperAddParameter> SqlHelperAddParameters
        {
            get { return _SqlHelperAddParameters; }
        }
        public List<SqlHelperDeleteParameter> SqlHelperDeleteParameter
        {
            get { return _SqlHelperDeleteParameter; }
        }
        public List<SHULParameter> SHULParameters
        {
            get { return _SHULParameter; }
        }


    }

    public enum SqlType
    {
        Int, String, Long, DateTime, Double, Boolean
    }

    /// <summary>
    /// SqlHelper 的摘要说明
    /// </summary>
    public class SqlHelper
    {


        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static readonly string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParams)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, cmdType, cmdText, commandParams);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParams)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, CommandType.Text, cmdText, commandParams);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }


        public static int ExecuteNonQuery(string cmdText)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    int val = cmd.ExecuteNonQuery();
                    return val;
                }
            }
        }
        #endregion

        #region ExecuteReader

        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(con, cmd, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        #endregion

        #region ExecuteDataSet |  ExecuteDataTable


        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, cmdType, cmdText, para);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }



        public static DataSet ExecuteDataSet(string cmdtext)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, CommandType.Text, cmdtext, null);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        public static DataTable ExecuteDataTable(string cmdtext)
        {
            DataSet ds = ExecuteDataSet(cmdtext);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return ds.Tables[0];
            }

        }

        public static DataTable ExecuteDataTable(string cmdtext, params SqlParameter[] para)
        {

            DataSet ds = ExecuteDataSet(cmdtext, para);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return ds.Tables[0];
            }
        }
        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdtext, params SqlParameter[] para)
        {
            DataSet ds = ExecuteDataSet(cmdType, cmdtext, para);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return ds.Tables[0];
            }
        }

        public static DataRow ExecuteDataRow(CommandType cmdType, string cmdText, params SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            dt = ExecuteDataTable(cmdType, cmdText, para);
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }
        public static DataRow ExecuteDataRow(string cmdText, params SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            dt = ExecuteDataTable(cmdText, para);
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }
        public static DataRow ExecuteDataRow(string cmdText)
        {
            DataTable dt = new DataTable();
            dt = ExecuteDataTable(cmdText);
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }
        /// <summary>
        /// 根据指定的SQL语句,返回DATASET
        /// </summary>
        /// <param name="cmdtext">要执行带参的SQL语句</param>
        /// <param name="para">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string cmdtext, params SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                using (SqlCommand cmd = new SqlCommand())
                {
                    DataSet ds = new DataSet();
                    PrepareCommand(con, cmd, CommandType.Text, cmdtext, para);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        public static DataRow GetListById(int Id, string tablename)
        {
            DataTable dt = new DataTable();
            SqlParameter[] ps = new SqlParameter[]{
                new SqlParameter("@Id",Id)

            };
            dt = ExecuteDataTable(string.Format("SELECT * FROM {0} WHERE Id=@Id", tablename), ps);
            return dt.Rows[0];

        }
        public static DataTable GetList(string tablename)
        {
            DataTable dt = new DataTable();
            dt = ExecuteDataTable(string.Format("SELECT * FROM {0}", tablename));

            return dt;
        }
        #endregion

        #region ExecuteScalar

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, cmdType, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }
        /// <summary>
        /// 自动生成sql语句保存数据进数据库如果不需要更新的话 Update = true,默认是true
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Update"></param>
        /// <param name="IsReturnPRIMARY_Value"></param>
        /// <returns></returns>
        public static object SaveData(Parameters parameters, bool Update = true, string ReturnPRIMARY = null)
        {

            List<SqlHelperAddParameter> SqlHelperParameters = parameters.SqlHelperAddParameters;
            string tablename = parameters.TableName;
            string sql = "";
            string sqladdkey = "";
            string sqladdvalue = "";
            string sqlupdate = "";
            string sqlwhere = "";
            string sqlreturn = "";
            string PRIMARY_Key = "";
            object PRIMARY_Value = null;
            List<SqlHelperAddParameter> _IsExistParameters = new List<SqlHelperAddParameter>();
            List<SqlParameter> spList = new List<System.Data.SqlClient.SqlParameter>();
            for (int i = 0; i < SqlHelperParameters.Count; i++)
            {

                SqlHelperAddParameter sqlp = SqlHelperParameters[i];

                object sqlval = null;
                switch (sqlp.sqltype)
                {
                    case SqlType.Int:
                        sqlval = LSParse.ToInt(sqlp.value);
                        break;
                    case SqlType.String:
                        sqlval = LSParse.ToString(sqlp.value);
                        break;
                    case SqlType.Long:
                        sqlval = LSParse.ToLong(sqlp.value);
                        break;
                    case SqlType.DateTime:
                        sqlval = LSParse.ToDateTime(sqlp.value);
                        break;
                    case SqlType.Double:
                        sqlval = LSParse.ToDouble(sqlp.value);
                        break;
                    case SqlType.Boolean:
                        sqlval = LSParse.ToBoolean(sqlp.value);
                        break;
                    default:
                        break;
                }
                ///这是主键
                if (sqlp.IsPRIMARY)
                {
                    _IsExistParameters.Add(new SqlHelperAddParameter { key = sqlp.key, value = sqlp.value, sqltype = sqlp.sqltype });

                    if (String.IsNullOrEmpty(PRIMARY_Key))
                    {
                        PRIMARY_Key = sqlp.key;
                        PRIMARY_Value = sqlp.value;
                        sqlwhere += string.Format(" [{0}]=@{0} ", sqlp.key);
                    }
                    else
                    {
                        sqlwhere += string.Format(" and [{0}]=@{0} ", sqlp.key);
                    }

                }
                if (sqlval != null)
                {
                    spList.Add(new System.Data.SqlClient.SqlParameter(sqlp.key, sqlval));
                    sqladdkey += string.Format("[{0}],", sqlp.key);
                    sqladdvalue += string.Format("@{0},", sqlp.key);
                    sqlupdate += string.Format("[{0}]=@{0},", sqlp.key);
                }

            }
            if (!String.IsNullOrEmpty(ReturnPRIMARY))
            {
                sqlreturn = string.Format("Inserted.{0}", ReturnPRIMARY.Trim());
            }
            //判断该数据是否存在,并且要有where条件
            int isexist = IsExist(tablename, _IsExistParameters);
            if (isexist > 0 && !String.IsNullOrEmpty(sqlwhere))
            {
                if (Update)
                {
                    sql = string.Format(@"UPDATE  {0} SET  {1} WHERE {2} ", tablename, sqlupdate.TrimEnd(','), sqlwhere);
                    if (ExecuteNonQuery(sql, spList.ToArray()) > 0)
                    {
                        if (!String.IsNullOrEmpty(ReturnPRIMARY))
                        {
                            return PRIMARY_Value;
                        }
                    }
                    else
                    {
                        return -1;
                    }


                }
                else
                {
                    return -1;
                }
            }
            else
            {
                sql = string.Format(@"INSERT INTO  {0}({1}) 
                                                    {3}
                                                VALUES ({2})
                                             ", tablename, sqladdkey.TrimEnd(','), sqladdvalue.TrimEnd(','), sqlreturn);

                if (!String.IsNullOrEmpty(ReturnPRIMARY))
                {
                    return ExecuteScalar(sql, spList.ToArray());
                }
                else
                {
                    return ExecuteNonQuery(sql, spList.ToArray());
                }

            }


            return -1;


        }
        /// <summary>
        /// 已放弃不建议使用该方法用SaveData
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="PRIMARY_Key"></param>
        /// <param name="SqlHelperParameters"></param>
        /// <param name="IsReturnPRIMARY_Value"></param>
        /// <returns></returns>
        public static object AddData(Parameters parameters, bool Update = true, bool IsReturnPRIMARY_Value = false)
        {

            List<SqlHelperAddParameter> SqlHelperParameters = parameters.SqlHelperAddParameters;
            string tablename = parameters.TableName;
            string sql = "";
            string sqladdkey = "";
            string sqladdvalue = "";
            string sqlupdate = "";
            string sqlwhere = "";
            string sqlreturn = "";
            string PRIMARY_Key = "";
            object PRIMARY_Value = null;
            List<SqlHelperAddParameter> _IsExistParameters = new List<SqlHelperAddParameter>();
            List<SqlParameter> spList = new List<System.Data.SqlClient.SqlParameter>();
            for (int i = 0; i < SqlHelperParameters.Count; i++)
            {

                SqlHelperAddParameter sqlp = SqlHelperParameters[i];

                object sqlval = null;
                switch (sqlp.sqltype)
                {
                    case SqlType.Int:
                        sqlval = LSParse.ToInt(sqlp.value);
                        break;
                    case SqlType.String:
                        sqlval = LSParse.ToString(sqlp.value);
                        break;
                    case SqlType.Long:
                        sqlval = LSParse.ToLong(sqlp.value);
                        break;
                    case SqlType.DateTime:
                        sqlval = LSParse.ToDateTime(sqlp.value);
                        break;
                    case SqlType.Double:
                        sqlval = LSParse.ToDouble(sqlp.value);
                        break;
                    case SqlType.Boolean:
                        sqlval = LSParse.ToBoolean(sqlp.value);
                        break;
                    default:
                        break;
                }
                ///这是主键
                if (sqlp.IsPRIMARY)
                {
                    _IsExistParameters.Add(new SqlHelperAddParameter { key = sqlp.key, value = sqlp.value, sqltype = sqlp.sqltype });

                    if (String.IsNullOrEmpty(PRIMARY_Key))
                    {
                        PRIMARY_Key = sqlp.key;
                        PRIMARY_Value = sqlp.value;
                        sqlwhere += string.Format(" [{0}]=@{0} ", sqlp.key);
                    }
                    else
                    {
                        sqlwhere += string.Format(" and [{0}]=@{0} ", sqlp.key);
                    }

                }
                if (sqlval != null)
                {
                    spList.Add(new System.Data.SqlClient.SqlParameter(sqlp.key, sqlval));
                    sqladdkey += string.Format("[{0}],", sqlp.key);
                    sqladdvalue += string.Format("@{0},", sqlp.key);
                    sqlupdate += string.Format("[{0}]=@{0},", sqlp.key);
                }

            }
            if (IsReturnPRIMARY_Value)
            {
                sqlreturn = " select @@IDENTITY ";
            }
            //判断该数据是否存在,并且要有where条件
            int isexist = IsExist(tablename, _IsExistParameters);
            if (isexist > 0 && !String.IsNullOrEmpty(sqlwhere))
            {
                if (Update)
                {
                    sql = string.Format(@"UPDATE  {0} SET  {1} WHERE {2} ", tablename, sqlupdate.TrimEnd(','), sqlwhere);
                    if (ExecuteNonQuery(sql, spList.ToArray()) > 0)
                    {
                        if (IsReturnPRIMARY_Value)
                        {
                            return PRIMARY_Value;
                        }
                    }
                    else
                    {
                        return -1;
                    }


                }
                else
                {
                    return -1;
                }
            }
            else
            {
                sql = string.Format(@"INSERT INTO  {0}({1}) VALUES ({2})
                                            {3} ", tablename, sqladdkey.TrimEnd(','), sqladdvalue.TrimEnd(','), sqlreturn);

                if (IsReturnPRIMARY_Value)
                {
                    return ExecuteScalar(sql, spList.ToArray());
                }
                else
                {
                    return ExecuteNonQuery(sql, spList.ToArray());
                }

            }


            return -1;


        }
        public static int IsExist(string tablename, List<SqlHelperAddParameter> _SqlHelperAddParameter)
        {
            List<SqlParameter> spList = new List<System.Data.SqlClient.SqlParameter>();
            string sqlwhere = "";
            for (int i = 0; i < _SqlHelperAddParameter.Count; i++)
            {
                SqlHelperAddParameter shaps = _SqlHelperAddParameter[i];
                spList.Add(new System.Data.SqlClient.SqlParameter(shaps.key, shaps.value));
                sqlwhere += string.Format("[{0}]= @{0},", shaps.key);
            }


            string sql = string.Format("select count(*) from {0} where {1}", tablename, sqlwhere.TrimEnd(',').Replace(",", " and "));
            return LSParse.ToInt(ExecuteScalar(sql, spList.ToArray()));
        }
        public static bool IsExist(string tablename, string key, object value)
        {
            List<SqlParameter> spList = new List<System.Data.SqlClient.SqlParameter>();
            spList.Add(new System.Data.SqlClient.SqlParameter(key, value));
            string sql = string.Format("select count({0}) from {1} where [{0}] = @{0}", key, tablename);
            return LSParse.ToInt(ExecuteScalar(sql, spList.ToArray())) > 0;
        }
        public static int DeleteData(string tablename, string IdKey, object IdValue, SqlType sqltype)
        {
            return DeleteData(tablename, new List<SqlHelperDeleteParameter> { new SqlHelperDeleteParameter{
                IsCondtion=true,
                key=IdKey,
                value=IdValue,
                sqltype=sqltype
            }});
        }
        public static int DeleteData(string tablename, List<SqlHelperDeleteParameter> SqlHelperParameters)
        {

            string sql = "";
            string sqlwhere = "";

            List<SqlParameter> spList = new List<System.Data.SqlClient.SqlParameter>();
            for (int i = 0; i < SqlHelperParameters.Count; i++)
            {

                SqlHelperDeleteParameter sqlp = SqlHelperParameters[i];

                object sqlval = null;
                switch (sqlp.sqltype)
                {
                    case SqlType.Int:
                        sqlval = LSParse.ToInt(sqlp.value);
                        break;
                    case SqlType.String:
                        sqlval = LSParse.ToString(sqlp.value);
                        break;
                    case SqlType.Long:
                        sqlval = LSParse.ToLong(sqlp.value);
                        break;
                    case SqlType.DateTime:
                        sqlval = LSParse.ToDateTime(sqlp.value);
                        break;
                    case SqlType.Double:
                        sqlval = LSParse.ToDouble(sqlp.value);
                        break;
                    case SqlType.Boolean:
                        sqlval = LSParse.ToBoolean(sqlp.value);
                        break;
                    default:
                        break;
                }
                if (sqlp.IsCondtion)
                {
                    sqlwhere += string.Format("{0}=@{0} , ", sqlp.key);
                }

            }
            sql = string.Format(" DELETE FROM {0}  WHERE {1}", tablename, sqlwhere.TrimEnd(',').Replace(",", "and"));

            return ExecuteNonQuery(sql, spList.ToArray());
        }

        public static DataRowCollection ExecuteDataRowList(string cmdtext, params SqlParameter[] para)
        {
            DataTable dt = ExecuteDataTable(cmdtext, para);
            if (dt != null)
            {
                return dt.Rows;
            }
            return null;
        }
        public static DataRowCollection ExecuteDataRowList(Parameters parametes)
        {
            string tablename = parametes.TableName;
            string sqlwhere = "";
            string sqlrow = "";


            List<SHULParameter> _SHULParameters = parametes.SHULParameters;
            List<SqlParameter> spList = new List<System.Data.SqlClient.SqlParameter>();
            for (int i = 0; i < _SHULParameters.Count; i++)
            {
                SHULParameter _SHULParameter = _SHULParameters[i];
                sqlrow += string.Format("[{0}],", _SHULParameter.key);
                if (_SHULParameter.IsCondtionOrPRIMARY)
                {
                    if (_SHULParameter.value != null)
                    {
                        if (sqlwhere == "")
                        {
                            sqlwhere += string.Format(" where [{0}]=@{0}", _SHULParameter.key);
                        }
                        else
                        {
                            sqlwhere += string.Format(" and [{0}]=@{0}", _SHULParameter.key);
                        }
                        spList.Add(new SqlParameter(_SHULParameter.key, _SHULParameter.value));
                    }

                }

            }
            string sql = string.Format("SELECT {0} FROM {1} {2}", sqlrow.TrimEnd(','), tablename, sqlwhere);

            DataTable dt = ExecuteDataTable(sql, spList.ToArray());
            if (dt != null)
            {
                return dt.Rows;
            }
            return null;
        }
        public static object ExecuteScalar(string cmdText)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(con, cmd, CommandType.Text, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        public static bool DeleteById(string tableName, int Id)
        {

            string delsql = "DELETE FROM " + tableName + " WHERE Id=@Id ";
            SqlParameter[] ps = new SqlParameter[]{

                new SqlParameter("@Id",Id),

             };
            int excount = 0;
            excount = SqlHelper.ExecuteNonQuery(delsql, ps);
            return excount > 0;
        }


        public static int GetPageSize(DataTable dt, int pagesize)
        {
            if (dt != null && dt.Rows.Count > 0 && pagesize > 0)
            {
                int totalsize = dt.Rows.Count;

                return GetPageSize(totalsize, pagesize);
            }

            return 0;

        }
        public static int GetPageSize(double totalsize, double pagesize)
        {
            if (totalsize > 0 && pagesize > 0)
            {
                double s = totalsize / pagesize;
                if (s > 0)
                {
                    if (s.ToString().IndexOf(".") > 0)
                    {
                        s = int.Parse(s.ToString().Split('.')[0]);// +1;
                    }
                    return int.Parse(s.ToString());
                }

            }
            return 0;

        }
        #endregion

        #region 获取指定表中指定字段的最大值
        /// <summary>
        /// 获取指定表中指定字段的最大值
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="field">字段</param>
        /// <returns>Return Type:Int</returns>
        public static int GetMaxID(string tableName, string field)
        {
            string s = "select Max({0}) from {1}";
            s = string.Format(s, field, tableName);
            int i = Convert.ToInt32(SqlHelper.ExecuteScalar(s) == DBNull.Value ? "0" : SqlHelper.ExecuteScalar(s));
            return i;
        }
        #endregion

        #region 建立SqlCommand
        /// <summary>
        /// 建立SqlCommand
        /// </summary>
        /// <param name="con">SqlConnection　对象</param>
        /// <param name="cmd">要建立的Command</param>
        /// <param name="cmdType">CommandType</param>
        /// <param name="cmdText">执行的SQL语句</param>
        /// <param name="cmdParms">参数</param>
        private static void PrepareCommand(SqlConnection con, SqlCommand cmd, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (con.State != ConnectionState.Open)
                con.Open();

            cmd.Connection = con;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            if (cmdParms != null)
                foreach (SqlParameter para in cmdParms)
                    cmd.Parameters.Add(para);
        }

        #endregion

        #region 更新某一字段的值

        /// <summary>
        /// 更新某表中某一字段的值
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="fieldName">更新的字段名</param>
        /// <param name="fieldValue">更新的字段的值</param>
        /// <param name="identityFieldName">标识字段的名称</param>
        /// <param name="identityValue">标识字段的值</param>
        public static void UpdateAfield(string tablename, string fieldName, string fieldValue, string identityFieldName, string identityValue)
        {
            string s = "Update {0} set {1}='{2}' where {3}='{4}'";
            s = string.Format(s, tablename, fieldName, fieldValue, identityFieldName, identityValue);
            ExecuteNonQuery(s);
        }

        #endregion

        #region 分页获取

        /// <summary>
        /// 分页获取数据列表 适用于SQL2000
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="key">主键</param>
        /// <param name="where">查询条件</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="pageindex">页索引</param>
        /// <param name="orderfield">排序字段</param>
        /// <param name="ordertype">排序方式 1=ASC 0=DESC</param>
        /// <param name="fieldlist">查找的字段</param>
        /// <param name="recordcount">总记录数</param>
        /// <returns></returns>
        public static DataTable GetDataByPager2000(string tablename, string key, string where, int pagesize, int pageindex, string orderfield, int ordertype, string fieldlist, int recordcount)
        {
            string cmd = "ProcCustomPage";
            SqlParameter[] para = new SqlParameter[9];
            para[0] = new SqlParameter("@Table_Name", tablename);
            para[1] = new SqlParameter("@Sign_Record", key);
            para[2] = new SqlParameter("@Filter_Condition", where);
            para[3] = new SqlParameter("@Page_Size", pagesize);
            para[4] = new SqlParameter("@Page_Index", pageindex);
            para[5] = new SqlParameter("@TaxisField", orderfield);
            para[6] = new SqlParameter("@Taxis_Sign", ordertype);
            para[7] = new SqlParameter("@Find_RecordList", fieldlist);
            para[8] = new SqlParameter("@Record_Count", recordcount);

            return ExecuteDataSet(CommandType.StoredProcedure, cmd, para).Tables[0];

        }


        /// <summary>
        /// 分页获取数据列表 适用于SQL2005
        /// </summary>
        /// <param name="SelectList">选取字段列表</param>
        /// <param name="tablename">数据源名称表名或视图名称</param>
        /// <param name="where">筛选条件</param>
        /// <param name="OrderExpression">排序 必须指定一个排序字段</param>
        /// <param name="pageindex">页索引 从0开始</param>
        /// <param name="pagesize">每页记录数</param>
        /// <returns></returns>
        public static DataTable GetDataByPager2005(string SelectList, string tablename, string where, string OrderExpression, int pageindex, int pagesize)
        {
            string cmd = "GetRecordFromPage2005";
            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter("@SelectList", SelectList);
            para[1] = new SqlParameter("@TableSource", tablename);
            para[2] = new SqlParameter("@SearchCondition", where);
            para[3] = new SqlParameter("@OrderExpression", OrderExpression);
            para[4] = new SqlParameter("@pageindex", pageindex);
            para[5] = new SqlParameter("@pagesize", pagesize);

            return ExecuteDataSet(CommandType.StoredProcedure, cmd, para).Tables[0];
        }

        #endregion

        #region 获取某表中的总记录数

        /// <summary>
        /// 获取某表中的总记录数
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static int GetRecordCount(string tablename)
        {
            string s = "select count(*) from {0}";
            s = string.Format(s, tablename);
            return Convert.ToInt32(ExecuteScalar(s));
        }

        public static int GetRecordCount(string tablename, string where)
        {
            string s = "select count(*) from {0} where {1}";
            s = string.Format(s, tablename, where);
            return Convert.ToInt32(ExecuteScalar(s));
        }

        #endregion

        #region 根据条件获取指定表中的数据

        /// <summary>
        /// 根据条件获取指定表中的数据
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string tablename, string where)
        {
            string s = "select * from " + tablename;
            if (where != "")
                s += " where " + where;

            return SqlHelper.ExecuteDataSet(s).Tables[0];
        }


        #endregion

        #region 根据ID 获取一行数据

        /// <summary>
        /// 根据主键Id,获取一行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="keyName">主键名称</param>
        /// <param name="value">值</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public static DataRow GetADataRow(string tableName, string keyName, string value, out string msg)
        {
            try
            {
                string s = "select * from {0} where {1}='{2}'";
                s = string.Format(s, tableName, keyName, value);
                DataTable dt = ExecuteDataSet(s).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    msg = "OK";
                    return dt.Rows[0];
                }
                else
                {
                    msg = "";
                    return null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        #endregion

        #region 由Object取值
        /// <summary>
        /// 取得Int值,如果为Null 则返回０
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetInt(object obj)
        {
            if (obj.ToString() != "")
                return int.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// 取得byte值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte Getbyte(object obj)
        {
            if (obj.ToString() != "")
                return byte.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// 获得Long值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetLong(object obj)
        {
            if (obj.ToString() != "")
                return long.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// 取得Decimal值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object obj)
        {
            if (obj.ToString() != "")
                return decimal.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// 取得Guid值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Guid GetGuid(object obj)
        {
            if (obj.ToString() != "")
                return new Guid(obj.ToString());
            else
                return Guid.Empty;
        }

        /// <summary>
        /// 取得DateTime值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object obj)
        {
            if (obj.ToString() != "")
                return DateTime.Parse(obj.ToString());
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// 取得bool值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBool(object obj)
        {
            if (obj.ToString() == "1" || obj.ToString().ToLower() == "true")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 取得byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Byte[] GetByte(object obj)
        {
            if (obj.ToString() != "")
            {
                return (Byte[])obj;
            }
            else
                return null;
        }

        /// <summary>
        /// 取得string值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(object obj)
        {
            if (obj != null && obj != DBNull.Value)
                return obj.ToString();
            else
                return "";
        }
        #endregion

        #region 分页存储过程

        #region  sql 2000 分页存储过程
        /*
     * 
     * CREATE  PROCEDURE [dbo].[ProcCustomPage]
		(
		    @Table_Name               varchar(5000),      	    --表名
		    @Sign_Record              varchar(50),       		--主键
		    @Filter_Condition         varchar(1000),     	    --筛选条件,不带where
		    @Page_Size                int,               		--页大小
		    @Page_Index               int,          			--页索引     			
	        @TaxisField               varchar(1000),            --排序字段
		    @Taxis_Sign               int,               		--排序方式 1为 DESC, 0为 ASC
            @Find_RecordList          varchar(1000),        	--查找的字段
		    @Record_Count             int                		--总记录数
		 )
		 AS
			BEGIN 
			DECLARE  @Start_Number          int
			DECLARE  @End_Number            int
			DECLARE  @TopN_Number           int
		 DECLARE  @sSQL                  varchar(8000)
                 if(@Find_RecordList='')
                 BEGIN
                      SELECT @Find_RecordList='*'
                 END
		 SELECT @Start_Number =(@Page_Index-1) * @Page_Size
			IF @Start_Number<=0
		 SElECT @Start_Number=0
			SELECT @End_Number=@Start_Number+@Page_Size
			IF @End_Number>@Record_Count
		 SELECT @End_Number=@Record_Count
		 SELECT @TopN_Number=@End_Number-@Start_Number
		 IF @TopN_Number<=0
		 SELECT @TopN_Number=0
			print @TopN_Number
		 print @Start_Number
		 print @End_Number
		 print @Record_Count
                 IF @TaxisField=''
                 begin
                    select  @TaxisField=@Sign_Record
                 end
		 IF @Taxis_Sign=0
		  	BEGIN
		 		IF @Filter_Condition=''
				 BEGIN
		 			SELECT @sSQL='SELECT '+@Find_RecordList+' FROM '+@Table_Name+' 
		     			WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@TopN_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
		    			 WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@End_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
		    		 ORDER BY '+@TaxisField+') order by '+@TaxisField+' DESC)order by '+@TaxisField
		 		END
				ELSE
				BEGIN
				SELECT @sSQL='SELECT '+@Find_RecordList+' FROM '+@Table_Name+' 
		     WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@TopN_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
		     WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@End_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
		     WHERE '+@Filter_Condition+' ORDER BY '+@TaxisField+') and '+@Filter_Condition+' order by '+@TaxisField+' DESC) and '+@Filter_Condition+' order by '+@TaxisField
				 END
			END
		ELSE
			BEGIN
			IF @Filter_Condition=''
				BEGIN
					SELECT @sSQL='SELECT '+@Find_RecordList+' FROM '+@Table_Name+' 
		         WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@TopN_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
		         WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@End_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
		         ORDER BY '+@TaxisField+' DESC) order by '+@TaxisField+')order by '+@TaxisField+' DESC'
		     END
			ELSE
			BEGIN
				SELECT @sSQL='SELECT '+@Find_RecordList+' FROM '+@Table_Name+' 
		     WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@TopN_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
		     WHERE '+@Sign_Record+' in (SELECT TOP '+CAST(@End_Number AS VARCHAR(10))+' '+@Sign_Record+' FROM '+@Table_Name+' 
		     WHERE '+@Filter_Condition+' ORDER BY '+@TaxisField+' DESC) and '+@Filter_Condition+' order by '+@TaxisField+') and '+@Filter_Condition+' order by '+@TaxisField+' DESC'
		 END
			END
			EXEC (@sSQL)
			IF @@ERROR<>0
			RETURN -3              
		 RETURN 0
		 END
		 
		 PRINT  @sSQL
        GO

     * */


        #endregion

        #region SQL2005 分页存储过程
        /*
     * 
   CREATE PROCEDURE [dbo].[GetRecordFromPage] 
    @SelectList            VARCHAR(2000),    --欲选择字段列表
    @TableSource        VARCHAR(100),    --表名或视图表 
    @SearchCondition    VARCHAR(2000),    --查询条件
    @OrderExpression    VARCHAR(1000),    --排序表达式
    @PageIndex            INT = 1,        --页号,从0开始
    @PageSize            INT = 10        --页尺寸
AS 
BEGIN
    IF @SelectList IS NULL OR LTRIM(RTRIM(@SelectList)) = ''
    BEGIN
        SET @SelectList = '*'
    END
    PRINT @SelectList
    
    SET @SearchCondition = ISNULL(@SearchCondition,'')
    SET @SearchCondition = LTRIM(RTRIM(@SearchCondition))
    IF @SearchCondition <> ''
    BEGIN
        IF UPPER(SUBSTRING(@SearchCondition,1,5)) <> 'WHERE'
        BEGIN
            SET @SearchCondition = 'WHERE ' + @SearchCondition
        END
    END
    PRINT @SearchCondition

    SET @OrderExpression = ISNULL(@OrderExpression,'')
    SET @OrderExpression = LTRIM(RTRIM(@OrderExpression))
    IF @OrderExpression <> ''
    BEGIN
        IF UPPER(SUBSTRING(@OrderExpression,1,5)) <> 'WHERE'
        BEGIN
            SET @OrderExpression = 'ORDER BY ' + @OrderExpression
        END
    END
    PRINT @OrderExpression

    IF @PageIndex IS NULL OR @PageIndex < 1
    BEGIN
        SET @PageIndex = 1
    END
    PRINT @PageIndex
    IF @PageSize IS NULL OR @PageSize < 1
    BEGIN
        SET @PageSize = 10
    END
    PRINT  @PageSize

    DECLARE @SqlQuery VARCHAR(4000)

    SET @SqlQuery='SELECT '+@SelectList+',RowNumber 
    FROM 
        (SELECT ' + @SelectList + ',ROW_NUMBER() OVER( '+ @OrderExpression +') AS RowNumber 
          FROM '+@TableSource+' '+ @SearchCondition +') AS RowNumberTableSource 
    WHERE RowNumber BETWEEN ' + CAST(((@PageIndex - 1)* @PageSize+1) AS VARCHAR) 
    + ' AND ' + 
    CAST((@PageIndex * @PageSize) AS VARCHAR) 
--    ORDER BY ' + @OrderExpression
    PRINT @SqlQuery
    SET NOCOUNT ON
    EXECUTE(@SqlQuery)
    SET NOCOUNT OFF
 
    RETURN @@RowCount
END
     **/

        #endregion

        #endregion


    }



}
