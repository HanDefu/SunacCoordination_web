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

    public class SysUserController : Controller
    {
        public SysUserController() 
        {
            ViewBag.SelectModel = 11;
        }

        /// <summary>
        ///   用户表-列表
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/Sys_User</get>
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
            startRowNum = ((currentPage - 1) * pageSize) + 1;
            endRowNum = currentPage * pageSize;
            string username = HttpUtility.UrlDecode(Request.QueryString["username"]);
            if (!string.IsNullOrEmpty(username))
            {
                _where += " and  username='" + username + "'";
                _url += "username=" + username + "&";
            }
            ViewBag.username = username;
            string truename = HttpUtility.UrlDecode(Request.QueryString["truename"]);
            if (!string.IsNullOrEmpty(truename))
            {
                _where += " and  truename='" + truename + "'";
                _url += "truename=" + truename + "&";
            }
            ViewBag.truename = truename;
            string isinternal = HttpUtility.UrlDecode(Request.QueryString["isinternal"]);
            if (!string.IsNullOrEmpty(isinternal))
            {
                _where += " and  isinternal='" + isinternal + "'";
                _url += "isinternal=" + isinternal + "&";
            }
            ViewBag.isinternal = isinternal;
            string professionid = HttpUtility.UrlDecode(Request.QueryString["professionid"]);
            if (!string.IsNullOrEmpty(professionid))
            {
                _where += " and  professionid='" + professionid + "'";
                _url += "professionid=" + professionid + "&";
            }
            ViewBag.professionid = professionid;
            string roleid = HttpUtility.UrlDecode(Request.QueryString["roleid"]);
            if (!string.IsNullOrEmpty(roleid))
            {
                _where += " and  roleid='" + roleid + "'";
                _url += "roleid=" + roleid + "&";
            }
            ViewBag.roleid = roleid;
            IList<Sys_User> lst = Sys_UserDB.GetPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = Sys_UserDB.GetPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            return View();
        }
        /// <summary>
        ///   用户表-新增
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/Sys_User/add</get>
        [ValidateInput(false)]
        public ActionResult Add()
        {
            string _wh = " a.Enabled=1";
            string _order = string.Empty;
            IList<BaseCompanyInfo> companyInfo = BaseCompanyInfoDB.GetPageInfoByParameter(_wh, _order, 1, 1000);
            ViewBag.CompanyInfoList = companyInfo;
            _wh = " ParentID=1";
            IList<BasArgumentSetting> argumentSettingList = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_wh);
            ViewBag.ArgumentSettingList = argumentSettingList;

            IList<Sys_Role> SysRoleList = SysRoleDB.GetSysRoleListById();
            ViewBag.SysRoleList = SysRoleList;


            return View();
        }
        /// <summary>
        ///   用户表-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_User/addhandle</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Addhandle()
        {
            Sys_User sys_user = new Sys_User();
            string areaids = Request.Form["select_areaid"];
            sys_user.User_Name = Request.Form["txt_user_name"].ConventToString(string.Empty);
            sys_user.User_Psd = CommonLib.UserMd5("123456");
            sys_user.True_Name = Request.Form["txt_true_name"].ConventToString(string.Empty);
            sys_user.Telephone = Request.Form["txt_telephone"].ConventToString(string.Empty);
            sys_user.Email = Request.Form["txt_email"].ConventToString(string.Empty);
            sys_user.Is_Used = "1";
            sys_user.Used_Begin_DateTime = Request.Form["datetime_used_begin_datetime"].ConverToDataTime();
            sys_user.Used_End_DateTime = Request.Form["datetime_used_end_datetime"].ConverToDataTime();
            sys_user.Is_Internal = "1";
            sys_user.CompanyID = Request.Form["select_companyid"].ConvertToInt32(0);
            sys_user.RoleID = Request.Form["select_roleid"].ConvertToInt32(0);
            sys_user.CreateOn = DateTime.Now;
            sys_user.Reorder = 0;
            sys_user.Enabled =1;
            sys_user.CreateUserId = 0;
            sys_user.CreateBy = "admin";
            int UserId = Sys_UserDB.AddHandle(sys_user);
            if (UserId > 0)
            {
                if(!string.IsNullOrEmpty(areaids))
                {
                    string[] areas = areaids.Split(',');
                    foreach (string areaid in areas) 
                    {
                        Sys_User_Area_Relation userArea = new Sys_User_Area_Relation();
                        userArea.User_ID = UserId;
                        userArea.Area_ID = areaid.ConvertToInt32(-1);
                        Sys_User_Area_RelationDB.AddHandle(userArea);
                    }
                }
                
                return Json(new { code = 100, message = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///   用户表-修改视图
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_User/edit/id</get> 
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult Edit(int Id)
        {
            string _wh = " a.Enabled=1";
            string _order = string.Empty;
            IList<BaseCompanyInfo> companyInfo = BaseCompanyInfoDB.GetPageInfoByParameter(_wh, _order, 1, 1000);
            ViewBag.CompanyInfoList = companyInfo;
            _wh = " ParentID=1";
            IList<BasArgumentSetting> argumentSettingList = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_wh);
            ViewBag.ArgumentSettingList = argumentSettingList;

            IList<Sys_Role> SysRoleList = SysRoleDB.GetSysRoleListById();
            ViewBag.SysRoleList = SysRoleList;
            Sys_User sys_user = Sys_UserDB.GetSingleEntityById(Id);
            ViewBag.Sys_User = sys_user;
            return View();
        }
        /// <summary>
        ///   用户表-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_User/edithandle</get> 
        /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
        {
            Sys_User sys_user = new Sys_User();
            int Id = Request.Form["hid_id"].ConvertToInt32(0);
            sys_user.Id = Id;
            sys_user.User_Name = Request.Form["txt_user_name"].ConventToString(string.Empty);
            sys_user.User_Psd = Request.Form["txt_user_psd"].ConventToString(string.Empty);
            sys_user.True_Name = Request.Form["txt_true_name"].ConventToString(string.Empty);
            sys_user.Telephone = Request.Form["txt_telephone"].ConventToString(string.Empty);
            sys_user.Email = Request.Form["txt_email"].ConventToString(string.Empty);
            sys_user.Is_Used = Request.Form["txt_is_used"].ConventToString(string.Empty);
            sys_user.Used_Begin_DateTime = Request.Form["datetime_used_begin_datetime"].ConverToDataTime();
            sys_user.Used_End_DateTime = Request.Form["datetime_used_end_datetime"].ConverToDataTime();
            sys_user.Is_Internal = Request.Form["txt_is_internal"].ConventToString(string.Empty);
            sys_user.Orgnazation_Name = Request.Form["txt_orgnazation_name"].ConventToString(string.Empty);
            sys_user.CompanyID = Request.Form["checkbox_companyid"].ConvertToInt32(0);
            sys_user.RoleID = Request.Form["select_roleid"].ConvertToInt32(0);
            sys_user.AreaID = Request.Form["checkbox_areaid"].ConvertToInt32(0);
            sys_user.CreateOn = DateTime.Now;
            sys_user.Reorder = Request.Form["txt_reorder"].ConvertToInt32(0);
            sys_user.Enabled = Request.Form["select_enabled"].ConvertToInt32(0);
            sys_user.CreateUserId = 0;
            sys_user.CreateBy = "admin";
            int rtv = Sys_UserDB.EditHandle(sys_user, string.Empty);
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
        ///   用户表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_User/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            int rtv = Sys_UserDB.DeleteHandleById(Id);
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
        ///   用户表-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_User/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            string ids = Request.QueryString["ids"].ConventToString(string.Empty);
            int rtv = Sys_UserDB.DeleteHandleByIds(ids);
            if (rtv > 0)
            {
                return Json(new { code = 100, message = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SelectedProjectInfo() 
        {
            string  _wh = " ParentID=1";
            IList<BasArgumentSetting> argumentSettingList = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_wh);
            ViewBag.ArgumentSettingList = argumentSettingList;
            return View();
        }

        /// <summary>
        ///   获取项目信息
        /// </summary>
        /// <returns></returns>
        /// <get>/SysUser/GetJsonProjectInfoByAreaName</get> 
        /// <author>alon<84789887@qq.com></author> 

        public ActionResult GetJsonProjectInfoByAreaName()
        {
            string areaName = Request.QueryString["AreaName"].ConventToString(string.Empty);
            if (string.IsNullOrEmpty(areaName))
            {
                return Json(new { code = -100, message = "查询失败" }, JsonRequestBehavior.AllowGet);
            }

            IList<Project_Information> list = Project_InformationDB.GetProjectInformationByAreaName(areaName);
            if (list.Count() > 0) {
                return Json(new { code = 100, message = "查询成功", list = list }, JsonRequestBehavior.AllowGet);
            } else {
                return Json(new { code = -100, message = "查询失败" }, JsonRequestBehavior.AllowGet);
            }
            
        }


        /// <summary>
        ///   获取项目信息
        /// </summary>
        /// <returns></returns>
        /// <get>/SysUser/GetJsonProjectInfoBySearch</get> 
        /// <author>alon<84789887@qq.com></author> 

        public ActionResult GetJsonProjectInfoBySearch()
        {
            string areaName = Request.Form["AreaName"].ConventToString(string.Empty);
            string cityCompany = Request.Form["CityCompany"].ConventToString(string.Empty);
            string keyword = Request.Form["Keyword"].ConventToString(string.Empty);
            IList<ProjectInfo> list = Project_InformationDB.GetProjectInformationByAreaName(areaName, cityCompany, keyword);
            if (list.Count() > 0)
            {
                return Json(new { code = 100, message = "查询成功", list = list }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "查询失败" }, JsonRequestBehavior.AllowGet);
            }

        }
        



    }
}
