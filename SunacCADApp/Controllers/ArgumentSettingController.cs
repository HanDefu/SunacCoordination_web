using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Entity;
using SunacCADApp.Data;
namespace SunacCADApp.Controllers
{

    public class ArgumentSettingController : Controller
    {
        private int UserId = 0;
        private string UserName = string.Empty;
        int logCode = 3;
        string logName = "系统参数";
        string logInfo = string.Empty;
        string logDesc = string.Empty;
        string createBy = string.Empty;
        int createUserId = 0;
        public ArgumentSettingController()
        {
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            createBy = UserName;
            createUserId = UserId;
            ViewBag.SelectModel = 13;
        }
        /// <summary>
        ///   参数配置表-列表
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/BasArgumentSetting</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            IList<BasArgumentSetting> parentBasArgumentSetting = BasArgumentSettingDB.GetBasArgumentSettingParent();
            IList<BasArgumentSetting> childBasArgumentSetting = BasArgumentSettingDB.GetBasArgumentSettingChild();
            ViewBag.parentBasArgumentSetting = parentBasArgumentSetting;
            ViewBag.childBasArgumentSetting = childBasArgumentSetting;
            return View();
        }
        /// <summary>
        ///   参数配置表-新增
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/BasArgumentSetting/add</get>
        [ValidateInput(false)]
        public ActionResult Add()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            return View();
        }
        /// <summary>
        ///   参数配置表-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BasArgumentSetting/addhandle</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Addhandle()
        {
            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            BasArgumentSetting basargumentsetting = new BasArgumentSetting();
            string ArgumentText = Request.Form["argumenttext"].ConventToString(string.Empty);
            basargumentsetting.ArgumentText = Request.Form["argumenttext"].ConventToString(string.Empty);
            basargumentsetting.TypeCode = Request.Form["typecode"].ConventToString(string.Empty);
            basargumentsetting.TypeName = Request.Form["typename"].ConventToString(string.Empty);
            basargumentsetting.ParentID = Request.Form["parentid"].ConvertToInt32(0);
            basargumentsetting.CreateOn = DateTime.Now;
            basargumentsetting.Reorder = 0;
            basargumentsetting.Enabled = 1;
            basargumentsetting.CreateUserId = 0;
            basargumentsetting.CreateBy = "admin";
            basargumentsetting.ModifiedOn = DateTime.Now;
            int rtv = BasArgumentSettingDB.AddHandle(basargumentsetting);
            if (rtv > 0)
            {
                string ipAddress = ClientPublicLib.GetLoginIp();
                logDesc = string.Format(@"{0}；参数名称:{1};IP:{2};", "系统参数添加", ArgumentText, ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                return Json(new { code = 100, message = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///   参数配置表-修改视图
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BasArgumentSetting/edit/id</get> 
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult Edit(int Id)
        {
            BasArgumentSetting basargumentsetting = BasArgumentSettingDB.GetSingleEntityById(Id);
            ViewBag.BasArgumentSetting = basargumentsetting;
            return View();
        }
        /// <summary>
        ///   参数配置表-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BasArgumentSetting/edithandle</get> 
        /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
        {
            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            BasArgumentSetting basargumentsetting = new BasArgumentSetting();
            int Id = Request.Form["hid_id"].ConvertToInt32(0);
            basargumentsetting.Id = Id;
            string ArgumentText = Request.Form["txt_argumenttext"].ConventToString(string.Empty);
            basargumentsetting.ArgumentText = Request.Form["txt_argumenttext"].ConventToString(string.Empty);
            basargumentsetting.TypeCode = Request.Form["txt_typecode"].ConventToString(string.Empty);
            basargumentsetting.TypeName = Request.Form["txt_typename"].ConventToString(string.Empty);
            basargumentsetting.ParentID = Request.Form["txt_parentid"].ConvertToInt32(0);
            basargumentsetting.CreateOn = DateTime.Now;
            basargumentsetting.Reorder = Request.Form["txt_reorder"].ConvertToInt32(0);
            basargumentsetting.Enabled = Request.Form["select_enabled"].ConvertToInt32(0);
            basargumentsetting.CreateUserId = 0;
            basargumentsetting.CreateBy = "admin";
            int rtv = BasArgumentSettingDB.EditHandle(basargumentsetting, string.Empty);
            if (rtv > 0)
            {
                string ipAddress = ClientPublicLib.GetLoginIp();
                logDesc = string.Format(@"{0}；参数名称:{1};IP:{2};", "系统参数修改", ArgumentText, ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                return Json(new { code = 100, message = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///   参数配置表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BasArgumentSetting/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            int rtv = BasArgumentSettingDB.DeleteHandleById(Id);
            if (rtv > 0)
            {
                string ipAddress = ClientPublicLib.GetLoginIp();
                logDesc = string.Format(@"{0}；参数ID:{1};IP:{2};", "系统参数删除", Id, ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                return Json(new { code = 100, message = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///   参数配置表-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BasArgumentSetting/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            string ids = Request.QueryString["ids"].ConventToString(string.Empty);
            int rtv = BasArgumentSettingDB.DeleteHandleByIds(ids);
            if (rtv > 0)
            {
                return Json(new { code = 100, message = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///  /ArgumentSetting/deleteargumentsettingbyid
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteHandleByTo() 
        {
            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            int Id = Request.Form["supid"].ConvertToInt32(0);
            if (Id < 1) 
            {
                return Json(new { code = -100, message = "传入值异常" }, JsonRequestBehavior.AllowGet);
            }
            int rtv = BasArgumentSettingDB.DeleteArgumentSettingById(Id);
            if (rtv > 0)
            {
                return Json(new { code = 100, message = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddSuperArgumentSetting() 
        {
            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            string ArgumentText = Request.Form["ArgumentText"];
            string ArgumentCode = BasArgumentSettingDB.GetArgumentSettingCode();
            BasArgumentSetting setting = new BasArgumentSetting();
            setting.ArgumentText = ArgumentText;
            setting.TypeCode = ArgumentCode;
            setting.TypeName = ArgumentText;
            setting.ParentID = 0;
            
            setting.Reorder = 0;
            setting.Enabled = 1;
            setting.CreateUserId = UserId;
            setting.CreateBy = UserName;
            setting.CreateOn = DateTime.Now;
            setting.ModifiedUserId = UserId;
            setting.ModifiedBy = UserName;
            setting.ModifiedOn = DateTime.Now;
            int rtv = BasArgumentSettingDB.AddHandle(setting);
            if (rtv > 0)
            {
                return Json(new { code = 100, message = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "删除失败" }, JsonRequestBehavior.AllowGet);
            }

        }




    }
}
