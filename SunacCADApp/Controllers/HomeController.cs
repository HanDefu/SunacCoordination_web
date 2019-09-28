using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;

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

        public ActionResult Get()
        {

       
            return Json(new { code = 1000 });
        }
    }
}