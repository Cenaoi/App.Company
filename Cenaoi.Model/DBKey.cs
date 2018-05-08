using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cenaoi.Model
{

    /// <summary>
    /// 主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class DBKeyAttribute : Attribute
    {
        public DBKeyAttribute()
        {

        }
    }



    public class DBKey
    {
        public DBKey()
        {
            new DBKeyAttribute();
        }
    }

}
