using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cenaoi.Model
{

    /// <summary>
    /// 主键编码
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class DBCodeKeyAttribute : Attribute
    {
        public DBCodeKeyAttribute()
        {

        }
    }



    /// <summary>
    /// 主键编码
    /// </summary>
    public class DBCodeKey
    {

        public DBCodeKey()
        {
            new DBKeyAttribute();
        }


        public int MyProperty { get; set; }

    }





}
