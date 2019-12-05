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

    public class SysOperateLogController : Controller
    {
        /// <summary>
        ///   系统操作日志-列表
        /// </summary>
        /// <returns></returns>
        /// <get>/SysOperateLog</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Index()
        {
            string _where = "1=1";  //查询
            string _orderby = string.Empty;  //排序
            string _url = string.Empty;
            int recordCount = 0;    //记录总数
            int pageSize = 15;      //每页条数
            int currentPage = 0;    //当前页数
            int pageCount = 0;      //总页数
            int startRowNum = 0;    //开始行数
            int endRowNum = 0;      //结束行数
            currentPage = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : Request.QueryString["page"].ConvertToInt32(0);
            pageSize = string.IsNullOrEmpty(Request.QueryString["pagesize"]) ? pageSize : Request.QueryString["pagesize"].ConvertToInt32(0);
            startRowNum = ((currentPage - 1) * pageSize) + 1;
            endRowNum = currentPage * pageSize;
            string systypecode = HttpUtility.UrlDecode(Request.QueryString["systypecode"]);
            if (!string.IsNullOrEmpty(systypecode))
            {
                _where += " and  systypecode='" + systypecode + "'";
                _url += "systypecode=" + systypecode + "&";
            }
            ViewBag.systypecode = systypecode;
            string loginfo = HttpUtility.UrlDecode(Request.QueryString["loginfo"]);
            if (!string.IsNullOrEmpty(loginfo))
            {
                _where += " and  loginfo='" + loginfo + "'";
                _url += "loginfo=" + loginfo + "&";
            }
            ViewBag.loginfo = loginfo;
            IList<Sys_Operate_Log> lst = SysOperateLogDB.GetPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = SysOperateLogDB.GetPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            int[] page = CommonLib.PageHelper(pageCount, currentPage);

            IList<PageNum> pageNumList = CommonLib.GetPageNum();
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecordCount = recordCount;
            ViewBag.PageCount = pageCount;
            ViewBag.PageNumList = pageNumList;
            ViewBag.CurrentPage = currentPage;
            ViewBag.NextPage = currentPage + 1;
            ViewBag.PreviousPage = currentPage - 1;
            ViewBag.StartPage = page[0];
            ViewBag.EndPage = page[1];
            ViewBag.PageSize = pageSize;
            return View();
        }
        /// <summary>
        ///   系统操作日志-新增
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/Sys_Operate_Log/add</get>
        [ValidateInput(false)]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        ///   系统操作日志-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Operate_Log/addhandle</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Addhandle()
        {
            Sys_Operate_Log sys_operate_log = new Sys_Operate_Log();
            sys_operate_log.SysTypeCode = Request.Form["txt_systypecode"].ConvertToInt32(0);
            sys_operate_log.SysTypeName = Request.Form["txt_systypename"].ConventToString(string.Empty);
            sys_operate_log.LogInfo = Request.Form["txt_loginfo"].ConventToString(string.Empty);
            sys_operate_log.CreateOn = DateTime.Now;
            sys_operate_log.Reorder = Request.Form["txt_reorder"].ConvertToInt32(0);
            sys_operate_log.Enabled = Request.Form["select_enabled"].ConvertToInt32(0);
            sys_operate_log.CreateUserId = 0;
            sys_operate_log.CreateBy = "admin";
            int rtv = SysOperateLogDB.AddHandle(sys_operate_log);
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
        ///   系统操作日志-修改视图
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Operate_Log/edit/id</get> 
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult Edit(int Id)
        {
            Sys_Operate_Log sys_operate_log = SysOperateLogDB.GetSingleEntityById(Id);
            ViewBag.Sys_Operate_Log = sys_operate_log;
            return View();
        }
        /// <summary>
        ///   系统操作日志-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Operate_Log/edithandle</get> 
        /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
        {
            Sys_Operate_Log sys_operate_log = new Sys_Operate_Log();
            int Id = Request.Form["hid_id"].ConvertToInt32(0);
            sys_operate_log.Id = Id;
            sys_operate_log.SysTypeCode = Request.Form["txt_systypecode"].ConvertToInt32(0);
            sys_operate_log.SysTypeName = Request.Form["txt_systypename"].ConventToString(string.Empty);
            sys_operate_log.LogInfo = Request.Form["txt_loginfo"].ConventToString(string.Empty);
            sys_operate_log.CreateOn = DateTime.Now;
            sys_operate_log.Reorder = Request.Form["txt_reorder"].ConvertToInt32(0);
            sys_operate_log.Enabled = Request.Form["select_enabled"].ConvertToInt32(0);
            sys_operate_log.CreateUserId = 0;
            sys_operate_log.CreateBy = "admin";
            int rtv = SysOperateLogDB.EditHandle(sys_operate_log, string.Empty);
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
        ///   系统操作日志-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Operate_Log/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            int rtv = SysOperateLogDB.DeleteHandleById(Id);
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
        ///   系统操作日志-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Operate_Log/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            string ids = Request.QueryString["ids"].ConventToString(string.Empty);
            int rtv = SysOperateLogDB.DeleteHandleByIds(ids);
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
