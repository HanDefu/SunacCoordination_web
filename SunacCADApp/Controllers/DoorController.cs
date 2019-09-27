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
    public class DoorController : Controller
    {
        // GET: Door
        public ActionResult Index()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;
            string _search_where = "";
            string _url = "1";
            int scope = HttpUtility.UrlDecode(Request.QueryString["scope"]).ConvertToInt32(-1);
            if (scope == 1)
            {
                _search_where += " and  a.scope='" + scope + "'";
                _url += "&scope=" + scope;
            }
            ViewBag.scope = scope;

            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(-1);
            if (area == 1)
            {
                _search_where += string.Format(@"  AND  EXISTS(SELECT 1 FROM dbo.CadDrawingByArea ba WHERE a.Id=ba.MId AND ba.AreaID={0})", area);
                _url += "&area=" + area;
            }

            ViewBag.area = area;

            int doortype = HttpUtility.UrlDecode(Request.QueryString["doortype"]).ConvertToInt32(-1);
            if (doortype == 1)
            {
                _search_where += string.Format(@"  AND b.DoorType={0}", doortype);
                _url += "&doortype=" + doortype;
            }

            ViewBag.doortype = doortype;
            string _orderby = string.Empty;  //排序
            int recordCount = 0;    //记录总数
            int pageSize = 15;      //每页条数
            int currentPage = 0;    //当前页数
            int pageCount = 0;      //总页数
            int startRowNum = 0;    //开始行数
            int endRowNum = 0;      //结束行数
            currentPage = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : Request.QueryString["page"].ConvertToInt32(0);
            startRowNum = ((currentPage - 1) * pageSize) + 1;
            endRowNum = currentPage * pageSize;

            IList<CadDrawingWindowSearch> lst = CadDrawingDoorDetailDB.GetSearchPageInfoByParameter(_search_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingDoorDetailDB.GetSearchPageCountByParameter(_search_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageCount = pageCount;
            return View();
        }

        /// <summary>
        /// GET:/Door/LookOver
        /// </summary>
        /// <returns></returns>
        public ActionResult LookOver(int Id=0)
        {
            if (Id < 1)
            {
                Redirect("/door/Index");
            }
            string _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;
            _where = string.Format("  a.MId={0} ", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;
            _where = string.Format("  a.MId={0} ", Id);
            CadDrawingDoorDetail door = CadDrawingDoorDetailDB.GetSingleEntityByparam(_where);
            ViewBag.Door = door;
            return View();
        }

        public ActionResult Add()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;
            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;
            return View();
        }

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
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_Area = areaid.Split(',');
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

                
                int DoorType = Request.Form["selectDoorType"].ConvertToInt32(-1);
                int WindowSizeMin = Request.Form["txtWindowSizeMin"].ConvertToInt32(-1);
                int WindowSizeMax = Request.Form["txtWindowSizeMax"].ConvertToInt32(-1);
                CadDrawingDoorDetail door = new CadDrawingDoorDetail();
                door.MId = mId;
                door.DoorType = DoorType;
                door.WindowSizeMin = WindowSizeMin;
                door.WindowSizeMax = WindowSizeMax;
                door.CreateOn = DateTime.Now;
                door.ModifiedOn = DateTime.Now;
                int rtv=  CadDrawingDoorDetailDB.AddHandle(door);
                if (rtv > 0 && mId > 0)
                {
                    return Json(new { code = 100, message = "门原型图纸添加成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "门原型图纸添加失败" }, JsonRequestBehavior.AllowGet);

                }



              
            }
            catch (Exception ex) {
                return Json(new { code = -110, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  /door/edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int Id=0)
        {
            if (Id < 1)
            {
                Redirect("/door/Index");
            }
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;
            _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.byAreaList = ByAreas;
            _where = string.Format("  a.MId={0} ", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;
            _where = string.Format("  a.MId={0} ", Id);
            CadDrawingDoorDetail door = CadDrawingDoorDetailDB.GetSingleEntityByparam(_where);
            ViewBag.Door = door;
            ViewBag.Id = Id;

            return View();
        }
        /// <summary>
        ///   门CAD原型属性表-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>caddrawingdoordetail/edithandle</get> 
        /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
        {
            try
            {
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["cad_file"];
                string imgFile = Request.Form["img_file"];
                string areaid = Request.Form["checkbox_areaid"];
                int Id = Request.Form["Id"].ConvertToInt32(-1);
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
                int mId = CadDrawingMasterDB.EditHandle(caddrawingmaster, string.Format(@" And id={0}",Id));
                mId = Id;
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_Area = areaid.Split(',');
                CadDrawingDWG dwg = null;
                int index = 0;
                CadDrawingDWGDB.DeleteHandleByParam(string.Format(@" MId={0}", Id));
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

                CadDrawingByAreaDB.DeleteHandleByParam(string.Format(@" MId={0}", Id));
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

                CadDrawingDoorDetailDB.DeleteHandleByParam(string.Format(@" MId={0}", Id));
                int DoorType = Request.Form["selectDoorType"].ConvertToInt32(-1);
                int WindowSizeMin = Request.Form["txtWindowSizeMin"].ConvertToInt32(-1);
                int WindowSizeMax = Request.Form["txtWindowSizeMax"].ConvertToInt32(-1);
                CadDrawingDoorDetail door = new CadDrawingDoorDetail();
                door.MId = mId;
                door.DoorType = DoorType;
                door.WindowSizeMin = WindowSizeMin;
                door.WindowSizeMax = WindowSizeMax;
                door.CreateOn = DateTime.Now;
                door.ModifiedOn = DateTime.Now;
                int rtv = CadDrawingDoorDetailDB.AddHandle(door);
                if (rtv > 0 && mId > 0)
                {
                    return Json(new { code = 100, message = "门原型图纸添加成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "门原型图纸添加失败" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -110, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///   门CAD原型属性表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>caddrawingdoordetail/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            int rtv = CadDrawingDoorDetailDB.DeleteHandleById(Id);
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
        ///   门CAD原型属性表-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>caddrawingdoordetail/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            string ids = Request.QueryString["ids"].ConventToString(string.Empty);
            int rtv = CadDrawingDoorDetailDB.DeleteHandleByIds(ids);
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