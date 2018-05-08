using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Cenaoi.SQL.Request;

namespace Cenaoi.Model
{


    /// <summary>
    /// 自定义实体
    /// </summary>
    public class CModel : RawModel, ISerializable
    {



        /// <summary>
        /// 初始化
        /// </summary>
        public CModel()
        {

        }



        /// <summary>
        /// 实体
        /// </summary>
        public Type  Model  { get; set; }



        /// <summary>
        /// 实体名
        /// </summary>
        public string ModelName { get; set; }



        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }



        /// <summary>
        /// 获取字段集合
        /// </summary>
        public List<string> Fields { get; set; } = new List<string>();



        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns></returns>
        public dynamic this[string field]
        {
            get
            {
                return FieldsValue[field];
            }
            set
            {
                object val = value;

                Type type = val.GetType();

                if (type.IsGenericType)    //判断是否为泛型
                {
                    Type[] genericArgTypes = type.GetGenericArguments();

                    if (genericArgTypes[0] == typeof(CModel))
                    {
                        List<CModel> models = val as List<CModel>;

                        List<object> vals = new List<object>();

                        foreach (var model in models)
                        {
                            vals.Add(model.m_values);
                        }

                        value = vals;
                    }
                }

                if (type == typeof(CModel))
                {
                    CModel model = val as CModel;
                    value = model.m_values;
                }

                FieldsValue[field] = value;
            }
        }



        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public dynamic this[int index]
        {
            get
            {
                string[] keys = FieldsValue.Keys.ToArray();
                return FieldsValue[keys[index]];
            }
            set
            {
                string[] keys = FieldsValue.Keys.ToArray();
                FieldsValue[keys[index]] = value;
            }
        }



