using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using Common.Utility.Extender;
using Common.Utility;

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
            int pageSize = 15;      //每页条数
            int currentPage = 0;    //当前页数
            int pageCount = 0;      //总页数
            int startRowNum = 0;    //开始行数
            int endRowNum = 0;      //结束行数
            currentPage = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : Request.QueryString["page"].ConvertToInt32(0);
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

            IList<Bas_Idm_Project> lst = BasIdmProjectDB.GetBasIdmProjectByParameterList(_where, _orderby, startRowNum, endRowNum);
            recordCount = BasIdmProjectDB.GetPageCountBasIdmProjectByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
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
            string code = Request.QueryString["code"];
            string where = string.Format(" And POSID='{0}'",code);
            Bas_Idm_Project project = BasIdmProjectDB.GetBasIdmProjectByProjectId(where);
            ViewBag.Project = project;
            IList<Bas_Idm_ProjectDirectory> listDir = BasIdmProjectDirectoryDB.GetIdmProjectDirectoryByOID(29190);
            ViewBag.ListDir = listDir;
            int dirId=listDir.FirstOrDefault<Bas_Idm_ProjectDirectory>().Id;
            int oid = project.Id;
            where = string.Format(@" DirId='{0}' AND OID='{1}'", dirId, "29190");
             IList<Bas_Idm_ProjectFile> listFile = BasIdmProjectFileDB.GetPageInfoByParameter(where, string.Empty, 0, 300);
             ViewBag.ListFile = listFile;
            return View();
        }
    }
}