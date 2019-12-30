using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.Library;
namespace SunacCADApp.Controllers
{

    public class SysOperateLogController : Controller
    {
        private int UserId = 0;
        private string UserName = string.Empty;
        private bool IsSuper = false;
      
        public SysOperateLogController() 
        {
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            int _roleId = API_Common.GlobalParam("RoleId").ConvertToInt32(-1);
            int RoleId = InitUtility.Instance.InitSessionHelper.Get("RoleId").ConvertToInt32(0);
            ViewBag.SelectModel = 15;
            if (RoleId == _roleId)
            {
                IsSuper = true;
            }


        }
        /// <summary>
        ///   系统操作日志-列表
        /// </summary>
        /// <returns></returns>
        /// <get>/SysOperateLog</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = "1=1";  //查询
            string _orderby = string.Empty;  //排序
            string _url = string.Empty;
            int recordCount = 0;    //记录总数
            int pageSize = 10;      //每页条数
            int currentPage = 0;    //当前页数
            int pageCount = 0;      //总页数
            int startRowNum = 0;    //开始行数
            int endRowNum = 0;      //结束行数
            currentPage = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : Request.QueryString["page"].ConvertToInt32(0);
            pageSize = string.IsNullOrEmpty(Request.QueryString["pagesize"]) ? pageSize : Request.QueryString["pagesize"].ConvertToInt32(0);
            startRowNum = ((currentPage - 1) * pageSize) + 1;
            endRowNum = currentPage * pageSize;
            string CreateBy = HttpUtility.UrlDecode(Request.QueryString["CreateBy"]);
            if (!string.IsNullOrEmpty(CreateBy))
            {
                _where += " and  CreateBy='" + CreateBy + "'";
                _url += "CreateBy=" + CreateBy + "&";
            }
            ViewBag.CreateBy = CreateBy;
            

            if (!IsSuper) 
            {
                ViewBag.CreateBy = UserName;
                ViewBag.IsSuper = IsSuper;
                _where += string.Format(@" and   CreateBy='{0}'", UserName);
            }
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
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            if (Id < 1)
            {
                return Json(new { code = -101, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
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
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            string ids = Request.Form["ids"].ConventToString(string.Empty);

            if (string.IsNullOrEmpty(ids))
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
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
