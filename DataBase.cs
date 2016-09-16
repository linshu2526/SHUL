using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace SHUL
{
    /// <summary>
    /// add key="IsMsSql" value="true" 
    /// add name="DbConnection" 
    /// connectionString="server=.;
    /// database=;uid=;pwd=;Min Pool Size = 3" 
    /// providerName="System.Data.SqlClient"
    /// Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:/myfile/17gzs/web/data/dddddd.mdb
    /// </summary>
    public class DataBase
    {

        public static readonly string ShiYongShuoMing = "web.config : name=\"DbConnection\" ,指定是否MsSql: add key=\"IsMsSql\" value=\"true\" ";

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectionString = AppSetting.DbConnection;
        /// <summary>
        /// 是否Mssql 默认true
        /// </summary>
        private bool IsMsSql = AppSetting.IsMsSql;


        #region 初始化
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataAdapter adapter = null;
        List<SqlParameter> ps;

        OleDbConnection ocon = null;
        OleDbCommand ocmd = null;
        OleDbDataAdapter oadapter = null;
        List<OleDbParameter> ops;

        private SqlParameter[] PsList
        {
            get
            {
                return ps.ToArray();
            }
        }
        private OleDbParameter[] OPsList
        {
            get
            {
                return ops.ToArray();
            }
        }
        /// <summary>
        /// 默认DbConnection
        /// </summary>
        public DataBase()
        {
            ps = new List<SqlParameter>();
            ops = new List<OleDbParameter>();
        }
        /// <summary>
        /// 是否配置文件key
        /// </summary>
        /// <param name="_connectionString"></param>
        /// <param name="isConfigKey"></param>
        public DataBase(string _connectionString, bool isConfigKey)
            : this()
        {
            if (isConfigKey)
            {
                connectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
            }
            else
            {
                connectionString = _connectionString;
            }
        }
        /// <summary>
        /// 是否MsSql
        /// </summary>
        /// <param name="_connectionString"></param>
        /// <param name="isConfigKey"></param>
        public DataBase(bool ismssql)
            : this()
        {
            IsMsSql = ismssql;
        }
        /// <summary>
        /// 是否MsSql,是否配置文件key
        /// </summary>
        /// <param name="_connectionString"></param>
        /// <param name="isConfigKey"></param>
        public DataBase(string _connectionString, bool isConfigKey, bool ismssql)
            : this(_connectionString, isConfigKey)
        {
            IsMsSql = ismssql;
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Add(string key, object obj)
        {
            if (IsMsSql)
            {
                ps.Add(new SqlParameter(key, obj));
            }
            else
            {
                ops.Add(new OleDbParameter(key, obj));
            }
        }
        /// <summary>
        /// 初始化对象
        /// </summary>
        private void Init()
        {
            if (IsMsSql)
            {
                con = new SqlConnection(connectionString);
                cmd = new SqlCommand();
                adapter = new SqlDataAdapter();
            }
            else
            {
                ocon = new OleDbConnection(connectionString);
                ocmd = new OleDbCommand();
                oadapter = new OleDbDataAdapter();
            } 
            
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (IsMsSql)
            {
                MsSqlDispose();
            }
            else
            {
                OleDbDispose();
            }
            

        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private void MsSqlDispose()
        {
            ps.Clear();
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (cmd != null)
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
                cmd = null;
            }
            if (con != null)
            {
                con.Dispose();
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                con = null;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private void OleDbDispose()
        {
            ops.Clear();
            if (oadapter != null)
            {
                oadapter.Dispose();
                oadapter = null;
            }
            if (ocmd != null)
            {
                ocmd.Parameters.Clear();
                ocmd.Dispose();
                ocmd = null;
            }
            if (ocon != null)
            {
                ocon.Dispose();
                if (ocon.State != ConnectionState.Closed)
                {
                    ocon.Close();
                }
                ocon = null;
            }
        }
        /// <summary>
        /// 准备命令
        /// </summary>
        /// <param name="con"></param>
        /// <param name="cmd"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        private void PrepareCommand(CommandType cmdType, string cmdText)
        {
            if (IsMsSql)
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Connection = con;
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;
                SqlParameter[] sqlps = PsList;
                if (sqlps != null)
                {
                    foreach (SqlParameter para in sqlps)
                    {
                        cmd.Parameters.Add(para);
                    }
                }
            }
            else
            {
                if (ocon.State != ConnectionState.Open)
                {
                    ocon.Open();
                }
                ocmd.Connection = ocon;
                ocmd.CommandType = cmdType;
                ocmd.CommandText = cmdText;
                OleDbParameter[] osqlps = OPsList;
                if (osqlps != null)
                {
                    foreach (OleDbParameter para in osqlps)
                    {
                        ocmd.Parameters.Add(para);
                    }
                }
            }
           
        }
        #endregion

        #region 原始方法
        public int AddValue(CommandType cmdType, string cmdText)
        {
            int val = 0;
            try
            {
                Init();
                PrepareCommand(cmdType, cmdText);
                if (IsMsSql)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        cmd.Dispose();
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT @@IDENTITY";
                        val = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                else
                {
                    if (ocmd.ExecuteNonQuery() > 0)
                    {
                        ocmd.Dispose();
                        ocmd.Parameters.Clear();
                        ocmd.CommandText = "SELECT @@IDENTITY";
                        val = Convert.ToInt32(ocmd.ExecuteScalar());
                    }
                }
                

            }
            finally
            {
                Dispose();
            }
            return val;
        }
        public int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {

            int val = 0;
            try
            {
                Init();
                PrepareCommand(cmdType, cmdText);
                if (IsMsSql)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        cmd.Dispose();
                        cmd.Parameters.Clear();
                        cmd.CommandText = "SELECT @@IDENTITY";
                        val = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                else
                {
                    val = ocmd.ExecuteNonQuery();
                    //if (ocmd.ExecuteNonQuery() > 0)
                    //{
                    //    ocmd.Dispose();
                    //    ocmd.Parameters.Clear();
                    //    ocmd.CommandText = "SELECT @@IDENTITY";
                    //    val = Convert.ToInt32(ocmd.ExecuteScalar());
                    //}
                }
               
            }
            finally
            {
                Dispose();
            }
            return val;
        }
        public object ExecuteScalar(CommandType cmdType, string cmdText)
        {

            object val = null;
            try
            {
                Init();
                PrepareCommand(cmdType, cmdText);
                if (IsMsSql)
                {
                    val = cmd.ExecuteScalar();
                }
                else
                {
                    val = ocmd.ExecuteScalar();
                } 
            }
            finally
            {
                Dispose();
            }
            return val;
        }
        public DataSet ExecuteDataSet(CommandType cmdType, string cmdText)
        {

            DataSet ds = null;
            try
            {
                Init();
                PrepareCommand( cmdType, cmdText);
                if (IsMsSql)
                {
                    adapter.SelectCommand = cmd;
                    ds = new DataSet();
                    adapter.Fill(ds);
                }
                else
                {
                    oadapter.SelectCommand = ocmd;
                    ds = new DataSet();
                    oadapter.Fill(ds);
                } 
               
            }
            finally
            {
                Dispose();
            }
            return ds;
        }

        #endregion

        #region 衍生
        public int AddValue(string cmdText)
        {
            return AddValue(CommandType.Text, cmdText);
        }
        public object ExecuteScalar(string cmdText)
        {
            return ExecuteScalar(CommandType.Text, cmdText);
        }
        public int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText);
        }
        public DataSet ExecuteDataSet(string cmdtext)
        {
            return ExecuteDataSet(CommandType.Text, cmdtext);
        }
        public DataTable ExecuteDataTable(string cmdtext)
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
        public DataTable ExecuteDataTable(CommandType cmdType, string cmdtext)
        {
            DataSet ds = ExecuteDataSet(cmdType, cmdtext);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return ds.Tables[0];
            }
        }
        public DataRow ExecuteDataRow(CommandType cmdType, string cmdText)
        {
            DataTable dt = new DataTable();
            dt = ExecuteDataTable(cmdType, cmdText);
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }
        public DataRow ExecuteDataRow(string cmdText)
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
        public DataRow GetListById(int Id, string tablename)
        {
            DataTable dt = new DataTable();
            Add("Id", Id);
            return ExecuteDataRow(string.Format("SELECT * FROM {0} WHERE Id=@Id", tablename));

        }
        public DataTable GetList(string tablename)
        {
            DataTable dt = new DataTable();
            dt = ExecuteDataTable(string.Format("SELECT * FROM {0}", tablename));

            return dt;
        }
        public bool DeleteById(string tn, int Id)
        {

            string delsql = "DELETE FROM " + tn.ToString() + " WHERE Id=@Id ";
            Add("Id", Id);
            int excount = 0;
            excount = ExecuteNonQuery(delsql);
            return excount > 0;
        }
        public int GetPageSize(DataTable dt, int pagesize)
        {
            if (dt != null && dt.Rows.Count > 0 && pagesize > 0)
            {
                int totalsize = dt.Rows.Count;

                return GetPageSize(totalsize, pagesize);
            }

            return 0;

        }
        public int GetPageSize(double totalsize, double pagesize)
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

       

    }
}
