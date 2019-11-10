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

    public class SysRoleController : Controller
    {

        private string UserId = string.Empty;
        private string UserName = string.Empty;
        private int IUserId = 0;
        public SysRoleController() 
        {
            IUserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID");
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            ViewBag.SelectModel = 12;
        }

        /// <summary>
        ///   角色表-列表
        /// </summary>
        /// <returns></returns>
        /// <get>/SysRole/Index</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Index()
        {
            if (IUserId < 1)
            {
                return Redirect("/home");
            }
            string _where = "1=1";  //查询
            string _orderby = string.Empty;  //排序
            string _url ="1&";
            int recordCount = 0;    //记录总数
            int pageSize = 15;      //每页条数
            int currentPage = 0;    //当前页数
            int pageCount = 0;      //总页数
            int startRowNum = 0;    //开始行数
            int endRowNum = 0;      //结束行数
            currentPage = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : Request.QueryString["page"].ConvertToInt32(0);
            startRowNum = ((currentPage - 1) * pageSize) + 1;
            endRowNum = currentPage * pageSize;
            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"]);
            if (!string.IsNullOrEmpty(keyword))
            {
                _where += " and  a.Role_Name like '" + keyword + "%'";
                _url += "rolename=" + keyword + "&";
            }
            ViewBag.rolename = keyword;
            IList<Sys_Role> lst = SysRoleDB.GetPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = SysRoleDB.GetPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);

            
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            return View();
        }
        /// <summary>
        ///   角色表-新增
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/Sys_Role/add</get>
        [ValidateInput(false)]
        public ActionResult Add()
        {
            if (IUserId < 1)
            {
                return Redirect("/home");
            }
            string _wh = "1=1";
            string _ordby = " Id asc,";
            IList<Sys_Model> models = Sys_ModelDB.GetPageInfoByParameter(_wh, _ordby, 0, 30);
            ViewBag.Models = models;
            return View();
        }
        /// <summary>
        ///   角色表-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Role/addhandle</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Addhandle()
        {
            if (IUserId < 1)
            {
                return Redirect("/home");
            }
            string role_name = Request.Form["txt_role_name"].ConventToString(string.Empty);
             Sys_Role hasRole =  SysRoleDB.GetSingleEntityByparam(string.Format(" And  Role_Name='{0}'",role_name));
             if (hasRole != null) 
             {
                 return Json(new { code = -100, message = role_name+"角色已存在" }, JsonRequestBehavior.AllowGet);
             }
            Sys_Role sys_role = new Sys_Role();
            sys_role.Role_Name = Request.Form["txt_role_name"].ConventToString(string.Empty);
            sys_role.Role_Remark = Request.Form["area_role_remark"].ConventToString(string.Empty);
            sys_role.CreateOn = DateTime.Now;
            sys_role.Reorder = Request.Form["txt_reorder"].ConvertToInt32(0);
            sys_role.Enabled = 1;
            sys_role.CreateUserId = UserId.ConvertToInt32(0);
            sys_role.CreateBy = UserName;
            sys_role.CreateOn = DateTime.Now;
            sys_role.ModifiedUserId = UserId.ConvertToInt32(0);
            sys_role.ModifiedBy = UserName;
            sys_role.ModifiedOn = DateTime.Now;
            int roleid = SysRoleDB.AddHandle(sys_role);
            string powers = Request.Form["hid_power"].ConventToString(string.Empty);
            if (!string.IsNullOrEmpty(powers)) 
            {
                string[] arr_power =powers.Split(',');
                foreach (string power in arr_power)
                {
                    Sys_Role_Model_Relation RoleModel = new Sys_Role_Model_Relation();
                    RoleModel.Model_Id = power.ConvertToInt32(-1);
                    RoleModel.Role_Id = roleid;
                    RoleModel.Enabled = 1;
                    RoleModel.Reorder = 0;
                    RoleModel.CreateUserId = UserId.ConvertToInt32(0);
                    RoleModel.CreateBy = UserName;
                    RoleModel.CreateOn = DateTime.Now;
                    RoleModel.ModifiedUserId = UserId.ConvertToInt32(0);
                    RoleModel.ModifiedBy = UserName;
                    RoleModel.ModifiedOn = DateTime.Now;
                    SysRoleModelRelationDB.AddHandle(RoleModel);
                }

            }
            if (roleid > 0)
            {
                return Json(new { code = 100, message = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///   角色表-修改视图
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Role/edit/id</get> 
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult Edit(int Id)
        {
            if (IUserId < 1)
            {
                return Redirect("/home");
            }
            string _wh = "1=1";
            string _ordby = " Id asc,";
            IList<Sys_Model> models = Sys_ModelDB.GetPageInfoByParameter(_wh, _ordby, 0, 30);
            ViewBag.Models = models;

            Sys_Role sys_role = SysRoleDB.GetSingleEntityById(Id);
            ViewBag.Sys_Role = sys_role;

            IList<Sys_Model> listModel = Sys_ModelDB.GetSysModeByRoleID(Id);
            ViewBag.listModel = listModel;
            return View();
        }
        /// <summary>
        ///   角色表-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Role/edithandle</get> 
        /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
        {
            if (IUserId < 1)
            {
                return Redirect("/home");
            }
            Sys_Role sys_role = new Sys_Role();
            int Id = Request.Form["hid_id"].ConvertToInt32(0);
            sys_role.Id = Id;
            sys_role.Role_Name = Request.Form["txt_role_name"].ConventToString(string.Empty);
            sys_role.Role_Remark = Request.Form["area_role_remark"].ConventToString(string.Empty);
            sys_role.CreateOn = DateTime.Now;
            sys_role.Reorder = Request.Form["txt_reorder"].ConvertToInt32(0);
            sys_role.Enabled = 1;
            sys_role.ModifiedOn = DateTime.Now;
            sys_role.ModifiedUserId = UserId.ConvertToInt32(0);
            sys_role.ModifiedBy = UserName;
            int rtv = SysRoleDB.EditHandle(sys_role, string.Empty);
            string powers = Request.Form["hid_power"].ConventToString(string.Empty);
            if (!string.IsNullOrEmpty(powers))
            {
                string _wh = string.Format(" Role_Id='{0}'", Id);
                SysRoleModelRelationDB.DeleteHandleByParam(_wh);
                string[] arr_power = powers.Split(',');
                foreach (string power in arr_power)
                {
                    Sys_Role_Model_Relation RoleModel = new Sys_Role_Model_Relation();
                    RoleModel.Model_Id = power.ConvertToInt32(-1);
                    RoleModel.Role_Id = Id;
                    RoleModel.Enabled = 1;
                    RoleModel.Reorder = 0;
                    RoleModel.CreateUserId = UserId.ConvertToInt32(0);
                    RoleModel.CreateBy = UserName;
                    RoleModel.CreateOn = DateTime.Now;
                    RoleModel.ModifiedUserId = UserId.ConvertToInt32(0);
                    RoleModel.ModifiedBy = UserName;
                    RoleModel.ModifiedOn = DateTime.Now;
                    SysRoleModelRelationDB.AddHandle(RoleModel);
                }
            }

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
        ///   角色表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Role/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            if (IUserId < 1)
            {
                return Redirect("/home");
            }
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            int rtv = SysRoleDB.DeleteHandleById(Id);
            string _wh = string.Format(" Role_Id='{0}'",Id);
            SysRoleModelRelationDB.DeleteHandleByParam(_wh);
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
        ///   角色表-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_Role/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            if (IUserId < 1)
            {
                return Redirect("/home");
            }
            string ids = Request.QueryString["ids"].ConventToString(string.Empty);
            int rtv = SysRoleDB.DeleteHandleByIds(ids);
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
