using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cenaoi.Model
{
    public class CValue
    {

        public string Key { get; set; }

        public object Value { get; set; }

        public bool KeyAttr { get; set; }


        public CValue()
        {

        }

        public CValue(string key, object value, bool keyAttr = false)
        {
            Key = key;
            Value = value;
            KeyAttr = keyAttr;
        }

    }
}
