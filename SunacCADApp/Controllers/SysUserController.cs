using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using System.Collections;
using Newtonsoft.Json;
using SunacCADApp.Library;

namespace SunacCADApp.Controllers
{
  
    public class SysUserController : Controller
    {
        private int UserId = 0;
        private string UserName = string.Empty;
        int logCode = 4;
        string logName = "角色管理";
        string logInfo = string.Empty;
        string logDesc = string.Empty;
        string createBy = string.Empty;
        int createUserId = 0;
        /// <summary>
        /// 权限
        /// </summary>
       
        public SysUserController() 
        {
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            int _roleId = API_Common.GlobalParam("RoleId").ConvertToInt32(-1);
            int RoleId = InitUtility.Instance.InitSessionHelper.Get("RoleId").ConvertToInt32(0);
            bool IsSuper = false;
            if (RoleId == _roleId) {
                IsSuper = true;
            }
            ViewBag.SelectModel = 11;
            ViewBag.SysUserName = UserName;
            ViewBag.IsSuper = IsSuper;
            ViewBag.RoleId = _roleId;
            createBy = UserName;
            createUserId = UserId;

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
            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"]);
            
            if (!string.IsNullOrEmpty(keyword))
            {
                _where += " and  (a.[User_Name] like '%" + keyword + "%' OR a.[True_Name] like '%" + keyword + "%') ";
                _url += "keyword=" + keyword + "&";
            }
            ViewBag.keyword = keyword;
            string isinternal = HttpUtility.UrlDecode(Request.QueryString["isinternal"]);
            if (!string.IsNullOrEmpty(isinternal))
            {
                _where += " and  a.Is_Internal='" + isinternal + "'";
                _url += "isinternal=" + isinternal + "&";
            }
            else 
            {
                isinternal = "1";
                _where += " and  a.Is_Internal='" + isinternal + "'";
                _url += "isinternal=" + isinternal + "&";
            }
            ViewBag.isinternal = isinternal;

            string institutionid = HttpUtility.UrlDecode(Request.QueryString["institutionid"]);
            if (!string.IsNullOrEmpty(institutionid))
            {
                _where += " and  a.companyid='" + institutionid + "'";
                _url += "companyid=" + institutionid + "&";
            }

            ViewBag.institutionid = institutionid;
            string organ = HttpUtility.UrlDecode(Request.QueryString["organ"]);
            if (!string.IsNullOrEmpty(organ))
            {
                _where += " and  a.UserDeptNo='" + organ + "'";
                _url += "organ=" + organ + "&";
            }

            ViewBag.organ = organ;
            string roleid = HttpUtility.UrlDecode(Request.QueryString["roleid"]);
            if (!string.IsNullOrEmpty(roleid))
            {
                _where += " and  a.roleid='" + roleid + "'";
                _url += "roleid=" + roleid + "&";
            }
            ViewBag.roleid = roleid;
            string where = "  [Enabled]=1";
            IList<Sys_Role> RoleList = Sys_UserDB.GetSysRoleListByWh(where);

            IList<BasInstitutionData> InstitutionData = BasInstitutionDataDB.GetTop10InstitutionInit();
            ViewBag.InstitutionList = InstitutionData;

            IList<DataSourceMember> IdmOrginList = BasInstitutionDataDB.GetInnerIdmOrgan();
            ViewBag.IdmOrginList = IdmOrginList;

           // IList<Sys_User> lst = Sys_UserDB.GetPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            IList<Sys_User> lst = Sys_UserDB.GetPageInfoParameterMax2012(_where, _orderby, currentPage, pageSize);
            recordCount = Sys_UserDB.GetPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
                      
            int[] page = CommonLib.PageHelper(pageCount,currentPage);
         
            IList<PageNum> pageNumList = CommonLib.GetPageNum();
            ViewBag.URL = _url;
            ViewBag.SysUserList = lst;
            ViewBag.RoleList = RoleList;
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
        ///   用户表-新增
        /// </summary>
        /// <returns></returns>
        /// <get>/sysuser/add</get>
        [ValidateInput(false)]
        public ActionResult Add()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _wh = " a.Enabled=1";
            string _order = string.Empty;

            IList<BasInstitutionData> InstitutionData = BasInstitutionDataDB.GetTop10InstitutionInit();
            ViewBag.InstitutionList = InstitutionData;
            _wh = " TypeCode='area' AND ParentID!=0";
            IList<BasArgumentSetting> argumentSettingList = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_wh);
            ViewBag.ArgumentSettingList = argumentSettingList;

