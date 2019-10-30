using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.PI;


namespace SunacCADApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        ///  /home/usercenter
        /// </summary>
        /// <returns></returns>
        public ActionResult UserCenter() 
        {

            IList<Sys_Model> models = Sys_ModelDB.GetPageInfoByParameter("1=1", string.Empty, 0, 30);
            ViewBag.models = models;
            return View();
        }


        /// <summary>
        /// Home/CheckUser
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckUser() 
        {
            string username = Request.Form["username"];
            string password = CommonLib.UserMd5(Request.Form["password"]);
            Sys_User user = Sys_UserDB.GetSingleEntityByparam(" And User_Name='"+username+"'");
            if (string.IsNullOrEmpty(user.User_Psd)) 
            {
                return Json(new { code = -100, message = "用户名不正确" }, JsonRequestBehavior.AllowGet);
            }
            if (user.User_Psd!= password) 
            {
                return Json(new { code = -100, message = "密码不正确" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 100, message = "用户登陆成功" }, JsonRequestBehavior.AllowGet);
            
        }


        /// <summary>
        ///   /home/get
        /// </summary>
        /// <returns></returns>
        public ActionResult Get()
        {
            cn.com.sunac.sappoqas.DT_COM_Projectstage_REQBaseInfo baseInfo = new cn.com.sunac.sappoqas.DT_COM_Projectstage_REQBaseInfo();
            baseInfo.ServiceName="SI_MD_COMMON_OUT";
            baseInfo.SourceSystem="BS-CAD-Q";
            baseInfo.TargetSystem="S4QCLNT200";
            cn.com.sunac.sappoqas.DT_COM_Projectstage_REQ request = new cn.com.sunac.sappoqas.DT_COM_Projectstage_REQ();
            request.BaseInfo = baseInfo;
            request.Begindate = "20180911095448.3007171";
            request.Numb = "100";

            cn.com.sunac.sappoqas.SI_MD_COMMON_OUTService client = new cn.com.sunac.sappoqas.SI_MD_COMMON_OUTService();
            client.Credentials = new System.Net.NetworkCredential("POQ_CAD", "cad@1234");
            cn.com.sunac.sappoqas.DT_MDM_Projectstage_RESP response = client.SI_MD_PROJEC_STAGE_OUT(request);
            return Json(new { code = -100, message = "开发成功" }, JsonRequestBehavior.AllowGet);
            
        }


    }
}