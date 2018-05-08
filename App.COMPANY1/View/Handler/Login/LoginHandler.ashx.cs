using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cenaoi.DataBase.Configure;
using Cenaoi.Model;
using Cenaoi.SQL.Helper;
using Model;
using Cenaoi.Web.HttpRequest;
using Cenaoi.Web;
using System.Reflection;
using Cenaoi.Json.Serialization;

namespace App.COMPANY1.View.Handler.Login
{
    /// <summary>
    /// LoginHandler 的摘要说明
    /// </summary>
    public class LoginHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = HttpRequestData.Form<string>("action");

            HttpResult result = null;


            string ac = HttpRequestData.Form<string>("action1");


            

            try
            {

                switch (action)
                {

                    case "GO_LOGIN":
                        result = Login();
                        break;
                    case "GO_TEST_CMODELLIST":
                        result = TestCModelList();
                        break;
                    default:
                        break;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            HttpRequestData.ClearParam();
            int count = HttpRequestData.Count();
            context.Response.Write(result.ResultData);

        }



        /// <summary>
        /// 登录
        /// </summary>
        public HttpResult Login()
        {

            string name = HttpRequestData.FormString("user_name");
            string pwd = HttpRequestData.Form<string>("user_pwd");

            DBManipulator manipulator = DBConfigurator.OpenDBManipulator();

            ModelFilter filter = new ModelFilter(typeof(C_ACCOUNT));
            filter.PageQuery = new PagingQuery(5, 0);


            //int count = manipulator.SelectCount(typeof(C_ACCOUNT));


            //filter.AndParam("USER_NAME", name);
            //filter.AndParam("USER_PWD", pwd);
            //filter.PageQuery = new PagingQuery() { PageCount = 5, PageIndex = 0 };

            //CModel cm = new CModel(typeof(C_ACCOUNT));


            //C_ACCOUNT xincm = manipulator.GetModelByKey<C_ACCOUNT>(43);


            //DateTime st = xincm.CREATE_DATE;

            List<CModel> cs = manipulator.GetModels(filter);

            //C_ACCOUNT model = manipulator.GetModel<C_ACCOUNT>(filter);


            //string c_json1 = JsonConverter.JsonSerializer<C_ACCOUNT>(model);

            //C_ACCOUNT c_json2 = JsonConverter.JsonDeserialize<C_ACCOUNT>(c_json1);


            //CModel cmodel = manipulator.GetModel(filter);

            //string c_json3 = JsonConverter.CModelToJson(cmodel);

            //CModel c_json4 = JsonConverter.JsonToCModel(c_json3);


            //C_ACCOUNT dsa = JsonConverter.CModelToModel<C_ACCOUNT>(c_json4);

            //CModel cmode = manipulator.GetModelByKey("C_ACCOUNT", 43);


            //CModel cmodel = manipulator.GetModel(filter);

            //CModel col = new CModel();
            //col["user"] = "cc";
            //col["pwd"] = "123";

            //List<CModel> firends = new List<CModel>();
            //firends.Add(cmodel);
            //firends.Add(cmodel);
            //col["firends"] = firends;

            //List<string> familys = new List<string>();
            //familys.Add("aa");
            //familys.Add("bb");
            //familys.Add("dd");
            //col["familys"] = familys;

            //CModel col1 = new CModel();
            //col1["user"] = "cc";
            //col1["pwd"] = "123";
            //col1["firends"] = firends;
            //col1["familys"] = familys;
            //col1["col"] = col;
            //col1["col1"] = col;

            //CModelList<C_ACCOUNT> models = new CModelList<C_ACCOUNT>();

            //foreach (var item in models)
            //{
                 
            //}


            return WebResponse.Result("");


        }




        /// <summary>
        /// 测试CModelList  发现问题：
        /// </summary>
        /// <returns></returns>
        public HttpResult TestCModelList()
        {


            CModelList models = new CModelList();
            models.Add(new CModel()
            {
                ["id"] = 1,
                ["name"] = "小白"
            });
            models.Add(new CModel()
            {
                ["id"] = 2,
                ["name"] = "小黑"
            });
            models.Add(new CModel()
            {
                ["id"] = 3,
                ["name"] = "小红"
            });



            return WebResponse.Result("测试");
        }














        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}