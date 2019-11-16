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

    public class CompanyInfoController : Controller
    {
        private int UserId = 0;
        private string UserName = string.Empty;
        public CompanyInfoController() 
        {
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            ViewBag.SelectModel = 14;
            ViewBag.Title = "机构管理";
        }

        /// <summary>
        ///   机构信息-列表
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/CompanyInfo</get>
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
            string _url = "1";
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
            string companyname = HttpUtility.UrlDecode(Request.QueryString["keyword"]);
            if (!string.IsNullOrEmpty(companyname))
            {
                _where += " and  InsName like '" + companyname + "%'";
                _url += "&InsName=" + companyname;
            }
            ViewBag.Keyword = companyname;
            //IList<BaseCompanyInfo> lst = BaseCompanyInfoDB.GetPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            //recordCount = BaseCompanyInfoDB.GetPageCountByParameter(_where);
            IList<BasInstitutionData> InstitutionInfo = BasInstitutionDataDB.GetPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = BasInstitutionDataDB.GetPageCountByParameter(_where);
            pageCount =  (int)Math.Ceiling((Double)recordCount / (Double)pageSize);
            int[] page = CommonLib.PageHelper(pageCount, currentPage);
            IList<PageNum> pageNumList = CommonLib.GetPageNum();
            ViewBag.URL = _url;
            ViewBag.List = InstitutionInfo;
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
        ///   机构信息-新增
        /// </summary>
        /// <returns></returns>
        /// <get>/Manage/BaseCompanyInfo/add</get>
        [ValidateInput(false)]
        public ActionResult Add()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            ViewBag.companynameSource = BaseCompanyInfoDB.GetInstitutionData();
            return View();
        }
        /// <summary>
        ///   机构信息-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BaseCompanyInfo/addhandle</get>
        /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Addhandle()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            BaseCompanyInfo basecompanyinfo = new BaseCompanyInfo();
            int companyId = Request.Form["select_companyname"].ConvertToInt32(0);
            if (BaseCompanyInfoDB.IsExistsInstitutionDataById(companyId)>0) 
            {
                return Json(new { code = -100, message = "机构已添加不能重复添加" }, JsonRequestBehavior.AllowGet);
            }
            basecompanyinfo.CompanyID = Request.Form["select_companyname"].ConvertToInt32(0);
            basecompanyinfo.CompanyName = Request.Form["hid_companyname"].ConventToString(string.Empty);
            basecompanyinfo.CompanyCode = "00000";
            basecompanyinfo.CompanyRemark = Request.Form["area_companyremark"].ConventToString(string.Empty);
    
            basecompanyinfo.Reorder = 0;
            basecompanyinfo.Enabled = Request.Form["select_enabled"].ConvertToInt32(0);
            basecompanyinfo.CreateUserId = UserId;
            basecompanyinfo.CreateBy = UserName;
            basecompanyinfo.CreateOn = DateTime.Now;
            basecompanyinfo.ModifiedUserId = UserId;
            basecompanyinfo.ModifiedBy = UserName;
            basecompanyinfo.ModifiedOn = DateTime.Now;
            int rtv = BaseCompanyInfoDB.AddHandle(basecompanyinfo);
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
        ///   机构信息-修改视图
        /// </summary>
        /// <returns></returns>
        /// <get>/CompanyInfo/edit/id</get> 
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult Edit(int Id)
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            if (Id < 1) 
            {
                return Redirect("/companyinfo/index");
            }
            ViewBag.companynameSource = BaseCompanyInfoDB.GetInstitutionData();
            BaseCompanyInfo basecompanyinfo = BaseCompanyInfoDB.GetSingleEntityById(Id);
            ViewBag.BaseCompanyInfo = basecompanyinfo;
            return View();
        }



        public ActionResult View(int Id) 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            if (Id < 1)
            {
                return Redirect("/companyinfo/index");
            }

            BasInstitutionData Ins = BasInstitutionDataDB.GetSingleEntityById(Id);
            ViewBag.Ins = Ins;
            return View();

        }



        /// <summary>
        ///   机构信息-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BaseCompanyInfo/edithandle</get> 
        /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            BaseCompanyInfo basecompanyinfo = new BaseCompanyInfo();
            int Id = Request.Form["hid_id"].ConvertToInt32(0);
            int new_companyid = Request.Form["select_companyname"].ConvertToInt32(0);
            int old_companyid = Request.Form["hid_companyId"].ConvertToInt32(0);
            if (new_companyid != old_companyid) 
            {
                if (BaseCompanyInfoDB.IsExistsInstitutionDataById(new_companyid) > 0)
                {
                    return Json(new { code = -100, message = "机构已添加不能重复添加" }, JsonRequestBehavior.AllowGet);
                }
            }
            basecompanyinfo.Id = Id;
            basecompanyinfo.CompanyID = new_companyid;
            basecompanyinfo.CompanyName = Request.Form["hid_companyname"].ConventToString(string.Empty);
            basecompanyinfo.CompanyCode = "";
            basecompanyinfo.CompanyRemark = Request.Form["area_companyremark"].ConventToString(string.Empty);
            basecompanyinfo.Reorder = 0;
            basecompanyinfo.Enabled = Request.Form["select_enabled"].ConvertToInt32(0);
            basecompanyinfo.ModifiedUserId = UserId;
            basecompanyinfo.ModifiedBy = UserName;
            basecompanyinfo.ModifiedOn = DateTime.Now;
            int rtv = BaseCompanyInfoDB.EditHandle(basecompanyinfo, string.Empty);
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
        ///   机构信息-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BaseCompanyInfo/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            int rtv = BaseCompanyInfoDB.DeleteHandleById(Id);
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
        ///   机构信息-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/BaseCompanyInfo/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string ids = Request.QueryString["ids"].ConventToString(string.Empty);
            int rtv = BaseCompanyInfoDB.DeleteHandleByIds(ids);
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
        ///  /BaseCompanyInfo/ChangeCompanyEnabled
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeCompanyEnabled() 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            int id = Request.Form["id"].ConvertToInt32(0);
            int enabled = Request.Form["enabled"].ConvertToInt32(0);
            int flag = BasInstitutionDataDB.IsChangeCompanyInfo(id, enabled, UserId, UserName);
            if (flag > 0)
            {
                return Json(new { code = 100, message = "状态更改成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "状态更改失败" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