        /// <summary>
        /// 获取主键值
        /// </summary>
        public dynamic KeyValue
        {
            get
            {
                if (FieldsValue.Count > 0)
                {
                    return FieldsValue[KeyName];
                }
                else
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// 主键字段名
        /// </summary>
        public string KeyName{ get; set;  }



        /// <summary>
        /// 数据集合
        /// </summary>
        private Dictionary<string, object> m_values = new Dictionary<string, object>();



        /// <summary>
        /// 实体字段的数据(集合)
        /// </summary>
        public Dictionary<string, object> FieldsValue
        {
            get { return m_values; }
            set { m_values = value; }
        }


        /// <summary>
        /// 正在测试中的数据结构
        /// </summary>
        public CModelValues valuesv1 = new CModelValues();



        /// <summary>
        /// 初始化Model
        /// </summary>
        /// <param name="objmodel">实体</param>
        public CModel(Type objmodel)
        {

            Type type = objmodel;

            Model = objmodel;

            ModelName = objmodel.Name;

            TableName = objmodel.Name;

            PropertyInfo[] model_fields = type.GetProperties();

            foreach (var field in model_fields)
            {
                FieldsValue.Add(field.Name, "");
                Fields.Add(field.Name);
                valuesv1.Add(field.Name, "");
            }

            //KeyName = SqlRequest.GetKeyName(TableName);

            KeyName = GetKeyName(model_fields);



        }



        /// <summary>
        /// 初始化Model
        /// </summary>
        /// <param name="tableName">表名</param>
        public CModel(string tableName)
        {

            if (string.IsNullOrWhiteSpace(tableName))
            {
                return;
            }

            TableName = tableName;  

            string sql = "SELECT NAME FROM SysColumns WHERE ID=Object_Id(@Table)";

            List<SqlParameter> s_param = new List<SqlParameter>();

            s_param.Add(new SqlParameter("@Table", tableName));

            try
            {


                SqlDataReader reader = SqlRequest.GetReader(sql, s_param.ToArray());

                string field_name = string.Empty;

                while (reader.Read())
                {
                    field_name = reader["NAME"].ToString();
                    Fields.Add(field_name);
                    FieldsValue.Add(field_name, "");
                }

                reader.Close();

                KeyName = SqlRequest.GetKeyName(TableName);

            }
            catch (Exception ex)
            {

                throw new Exception("根据表名初始化Model失败", ex);

            }


        }



        /// <summary>
        /// 获取数据表的主键
        /// </summary>
        /// <param name="tableName">表名</param>
        private void GetKeyName(string tableName)
        {

            if (string.IsNullOrWhiteSpace(tableName))
            {
                return;
            }

            string sql = " SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE  WHERE TABLE_NAME = @TableName ";

            List<SqlParameter> s_param = new List<SqlParameter>();

            s_param.Add(new SqlParameter("@TableName", tableName));

            try
            {

                SqlDataReader reader = SqlRequest.GetReader(sql, s_param.ToArray());

                string field_name = string.Empty;

                while (reader.Read())
                {
                    KeyName = reader["COLUMN_NAME"].ToString();
                }

                reader.Close();

            }
            catch (Exception ex)
            {

                throw new Exception("获取数据表的主键名称出错了", ex);

            }

        }



        /// <summary>
        /// 根据特性找出主键
        /// </summary>
        /// <param name="model_fields">实体的字段集合</param>
        /// <returns></returns>
        public string GetKeyName(PropertyInfo[] model_fields)
        {
            string keyName = string.Empty;

            foreach (var field in model_fields)
            {
                object[] fieldattr = field.GetCustomAttributes(false);

                foreach (var item in fieldattr)  //字段的所有特性
                {
                    Type attr_type = item.GetType();
                    if (attr_type.Name == "DBKeyAttribute")   //主键的特性
                    {
                        keyName = field.Name; //主键字段名
                        break;
                    }
                }
            }

            return keyName;

        }



        /// <summary>
        /// 根据实体获取实体主键
        /// </summary>
        /// <param name="type">实体</param>
        /// <returns></returns>
        public string GetKeyName(Type type)
        {

            PropertyInfo[] model_fields = type.GetProperties();

            string keyName = string.Empty;

            foreach (var field in model_fields)
            {
                object[] fieldattr = field.GetCustomAttributes(false);

                foreach (var item in fieldattr)  //字段的所有特性
                {
                    Type attr_type = item.GetType();
                    if (attr_type.Name == "DBKeyAttribute")   //主键的特性
                    {
                        keyName = field.Name; //主键字段名
                        break;
                    }
                }
            }

            return keyName;

        }






        ///// <summary>
        ///// 返回Json格式数据
        ///// </summary>
        ///// <returns></returns>
        //public string ToJson()
        //{

        //    string json =  JsonConverter.JsonSerializer(values);

        //    //object m_obj = JsonConverter.JsonDeserialize(json);

        //    json = json.Replace(",\"Value\"", string.Empty);
        //    json = json.Replace("\"Key\":", string.Empty);
        //    json = json.Replace("{", string.Empty);
        //    json = json.Replace("}", string.Empty);
        //    json = json.Replace("[", "{");
        //    json = json.Replace("]", "}");

        //    return json;

        //}



        ///// <summary>
        ///// 将json数据转为CModel对象
        ///// </summary>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public static CModel JsonToCModel(string json)
        //{
        //    CModel model = new CModel();

        //    model.values = JsonConverter.JsonDeserialize<Dictionary<string, object>>(json);

        //    return model;
        //}



        /// <summary>
        /// 判断是否存在某字段
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool IsHasField(string field)
        {
            if (FieldsValue.Keys.Contains(field))
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        /// <summary>
        /// 获取字段值(返回指定类型的值)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="field">字段名</param>
        /// <returns></returns>
        public T GetValue<T>(string field)
        {

            if (!IsHasField(field))  //判断是否存在此字段
            {
                return default(T);
            }

            try
            {
                return (T)Convert.ChangeType(FieldsValue[field], typeof(T));  //返回指定类型值
            }
            catch (Exception ex)
            {
                return default(T);
                throw new Exception("格式转换出错了",ex);
            }


        }



        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="field">字段名</param>
        /// <returns></returns>
        public dynamic GetValue(string field)
        {

            if (!IsHasField(field))  //判断是否存在此字段
            {
                return null;
            }

            try
            {
                return FieldsValue[field];

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }




        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //throw new NotImplementedException();
        }



    }


}
