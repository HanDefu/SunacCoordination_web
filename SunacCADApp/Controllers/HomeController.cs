using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.PI;
using SunacCADApp;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Library;


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
            if (string.IsNullOrEmpty(username)) 
            {
                return Json(new { code = -100, message = "用户名不能为空" }, JsonRequestBehavior.AllowGet);
            }

            Sys_User user = Sys_UserDB.GetSingleEntityByparam(" And User_Name='" + username + "'");
          
            if (user.Is_Internal == 2) {
                if (user.User_Psd != password)
                {
                    return Json(new { code = -101, message = "用户密码不能为空" }, JsonRequestBehavior.AllowGet);
                }
                else 
                {
                    string userid = user.Id.ConventToString(string.Empty);
                    string roleId=user.RoleID.ConventToString(string.Empty);
                    string isInternal=user.Is_Internal.ConventToString(string.Empty);
                    InitUtility.Instance.InitSessionHelper.Add("UserID", userid);
                    InitUtility.Instance.InitSessionHelper.Add("UserName",user.User_Name);
                    InitUtility.Instance.InitSessionHelper.Add("RoleId", roleId);
                    InitUtility.Instance.InitSessionHelper.Add("IsInternal", isInternal);
                    return Json(new { code = 100, message = "内部用户登陆成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (user.Is_Internal == 1)
            {
                WebService932.Header header = new WebService932.Header();
                header.BIZTRANSACTIONID = "sdfdssdfds";
                header.COUNT = "";
                header.CONSUMER = "";
                header.ACCOUNT = "wdaccount";
                header.PASSWORD = "wdpwd";
                WebService932.user webUser = new WebService932.user();
                webUser.username = username;
                webUser.password = password;
                WebService932.IDM_SUNAC_392_validatePwd_pttbindingQSService client = new WebService932.IDM_SUNAC_392_validatePwd_pttbindingQSService();
                client.commonHeader = header;
                string LIST = "";
                WebService932.HEADER backheader = client.IDM_SUNAC_392_validatePwd(webUser, out LIST);
                int resultcode = backheader.RESULT.ConvertToInt32(0);
                if (resultcode == -1)
                {
                    return Json(new { code = -101, message = "其他错误" }, JsonRequestBehavior.AllowGet);
                }
                else if (resultcode == 0)
                {
                    string userid = user.Id.ConventToString(string.Empty);
                    string roleId = user.RoleID.ConventToString(string.Empty);
                    string isInternal = user.Is_Internal.ConventToString(string.Empty);
                    InitUtility.Instance.InitSessionHelper.Add("UserID", userid);
                    InitUtility.Instance.InitSessionHelper.Add("UserName", user.User_Name);
                    InitUtility.Instance.InitSessionHelper.Add("RoleId", roleId);
                    InitUtility.Instance.InitSessionHelper.Add("IsInternal", isInternal);
                    return Json(new { code = 100, message = "用户名密码验证成功" }, JsonRequestBehavior.AllowGet);
                }
                else if (resultcode == 1)
                {
                    return Json(new { code = 100, message = "用户名不存在" }, JsonRequestBehavior.AllowGet);
                }
                else if (resultcode == 2)
                {
                    return Json(new { code = 100, message = "密码错误" }, JsonRequestBehavior.AllowGet);
                }
                else if (resultcode == 3)
                {
                    return Json(new { code = 100, message = "参数不能为空" }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new { code = -101, message = "接口异常" }, JsonRequestBehavior.AllowGet);
                }

            }
            else 
            {
                return Json(new { code = -100, message = "用户名不能为空" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        /// <web> /home/loginout </web>
        public ActionResult LoginOut() 
        {
            InitUtility.Instance.InitSessionHelper.Del("UserID");
            InitUtility.Instance.InitSessionHelper.Del("UserName");
            InitUtility.Instance.InitSessionHelper.Del("RoleId");
            InitUtility.Instance.InitSessionHelper.Del("IsInternal");
            return Redirect("/home");
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

            string XMLResp = XmlSerializeHelper.XmlSerialize<cn.com.sunac.sappoqas.DT_MDM_Projectstage_RESP>(response);
            return Json(new { code = -100, message = "开发成功" }, JsonRequestBehavior.AllowGet);
            
        }


    }
}