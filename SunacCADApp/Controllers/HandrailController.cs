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
    public class HandrailController : Controller
    {
        // GET: Handrail
        public ActionResult Index()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='HandRail' And ParentID!=0";
            IList<BasArgumentSetting> HandRails = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HandRails = HandRails;


            _where = string.Empty;  //查询
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

            IList<CadDrawingWindowSearch> lst = CadDrawingHandrailDetailDB.GetSearchPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingHandrailDetailDB.GetSearchPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageCount = pageCount;
            return View();
        }

        /// <summary>
        /// GET:/Handrail/LookOver
        /// </summary>
        /// <returns></returns>
        public ActionResult LookOver(int Id=0)
        {
            if (Id < 1) 
            {
                return Redirect("/handrail/index");
            }

            string _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;
            _where = string.Format("  a.MId={0}", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;

            CadDrawingHandrailDetail handrail = CadDrawingHandrailDetailDB.GetCadDrawingHandrailDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.handrail = handrail;
            
            return View();
        }

        /// <summary>
        /// GET:/Handrail/Add
        /// </summary>
        /// <returns></returns>

        public ActionResult Add()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='HandRail' And ParentID!=0";
            IList<BasArgumentSetting> HandRails = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HandRails = HandRails;


            return View();
        }

         /// <summary>
       ///   栏杆CAD原型属性表-新增方法
       /// </summary>
       /// <returns></returns>
        /// <get>/Handrail/addhandle</get>
       /// <author>alon<84789887@qq.com></author> 
        [ValidateInput(false)]
        public ActionResult Addhandle()
        {
            try
            {
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["cad_file"];
                string imgFile = Request.Form["img_file"];
                string areaid = Request.Form["checkbox_areaid"];
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chekbox_group"].ConvertToInt32(0);
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
                caddrawingmaster.CreateOn = DateTime.Now;
                caddrawingmaster.Reorder = 2;
                caddrawingmaster.Enabled = 1;
                caddrawingmaster.CreateUserId = 0;
                caddrawingmaster.CreateBy = "admin";
                int mId = CadDrawingMasterDB.AddHandle(caddrawingmaster);
                string[] arr_CADFile = new string[] { };
                if (!string.IsNullOrEmpty(cadFile))
                {
                    arr_CADFile = cadFile.Split(',');
                }
                string[] arr_IMGFile = new string[] { };
                if (!string.IsNullOrEmpty(cadFile))
                {
                    arr_IMGFile = imgFile.Split(',');
                }
                string[] arr_Area = new string[] { };
                if (!string.IsNullOrEmpty(areaid))
                {
                    arr_Area = areaid.Split(',');
                }


                CadDrawingDWG dwg = null;
                int index = 0;
                foreach (string cad in arr_CADFile)
                {
                    if (!string.IsNullOrEmpty(cad))
                    {
                        dwg = new CadDrawingDWG();
                        dwg.MId = mId;
                        dwg.DWGPath = arr_IMGFile[index];
                        dwg.FileClass = "DWG";
                        dwg.CADPath = cad;
                        CadDrawingDWGDB.AddHandle(dwg);
                        index++;
                    }
                }
                foreach (string area in arr_Area)
                {
                    if (!string.IsNullOrEmpty(area))
                    {
                        CadDrawingByArea byArea = new CadDrawingByArea();
                        byArea.AreaID = area.ConvertToInt32(-1);
                        byArea.MId = mId;
                        CadDrawingByAreaDB.AddHandle(byArea);
                    }
                }
                CadDrawingHandrailDetail handrail = new CadDrawingHandrailDetail();
                handrail.MId= mId;
                handrail.HandrailType = Request.Form["HandRailType"].ConvertToInt32(0);
                handrail.CreateOn = DateTime.Now;
                int detailId = CadDrawingHandrailDetailDB.AddHandle(handrail);

                if (mId > 0 && detailId > 0)
                {
                    return Json(new { code = 100, message = "添加成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "添加失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  /Handrail/Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int Id=0)
        {
            if (Id < 1)
            {
                return Redirect("/handrail/index");
            }
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='HandRail' And ParentID!=0";
            IList<BasArgumentSetting> HandRails = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HandRails = HandRails;


            _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;

            _where = string.Format("  a.MId={0}", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;

            CadDrawingHandrailDetail HandrailDetail = CadDrawingHandrailDetailDB.GetCadDrawingHandrailDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.HandrailDetail = HandrailDetail;
            return View();
        }

        /// <summary>
        ///   栏杆CAD原型属性表-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>/handrail/edithandle</get> 
        /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
        {
            CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
            int Id = Request.Form["Id"].ConvertToInt32(0);
            string cadFile = Request.Form["cad_file"];
            string imgFile = Request.Form["img_file"];
            string areaid = Request.Form["checkbox_areaid"];
            int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
            caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
            caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
            caddrawingmaster.Scope = Request.Form["chekbox_group"].ConvertToInt32(0);
            caddrawingmaster.AreaId = 0;
            caddrawingmaster.DynamicType = DynamicType;
            caddrawingmaster.CreateOn = DateTime.Now;
            caddrawingmaster.Reorder = 2;
            caddrawingmaster.Enabled = 1;
            caddrawingmaster.CreateUserId = 0;
            caddrawingmaster.CreateBy = "admin";
            caddrawingmaster.Id = Id;
            int mId = CadDrawingMasterDB.EditHandle(caddrawingmaster,string.Empty);
            mId = Id;
            string[] arr_CADFile = new string[] { };
            if (!string.IsNullOrEmpty(cadFile))
            {
                arr_CADFile = cadFile.Split(',');
            }
            string[] arr_IMGFile = new string[] { };
            if (!string.IsNullOrEmpty(cadFile))
            {
                arr_IMGFile = imgFile.Split(',');
            }
            string[] arr_Area = new string[] { };
            if (!string.IsNullOrEmpty(areaid))
            {
                arr_Area = areaid.Split(',');
            }

            CadDrawingDWGDB.DeleteHandleByParam(string.Format(@" MId={0}",mId));
            CadDrawingDWG dwg = null;
            int index = 0;
            foreach (string cad in arr_CADFile)
            {
                if (!string.IsNullOrEmpty(cad))
                {
                    dwg = new CadDrawingDWG();
                    dwg.MId = mId;
                    dwg.DWGPath = arr_IMGFile[index];
                    dwg.FileClass = "DWG";
                    dwg.CADPath = cad;
                    CadDrawingDWGDB.AddHandle(dwg);
                    index++;
                }
            }
            CadDrawingByAreaDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
            foreach (string area in arr_Area)
            {
                if (!string.IsNullOrEmpty(area))
                {
                    CadDrawingByArea byArea = new CadDrawingByArea();
                    byArea.AreaID = area.ConvertToInt32(-1);
                    byArea.MId = mId;
                    CadDrawingByAreaDB.AddHandle(byArea);
                }
            }
            CadDrawingHandrailDetailDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
            CadDrawingHandrailDetail handrail = new CadDrawingHandrailDetail();
            handrail.MId = mId;
            handrail.HandrailType = Request.Form["HandRailType"].ConvertToInt32(0);
            handrail.CreateOn = DateTime.Now;
            int detailId = CadDrawingHandrailDetailDB.AddHandle(handrail);

            if (mId > 0 && detailId > 0)
            {
                return Json(new { code = 100, message = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}