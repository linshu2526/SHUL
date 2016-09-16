using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SHUL
{

    public class DataHelper : DataBase
    {

        string tablename;
        StringBuilder sbUpdate;
        StringBuilder sbAdd1;
        StringBuilder sbAdd2;
        StringBuilder sbWhere;
        string orderby;
        string select;
        public string sql;

        public DataHelper(string _tablename, string _connectionString, bool _isConfiKey)
            : base(_connectionString, _isConfiKey)
        {
            tablename = _tablename;
            Init();

        }

        public DataHelper(string _tablename)
            : base()
        {
            tablename = _tablename;
            Init();

        }
        private void Init()
        {
            sbUpdate = new StringBuilder();
            sbAdd1 = new StringBuilder();
            sbAdd2 = new StringBuilder();
            sbWhere = new StringBuilder();
            select = " * ";

        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void _DisposeAll()
        {
            Init();
            base.Dispose();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void _Dispose()
        {
            //Init();
            base.Dispose();
        }
        /// <summary>
        /// 设置值可以给插入 修改 使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void SetValues(string key, object obj)
        {
            sbUpdate.AppendFormat("[{0}]=@{0},", key);
            sbAdd1.AppendFormat("[{0}],", key);
            sbAdd2.AppendFormat("@{0},", key);
            base.Add(key, obj);
        }

        public void OrderBy(string _orderby)
        {
            orderby = string.Format(" ORDER BY {0}", _orderby);
        }
        private void Where(string format)
        {
            if (string.IsNullOrEmpty(sbWhere.ToString()))
            {
                sbWhere.AppendFormat("WHERE {0}", format.Replace("AND", "").Replace("OR", ""));
            }
            else
            {
                sbWhere.AppendFormat(" {0}", format);
            }
        }
        public DataHelper Where_And(string key, object obj)
        {
            this.Where(string.Format("AND {0}=@{0}", key));
            base.Add(key, obj);
            return this;
        }

        public DataHelper Where_And_big(string key, object obj)
        {
            this.Where(string.Format("AND {0}>@{0}", key));
            base.Add(key, obj);
            return this;
        }
        public DataHelper Where_And_small(string key, object obj)
        {
            this.Where(string.Format("AND {0}<@{0}", key));
            base.Add(key, obj);
            return this;
        }

        public DataHelper Not_Where_And(string key, object obj)
        {
            this.Where(string.Format("AND {0}<>@{0}", key));
            base.Add(key, obj);
            return this;
        }

        public DataHelper Where_OR(string key, object obj)
        {
            Where(string.Format("OR {0}=@{0}", key));
            base.Add(key, obj);
            return this;
        }

        public DataHelper Where_LIKE_OR(string key, object obj)
        {
            Where(string.Format("OR {0} LIKE @{0}", key));
            base.Add(key, obj);
            return this;
        }

        public DataHelper Where_LIKE_AND(string key, object obj)
        {
            Where(string.Format("AND {0} LIKE @{0}", key));
            base.Add(key, obj);
            return this;
        }

        public DataHelper Between2Time(string begintime,string endtime,string dateColumn)
        {
            Where(string.Format("AND {0} between @betweenbegintime and @betweenendtime", dateColumn));

            if (begintime == "")
            {
                begintime = "1900-01-01";
            }
            if (endtime == "")
            {
                endtime = "2100-01-01";
            }

            DateTime _begintime = DateTime.Parse(begintime);
            DateTime _endtime = DateTime.Parse(endtime);
            _endtime = _endtime.AddDays(1.0);
            base.Add("betweenbegintime", _begintime);
            base.Add("betweenendtime", _endtime);
            return this;
        }

        public DataHelper Where_LIKE_ANDQH(string key, object obj)
        {
            if (obj.ToString() == "")
            {
                Where(string.Format("AND ({0} is null or {0} LIKE @{1}) ", key, key.Replace("[", "").Replace("]", "")));
            }
            else
            {
                Where(string.Format("AND {0} LIKE @{1}", key, key.Replace("[", "").Replace("]", "")));
            }

            base.Add(key, "%" + obj.ToString() + "%");
            return this;
        }

        /// <summary>
        /// 执行sql语句（非查询）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Submit(string sql)
        {
            int rt = ExecuteNonQuery(sql);
            _Dispose();
            return rt;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            sql = string.Format("INSERT INTO [{0}]({1}) VALUES ({2})",
                tablename,
                sbAdd1.ToString().TrimEnd(','),
                sbAdd2.ToString().TrimEnd(','));
            return Submit(sql);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            sql = string.Format("UPDATE [{0}] SET {1} {2}",
                tablename,
                sbUpdate.ToString().TrimEnd(','),
                sbWhere.ToString());

            return Submit(sql);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            sql = string.Format("DELETE FROM [{0}] {1}",
                tablename,
                sbWhere.ToString());

            return Submit(sql);
        }
        /// <summary>
        /// 显示哪些字段
        /// </summary>
        /// <param name="_select"></param>
        public DataHelper Select(string _select)
        {
            select = _select;
            return this;
        }
        /// <summary>
        /// 获取数据表
        /// </summary>
        public DataTable DataTable
        {
            get
            {
                sql = string.Format("SELECT {2} FROM [{0}] {1} {3}",
                   tablename,
                   sbWhere.ToString(),
                   select,
                   orderby);
                DataTable dt = ExecuteDataTable(sql);
                _Dispose();
                return dt;
            }
        }
          /// <summary>
        /// 获取数据表
        /// </summary>
        public DataTable List()
        {
             
                sql = string.Format("SELECT {2} FROM [{0}] {1} {3}",
                   tablename,
                   sbWhere.ToString(),
                   select,
                   orderby);
                DataTable dt = ExecuteDataTable(sql);
                _Dispose();
                return dt;
            
        }
        /// <summary>
        /// 获取一个值
        /// </summary>
        public object OneObject
        {
            get
            {
                sql = string.Format("SELECT {2} FROM [{0}] {1} {3}",
                   tablename,
                   sbWhere.ToString(),
                   select,
                   orderby);
                object obj= ExecuteScalar(sql);
                _Dispose();
                return obj;
            }
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        public DataRow DataRow
        {
            get
            {
                DataTable dt = DataTable;

                if (dt == null)
                {
                    return null;
                }
                else
                {
                    return dt.Rows[0];
                }
                
            }
        }
    }
}
