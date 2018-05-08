using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cenaoi.Json.Serialization;

namespace Cenaoi.Web
{

    /// <summary>
    /// Web请求响应结果库
    /// </summary>
    public class HttpResult
    {



        /// <summary>
        /// 响应的状态
        /// </summary>
        public bool Success { get; set; } = true;


        /// <summary>
        /// 响应的数据
        /// </summary>
        public object ResponseResult { get; set; }



        /// <summary>
        /// 响应的字符串结果
        /// </summary>
        public string ResultData { get; set; }



        /// <summary>
        /// 响应的消息
        /// </summary>
        public string ResponseMsg { get; set; } = "成功了";






    }
}
