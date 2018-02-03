using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

namespace DAL
{
    public class SQLHelper
    {
        public static readonly string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["Sands_GPCT_Orders"].ConnectionString;
        #region SqlConnection
        private static object _objectLock = new object();
        private static SqlConnection _SqlConn;
        /// <summary>
        /// 连接对象属性访问器
        /// </summary>
        public static SqlConnection SqlConn
        {
            get
            {
                lock (_objectLock)
                {
                    if (_SqlConn == null)
                    {
                        try
                        {    //利用 CopyTable 方法可以很轻松得实现多个表的数据导入
                            _SqlConn = new SqlConnection(ConnStr);/*CS程序*/
                            _SqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            string ss = "";
                        }
                    }
                    else if (_SqlConn.State == ConnectionState.Closed)
                    {
                        _SqlConn.Open();
                    }
                    else if (_SqlConn.State == ConnectionState.Broken)
                    {
                        _SqlConn.Close();
                        _SqlConn.Open();
                    }
                }
                return _SqlConn;
            }
        }
        #endregion
        #region NewSqlCommand
        /// <summary>
        /// 提取SqlCommand对象
        /// </summary>
        /// <param name="text">cmd.CommandText是SQL语句或存储过程名</param>
        /// <param name="type">CommandType判断执行是存储过程或SQL语句那一种</param>
        /// <param name="paras">SqlParameter集合</param>
        /// <returns></returns>
        private static SqlCommand NewSqlCommand(string cmdText, CommandType type, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand(cmdText, SqlConn);
            cmd.CommandTimeout = 300;
            if (type == CommandType.StoredProcedure)
            {
                cmd.CommandType = type;
            }
            if (paras != null)
            {
                foreach (SqlParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            return cmd;
        }
        #endregion
        #region ExecuteNonQuery
        /// <summary>
        /// 传入SQL语句执行后返回int
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, null);
            int reslut = cmd.ExecuteNonQuery();
            return reslut;
        }
        /// <summary>
        /// 传入SQL语句和参数集合执行后返回int
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, paras);
            int result = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return result;
        }
        /// <summary>
        /// 增加操作需要返回自增列值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="outPutParam">输出的参数@ID,可以为null</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, SqlParameter[] paras, string outPutParam)
        {

            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, paras);
            cmd.ExecuteNonQuery();
            SqlParameter param = cmd.Parameters[outPutParam];
            int result = Convert.ToInt32(param.Value.ToString());
            return result;
        }
        /// <summary>
        /// 传入存储过程名执行后返回int
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string proc, CommandType cmdType)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, null);
            int result = cmd.ExecuteNonQuery();
            return result;
        }
        /// <summary>
        /// 传入存储过程名和参数集合执行后返回int
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string proc, CommandType cmdType, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, paras);
            int result = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return result;
        }
        /// <summary>
        /// 增加操作需要返回自增列值
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="paras"></param>
        /// <param name="outPutParam">输出的参数@ID,可以为null</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string proc, CommandType cmdType, SqlParameter[] paras, string outPutPara)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, paras);
            cmd.ExecuteNonQuery();
            SqlParameter param = cmd.Parameters[outPutPara];
            int result = Convert.ToInt32(param.Value.ToString());
            return result;
        }
        #endregion
        #region ExecuteReader
        /// <summary>
        /// 传入SQL语句执行后返回SqlDataReader
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql)
        {
            return NewSqlCommand(sql, CommandType.Text, null).ExecuteReader(CommandBehavior.CloseConnection);
        }
        /// <summary>
        /// 传入SQL语句和参数集合执行后返回SqlDataReader
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, paras);
            SqlDataReader dr = cmd.ExecuteReader();
            cmd.Parameters.Clear();
            return dr;
        }
        /// <summary>
        /// 传入存储过程名执行后返回SqlDataReader
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string proc, CommandType cmdType)
        {
            return NewSqlCommand(proc, CommandType.StoredProcedure, null).ExecuteReader(CommandBehavior.CloseConnection);
        }
        /// <summary>
        /// 传入存储过程名和参数集合执行后返回SqlDataReader
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string proc, CommandType cmdType, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, paras);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return dr;
        }
        #endregion
        #region ExecuteScalar
        /// <summary>
        /// 传入SQL语句执行后返回int结果集中第一行第一列
        /// insert into Customer (Name,PWD,Email) output inserted.ID VALUES('admin','123','test@163.com')
        /// insert into Customer (Name,PWD,Email) VALUES('admin','123','test@163.com');select @@IDENTITY;
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static int ExecuteScalar(string sql)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, null);
            object obj = cmd.ExecuteScalar();
            int result = 0;
            if (obj != null && obj != DBNull.Value)
            {
                result = Convert.ToInt32(obj);
            }
            return result;
        }
        /// <summary>
        /// 传入SQL语句和参数集合执行后返回int结果集中第一行第一列
        /// insert into Customer (Name,PWD,Email) output inserted.ID VALUES('admin','123','test@163.com')
        /// insert into Customer (Name,PWD,Email) VALUES('admin','123','test@163.com');select @@IDENTITY;
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static int ExecuteScalar(string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, paras);
            object obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            int result = 0;
            if (obj != null && obj != DBNull.Value)
            {
                result = Convert.ToInt32(obj);
            }
            return result;
        }
        /// <summary>
        /// 传入存储过程名执行后返回int结果集中第一行第一列
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <returns></returns>
        public static int ExecuteScalar(string proc, CommandType cmdType)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, null);
            object obj = cmd.ExecuteScalar();
            int result = 0;
            if (obj != null && obj != DBNull.Value)
            {
                result = Convert.ToInt32(obj);
            }
            return result;
        }
        /// <summary>
        /// 传入存储过程名和参数集合执行后返回int结果集中第一行第一列
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static int ExecuteScalar(string proc, CommandType cmdType, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, paras);
            object obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            int result = 0;
            if (obj != null && obj != DBNull.Value)
            {
                result = Convert.ToInt32(obj);
            }
            return result;
        }
        #endregion
        #region ExecuteDataSet
        /// <summary>
        /// 传入SQL语句执行后返回DataSet结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string sql)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, null);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 传入SQL语句和参数集合执行后返回DataSet结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, paras);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
        /// <summary>
        /// 传入存储过程名执行后返回DataSet结果集
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string proc, CommandType cmdType)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, null);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 传入存储过程名和参数集合执行后返回DataSet结果集
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string proc, CommandType cmdType, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, paras);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        #endregion
        #region ExecuteDataTable
        /// <summary>
        /// 传入SQL语句执行后返回DataTable结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql,SqlConn);
            da.Fill(dt);
            return dt;
        }
        /// <summary>
        /// 传入SQL语句和参数集合执行后返回DataTable结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, paras);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return dt;
        }
        /// <summary>
        /// 传入存储过程名执行后返回DataTable
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string proc, CommandType cmdType)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, null);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        /// <summary>
        /// 传入存储过程名和参数集合执行后返回DataTable
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string proc, CommandType cmdType, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, paras);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #endregion
        #region ExecuteXmlReader
        /// <summary>
        /// 传入SQL语句执行后返回XmlReader
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(string sql)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, null);
            XmlReader xr = cmd.ExecuteXmlReader();
            return xr;
        }
        /// <summary>
        /// 传入SQL语句和参数集合执行后返回XmlReader
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(string sql, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(sql, CommandType.Text, paras);
            XmlReader xr = cmd.ExecuteXmlReader();
            cmd.Parameters.Clear();
            return xr;
        }
        /// <summary>
        /// 传入SQL语句执行后返回XmlReader
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(string proc, CommandType cmdType)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, null);
            XmlReader xr = cmd.ExecuteXmlReader();
            return xr;
        }
        /// <summary>
        /// 传入SQL语句和参数集合执行后返回XmlReader
        /// </summary>
        /// <param name="proc">存储过程名</param>
        /// <param name="values">参数集合</param>
        /// <returns></returns>
        public static XmlReader ExecuteXmlReader(string proc, CommandType cmdType, params SqlParameter[] paras)
        {
            SqlCommand cmd = NewSqlCommand(proc, CommandType.StoredProcedure, paras);
            XmlReader xr = cmd.ExecuteXmlReader();
            cmd.Parameters.Clear();
            return xr;
        }
        #endregion

        #region Paging分页
        public static DataSet Paging(string sql, int startPage, int endPage)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, SqlConn);
            DataSet ds = new DataSet();
            da.Fill(ds, startPage, endPage, "table1");
            return ds;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="startPage">起始页</param>
        /// <param name="endPage">结束页</param>
        /// <param name="strsql">SQL语句</param>
        /// <returns></returns>
        public static DataSet Paging(string sql, SqlParameter[] paras, int startPage, int endPage)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, SqlConn);
            DataSet ds = new DataSet();
            da.Fill(ds, startPage, endPage, "table1");
            return ds;
        }
        /// <summary>
        /// 返回分页的字符串
        /// </summary>
        /// <param name="tables">表名，可以为多表(必填)</param>
        /// <param name="columns">要获取的字段(必填)</param>
        /// <param name="sort_column">按字段排序(必填)</param>
        /// <param name="isDesc">true 降序（desc）false 升序(asc)</param>
        /// <param name="where">查询条件</param>
        /// <param name="page_index">当前页码(必填)</param>
        /// <param name="page_size">页面显示条数(必填)</param>
        /// <returns>分页的字符串</returns>
        public static string Paging(string tables, string columns, string sort_column, bool isDesc, string where, int page_index, int page_size)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select " + columns + " from ( ");
            sb.Append("select " + columns + ",row_number() over (order by " + sort_column + " " + (isDesc ? "desc" : "asc") + ") as row_id from " + tables + " where 1=1 " + where + "");
            sb.Append(") as tb where row_id>=" + (page_index - 1) * page_size + 1 + " and row_id<=" + page_index * page_size + ";");
            sb.Append("select count(*) from " + tables + " where 1=1 " + where + "");
            return sb.ToString();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="startPage">起始页</param>
        /// <param name="endPage">结束页</param>
        /// <param name="strsql">SQL语句</param>
        /// <returns></returns>
        public static DataSet Paging(string proc, CommandType cmdType, int startPage, int endPage)
        {
            SqlDataAdapter da = new SqlDataAdapter(proc, SqlConn);
            DataSet ds = new DataSet();
            da.Fill(ds, startPage, endPage, "table1");
            return ds;
        }
        public static DataSet Paging(string proc, CommandType cmdType, SqlParameter[] paras, int startPage, int endPage)
        {
            SqlDataAdapter da = new SqlDataAdapter(proc, SqlConn);
            DataSet ds = new DataSet();
            da.Fill(ds, startPage, endPage, "table1");
            return ds;
        }
        #endregion
        
    }
}