            IList<Sys_Role> SysRoleList = SysRoleDB.GetSysRoleListById();
            ViewBag.SysRoleList = SysRoleList;
            ViewBag.StartDate = DateTime.Now.ToString("yyyy-mm-dd");
            ViewBag.EndDate = DateTime.Now.AddYears(5).ToString("yyyy-mm-dd");

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
            if (UserId < 1)
            {
                return Redirect("/home");
            }
             string userName = Request.Form["txt_user_name"].ConventToString(string.Empty);
             int hasUser = Sys_UserDB.HasSysUserByUserName(userName);
            if (hasUser > 0) 
            {
                return Json(new { code = -100, message = "登陆名已存在，请更换登陆名" }, JsonRequestBehavior.AllowGet);
            }
            Sys_User sys_user = new Sys_User();
            string areaids = Request.Form["chk_area"];
            sys_user.User_Name = userName;
            sys_user.User_Psd = CommonLib.UserMd5("123456");
            sys_user.True_Name = Request.Form["txt_true_name"].ConventToString(string.Empty);
            sys_user.Telephone = Request.Form["txt_phone"].ConventToString(string.Empty);
            sys_user.Email = Request.Form["txt_email"].ConventToString(string.Empty);
            sys_user.Is_Used = "1";
            sys_user.Used_Begin_DateTime = Request.Form["datetime_used_begin_datetime"].ConverToDataTime();
            sys_user.Used_End_DateTime = Request.Form["datetime_used_end_datetime"].ConverToDataTime();
            sys_user.Is_Internal = 2;
            sys_user.CompanyID = Request.Form["select_companyid"].ConvertToInt32(0);
            sys_user.RoleID = Request.Form["select_roleid"].ConvertToInt32(0);
            sys_user.Reorder = 0;
            sys_user.Enabled =1;
            sys_user.CreateUserId = UserId;
            sys_user.CreateBy = UserName;
            sys_user.CreateOn = DateTime.Now;
            sys_user.ModifiedUserId = UserId;
            sys_user.ModifiedBy = UserName;
            sys_user.ModifiedOn = DateTime.Now;

