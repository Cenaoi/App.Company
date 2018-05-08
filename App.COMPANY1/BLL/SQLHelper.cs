using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.Text;
using Model;
using Cenaoi.Model;

namespace App.COMPANY1.BLL
{
    public class SQLHelper
    {
        string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();

        public CModel GetData<T>()
        {
            Type type = typeof(T);

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

            SqlConnection conn = new SqlConnection(connString);

            SqlCommand cmd = new SqlCommand(sql.ToString(), conn);

            CModel model = new CModel(typeof(C_ACCOUNT));

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    foreach (var field in fields)
                    {
                        string f_name = field.Name;

                        model[f_name] = reader[f_name];

                    }
                }


                reader.Close();

                return model;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }


        }


    }
}