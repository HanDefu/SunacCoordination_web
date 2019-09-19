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
        /// <summary>
        ///   参数配置表-列表
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/BasArgumentSetting</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Index()
        {

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
            BasArgumentSetting basargumentsetting = new BasArgumentSetting();
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
            BasArgumentSetting basargumentsetting = new BasArgumentSetting();
            int Id = Request.Form["hid_id"].ConvertToInt32(0);
            basargumentsetting.Id = Id;
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
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            int rtv = BasArgumentSettingDB.DeleteHandleById(Id);
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
        ///   参数配置表-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BasArgumentSetting/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
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




    }
}
