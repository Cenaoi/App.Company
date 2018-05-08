using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cenaoi.Model
{

    /// <summary>
    /// 显示名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class DisplayNameAttribute : Attribute
    {
        string m_name;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">显示名称</param>
        public DisplayNameAttribute(string name)
        {
            m_name = name;
            DisplayNames.Add(m_name);
        }

        List<string> DisplayNames = new List<string>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsHasName(object obj)
        {
            if (DisplayNames.Contains(obj))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


    public class DisplayName
    {
        public DisplayName(string name)
        {
            new DisplayNameAttribute(name);
        }
    }

}
