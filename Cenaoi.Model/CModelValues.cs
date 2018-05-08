using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cenaoi.Model
{

    public class CModelValues
    {


        List<CValue> ModelValues = new List<CValue>();



        /// <summary>
        /// 添加一个键值对象 
        /// </summary>
        /// <param name="value"></param>
        public void Add(string key, object value, bool keyAttr = false)
        {

            CValue cValue = new CValue(key, value, keyAttr);

            ModelValues.Add(cValue);

        }



        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public dynamic this[string key]
        {
            get
            {
                object value = string.Empty;

                foreach (var model in ModelValues)
                {
                    if (model.Key == key)
                    {
                        value = model.Value;
                    }
                }

                return value;

            }
            set
            {
                foreach (var model in ModelValues)
                {
                    if (model.Key == key)
                    {
                        model.Value = value;
                    }
                }
            }
        }



        /// <summary>
        /// 键集合
        /// </summary>
        private string[] Keys
        {
            get
            {
                List<string> keys = new List<string>();

                foreach (var model in ModelValues)
                {
                    keys.Add(model.Key);
                }

                return keys.ToArray();

            }

        }



        /// <summary>
        /// 值集合
        /// </summary>
        private object[] Values
        {
            get
            {
                List<object> values = new List<object>();

                foreach (var model in ModelValues)
                {
                    values.Add(model.Value);
                }

                return values.ToArray();
            }

        }




    }


}
