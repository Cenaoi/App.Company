using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cenaoi.Web.HttpRequest
{
    /// <summary>
    /// 请求参数集合
    /// </summary>
    public class RequestParamCollection
    {
        ConcurrentDictionary<string, List<RequestParam>> m_Param = new ConcurrentDictionary<string, List<RequestParam>>();


        /// <summary>
        /// 集合数量
        /// </summary>
        public int Count
        {
            get
            {
                return m_Param.Count;
            }
        }


        public List<RequestParam> this[string key]
        {
            get
            {
                return m_Param[key];
            }
        }




    }


}
