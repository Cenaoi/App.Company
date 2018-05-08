using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cenaoi.Model;

namespace Model
{

    public class C_ACCOUNT : RawModel
    {



        [DBField(FieldName = "ROW_ID", FieldType = typeof(int)), DBKey]
        [DisplayName("记录ID")]
        public int ROW_ID { get; set; }


        [DBField(FieldName = "USER_ID", FieldType = typeof(String), FieldLength = 50)]
        [DisplayName("用户ID")]
        public string USER_ID { get; set; }


        [DBField(FieldName = "USER_NAME", FieldType = typeof(String), FieldLength = 50)]
        [DisplayName("用户名称")]
        public string USER_NAME { get; set; }


        [DBField(FieldName = "USER_PWD", FieldType = typeof(String), FieldLength = 50)]
        [DisplayName("用户密码")]
        public string USER_PWD { get; set; }


        [DBField(FieldName = "USER_CODE", FieldType = typeof(String), FieldLength = 50)]
        [DBCodeKey]
        public string USER_CODE { get; set; }


        public string SEX { get; set; }


        public int AGE { get; set; }


        public string PHONE { get; set; }


        [DBField(FieldName = "CREATE_DATE", FieldType = typeof(DateTime))]
        [DisplayName("创建时间")]
        public DateTime CREATE_DATE { get; set; }

        [DBField(FieldName = "UPDATE_DATE", FieldType = typeof(DateTime))]
        [DisplayName("更新时间")]
        public DateTime UPDATE_DATE { get; set; }

        [DBField(FieldName = "DELETE_DATE", FieldType = typeof(DateTime))]
        [DisplayName("删除时间")]
        public DateTime DELETE_DATE { get; set; }


    }
}
