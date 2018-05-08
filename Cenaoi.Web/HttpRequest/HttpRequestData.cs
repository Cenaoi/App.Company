using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.Collections.Concurrent;

namespace Cenaoi.Web.HttpRequest
{

    /// <summary>
    /// 用来获取请求HTTP的数据
    /// </summary>
    public class HttpRequestData
    {

        /*
        
            注意：post的参数名和地址传的参数名相同会冲突(要区分) 
             
        */


        #region   获取POST参数



        /// <summary>
        /// 请求的参数集合
        /// </summary>
        private static ConcurrentDictionary<string, object> m_RequestParams = null;



        /// <summary>
        /// 请求的参数集合
        /// </summary>
        public static ConcurrentDictionary<string, object> RequestParams
        {
            get
            {
                if (m_RequestParams == null || m_RequestParams.Count == 0)
                {
                    InitHttpRequestParams();
                }
                return m_RequestParams;
            }
            set { m_RequestParams = value; }

        }



        /// <summary>
        /// 初始化获取post请求参数
        /// </summary>
        private static void InitHttpRequestParams()
        {

            m_RequestParams = new ConcurrentDictionary<string, object>();

            foreach (var param in HttpContext.Current.Request.Form)
            {
                string param_name = param.ToString();

                object val = HttpContext.Current.Request.Params[param_name];

                if (UrlParams != null)
                {
                    if (UrlParams.ContainsKey(param_name))  //如果地址传的参数是否与post的参数键名相同
                    {
                        string v = val.ToString();

                        Type t = val.GetType();

                        v = v.Replace(",", string.Empty);
                        v = v.Replace(m_UrlParams[param_name].ToString(), string.Empty);  //将地址上的参数的值替换为空

                        val = v;  //剩下post的该参数的值

                    }
                }

                m_RequestParams.TryAdd(param_name, val);

                
            }

        }




        /// <summary>
        /// 判断是否存在该参数
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns></returns>
        public static bool IsHasRequestParam(string param_name)
        {
            if (RequestParams == null)
            {
                return false;
            }

            if (RequestParams.Keys.Contains(param_name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 获取请求参数值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="param_name">参数名</param>
        /// <returns>返回指定类型的值</returns>
        public static T Form<T>(string param_name)
        {

            if (string.IsNullOrWhiteSpace(param_name))
            {
                return default(T);
            }

            if (!IsHasRequestParam(param_name))
            {
                return default(T);
            }

            object param_value = HttpContext.Current.Request.Form[param_name];

            return (T)Convert.ChangeType(param_value, typeof(T));

        }



        /// <summary>
        /// 获取请求参数值
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回整型的值</returns>
        public static int FormInt(string param_name)
        {
            return Form<int>(param_name);
        }



        /// <summary>
        /// 获取请求参数值
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回字符串型的值</returns>
        public static string FormString(string param_name)
        {
            return Form<string>(param_name);
        }



        /// <summary>
        /// 获取请求参数值
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回浮点型的值</returns>
        public static double FormDouble(string param_name)
        {
            return Form<double>(param_name);
        }



        /// <summary>
        /// 获取请求参数值 ( 例："\"[1,2,3,4,5]\"" = 1,2,3,4,5 )
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="param_name">参数名</param>
        /// <returns>返回：指定类型数组</returns>
        public static T[] FormArray<T>(string param_name)
        {

            if (string.IsNullOrWhiteSpace(param_name))
            {
                return null;
            }

            if (!IsHasRequestParam(param_name))
            {
                return null;
            }

            object param_value = RequestParams[param_name];

            string str_val = param_value.ToString();

            str_val = str_val.Replace("\"[", string.Empty);

            str_val = str_val.Replace("]\"", string.Empty);

            string[] arr_val = str_val.Split(',');

            if (arr_val == null || arr_val.Length <= 0)
            {
                return null;
            }

            List<T> list_val = new List<T>();

            foreach (var val in arr_val)
            {
               T obj = (T)Convert.ChangeType(val, typeof(T));

               list_val.Add(obj);
            }

            return list_val.ToArray();

        }



        /// <summary>
        /// 获取请求参数值
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回：字符串类型数组</returns>
        public static string[] FormStringArray(string param_name)
        {
            return FormArray<string>(param_name);
        }



        /// <summary>
        /// 获取请求参数值
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回：整形数组</returns>
        public static int[] FormIntArray(string param_name)
        {
            return FormArray<int>(param_name);
        }



        /// <summary>
        /// 获取请求参数值
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回：双精度类型数组</returns>
        public static double[] FormDoubleArray(string param_name)
        {
            return FormArray<double>(param_name);
        }




        /// <summary>
        /// 清除所有参数
        /// </summary>
        public static void ClearParam()
        {
            RequestParams.Clear();
        }



        #endregion



        #region   获取地址上请求的参数



        /// <summary>
        /// 地址上的参数集合
        /// </summary>
        private static Dictionary<string, object> m_UrlParams = null;


        /// <summary>
        /// 地址上的参数集合
        /// </summary>
        public static Dictionary<string, object> UrlParams
        {
            get
            {
                if (m_UrlParams == null || m_UrlParams.Count == 0)
                {
                    InitUrlParams();
                }
                return m_UrlParams;
            }
            set { m_UrlParams = value; }

        }



        /// <summary>
        /// 地址上的参数(数组)
        /// </summary>
        private static string[] m_arr_UrlParams = null;



        /// <summary>
        /// 初始化获取RequestUrl上的所有参数
        /// </summary>
        private static void InitUrlParams()
        {

            m_UrlParams = new Dictionary<string, object>();

            string requestUrl = HttpContext.Current.Request.RawUrl;

            if (requestUrl.Contains("?"))
            {
                int queryindex = requestUrl.IndexOf('?');
                string urlparam = requestUrl.Substring(queryindex + 1, requestUrl.Length - queryindex - 1);
                m_arr_UrlParams = urlparam.Split('&');

                if (!string.IsNullOrWhiteSpace(urlparam))
                {
                    foreach (var param in m_arr_UrlParams)
                    {
                        string[] p = param.Split('=');
                        m_UrlParams.Add(p[0], p[1]);
                    }
                }

            }






        }



        /// <summary>
        /// 判断是否存在该参数
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns></returns>
        public static bool IsHasUrlParam(string param_name)
        {
            if (UrlParams.Keys.Contains(param_name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 获取地址上的参数
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="param_name">参数名</param>
        /// <returns>返回指定类型的值</returns>
        public static T Query<T>(string param_name)
        {
            if (string.IsNullOrWhiteSpace(param_name))
            {
                return default(T);
            }
            if (!IsHasUrlParam(param_name))
            {
                return default(T);
            }

            object param_value = UrlParams[param_name];

            return (T)Convert.ChangeType(param_value, typeof(T));

        }



        /// <summary>
        /// 获取地址上的参数
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回整形的值</returns>
        public static int QueryInt(string param_name)
        {
            return Query<int>(param_name);
        }



        /// <summary>
        /// 获取地址上的参数
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回字符串型的值</returns>
        public static string QueryString(string param_name)
        {
            return Query<string>(param_name);
        }



        /// <summary>
        /// 获取地址上的参数
        /// </summary>
        /// <param name="param_name">参数名</param>
        /// <returns>返回浮点型的值</returns>
        public static double QueryDouble(string param_name)
        {
            return Query<double>(param_name);
        }



        #endregion


        public static int Count()
        {
            return m_RequestParams.Count;
        }


    }
}
