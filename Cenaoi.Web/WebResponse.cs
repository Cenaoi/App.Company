using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cenaoi.Json.Serialization;
using Cenaoi.Model;

namespace Cenaoi.Web
{

    /// <summary>
    /// Web请求响应库
    /// </summary>
    public class WebResponse
    {



        /// <summary>
        /// 响应请求的数据
        /// </summary>
        public static HttpResult Result(object data)
        {
            HttpResult result = new HttpResult();

            result.ResponseResult = data;

            if (result.ResponseResult == null)
            {
                result.Success = false;
                result.ResponseMsg = "错误";
            }
            else
            {
                result.Success = true;
                result.ResponseMsg = "正确";
            }

            result.ResultData = ToJson(result);

            return result;

        }



        /// <summary>
        /// 响应请求的数据
        /// </summary>
        public static HttpResult Result(CModel model)
        {

            object data = model.FieldsValue;

            HttpResult result = new HttpResult();

            result.ResponseResult = data;

            if (result.ResponseResult == null)
            {
                result.Success = false;
                result.ResponseMsg = "错误";
            }
            else
            {
                result.Success = true;
                result.ResponseMsg = "正确";
            }

            result.ResultData = ToJson(result);

            return result;

        }



        /// <summary>
        /// 错误信息
        /// </summary>
        public static HttpResult ErrorMsg(string msg)
        {
            HttpResult result = new HttpResult();

            result.Success = false;

            result.ResponseMsg = msg;

            result.ResponseResult = null;

            result.ResultData = ToJson(result);

            return result;

        }



        /// <summary>
        /// 正确消息
        /// </summary>
        /// <param name="msg"></param>
        public static HttpResult SuccessMsg(string msg)
        {

            HttpResult result = new HttpResult();

            result.Success = true;

            result.ResponseMsg = msg;

            result.ResultData = ToJson(result);

            return result;

        }



        /// <summary>
        /// 转换Json数据(存在问题：日期格式转换失败)
        /// </summary>
        /// <returns></returns>
        public static string ToJson(HttpResult result)
        {

            //var RespnoseData = new[] { result.Success, result.ResponseMsg, result.ResponseResult }; 

            //采用键值对方式
            Dictionary<string, object> RespnoseData = new Dictionary<string, object>();

            RespnoseData.Add("success", result.Success);
            RespnoseData.Add("msg", result.ResponseMsg);
            RespnoseData.Add("data", result.ResponseResult);

            string json = JsonConverter.SerializeObject(RespnoseData);

            return json;

        }









    }
}