            int _userId = Sys_UserDB.AddHandle(sys_user);
            if (_userId > 0)
            {
                if(!string.IsNullOrEmpty(areaids))
                {
                    string[] areas = areaids.Split(',');
                    foreach (string areaid in areas) 
                    {
                        Sys_User_Area_Relation userArea = new Sys_User_Area_Relation();
                        userArea.User_ID = _userId;
                        userArea.Area_ID = areaid.ConvertToInt32(-1);
                        userArea.CreateUserId = UserId;
                        userArea.CreateBy = UserName;
                        userArea.CreateOn = DateTime.Now;
                        userArea.ModifiedUserId = UserId;
                        userArea.ModifiedBy = UserName;
                        userArea.ModifiedOn = DateTime.Now;
                        Sys_User_Area_RelationDB.AddHandle(userArea);
                    }
                }
                string proids = Request.Form["hid_proids"].ConventToString(string.Empty);
                if (!string.IsNullOrEmpty(proids)) 
                {
                    string[] arr_pro_ids = proids.Split(',');
                    foreach (string pid in arr_pro_ids) 
                    {
                        Sys_UserDB.InsertUserAndProjectRelation(_userId, pid,UserId,userName);
                    }
                }
                string ipAddress = ClientPublicLib.GetLoginIp();
                logDesc = string.Format(@"{0}；用户名称:{1};IP:{2};", "用户新增", userName, ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
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
        public ActionResult Edit(int Id=0)
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _wh = " a.Enabled=1";
            string _order = string.Empty;

            IList<BasInstitutionData> InstitutionData = BasInstitutionDataDB.GetTop10InstitutionInit();
            ViewBag.InstitutionList = InstitutionData;
            _wh = " TypeCode='area' AND ParentID!=0";
            IList<BasArgumentSetting> argumentSettingList = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_wh);
            ViewBag.ArgumentSettingList = argumentSettingList;

            IList<Sys_Role> SysRoleList = SysRoleDB.GetSysRoleListById();
            ViewBag.SysRoleList = SysRoleList;
            string _where = string.Format(@" and [User_ID]='{0}'",Id);
            IList<Sys_User_Area_Relation> areas = Sys_User_Area_RelationDB.GetListSysUserAreaRelationByWhere(_where);

            Sys_User sys_user = Sys_UserDB.GetSingleEntityById(Id);
            ViewBag.Sys_User = sys_user;
            ViewBag.Areas = areas;

            ViewBag.StartDateTime = sys_user.Used_Begin_DateTime.ToString("yyyy-MM-dd");
            ViewBag.EndDateTime = sys_user.Used_End_DateTime.ToString("yyyy-MM-dd");

            IList<SysUserAndProject> Projects = SysUserProjectRelationDB.GetUserAndProjectWhere(Id);
            ViewBag.UserProjects = Projects;
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
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            Sys_User sys_user = new Sys_User();
            int _userId = Request.Form["hid_id"].ConvertToInt32(0);
            sys_user.Id = _userId;
            sys_user.True_Name = Request.Form["txt_true_name"].ConventToString(string.Empty);
            sys_user.Telephone = Request.Form["txt_phone"].ConventToString(string.Empty);
            sys_user.Email = Request.Form["txt_email"].ConventToString(string.Empty);
            sys_user.Is_Used = "1";
            sys_user.Used_Begin_DateTime = Request.Form["datetime_used_begin_datetime"].ConverToDataTime();
            sys_user.Used_End_DateTime = Request.Form["datetime_used_end_datetime"].ConverToDataTime();
            sys_user.Is_Internal = 2;
            int companyId = Request.Form["select_companyid"].ConvertToInt32(0);
            sys_user.CompanyID = companyId;
            sys_user.RoleID = Request.Form["select_roleid"].ConvertToInt32(0);
            sys_user.Reorder = 0;
            sys_user.Enabled = 1;
            sys_user.CreateUserId = UserId;
            sys_user.CreateBy = UserName;
            sys_user.CreateOn = DateTime.Now;
            sys_user.ModifiedUserId = UserId;
            sys_user.ModifiedBy = UserName;
            sys_user.ModifiedOn = DateTime.Now;
            int rtv = Sys_UserDB.EditHandle(sys_user, string.Empty);
            string areaids = Request.Form["chk_area"];

            if (_userId > 0)
            {
                if (!string.IsNullOrEmpty(areaids))
                {
                    string _sub_where = string.Format(@" User_ID={0}", _userId);
                    Sys_User_Area_RelationDB.DeleteHandleByParam(_sub_where);
                    string[] areas = areaids.Split(',');
                    foreach (string areaid in areas)
                    {
                        Sys_User_Area_Relation userArea = new Sys_User_Area_Relation();
                        userArea.User_ID = _userId;
                        userArea.Area_ID = areaid.ConvertToInt32(-1);
                        userArea.CreateUserId = UserId;
                        userArea.CreateBy = UserName;
                        userArea.CreateOn = DateTime.Now;
                        userArea.ModifiedUserId = UserId;
                        userArea.ModifiedBy = UserName;
                        userArea.ModifiedOn = DateTime.Now;
                        Sys_User_Area_RelationDB.AddHandle(userArea);
                    }
                }
                string proids = Request.Form["hid_proids"].ConventToString(string.Empty);
                Sys_UserDB.DeleteSysUserProjectRelationByUId(_userId);
                if (!string.IsNullOrEmpty(proids))
                {
                    string[] arr_pro_ids = proids.Split(',');
                    foreach (string pid in arr_pro_ids)
                    {
                        Sys_UserDB.InsertUserAndProjectRelation(_userId, pid,UserId,UserName);
                    }
                }
            }
            if (rtv > 0)
            {
                string ipAddress = ClientPublicLib.GetLoginIp();
                logDesc = string.Format(@"{0}；用户ID:{1};IP:{2};", "用户修改", _userId, ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                return Json(new { code = 100, message = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 内部用户权限设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <uri>/sysuser/inneredit </uri>
        public ActionResult InnerEdit(int id=0)
        {
            if (UserId < 1 || id<0)
            {
                return Redirect("/home");
            }
            string _order = string.Empty;
            IList<Sys_Role> SysRoleList = SysRoleDB.GetSysRoleListById();
            ViewBag.SysRoleList = SysRoleList;
            Sys_User User = Sys_UserDB.GetInnerSysUserById(id);
            ViewBag.SysUser = User;
            IList<Sys_User_Organization_Relation> organizations = Sys_UserDB.GetUserOrganizationRelationByUserId(id);
            ViewBag.Organizations = organizations;
            SortedList<string, string> OrgSorts = Project_InformationDB.GetAreaSortedList;
            ViewBag.OrgSorted = OrgSorts;
            string  _wh = " TypeCode='area' AND ParentID!=0";
            IList<BasArgumentSetting> AreaList = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_wh);
            ViewBag.AreaList = AreaList;
            _wh = string.Format(@" And [User_ID]={0}", id);
            IList<Sys_User_Area_Relation> userArea = Sys_User_Area_RelationDB.GetListSysUserAreaRelationByWhere(_wh);
            ViewBag.UserAreas = userArea;
            return View();

        }

        public ActionResult EditInnerUserHandle() 
        {
            int rtv = 0;
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            int uid = Request.Form["hid_id"].ConvertToInt32(0);
            int roleid = Request.Form["select_roleid"].ConvertToInt32(0);
            Sys_User user = new Sys_User();
            user.Id = uid;
            user.RoleID = roleid;
            user.ModifiedBy = UserName;
            user.ModifiedUserId = UserId;
            user.ModifiedOn = DateTime.Now;
            rtv=Sys_UserDB.InnerEditHandle(user);


            string organiztionId = Request.Form["hid_organiztionId"].ConvertToTrim();
            Sys_UserDB.RemoveUserOrganizationRelationByUserId(uid);
            if (!string.IsNullOrEmpty(organiztionId))
            {
                string[] arr_organiztionId = organiztionId.Split(',');
                foreach (string organiztion in arr_organiztionId)
                {

                    Sys_User_Organization_Relation organization = new Sys_User_Organization_Relation();
                    organization.UserId = uid;
                    organization.OrgId = organiztion.ConvertToInt32(0);
                    organization.CreateBy = UserName;
                    organization.CreateUserId = UserId;
                    organization.ModifiedUserId = UserId;
                    organization.ModifiedBy = UserName;
                    Sys_UserDB.EditUserOrganizationRelationByOrganization(organization);
                }
            }

            string AreaIds = Request.Form["checkbox_area"];
            string _sub_where = string.Format(@" User_ID={0}", uid);
            Sys_User_Area_RelationDB.DeleteHandleByParam(_sub_where);
            if (!string.IsNullOrEmpty(AreaIds))
            {
                string[] areas = AreaIds.Split(',');
                foreach (string areaid in areas)
                {
                    Sys_User_Area_Relation userArea = new Sys_User_Area_Relation();
                    userArea.User_ID = uid;
                    userArea.Area_ID = areaid.ConvertToInt32(-1);
                    userArea.Enabled = 1;
                    userArea.CreateUserId = UserId;
                    userArea.CreateBy = UserName;
                    userArea.CreateOn = DateTime.Now;
                    userArea.ModifiedUserId = UserId;
                    userArea.ModifiedBy = UserName;
                    userArea.ModifiedOn = DateTime.Now;
                    Sys_User_Area_RelationDB.AddHandle(userArea);
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
        /// 内部用户权限设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <uri>/sysuser/treeprojectinfo </uri>
        public ActionResult TreeProjectInfo() 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            IList<BasIdmOrganization> _bas_idm_organizations = new List<BasIdmOrganization>();
            _bas_idm_organizations = IdmCommonLibDB.GetPageInfoByParameter(string.Empty);
            IList<TreeSource> treeSource = new List<TreeSource>();
            foreach (BasIdmOrganization institution in _bas_idm_organizations) 
            {

                string orgCode = institution.OrgCode;
                

                string orgName = string.Empty;
                if (Project_InformationDB.GetAreaSortedList.ContainsKey(orgCode))
                {
                    orgName = Project_InformationDB.GetAreaSortedList[orgCode];
                }
                else 
                {
                    continue;
                }
             
                TreeSource tree = new TreeSource();
                tree.id = institution.Id;
                tree.name = orgName;
                tree.isParent = "true";
                tree.halfCheck = "false";
                tree.pId = 0;
                tree.OrgCode = institution.OrgCode;
                tree.OrgName = orgName;
                tree.UpOrgName = string.Empty;
                tree.open = true;
                IList<BasIdmOrganization> _child_organizations = new List<BasIdmOrganization>();
                _child_organizations = IdmCommonLibDB.GetAreaCityByArea(institution.OrgCode);
                IList<TreeSource> treeChild = new List<TreeSource>();
                foreach(BasIdmOrganization child in  _child_organizations)
                {
                    TreeSource ctree = new TreeSource();
                    ctree.id = child.Id;
                    ctree.name = child.OrgName;
                    ctree.isParent = "false";
                    ctree.halfCheck = "false";
                    ctree.pId = institution.Id;
                    ctree.OrgCode = child.OrgCode;
                    ctree.OrgName = child.OrgName;
                    ctree.UpOrgName = orgName;
                    ctree.open = false;
                    treeChild.Add(ctree);
                }
                tree.children = treeChild;
                treeSource.Add(tree);
            }
            string _json=JsonConvert.SerializeObject(treeSource);
            ViewBag.TreeSource = _json;
            ViewBag.IdmOrganizations = _bas_idm_organizations;
            SortedList<string, string> OrgSorts = Project_InformationDB.GetAreaSortedList;
            ViewBag.OrgSorted = OrgSorts;
            
            return View();
        }

        /// <summary>
        /// 获取 Tree下面子节点
        /// </summary>
        /// <returns></returns>
        ///SysUser/GetTreeNodeJsonAreaCityByArea</get> 
        public ActionResult GetTreeNodeJsonAreaCityByArea() 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string orgCode = Request.Form["orgCode"].ConvertToTrim();
            IList<BasIdmOrganization> _bas_idm_organizations = new List<BasIdmOrganization>();
            _bas_idm_organizations = IdmCommonLibDB.GetAreaCityByArea(orgCode);
            return Json(_bas_idm_organizations, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        ///   用户表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/Sys_User/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
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
        /// <get>SysUser/deletehandlebyids</get> 
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
                return Json(new { code = -100, message = "请选择删除项" }, JsonRequestBehavior.AllowGet);
            }
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
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            IList<BasIdmOrganization> IdmOrganizations = new List<BasIdmOrganization>();
            IdmOrganizations = IdmCommonLibDB.GetPageInfoByParameter(string.Empty);
            ViewBag.IdmOrganizations = IdmOrganizations;
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
            if (UserId < 1)
            {
                return Redirect("/home");
            }
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
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string areaName = Request.Form["AreaName"].ConventToString(string.Empty);
            string cityCompany = Request.Form["CityCompany"].ConventToString(string.Empty);
            string keyword = Request.Form["Keyword"].ConventToString(string.Empty);
            IList<Bas_Idm_Project> list = BasIdmProjectDB.GetBasIdmProjectByWhere(areaName, cityCompany, keyword);
            if (list.Count() > 0)
            {
                return Json(new { code = 100, message = "查询成功", list = list }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "查询失败" }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult GetIdmCityByAreaCode() 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string areaCode = Request.Form["areaCode"].ConventToString(string.Empty);
            IList<BasIdmOrganization> cities = IdmCommonLibDB.GetIdmCityAndCompanyByArea(areaCode);
            if (cities.Count() > 0)
            {
                return Json(new { code = 100, message = "查询成功", list = cities }, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json(new { code = -100, message = "查询失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword() 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            return View();
        }

        /// <summary>
        ///  /
        /// </summary>
        /// <returns></returns>
        /// <http_post>/SysUser/ChangePasswordHandle</http_post>
        public ActionResult ChangePasswordHandle() 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string old_password = Request.Form["txt_old_password"].ConventToString(string.Empty);
            if (string.IsNullOrEmpty(old_password)) {
                return Json(new { code = -100, message = "旧密码不能为空" }, JsonRequestBehavior.AllowGet);
            }

            Sys_User user = Sys_UserDB.GetSingleEntityById(UserId);
            string old_md5_pwd=CommonLib.UserMd5(old_password);
            if (user.User_Psd != old_md5_pwd) 
            {
                return Json(new { code = -100, message = "旧密码不正确" }, JsonRequestBehavior.AllowGet);
            }


            string password = Request.Form["txt_password"].ConventToString(string.Empty);
            if (string.IsNullOrEmpty(password)) 
            {
                return Json(new { code = -100, message = "用户密码输入不能为空" }, JsonRequestBehavior.AllowGet);
            }
            string once_password = Request.Form["txt_once_password"].ConventToString(string.Empty);
            if (password!=once_password) 
            {
                return Json(new { code = -100, message = "两次输入密码不同" }, JsonRequestBehavior.AllowGet);
            }

            password = CommonLib.UserMd5(password);
            int flag = Sys_UserDB.ChangePassword(password,UserId);
            if (flag > 0)
            {
                string ipAddress = ClientPublicLib.GetLoginIp();
                logDesc = string.Format(@"{0}；用户名:{1};IP:{2};", "密码修改", user.User_Name, ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                return Json(new { code = 100, message = "密码修改成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "密码修改失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        /// <URL>SysUser/ChangeBackPassword</URL>
        public JsonResult ChangeBackPassword() 
        {
            if (UserId < 1)
            {
                return Json(new { code = -110, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            int userId = Request.Form["uid"].ConvertToInt32(0);
            if (userId == 0) 
            {
                return Json(new { code = -110, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            string password = "123456";
            password = CommonLib.UserMd5(password);
            int flag = Sys_UserDB.ChangePassword(password, userId);
            if (flag > 0)
            {
                string ipAddress = ClientPublicLib.GetLoginIp();
                logDesc = string.Format(@"{0}；UserId:{1};IP:{2};", "密码重置", userId, ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);
                return Json(new { code = 100, message = "密码修改成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "密码修改失败" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <http_post>/SysUser/MyEdit</http_post>
        public ActionResult MyEdit()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }

            Sys_User user = Sys_UserDB.GetSingleEntityById(UserId);
            ViewBag.User = user;
            string orgCode = string.Empty;
            int IsInternal=user.Is_Internal;
            if (user.Is_Internal == 2) 
            {
                orgCode = user.CompanyID.ConvertToTrim();
            }
            else if (user.Is_Internal == 1) 
            {
                orgCode = user.UserDeptNo;
            }

            string orgName = Sys_UserDB.GetUseOrganizationByOrgId(orgCode, IsInternal);
            string userName = Sys_UserDB.GetUserRoleName(user.RoleID);
            ViewBag.OrgName = orgName;
            ViewBag.UserName = userName;
            ViewBag.Is_Internal = user.Is_Internal;
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public ActionResult ViewProjectUserById() 
        {
           int companyid = Request.QueryString["companyid"].ConvertToInt32(0);
            if (UserId < 1 || companyid<1)
            {
                return Redirect("/home");
            }
            string _where = string.Format(@" a.CompanyID='{0}'",companyid);
            IList<Sys_User> lst = Sys_UserDB.GetPageInfoByParameter(_where, string.Empty, 0, 200);
            ViewBag.SysUserList = lst;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <get>/sysuser/changeisusedbyid</get>
        public ActionResult ChangeIsUsedById() 
        {
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }

            string uids = Request.Form["uids"].ConventToString(string.Empty);
            int used = Request.Form["used"].ConvertToInt32(0);
            int flag = Sys_UserDB.ChangeSysUsedByIds(uids, used);
            
            if (flag > 0)
            {
                string message = used == 1 ? "启用成功" : "禁用成功";
                return Json(new { code = 100, message = message}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string message = used == 1 ? "启用失败" : "禁用失败";
                return Json(new { code = -100, message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MyUserHandle() 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }


            int flag = 0;
            string truename = Request.Form["txt_true_name"].ConvertToTrim();
            string telephone = Request.Form["txt_telephone"].ConvertToTrim();
            string email = Request.Form["txt_email"].ConvertToTrim();
            flag = Sys_UserDB.EditSysUserByUserId(UserId, truename, telephone, email);

            if (flag > 0)
            {
                string ipAddress = ClientPublicLib.GetLoginIp();
                logDesc = string.Format(@"{0}；用户名:{1};IP:{2};", "用户修改", truename, ipAddress);
                SysOperateLogDB.SaveLogHandle(logCode, logName, logInfo, logDesc, createBy, createUserId);

             
                return Json(new { code = 100, message = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
        }
    
    }
}
