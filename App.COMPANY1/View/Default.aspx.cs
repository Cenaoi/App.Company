using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.COMPANY1.BLL;
using Model;
using Cenaoi.Model;
using Cenaoi.SQL.Helper;
using Cenaoi.Json.Serialization;
using Cenaoi.DataBase.Configure;

namespace App.COMPANY1.View
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DBManipulator dhelper = DBConfigurator.OpenDBManipulator();


            //List<CModel> models1 = dhelper.GetModels();


            //ModelFilter filter = new ModelFilter(typeof(C_ACCOUNT));
            //filter.AndParam("ROW_ID", 40);

            //CModel jsonmode = dhelper.GetModel(filter);


            //C_ACCOUNT s = JsonConverter.CModelToModel<C_ACCOUNT>(jsonmode);

            //string getdata = jsonmode.GetValue("USER_NAME");


            //C_ACCOUNT c = dhelper.GetModel<C_ACCOUNT>(filter);   //实体获取日期时间出错了,导致转换json数据的日期格式错误

            //c.CREATE_DATE = DateTime.Now;
            //c.DELETE_DATE = DateTime.Now;
            //c.UPDATE_DATE = DateTime.Now;



            //List<C_ACCOUNT> models2 = dhelper.GetModels<C_ACCOUNT>(filter);

            //CModel jsonmodel = dhelper.GetModel(filter);

            //string json1 = JsonConverter.CModelToJson(jsonmodel);

            //string json2 = JsonConverter.JsonSerializer(c);

            //CModel md = new CModel();

            //md = JsonConverter.JsonToCModel(json2);

            ////dhelper.DeleteModel(filter);

            ////dhelper.DeleteModels(models2);

            ////filter.AndParam("USER_ID", "C22");

            //ModelFilter filter1 = new ModelFilter(typeof(C_ACCOUNT));

            ////filter1.AndParam("ROW_ID", 1);

            //C_ACCOUNT model = dhelper.GetModel<C_ACCOUNT>(filter1);


            //dhelper.InsertModel(model);




            //List<C_ACCOUNT> c_accounts = dhelper.GetModels<C_ACCOUNT>();


            //dhelper.InsertModels(c_accounts);


            //ModelFilter filter2 = new ModelFilter("C_ACCOUNT");


            //CModel cm = dhelper.GetModel(filter1);




            ////model.SEX = "女";

            //cm["SEX"] = "女";

            //dhelper.UpdateModel(cm);

            CModel modela = new CModel(typeof(C_ACCOUNT));
            modela["USER_ID"] = "C21";
            modela["USER_NAME"] = "詹姆斯";
            modela["USER_PWD"] = "15768091384";
            modela["SEX"] = "男";
            modela["AGE"] = 34;
            modela["PHONE"] = "13232138056";
            modela["CREATE_DATE"] = DateTime.Now;
            modela["UPDATE_DATE"] = DateTime.Now;
            modela["DELETE_DATE"] = DateTime.Now;

            dhelper.InsertModel(modela);




            List<CModel> modess = new List<CModel>();

            modela = new CModel(typeof(C_ACCOUNT));
            modela["USER_ID"] = "C22";
            modela["USER_NAME"] = "詹姆斯";
            modela["USER_PWD"] = "15768091384";
            modela["SEX"] = "男";
            modela["AGE"] = 34;
            modela["PHONE"] = "13232138056";
            modela["CREATE_DATE"] = DateTime.Now;
            modela["UPDATE_DATE"] = DateTime.Now;
            modela["DELETE_DATE"] = DateTime.Now;

            modess.Add(modela);


            modela = new CModel(typeof(C_ACCOUNT));
            modela["USER_ID"] = "C23";
            modela["USER_NAME"] = "詹姆斯";
            modela["USER_PWD"] = "15768091384";
            modela["SEX"] = "男";
            modela["AGE"] = 34;
            modela["PHONE"] = "13232138056";
            modela["CREATE_DATE"] = DateTime.Now;
            modela["UPDATE_DATE"] = DateTime.Now;
            modela["DELETE_DATE"] = DateTime.Now;

            modess.Add(modela);



            modela = new CModel(typeof(C_ACCOUNT));
            modela["USER_ID"] = "C24";
            modela["USER_NAME"] = "詹姆斯";
            modela["USER_PWD"] = "15768091384";
            modela["SEX"] = "男";
            modela["AGE"] = 34;
            modela["PHONE"] = "13232138056";
            modela["CREATE_DATE"] = DateTime.Now;
            modela["UPDATE_DATE"] = DateTime.Now;
            modela["DELETE_DATE"] = DateTime.Now;

            modess.Add(modela);


            modela = new CModel(typeof(C_ACCOUNT));
            modela["USER_ID"] = "C25";
            modela["USER_NAME"] = "詹姆斯";
            modela["USER_PWD"] = "15768091384";
            modela["SEX"] = "男";
            modela["AGE"] = 34;
            modela["PHONE"] = "13232138056";
            modela["CREATE_DATE"] = DateTime.Now;
            modela["UPDATE_DATE"] = DateTime.Now;
            modela["DELETE_DATE"] = DateTime.Now;

            modess.Add(modela);


            modela = new CModel("C_ACCOUNT");
            modela["USER_ID"] = "C26";
            modela["USER_NAME"] = "詹姆斯";
            modela["USER_PWD"] = "15768091384";
            modela["SEX"] = "男";
            modela["AGE"] = 34;
            modela["PHONE"] = "13232138056";
            modela["CREATE_DATE"] = DateTime.Now;
            modela["UPDATE_DATE"] = DateTime.Now;
            modela["DELETE_DATE"] = DateTime.Now;

            modess.Add(modela);

            dhelper.InsertModels(modess);




        }
    }
}