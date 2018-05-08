using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace Cenaoi.SQL.Request
{

    /// <summary>
    /// 数据库请求库
    /// </summary>
    public class SqlRequest
    {

        /// <summary>
        /// 当前数据库连接
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (connstring == string.Empty)
                {
                    return ConfigurationManager.ConnectionStrings["connString"].ToString();
                }
                else
                {
                    return connstring;
                }
            }
            set
            {
                connstring = value;
            }

        }


        private static string connstring = ConfigurationManager.ConnectionStrings["connString"].ToString();
        //"Server=.;DataBase=Test;Uid=sa;pwd=sa";


        #region   格式化SQL语句执行的各种方法



        /// <summary>
        /// 执行增删改操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Update(string sql)
        {
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string info = "调用  public static int Update(string sql) 方法发生了错误：" + ex.Message;
                WriteaLog(info);
                throw new Exception(info);
            }
            finally
            {
                conn.Close();
            }
        }



        /// <summary>
        /// 执行单一结果查询（增删改操作）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object SingleResult(string sql)
        {
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string info = "调用  public static object SingleResult(string sql) 方法发生了错误：" + ex.Message;
                WriteaLog(info);
                throw new Exception(info);
            }
            finally
            {
                conn.Close();
            }
        }



        /// <summary>
        /// 返回只读数据集的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                string info = "调用  public SqlDataReader GetReader(string sql) 方法发生了错误：" + ex.Message;
                WriteaLog(info);
                throw new Exception(info);
            }
            finally
            {

            }
        }





        #endregion


        #region 带参数SQL语句执行的各种方法



        /// <summary>
        /// 执行增删改操作
        /// </summary>
        /// <param name="sql">执行的Sql语句</param>
        /// <param name="param">参数集合</param>
        /// <returns></returns>
        public static int Update(string sql, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                WriteaLog(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                string info = "调用   public static int Update(string sql,SqlParameter[] param) 方法发生了错误：" + ex.Message;
                WriteaLog(ex.ToString());
                throw new Exception(info);
            }
            finally
            {
                cmd.Parameters.Clear();
                conn.Close();
            }
        }



        /// <summary>
        /// 执行单一结果查询（增删改操作）
        /// </summary>
        /// <param name="sql">执行的Sql语句</param>
        /// <param name="param">参数集合</param>
        /// <returns></returns>
        public static object SingleResult(string sql, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string info = "调用  public static object SingleResult(string sql,SqlParameter[] param) 方法发生了错误：" + ex.Message;
                WriteaLog(info);
                throw new Exception(info);
            }
            finally
            {
                cmd.Parameters.Clear();
                conn.Close();
            }
        }



        /// <summary>
        /// 返回只读数据集的查询
        /// </summary>
        /// <param name="sql">执行的Sql语句</param>
        /// <param name="param">参数集合</param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                string info = "调用  public static SqlDataReader GetReader(string sql,SqlParameter[] param) 方法发生了错误：" + ex.Message.ToString();
                WriteaLog(info);
                throw new Exception(info);
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }



        #endregion


        #region  调用带参数存储过程执行的各种方法


        /// <summary>
        /// 当前登录ID
        /// </summary>
        public static int User_ID
        {
            get { return userid; }
            set { userid = value; }
        }

        private static int userid = User_ID;


        public static void EditOnStatus(int userid)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@UserID",userid)
            };
            if (userid >= 1000)
            {
                SqlRequest.UpdateByProcedure("usp_EditTeacherExitOnStatus", param);
            }
            else
            {
                SqlRequest.GetReaderByProcedure("usp_EditStudentExitOnStatus", param);
            }


        }



        /// <summary>
        /// 基于存储过程  执行增删改操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int UpdateByProcedure(string spName, SqlParameter[] param)
        {

            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(spName, conn);
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;  //声明当前操作是存储过程
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                WriteaLog(ex.Message);
                throw new Exception("调用 public  static  int UpdateByProcedure(string spName,SqlParameter[] param) 方法时出现错误 ：" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }



        /// <summary>
        /// 基于存储过程 返回只读数据集的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlDataReader GetReaderByProcedure(string spName, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(spName, conn);
            try
            {
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;  //声明当前操作是存储过程
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                WriteaLog(ex.Message);
                throw new Exception("调用 public static SqlDataReader  GetReaderByProcedure(string spName,SqlParameter[] param) 方法时出现错误 ：" + ex.Message);
            }
        }



        #endregion






        #region 其他方法



        /// <summary>
        /// 将错误信息写入文本
        /// </summary>
        /// <param name="log"></param>
        public static void WriteaLog(string log)
        {
            FileStream fs = new FileStream("SqlHelper.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString() + " 错误信息：" + log);
            sw.Close();
            fs.Close();
        }



        /// <summary>
        /// 获取数据表的主键名称
        /// </summary>
        /// <param name="tableName"></param>
        public static string GetKeyName(string tableName)
        {

            if (string.IsNullOrWhiteSpace(tableName))
            {
                return "";
            }

            string sql = " SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE  WHERE TABLE_NAME = @TableName ";

            List<SqlParameter> s_param = new List<SqlParameter>();

            s_param.Add(new SqlParameter("@TableName", tableName));

            try
            {

                SqlDataReader reader = GetReader(sql, s_param.ToArray());

                string keyName = string.Empty;

                while (reader.Read())
                {
                    keyName = reader["COLUMN_NAME"].ToString();
                }

                reader.Close();

                return keyName;

            }
            catch (Exception ex)
            {

                throw new Exception("获取数据表的主键名称出错了", ex);
            }

        }




        #endregion


    }
}
