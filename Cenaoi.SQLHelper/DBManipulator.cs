using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Cenaoi.Model;
using Model;
using System.Runtime.Serialization;
using Cenaoi.SQL.Request;
using Cenaoi.Json.Serialization;

namespace Cenaoi.SQL.Helper
{

    /// <summary>
    /// 数据库操作库
    /// </summary>
    public class DBManipulator
    {



        #region  CModel操作数据



        /// <summary>
        /// 查询一行记录
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns>返回单条数据</returns>
        public CModel GetModel(ModelFilter filter)
        {

            string sql = filter.ExecuteSql.ToString();

            CModel model = null;

            try
            {


                SqlDataReader reader = SqlRequest.GetReader(sql, filter.GetParams());

                while (reader.Read())
                {

                    model = new CModel(filter.TableName);

                    foreach (var field in filter.Fields)
                    {
                        model[field] = reader[field];
                    }
                }


                reader.Close();

                return model;

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }



        /// <summary>
        /// 根据主键获取一行记录
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public CModel GetModelByKey(string table,object key)
        {

            CModel m_table = new CModel(table);

            string sql = SqlSplice.GetSelect(m_table, key);

            CModel model = null;

            try
            {

                SqlDataReader reader = SqlRequest.GetReader(sql, SqlSplice.SqlParams.ToArray());

                while (reader.Read())
                {

                    model = new CModel(table);

                    foreach (var field in model.Fields)
                    {
                        model[field] = reader[field];
                    }

                }



                return model;

            }
            catch (Exception ex)
            {

                throw new Exception("根据主键查询一行记录出错了", ex);

            }

        }



        /// <summary>
        /// 查询多行记录
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        public List<CModel> GetModels(ModelFilter filter)
        {

            string sql = filter.ExecuteSql.ToString();

            List<CModel> models = new List<CModel>();

            try
            {

                SqlDataReader reader = SqlRequest.GetReader(sql, filter.GetParams());

                while (reader.Read())
                {

                    CModel model = new CModel(filter.TableName);

                    foreach (var field in filter.Fields)
                    {
                        model[field] = reader[field];
                    }

                    models.Add(model);

                }

                reader.Close();


                return models;

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }



        /// <summary>
        /// 新增一行记录
        /// </summary>
        /// <param name="model"></param>
        public void InsertModel(CModel model)
        {

            if (model == null)
            {
                return;
            }

            string sql = SqlSplice.GetInsert(model);

            try
            {

                SqlRequest.Update(sql, SqlSplice.GetSqlParams());

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        /// <summary>
        /// 新增多行记录
        /// </summary>
        /// <param name="models">集合</param>
        public void InsertModels(List<CModel> models)
        {

            if (models.Count <= 0)
            {
                return;
            }

            foreach (var model in models)
            {

                string sql = SqlSplice.GetInsert(model);

                try
                {

                    SqlRequest.Update(sql, SqlSplice.GetSqlParams());

                }
                catch (Exception ex)
                {
                    throw new Exception("新增多行记录出错了", ex);
                }


            }

        }



        /// <summary>
        /// 更新一行记录
        /// </summary>
        /// <param name="model"></param>
        public void UpdateModel(CModel model)
        {

            if (model == null)
            {
                return;
            }

            string sql = SqlSplice.GetUpdate(model);

            try
            {

                SqlRequest.Update(sql, SqlSplice.GetSqlParams());

            }
            catch (Exception ex)
            {
                throw new Exception("更新一行记录出错了", ex);
            }


        }



        /// <summary>
        /// 更新多行记录
        /// </summary>
        /// <param name="models"></param>
        public void UpdateModels(List<CModel> models)
        {

            if (models.Count <= 0)
            {
                return;
            }

            foreach (var model in models)
            {
                string sql = SqlSplice.GetUpdate(model);

                try
                {

                    SqlRequest.Update(sql, SqlSplice.GetSqlParams());

                }
                catch (Exception ex)
                {

                    throw new Exception("更新多行记录出错了", ex);
                }

            }

        }



        /// <summary>
        /// 删除一行记录
        /// </summary>
        /// <param name="model"></param>
        public void DeleteModel(CModel model)
        {

            if (model == null)
            {
                return;
            }

            string sql = SqlSplice.GetDelete(model);


            try
            {

                SqlRequest.Update(sql, SqlSplice.GetSqlParams());

            }
            catch (Exception ex)
            {

                throw new Exception("删除一行记录出错了", ex);
            }

        }



        /// <summary>
        /// 删除多行记录
        /// </summary>
        /// <param name="models"></param>
        public void DeleteModels(List<CModel> models)
        {

            if (models.Count <= 0)
            {
                return;
            }

            foreach (var model in models)
            {
                string sql = SqlSplice.GetDelete(model);

                try
                {

                    SqlRequest.Update(sql, SqlSplice.GetSqlParams());

                }
                catch (Exception ex)
                {

                    throw new Exception("删除多行记录出错了", ex);
                }

            }
        }



        #endregion



        #region   T操作数据




        /// <summary>
        /// 查询一行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回一条记录</returns>
        public T GetModel<T>(ModelFilter filter)
        {

            string sql = filter.ExecuteSql.ToString();

            CModel model = null;

            try
            {


                SqlDataReader reader = SqlRequest.GetReader(sql, filter.GetParams());

                while (reader.Read())
                {

                    model = new CModel(filter.TableName);

                    foreach (var field in filter.Fields)
                    {
                        model[field] = reader[field];
                    }

                }


                T m_obj = default(T);

                reader.Close();

                if (model == null)
                {
                   return m_obj;
                }

                //string str = JsonConverter.SerializeObject(model.FieldsValue);

                //m_obj = JsonConverter.JsonDeserialize<T>(str);


                m_obj = JsonConverter.CModelToModel<T>(model);

                return m_obj;


            }
            catch (Exception ex)
            {

                throw new Exception("查询一行数据出错了", ex);

            }


        }



        /// <summary>
        /// 根据主键查询一行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetModelByKey<T>(object key)
        {

            Type type = typeof(T);

            string sql = SqlSplice.GetSelect(type, key);

            CModel model = null;

            try
            {

                SqlDataReader reader = SqlRequest.GetReader(sql, SqlSplice.SqlParams.ToArray());

                while (reader.Read())
                {

                    model = new CModel(type);

                    foreach (var field in model.Fields)
                    {
                        model[field] = reader[field];
                    }

                }


                T m_obj = default(T);

                reader.Close();

                if (model == null)
                {
                    return m_obj;
                }

                //var t = Activator.CreateInstance(type);

                m_obj = JsonConverter.CModelToModel<T>(model);

                return m_obj;

            }
            catch (Exception ex)
            {

                throw new Exception("根据主键查询一行记录出错了", ex);

            }
        }



        /// <summary>
        /// 查询多行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetModels<T>()
        {

            Type type = typeof(T);

            string sql = SqlSplice.GetSelect(type);

            List<CModel> models = new List<CModel>();

            try
            {

                SqlDataReader reader = SqlRequest.GetReader(sql);

                while (reader.Read())
                {

                    CModel model = new CModel(type);

                    foreach (var field in model.Fields)
                    {
                        model[field] = reader[field];
                    }

                    models.Add(model);

                }

                reader.Close();


                List<T> m_objs = new List<T>();

                if (models.Count <= 0)
                {
                    return m_objs;
                }

                foreach (var model in models)
                {

                    T m_obj = JsonConverter.CModelToModel<T>(model);

                    m_objs.Add(m_obj);

                }

                return m_objs;

            }
            catch (Exception ex)
            {

                throw new Exception("查询多行记录出错了", ex);

            }

        }



        /// <summary>
        /// 查询多行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        public List<T> GetModels<T>(ModelFilter filter)
        {

            Type type = typeof(T);

            string sql = filter.ExecuteSql.ToString();

            List<CModel> models = new List<CModel>();

            try
            {

                SqlDataReader reader = SqlRequest.GetReader(sql, filter.Params.ToArray());

                while (reader.Read())
                {

                    CModel model = new CModel(type);

                    foreach (var field in model.Fields)
                    {
                        model[field] = reader[field];
                    }

                    models.Add(model);

                }

                reader.Close();


                List<T> m_objs = new List<T>();

                if (models.Count <= 0)
                {
                    return m_objs;
                }

                foreach (var model in models)
                {
                    T m_obj = JsonConverter.CModelToModel<T>(model);

                    m_objs.Add(m_obj);

                }

                return m_objs;

            }
            catch (Exception ex)
            {

                throw new Exception("查询多行记录出错了", ex);

            }

        }




        /// <summary>
        /// 插入一行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public void InsertModel<T>(T model)
        {

            if (model == null)
            {
                return;
            }

            CModel cmodel = null;

            cmodel = JsonConverter.ModelToCModel<T>(model);

            if (cmodel == null)
            {
                throw new Exception("实体对象为空");
            }

            InsertModel(cmodel);

        }



        /// <summary>
        /// 插入多行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        public void InsertModels<T>(List<T> models)
        {

            if (models.Count <= 0)
            {
                return;
            }

            List<CModel> cmodels = new List<CModel>();

            foreach (var model in models)
            {
                CModel cmodel = null;

                cmodel = JsonConverter.ModelToCModel<T>(model);

                if (cmodel == null)
                {
                    throw new Exception("实体对象为空");
                }

                cmodels.Add(cmodel);
            }

            if (cmodels.Count <= 0)
            {
                return;
            }

            InsertModels(cmodels);

        }



        /// <summary>
        /// 更新一行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public void UpdateModel<T>(T model)
        {

            if (model == null)
            {
                return;
            }

            CModel cmodel = null;

            cmodel = JsonConverter.ModelToCModel<T>(model);

            if (cmodel == null)
            {
                throw new Exception("实体对象为空");
            }

            UpdateModel(cmodel);

        }



        /// <summary>
        /// 更新多行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        public void UpdateModels<T>(List<T> models)
        {

            if (models.Count <= 0)
            {
                return;
            }

            List<CModel> cmodels = new List<CModel>();

            foreach (var model in models)
            {
                CModel cmodel = null;

                cmodel = JsonConverter.ModelToCModel<T>(model);

                if (cmodel == null)
                {
                    throw new Exception("实体对象为空");
                }

                cmodels.Add(cmodel);
            }

            if (cmodels.Count <= 0)
            {
                return;
            }

            UpdateModels(cmodels);

        }




        /// <summary>
        /// 删除一行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public void DeleteModel<T>(T model)
        {

            if (model == null)
            {
                return;
            }

            CModel cmodel = null;

            cmodel = JsonConverter.ModelToCModel<T>(model);

            if (cmodel == null)
            {
                throw new Exception("实体对象为空");
            }

            DeleteModel(cmodel);

        }



        /// <summary>
        /// 删除多行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        public void DeleteModels<T>(List<T> models)
        {

            if (models.Count <= 0)
            {
                return;
            }


            List<CModel> cmodels = new List<CModel>();

            foreach (var model in models)
            {

                CModel cmodel = null;

                cmodel = JsonConverter.ModelToCModel<T>(model);

                if (cmodel == null)
                {
                    throw new Exception("实体对象为空");
                }

                cmodels.Add(cmodel);
            }

            if (cmodels.Count <= 0)
            {
                return;
            }


            DeleteModels(cmodels);

        }



        /// <summary>
        /// 删除一行或多行记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        public void DeleteModel(ModelFilter filter)
        {

            string sql = SqlSplice.GetDelete(filter);

            try
            {
                SqlRequest.Update(sql, filter.Params.ToArray());
            }
            catch (Exception ex)
            {

                throw new Exception("删除记录出错了",ex);
            }

        }





        #endregion



        #region  CModelList操作数据

        

        #endregion


        /// <summary>
        /// 查询表的数据总数量
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public int SelectCount(string tableName)
        {

            string sql = $"SELECT COUNT(1) as DATA_COUNT FROM {tableName}";

            SqlDataReader reader = SqlRequest.GetReader(sql);

            int data_count = 0;

            if (reader.Read())
            {
                data_count = Convert.ToInt32(reader["DATA_COUNT"]);
            }

            return data_count;

        }


        /// <summary>
        /// 查询表的数据总数量
        /// </summary>
        /// <param name="type">实体</param>
        /// <returns></returns>
        public int SelectCount(Type type)
        {
            return SelectCount(type.Name);
        }


        



    }
}
