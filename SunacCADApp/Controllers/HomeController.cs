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
using Common.Utility.Lab;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



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
             string useName = Request.Form["username"];
            string username = API_Common.FilterIllegalChar(useName);
            string pwd = Request.Form["password"].ConvertToTrim();
            string password = CommonLib.UserMd5(Request.Form["password"]);
            int logCode = 1;
            string logName = "用户登陆";
            string logInfo = string.Empty;
            string logDesc = string.Empty;
            string createBy = useName;
            int createUserId = 0;
            string ipAddress = ClientPublicLib.GetLoginIp();
            if (string.IsNullOrEmpty(username)) 
            {

                logDesc = string.Format(@"用户名存在非法字符；IP:{0}", ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                return Json(new { code = -100, message = "用户名或密码错误" }, JsonRequestBehavior.AllowGet);
            }

            Sys_User user = Sys_UserDB.GetSingleEntityByparam(" And User_Name='" + username + "'");
          
            if (user.Is_Internal == 2) {
                if (user.User_Psd != password)
                {
                    logDesc = string.Format(@"密码错误；IP:{0}", ipAddress);
                    SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                    return Json(new { code = -101, message = "用户名或密码错误" }, JsonRequestBehavior.AllowGet);
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
                    logDesc = string.Format(@"外部用户登陆成功；IP:{0}", ipAddress);
                    SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                    return Json(new { code = 100, message = "外部用户登陆成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (user.Is_Internal == 1)
            {
                string webURL = "http://192.168.2.219:8002/WP_SUNAC/APP_RYG_SERVICES/Proxy_Services/TA_EOP/RYG_SUNAC_486_ValidatePwd_PS";
                HttpWebRequest request = WebRequest.Create(webURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                string data = "{\n\"username\": " + username + ",\n\"password\": " + pwd + "\n}";
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
                request.ContentLength = byteData.Length;
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string rescontent = reader.ReadToEnd();
                    JObject jO = JObject.Parse(rescontent);
                    string successCode = jO["successCode"].ConventToString(string.Empty);
                    if (successCode == "Y") 
                    {
                        string userid = user.Id.ConventToString(string.Empty);
                        string roleId = user.RoleID.ConventToString(string.Empty);
                        string isInternal = user.Is_Internal.ConventToString(string.Empty);
                        InitUtility.Instance.InitSessionHelper.Add("UserID", userid);
                        InitUtility.Instance.InitSessionHelper.Add("UserName", user.User_Name);
                        InitUtility.Instance.InitSessionHelper.Add("RoleId", roleId);
                        InitUtility.Instance.InitSessionHelper.Add("IsInternal", isInternal);
                        string errorText = jO["errorText"].ConventToString(string.Empty);
                        logDesc = string.Format(@"内部用户登陆成功；IP:{0}", ipAddress);
                        SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                        return Json(new { code = 100, message = "内部用户登陆成功" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (successCode == "N")
                    {
                        string errorText = jO["errorText"].ConventToString(string.Empty);
                        logDesc = string.Format(@"{1}；IP:{0}", ipAddress, errorText);
                        SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                        
                        return Json(new { code = -102, message = errorText }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { code = -103, message = "用户名或密码错误" }, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                logDesc = string.Format(@"{1}；IP:{0}", ipAddress, "登陆异常");
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                return Json(new { code = -104, message = "用户名或密码错误",User=user }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        /// <web> /home/loginout </web>
        public ActionResult LoginOut() 
        {
            int userid=    InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            string userName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            int logCode = 2;
            string logName = "用户退出";
            string logInfo = string.Empty;
            string logDesc = string.Empty;
            string createBy = userName;
            int createUserId = userid;
            string ipAddress = ClientPublicLib.GetLoginIp();
            logDesc = string.Format(@"{1}；IP:{0}", ipAddress, "用户退出");
            SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
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