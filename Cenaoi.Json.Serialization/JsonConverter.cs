using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Cenaoi.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cenaoi.Json.Serialization
{


    /// <summary>
    /// Json操作库
    /// </summary>
    public class JsonConverter
    {



        #region  第一种



        /// <summary>
        /// JSON序列化（带日期格式转换）
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);

            string jsonString = Encoding.UTF8.GetString(ms.ToArray());

            //替换Json的Date字符串
            string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            ms.Close();

            return jsonString;



        }



        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }



        /// <summary>
        /// JSON反序列化(Json字符串转对象)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>对象</returns>
        public static T JsonDeserializer<T>(string jsonString)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            T obj = (T)ser.ReadObject(ms);

            return obj;

        }



        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static object JsonDeserializer_v1<T>(string jsonString)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            return ser.ReadObject(ms);

        }



        /// <summary>
        /// JSON序列化(对象集合转Json字符串)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="objs">对象集合</param>
        /// <returns></returns>
        public static string JsonSerializer<T>(List<T> objs)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<T>));

            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, objs);

            string jsonString = Encoding.UTF8.GetString(ms.ToArray());

            //替换Json的Date字符串
            string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            ms.Close();

            return jsonString;

        }



        /// <summary>
        /// JSON反序列化(Json字符串转对象集合)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>返回对象集合</returns>
        public static List<T> JsonDeserializes<T>(string jsonString)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<T>));

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            Object obj = ser.ReadObject(ms);

            List<T> objlist = (List<T>)obj;

            return objlist;

        }



        #endregion



        #region  第二种



        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerialize(object obj)
        {

            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {

                string jsonString = jss.Serialize(obj);

                //替换Json的Date字符串
                string p = @"///Date/((/d+)/+/d+/)///"; /*////Date/((([/+/-]/d+)|(/d+))[/+/-]/d+/)////*/
                MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);

                return jsonString;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {

            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                T obj = jss.Deserialize<T>(jsonString);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        /// <summary>    
        /// 将时间字符串转为Json时间    
        /// </summary>    
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("///Date({0}+0800)///", ts.TotalMilliseconds);
            return result;
        }





        #endregion



        #region   命名空间Newtonsoft



        /// <summary>
        /// JSON序列化（带日期格式转换）
        /// </summary>
        /// <returns></returns>
        public static string SerializeObject<T>(T t)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            string jsonString = JsonConvert.SerializeObject(t, Formatting.Indented, timeFormat);

            return jsonString;
        }



        #endregion



        #region  针对CModel转换



        /// <summary>
        /// json数据转CModel
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="str_json"></param>
        /// <returns></returns>
        public static CModel JsonToCModel<T>(string str_json)
        {
            CModel model = new CModel(typeof(T));

            model.FieldsValue = JsonDeserialize<Dictionary<string, object>>(str_json);

            List<string> fields = new List<string>();

            foreach (var filed in model.FieldsValue.Keys)
            {
                fields.Add(filed);
            }

            model.Fields = fields;

            return model;
        }



        /// <summary>
        /// json数据转CModel
        /// </summary>
        /// <param name="str_json"></param>
        /// <returns></returns>
        public static CModel JsonToCModel(string str_json)
        {
            CModel model = new CModel();

            model.FieldsValue = JsonDeserialize<Dictionary<string, object>>(str_json);

            List<string> fields = new List<string>();

            foreach (var filed in model.FieldsValue.Keys)
            {
                fields.Add(filed);
            }

            model.Fields = fields;

            return model;
        }



        /// <summary>
        /// 未完成的方法
        /// </summary>
        /// <param name="str_json"></param>
        /// <returns></returns>
        public static List<CModel> JsonToCModels(string str_json)
        {
            List<CModel> cmodels = null;




            return cmodels;
        }



        /// <summary>
        /// CModel转json数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string CModelToJson(CModel model)
        {


            string json = JsonSerializer(model.FieldsValue);

            //object m_obj = JsonConverter.JsonDeserialize(json);

            json = json.Replace(",\"Value\"", string.Empty);
            json = json.Replace("\"Key\":", string.Empty);
            json = json.Replace("{", string.Empty);
            json = json.Replace("}", string.Empty);
            json = json.Replace("[", "{");
            json = json.Replace("]", "}");

            return json;

        }



        /// <summary>
        /// CModel转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T CModelToModel<T>(CModel model)
        {

            string json = JsonSerializer(model.FieldsValue);

            //object m_obj = JsonConverter.JsonDeserialize(json);

            json = json.Replace(",\"Value\"", string.Empty);
            json = json.Replace("\"Key\":", string.Empty);
            json = json.Replace("{", string.Empty);
            json = json.Replace("}", string.Empty);
            json = json.Replace("[", "{");
            json = json.Replace("]", "}");

            return JsonDeserialize<T>(json);

        }



        /// <summary>
        /// CModel转实体集合
        /// </summary>
        /// <param name="cmodels"></param>
        /// <returns></returns>
        public static List<T> CModelToModels<T>(List<CModel> cmodels)
        {
            List<T> objs = null;

            foreach (var cmodel in cmodels)
            {
                T obj = CModelToModel<T>(cmodel);

                if (obj != null)
                {
                    objs.Add(obj);
                }

            }

            return objs;
        }



        /// <summary>
        /// 实体转CModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static CModel ModelToCModel<T>(T t)
        {
            CModel model = null;

            string str_json = ObjectToJson<T>(t);

            model = JsonToCModel<T>(str_json);

            return model;
        }



        /// <summary>
        /// 实体集合转CModel集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static List<CModel> ModelToCModels<T>(List<T> objs)
        {
            List<CModel> cmodels = null;


            foreach (var obj in objs)
            {
                CModel cmodel = ModelToCModel<T>(obj);
                cmodels.Add(cmodel);
            }


            return cmodels;
        }



        #endregion



        /// <summary>
        /// json转对象(推荐使用)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>对象</returns>
        public static T JsonToObject<T>(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                T obj = jss.Deserialize<T>(json);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 对象转json (推荐使用)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns>json数据</returns>
        public static string ObjectToJson<T>(T t)
        {

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            string jsonString = JsonConvert.SerializeObject(t, Formatting.Indented, timeFormat);

            return jsonString;

        }



        /// <summary>
        /// json转对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> JsonToObjects<T>(string json)
        {
            List<T> objs = null;

            objs = JsonDeserializes<T>(json);

            return objs;
        }



        /// <summary>
        ///  对象集合转json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static string ObjectToJsons<T>(List<T> objs)
        {
            string str_json = string.Empty;


            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            str_json = JsonConvert.SerializeObject(objs, Formatting.Indented, timeFormat);

            return str_json;
        }




    }
}
