using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cenaoi.Model
{

    //限定特性类的应用范围
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    //定制MsgAttribute特性类，继承于Attribute
    public class DBFieldAttribute : Attribute
    {
        //定义_msg字段和Msg属性//Msg属性用于读写msg字段
        string m_FieldName;

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get { return m_FieldName; } set { m_FieldName = value; } }



        Type m_FieldType;
        /// <summary>
        /// 字段类型
        /// </summary>
        public Type FieldType { get { return m_FieldType; } set { m_FieldType = value; } }



        int m_FieldLength;
        /// <summary>
        /// 字段长度
        /// </summary>
        public int FieldLength { get { return m_FieldLength; } set { m_FieldLength = value; } }


        public DBFieldAttribute() { }


        /// <summary>
        /// 重载构造函数接收一个参数，赋值给m_FieldName字段
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <param name="length"></param>
        public DBFieldAttribute(string field,Type type,int length)
        {
            m_FieldName = field;
            m_FieldType = type;
            m_FieldLength = length;
            
        }


        /// <summary>
        /// 重载构造函数接收一个参数，赋值给_msg字段
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        public DBFieldAttribute(string field, Type type)
        {
            m_FieldName = field;
            m_FieldType = type;

        }


    }


    /// <summary>
    /// 简单实体字段
    /// </summary>
    public class DBField
    {
        /// <summary>
        /// 简单实体字段属性
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <param name="length"></param>
        public DBField(string field, Type type, int length)
        {
            new DBFieldAttribute(field, type, length);
        }
    }

}
