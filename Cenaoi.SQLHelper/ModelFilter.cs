using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Cenaoi.Model;
using Model;
using Cenaoi.SQL.Request;

namespace Cenaoi.SQL.Helper
{

    /// <summary>
    /// 数据过滤器
    /// </summary>
    public class ModelFilter
    {


        /// <summary>
        /// 实体
        /// </summary>
        public Type Model { get; set; }



        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }



        /// <summary>
        /// 字段集合
        /// </summary>
        public List<string> Fields { get; set; } = new List<string>();



        /// <summary>
        /// 初始化的Sql语句
        /// </summary>
        private string InitSql { get; set; }


        private StringBuilder m_ExecuteSql;


        /// <summary>
        /// 执行的Sql语句
        /// </summary>
        public StringBuilder ExecuteSql
        {
            get
            {
                GetInitSql();
                return m_ExecuteSql;
            }
            set
            {
                m_ExecuteSql = value;
            }
        }



        /// <summary>
        /// 过滤条件集合
        /// </summary>
        public List<string> Conditions { get; set; } = new List<string>();



        /// <summary>
        /// 过滤参数集合
        /// </summary>
        public List<SqlParameter> Params { get; set; } = new List<SqlParameter>();



        /// <summary>
        /// 过滤参数名称集合
        /// </summary>
        public List<string> ParamNames { get; set; } = new List<string>();



        /// <summary>
        /// 主键字段名
        /// </summary>
        public string KeyName { get; set; }



        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name = "type" ></param>
        public ModelFilter(Type type)
        {

            Model = type;

            PropertyInfo[] fields = type.GetProperties();


            foreach (var field in fields)
            {
                Fields.Add(field.Name);
            }


            TableName = type.Name;

            GetInitSql();

            KeyName = SqlRequest.GetKeyName(TableName);

        }



        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name = "table" ></param>
        public ModelFilter(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
            {
                return;
            }

            TableName = table;

            string sql = "SELECT NAME FROM SysColumns WHERE ID=Object_Id(@Table)";

            List<SqlParameter> s_param = new List<SqlParameter>();

            s_param.Add(new SqlParameter("@Table", table));

            try
            {


                SqlDataReader reader = SqlRequest.GetReader(sql, s_param.ToArray());

                string field_name = string.Empty;

                while (reader.Read())
                {
                    field_name = reader["NAME"].ToString();
                    Fields.Add(field_name);
                }

                reader.Close();

                GetInitSql();

                KeyName = SqlRequest.GetKeyName(TableName);

            }
            catch (Exception ex)
            {

                throw new Exception("根据表名初始化ModelFilter失败", ex);

            }

        }



        /// <summary>
        /// 获取初始化的Sql语句
        /// </summary>
        public void GetInitSql()
        {

            StringBuilder sql = new StringBuilder();

            int i = 0;

            if (PageQuery != null)
            {
                sql.Append("SELECT * FROM ( ");
                sql.Append("SELECT ");

                foreach (var field in Fields)
                {
                    sql.Append(field);
                    sql.Append(",");
                }

                //row_number() over(order by CREATE_DATE DESC,{KeyName} DESC) as PAGE_ROW_NUMBER FROM {TableName}  

                sql.Append($"row_number() over(order by {KeyName} DESC) as PAGE_ROW_NUMBER FROM {TableName} ");

                if (ParamNames.Count != 0)
                {
                    sql.Append(" WHERE ( ");

                    foreach (var param_name in ParamNames)
                    {
                        sql.Append($"{param_name}=@{param_name}");

                        if (i++ < Fields.Count - 1)
                        {
                            sql.Append(",");
                        }
                    }

                    sql.Append(" )");
                }

                sql.Append(" )");

                int start_row = PageQuery.PageIndex * PageQuery.PageCount + 1;
                int end_row = PageQuery.PageIndex * PageQuery.PageCount + PageQuery.PageCount;

                sql.Append($" as T WHERE (PAGE_ROW_NUMBER between {start_row} and {end_row})");

            }
            else
            {
                sql.Append("SELECT ");

                foreach (var field in Fields)
                {
                    sql.Append(field);

                    if (i++ < Fields.Count - 1)
                    {
                        sql.Append(",");
                    }

                }

                sql.Append(" FROM ");

                sql.Append(TableName);

                foreach (var param_name in ParamNames)
                {
                    string str_sql = sql.ToString();

                    if (str_sql.IndexOf("WHERE") == -1)
                    {
                        sql.Append(" WHERE ");
                        sql.Append($" {param_name}=@{param_name}");
                    }
                    else
                    {
                        sql.Append($" AND {param_name}=@{param_name}");
                    }

                }

                InitSql = sql.ToString();
            }

            m_ExecuteSql = sql;

        }



        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void And(string fieldName,object value)
        {
            string sql = ExecuteSql.ToString();

            Type type = value.GetType();

            string val = value.ToString();

            if (type.Name == "String")
            {
                val = $"'{val}'";
            }

            if (sql.IndexOf("WHERE") == -1)
            {
                ExecuteSql.Append(" WHERE ");
                ExecuteSql.Append($" {fieldName}={val}");
            }
            else
            {
                ExecuteSql.Append($" AND {fieldName}={val}");
            }

            Conditions.Add($"{fieldName}={val}");

            Params.Add(new SqlParameter($"@{fieldName}", val));
            
        }



        /// <summary>
        /// 添加过滤参数
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void AndParam(string fieldName, object value)
        {
            //string sql = ExecuteSql.ToString();

            //Type type = value.GetType();

            //if (sql.IndexOf("WHERE") == -1)
            //{
            //    ExecuteSql.Append(" WHERE ");
            //    ExecuteSql.Append($" {fieldName}=@{fieldName}");
            //}
            //else
            //{
            //    ExecuteSql.Append($" AND {fieldName}=@{fieldName}");
            //}

            Conditions.Add($"{fieldName}={value}");

            Params.Add(new SqlParameter($"@{fieldName}", value));

            ParamNames.Add(fieldName);

        }



        /// <summary>
        /// 过滤条件(废用)
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        private void And_0(string fieldName, object value)
        {

            Type type = value.GetType();

            string val = value.ToString();

            if (type.Name == "String")
            {
                val = $"'{val}'";
            }

            Conditions.Add($"{fieldName}={val}");

        }



        /// <summary>
        /// 获取过滤参数(数组)
        /// </summary>
        /// <returns></returns>
        public SqlParameter[] GetParams()
        {
            return Params.ToArray();
        }



        /// <summary>
        /// 清除全部过滤条件
        /// </summary>
        public void ClearAll()
        {
            Params.Clear();
            Conditions.Clear();
            ExecuteSql.Clear();
            ExecuteSql.Append(InitSql);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        public PagingQuery PageQuery { get; set; }


    }


    /// <summary>
    /// 分页查询
    /// </summary>
    public class PagingQuery
    {

        /// <summary>
        /// 分页构造函数
        /// </summary>
        public PagingQuery()
        {

        }

        /// <summary>
        /// 分页构造函数
        /// </summary>
        /// <param name="page_count"></param>
        /// <param name="page_index"></param>
        public PagingQuery(int page_count,int page_index)
        {
            this.PageCount = page_count;
            this.PageIndex = page_index;
        }


        /// <summary>
        /// 每页数据数量
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex { get; set; }

    }




}
