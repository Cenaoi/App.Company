using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Cenaoi.CModel
{
    public class CModel: ISerializable
    {


        public CModel()
        {

        }


        /// <summary>
        /// 数据集合
        /// </summary>
        private Dictionary<string, object> values = new Dictionary<string, object>();


        /// <summary>
        /// 字段名集合
        /// </summary>
        private List<string> fields = null;   


        /// <summary>
        /// 初始化Model
        /// </summary>
        /// <param name="model"></param>
        public CModel(Type model)
        {

            Type type = model;

            PropertyInfo[] model_fields = type.GetProperties();

            fields = new List<string>();

            foreach (var field in model_fields)
            {
                values.Add(field.Name, "");
                fields.Add(field.Name);
            }

        }



        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public dynamic this[string field]
        {
            get { return values[field]; }
            set { values[field] = value; }
        }



        /// <summary>
        /// 获取字段集合
        /// </summary>
        public List<string> GetFields { get { return fields; } set {; } }




        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }


    }
}
