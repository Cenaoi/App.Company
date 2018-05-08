using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Cenaoi.SQL.Helper;
using Cenaoi.SQL.Request;

namespace Cenaoi.DataBase.Configure
{

    /// <summary>
    /// 数据库配置器
    /// </summary>
    public class DBConfigurator
    {

        /// <summary>
        /// 是否允许打开数据库请求
        /// </summary>
        private static bool isOpen = false;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DBManipulator OpenDBManipulator()
        {
            if (string.IsNullOrWhiteSpace(SqlRequest.ConnectionString))
            {
                return null;
            }
            else
            {
                return new DBManipulator();
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_conn">指定请求的数据库的连接</param>
        /// <returns></returns>
        public static DBManipulator OpenDBManipulator(string str_conn)
        {

            SqlConnection conn = new SqlConnection(str_conn);

            try
            {
                conn.Open();

                SqlRequest.ConnectionString = str_conn;

                RequestOpen();

                if (string.IsNullOrWhiteSpace(SqlRequest.ConnectionString) || isOpen == false)
                {
                    return null;
                }
                else
                {
                    return new DBManipulator();
                }
            }
            catch (Exception ex)
            {
                RequestClose();
                return null;
                throw new Exception("OpenDBManipulator方法出错了", ex);
            }
            finally
            {
                conn.Close();
            }



        }



        /// <summary>
        /// 允许打开连接
        /// </summary>
        private static void RequestOpen()
        {
            isOpen = true;
        }



        /// <summary>
        /// 不允许打开连接
        /// </summary>
        private static void RequestClose()
        {
            isOpen = false;
        }



    }



}
