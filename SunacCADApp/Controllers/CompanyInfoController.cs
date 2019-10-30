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

        public CompanyInfoController() 
        {
            ViewBag.SelectModel = 14;
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
            startRowNum = ((currentPage - 1) * pageSize) + 1;
            endRowNum = currentPage * pageSize;
            string companyname = HttpUtility.UrlDecode(Request.QueryString["keyword"]);
            if (!string.IsNullOrEmpty(companyname))
            {
                _where += " and  companyname like '" + companyname + "%'";
                _url += "&companyname=" + companyname;
            }
            ViewBag.Keyword = companyname;
          
            IList<BaseCompanyInfo> lst = BaseCompanyInfoDB.GetPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = BaseCompanyInfoDB.GetPageCountByParameter(_where);
            pageCount =  (int)Math.Ceiling((Double)recordCount / (Double)pageSize);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = currentPage;
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
            basecompanyinfo.CreateOn = DateTime.Now;
            basecompanyinfo.Reorder = 0;
            basecompanyinfo.Enabled = Request.Form["select_enabled"].ConvertToInt32(0);
            basecompanyinfo.CreateUserId = 0;
            basecompanyinfo.CreateBy = "admin";
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
            if (Id < 1) 
            {
                return Redirect("/CompanyInfo/Index");
            }
            ViewBag.companynameSource = BaseCompanyInfoDB.GetInstitutionData();
            BaseCompanyInfo basecompanyinfo = BaseCompanyInfoDB.GetSingleEntityById(Id);
            ViewBag.BaseCompanyInfo = basecompanyinfo;
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
            basecompanyinfo.CreateUserId = 0;
            basecompanyinfo.CreateBy = "admin";
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
            int id = Request.Form["id"].ConvertToInt32(0);
            int enabled = Request.Form["enabled"].ConvertToInt32(0);
            int flag = BaseCompanyInfoDB.IsChangeCompanyInfo(id, enabled);
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
