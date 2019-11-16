using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using Common.Utility;
using Common.Utility.Extender;

namespace SunacCADApp.Controllers
{
    public class BasInstitutionDataController : Controller
    {
        private int UserId = 0;
        private string UserName = string.Empty;

        public BasInstitutionDataController() 
        {
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
        }
        // GET: BasInstitutionData
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <get> /BasInstitutionData/GetJsonBasInstitutionDataByKeyword</get>
        public ActionResult GetJsonBasInstitutionDataByKeyword() 
        {
            if (UserId < 1) 
            {
                return Redirect("/home");
            }
            string keyword = Request.Form["keyword"].ConventToString("");
            IList<BasInstitutionData> list = new List<BasInstitutionData>();
            list = BasInstitutionDataDB.GetBasInstitutionDataByKeyword(keyword);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
       

    }
}