using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using Common.Utility.Extender;
using Common.Utility;
using SunacCADApp.Library;

namespace SunacCADApp.Controllers
{
    public class ProjectInfoController : Controller
    {
        private int UserId = 0;
        private string UserName = string.Empty;
        public ProjectInfoController() 
        {
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            ViewBag.ParentId = 3;
        }
        // GET: ProjectInfo
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = string.Empty;  //查询
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
            IList<PArea> areas = Project_InformationDB.GetProjectAreaByList();
            ViewBag.Areas = areas;
            string areaCode = HttpUtility.UrlDecode(Request.QueryString["area"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(areaCode))
            {
                _where += string.Format(@" And a.PAREA_GS_NAME = '{0}'", areaCode);
                _url += "area=" + areaCode + "&";
            }
            ViewBag.AreaCode = areaCode;
            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword)) 
            {
                _where += string.Format(@" And a.POST1 like '%{0}%'", keyword);
                _url += "keyword=" + keyword + "&";
            }
            ViewBag.Keyword = keyword;

            IList<DataSourceMember> IdmOrginList = BasInstitutionDataDB.GetInnerIdmOrgan();
            ViewBag.IdmOrginList = IdmOrginList;

            IList<Bas_Idm_Project> lst = BasIdmProjectDB.GetBasIdmProjectByParameterList(_where, _orderby, startRowNum, endRowNum);
            recordCount = BasIdmProjectDB.GetPageCountBasIdmProjectByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);

            int[] page = CommonLib.PageHelper(pageCount, currentPage);

            IList<PageNum> pageNumList = CommonLib.GetPageNum();

            ViewBag.URL = _url;
            ViewBag.List = lst;
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
        ///  projectInfo/view
        /// </summary>
        /// <returns></returns>
        public ActionResult Item() 
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string code = Request.QueryString["code"].ConvertToTrim();
          
            string where = string.Format(" And POSID='{0}'",code);
            Bas_Idm_Project project = BasIdmProjectDB.GetBasIdmProjectByProjectId(where);
            ViewBag.Project = project;

            int proid = project.Id;
            IList<Bas_Idm_ProjectDirectory> listDir = BasIdmProjectDirectoryDB.GetIdmProjectDirectoryByOID(proid);
            ViewBag.ListDir = listDir;
            Bas_Idm_ProjectDirectory directorys = listDir.FirstOrDefault<Bas_Idm_ProjectDirectory>();
            int dirId = directorys == null ? 0 : directorys.Id;
            int oid = project.Id;
            where = string.Format(@" DirId='{0}' AND OID='{1}'", dirId, proid);
             IList<Bas_Idm_ProjectFile> listFile = BasIdmProjectFileDB.GetPageInfoByParameter(where, string.Empty, 0, 300);
             ViewBag.ListFile = listFile;
            return View();
        }

        public ActionResult FileDownload(int id) 
        {
            try
            {
                if (UserId < 1)
                {
                    return Content("非法操作");
                }
                Bas_Idm_ProjectFile file = BasIdmProjectFileDB.GetSingleEntityById(id);
                if (string.IsNullOrEmpty(file.SaveName))
                    return Content("没有找到你要下载的文件");
                string Root = API_Common.GlobalParam("projectFilePath");
                string Catalog = file.ModifiedOn.ToString("yyyyMMdd");
                string filePath = string.Concat(Root, "\\", Catalog, "\\", file.SaveName);
                API_Common.DownloadFile(filePath, file.FileName);
                return RedirectToAction("ListForStore");
            }
            catch (Exception ex) 
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 批量下载
        /// </summary>
        /// <returns></returns>
        public ActionResult FileDownLoadList() 
        {
            try
            {
                if (UserId < 1)
                {
                    return Content("非法操作");
                }
                string ids = Request.QueryString["ids"].ConvertToTrim();
                if (string.IsNullOrEmpty(ids))
                    return Content("没有找到你要下载的文件");
                string where = string.Format(@" Id IN ({0}) ",ids);
                IList<Bas_Idm_ProjectFile> fileList = BasIdmProjectFileDB.GetIdmProjectFileListByParam(where);
                string Root = API_Common.GlobalParam("projectFilePath");
                foreach (Bas_Idm_ProjectFile file in fileList)
                {
                    string _saveFile = file.SaveName;
                    if (string.IsNullOrEmpty(_saveFile))
                        continue;
                    string Catalog = file.ModifiedOn.ToString("yyyyMMdd");
                    string filePath = string.Concat(Root, "\\", Catalog, "\\", file.SaveName);
                    API_Common.DownloadFile(filePath, file.FileName);
                }
                return RedirectToAction("ListForStore");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteProjectFile(int id) 
        {
            if (UserId < 1)
            {
                return Json(new { Code = -100, Message = "非法操作" });
            }

            try 
            {
                BasIdmProjectFileDB.SetProjectFileEnabledById(1, id);
                return Json(new { Code = 100, Message = "删除成功" });
            }
            catch (Exception ex) 
            {
                return Json(new { Code = -100, Message = ex.Message });
            }
        }

        /// <summary>
        /// 删除多个文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteProjectFile()
        {
            if (UserId < 1)
            {
                return Json(new { Code = -100, Message = "非法操作" });
            }

            string ids = Request.QueryString["ids"].ConvertToTrim();
            if (string.IsNullOrEmpty(ids)) 
            {
                return Json(new { Code = -100, Message = "非法操作" });
            }
            try
            {
                BasIdmProjectFileDB.SetProjectFileEnabledById(1, ids);
                return Json(new { Code = 100, Message = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -100, Message = ex.Message });
            }
        }
    }
}