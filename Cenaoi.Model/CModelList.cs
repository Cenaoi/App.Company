using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Cenaoi.Model
{
    /// <summary>
    /// 简单的数据集合
    /// </summary>
    public class CModelList : List<CModel>, ISerializable
    {

        private List<CModel> m_Items { get; set; }


        /// <summary>
        /// 简单数据集合的构造方法
        /// </summary>
        public CModelList()
        {

        }



        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }


    }
}
