using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cenaoi.Model;
using System.Data;
using System.Data.SqlClient;

namespace Cenaoi.SQL.Helper
{


    public class SqlSplice
    {



        /// <summary>
        /// 参数集合
        /// </summary>
        public static List<SqlParameter> SqlParams { get; set; } = new List<SqlParameter>();



        /// <summary>
        /// sql语句
        /// </summary>
        public static string InitSql { get; set; }



        /// <summary>
        /// 返回查询语句
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSelect(Type type)
        {
            PropertyInfo[] fields = type.GetProperties();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            int i = 0;

            foreach (var field in fields)
            {
                sql.Append(field.Name);

                if (i++ < fields.Length - 1)
                {
                    sql.Append(",");
                }

            }

            sql.Append(" FROM ");

            sql.Append(type.Name);

            return sql.ToString();
        }



        /// <summary>
        /// 返回查询语句
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSelect(CModel model,object key)
        {
            SqlParamsClear();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            int i = 0;

            foreach (var field in model.Fields)
            {
                sql.Append(field);

                if (i++ < model.Fields.Count - 1)
                {
                    sql.Append(",");
                }

            }

            sql.Append(" FROM ");

            sql.Append(model.TableName);

            string keyName = model.KeyName;

            if (!string.IsNullOrWhiteSpace(keyName))
            {
                sql.Append($" WHERE {keyName}=@{keyName}");
                SqlParams.Add(new SqlParameter($"@{keyName}", key));
            }



            return sql.ToString();
        }



        /// <summary>
        /// 返回查询语句
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSelect(Type type, object key)
        {
            SqlParamsClear();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            int i = 0;

            PropertyInfo[] fields = type.GetProperties();

            foreach (var field in fields)
            {
                sql.Append(field.Name);

                if (i++ < fields.Length - 1)
                {
                    sql.Append(",");
                }

            }

            sql.Append(" FROM ");

            sql.Append(type.Name);

            string keyName = new CModel().GetKeyName(fields);

            if (!string.IsNullOrWhiteSpace(keyName))
            {
                sql.Append($" WHERE {keyName}=@{keyName}");
                SqlParams.Add(new SqlParameter($"@{keyName}", key));
            }



            return sql.ToString();
        }



        /// <summary>
        /// 返回查询语句（废用）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string GetSelect(Type type,ModelFilter filter)
        {
            PropertyInfo[] fields = type.GetProperties();

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            int i = 0;

            foreach (var field in fields)
            {
                sql.Append(field.Name);

                if (i++ < fields.Length - 1)
                {
                    sql.Append(",");
                }

            }

            sql.Append(" FROM ");

            sql.Append(type.Name);

            return sql.ToString();





        }



        /// <summary>
        /// 返回插入语句
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetInsert(CModel model)
        {

            SqlParamsClear();

            StringBuilder sql = new StringBuilder();

            sql.Append($" INSERT INTO {model.TableName} ");

            int i = 0;

            StringBuilder str_params = new StringBuilder();

            StringBuilder param_values = new StringBuilder();

            foreach (var field in model.Fields)
            {

                if (field == model.KeyName)  //不更新主键
                {
                    i++;
                    continue;
                }

                if (i++ > 1)
                {
                    str_params.Append(",");
                    param_values.Append(",");
                }

                str_params.Append($"{field}");
                param_values.Append($"@{field}");

                SqlParams.Add(new SqlParameter($"@{field}", model[field]));

            }

            sql.Append($"({str_params.ToString()}) VALUES ({param_values.ToString()})");

            InitSql = sql.ToString();

            return sql.ToString();

            

        }



        #region  SqlParams操作



        /// <summary>
        /// 获取Sql参数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SqlParameter[] GetSqlParams(CModel model)
        {

            foreach (var field in model.Fields)
            {
                SqlParams.Add(new SqlParameter($"@{field}", model[field]));
            }

            return SqlParams.ToArray();

        }



        /// <summary>
        /// 获取Sql参数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SqlParameter[] GetSqlParams()
        {

            return SqlParams.ToArray();

        }



        /// <summary>
        /// 清除Sql参数
        /// </summary>
        public static void SqlParamsClear()
        {

            SqlParams.Clear();

        }




        #endregion



        /// <summary>
        /// 返回更新语句
        /// </summary>
        /// <returns></returns>
        public static string GetUpdate(CModel model)
        {

            SqlParamsClear();

            StringBuilder sql = new StringBuilder();

            sql.Append($" UPDATE {model.TableName}  SET ");

            int i = 0;

            foreach (var field in model.Fields)
            {

                if (field == model.KeyName)  //不更新主键
                {
                    i++;
                    SqlParams.Add(new SqlParameter($"@{field}", model[field]));
                    continue;
                }

                if (i++ > 1)
                {
                    sql.Append(",");
                }

                sql.Append($"{field}=@{field}");


                SqlParams.Add(new SqlParameter($"@{field}", model[field]));

            }

            sql.Append($" WHERE {model.KeyName}=@{model.KeyName}");


            InitSql = sql.ToString();

            return sql.ToString();

        }



        /// <summary>
        /// 返回删除语句
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static string GetDelete(CModel model)
        {

            SqlParamsClear();

            StringBuilder sql = new StringBuilder();

            sql.Append($" DELETE  FROM  {model.TableName}  ");

            sql.Append($" WHERE {model.KeyName}=@{model.KeyName}");

            SqlParams.Add(new SqlParameter($"@{model.KeyName}", model.KeyValue));

            InitSql = sql.ToString();

            return sql.ToString();

        }



        /// <summary>
        /// 返回删除语句
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        public static string GetDelete(ModelFilter filter)
        {

            StringBuilder sql = new StringBuilder();

            List<string> param_names = filter.ParamNames;

            sql.Append($"DELETE FROM {filter.TableName} WHERE ");

            int i = 0;

            foreach (var param in param_names)
            {

                if (i++ > 0)
                {
                    sql.Append(",");
                }

                sql.Append($"{param}=@{param}");

            }


            return sql.ToString();
        }





    }


}
