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
    public class KitchenController : Controller
    {
        // GET: Kitchen
        public ActionResult Index()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='KitchenType' And ParentID!=0";
            IList<BasArgumentSetting> KitchenType = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.KitchenType = KitchenType;

            _where = "TypeCode='DoorWindowPosition' And ParentID!=0";
            IList<BasArgumentSetting> DoorWindowPositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorWindowPositions = DoorWindowPositions;

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

            IList<CadDrawingWindowSearch> lst = CadDrawingKitchenDetailDB.GetSearchPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingKitchenDetailDB.GetSearchPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageCount = pageCount;
            return View();
        }

        /// <summary>
        /// GET:/Kitchen/LookOver
        /// </summary>
        /// <returns></returns>
        public ActionResult LookOver(int Id=0)
        {
             if (Id < 1)
            {
                return Redirect("/Kitchen/Index");
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

            CadDrawingKitchenDetail Kitchen = CadDrawingKitchenDetailDB.GetCadDrawingKitchenDetailById(Id);
            ViewBag.Kitchen = Kitchen;
            return View();
        }

        /// <summary>
        /// GET:/Kitchen/Add
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='KitchenType' And ParentID!=0";
            IList<BasArgumentSetting> KitchenType = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.KitchenType = KitchenType;

            _where = "TypeCode='DoorWindowPosition' And ParentID!=0";
            IList<BasArgumentSetting> DoorWindowPositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorWindowPositions = DoorWindowPositions;

            _where = "TypeCode='KitchenBasinType' And ParentID!=0";
            IList<BasArgumentSetting> KitchenBasinTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.KitchenBasinTypes = KitchenBasinTypes;

            _where = "TypeCode='RefrigeratorType' And ParentID!=0";
            IList<BasArgumentSetting> RefrigeratorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.RefrigeratorTypes = RefrigeratorTypes;

            _where = "TypeCode='HearthWidth' And ParentID!=0";
            IList<BasArgumentSetting> HearthWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HearthWidths = HearthWidths;
            return View();
        }

        /// <summary>
        /// 厨房新增类
        /// </summary>
        /// <returns></returns>
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

                CadDrawingKitchenDetail kitchen = new CadDrawingKitchenDetail();
                kitchen.MId = mId;
                kitchen.KitchenType = Request.Form["selectKitchenType"].ConvertToInt32(0);
                kitchen.KitchenPosition = Request.Form["selectKitchenPosition"].ConvertToInt32(0);
                int model = Request.Form["radio_module"].ConvertToInt32(-1);
                kitchen.KitchenIsAirduct = Request.Form["radio_kitchenisairduct"].ConvertToInt32(0);
                if (model == 1) 
                {
                    kitchen.KitchenOpenSizeMin = Request.Form["txtKitchenOpenSizeMin"].ConvertToInt32(0);
                    kitchen.KitchenOpenSizeMax = Request.Form["txtKitchenOpenSizeMax"].ConvertToInt32(0);
                    kitchen.KitchenDepthsizeMin = Request.Form["txtKitchenDepthsizeMin"].ConvertToInt32(0);
                    kitchen.KitchenDepthsizeMax = Request.Form["txtKitchenDepthsizeMax"].ConvertToInt32(0);
                    
                }
                else if (model == 2) 
                {
                    kitchen.KitchenOpenSizeMin = Request.Form["txtWindowSizeMin"].ConvertToInt32(0);
                    kitchen.KitchenOpenSizeMax = Request.Form["txtWindowSizeMax"].ConvertToInt32(0);
                    kitchen.KitchenBasinSize = Request.Form["selectKitchenBasinSize"].ConvertToInt32(0);
                    kitchen.KitchenFridgSize = Request.Form["selectKitchenFridgSize"].ConvertToInt32(0);
                    kitchen.KitchenHearthSize = Request.Form["selectKitchenHearthSize"].ConvertToInt32(0);
                }
                int kitchenid=   CadDrawingKitchenDetailDB.AddHandle(kitchen);
                if (kitchenid > 0)
                {
                    return Json(new { code = 100, message = "厨房原型图纸添加成功" }, JsonRequestBehavior.AllowGet);
                }
                else 
                {
                    return Json(new { code = -100, message = "厨房原型图纸添加失败" }, JsonRequestBehavior.AllowGet);
                }
               
               
            }
            catch (Exception ex) 
            {
                return Json(new { code = -110, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        
        }



        /// <summary>
        ///  GET:/Kitchen/Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int Id=0)
        {
            if (Id < 1)
            {
               return  Redirect("/Kitchen/Index");
                
            }
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='KitchenType' And ParentID!=0";
            IList<BasArgumentSetting> KitchenType = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.KitchenType = KitchenType;

            _where = "TypeCode='DoorWindowPosition' And ParentID!=0";
            IList<BasArgumentSetting> DoorWindowPositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorWindowPositions = DoorWindowPositions;

            _where = "TypeCode='KitchenBasinType' And ParentID!=0";
            IList<BasArgumentSetting> KitchenBasinTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.KitchenBasinTypes = KitchenBasinTypes;

            _where = "TypeCode='RefrigeratorType' And ParentID!=0";
            IList<BasArgumentSetting> RefrigeratorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.RefrigeratorTypes = RefrigeratorTypes;

            _where = "TypeCode='HearthWidth' And ParentID!=0";
            IList<BasArgumentSetting> HearthWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HearthWidths = HearthWidths;

            _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;
            _where = string.Format("  a.MId={0} ", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;
            _where = string.Format("  a.MId={0} ", Id);

            CadDrawingKitchenDetail Kitchen = CadDrawingKitchenDetailDB.GetCadDrawingKitchenDetailById(Id);
            ViewBag.Kitchen = Kitchen;
            return View();
        }

        /// <summary>
        ///   厨房CAD原型属性表-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/CadDrawingKitchenDetail/edithandle</get> 
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
                int Id = Request.Form["hid_id"].ConvertToInt32(0);
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
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_Area = areaid.Split(',');
                CadDrawingDWG dwg = null;
                int index = 0;
                CadDrawingDWGDB.DeleteHandleByParam(string.Format(@" MId={0}",Id));
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

                CadDrawingKitchenDetailDB.DeleteHandleByParam(string.Format(@" MId={0}", Id));
                CadDrawingKitchenDetail kitchen = new CadDrawingKitchenDetail();
                kitchen.MId = mId;
                kitchen.KitchenType = Request.Form["selectKitchenType"].ConvertToInt32(0);
                kitchen.KitchenPosition = Request.Form["selectKitchenPosition"].ConvertToInt32(0);
                int model = Request.Form["radio_module"].ConvertToInt32(-1);
                kitchen.KitchenIsAirduct = Request.Form["radio_kitchenisairduct"].ConvertToInt32(0);
                if (model == 1)
                {
                    kitchen.KitchenOpenSizeMin = Request.Form["txtKitchenOpenSizeMin"].ConvertToInt32(0);
                    kitchen.KitchenOpenSizeMax = Request.Form["txtKitchenOpenSizeMax"].ConvertToInt32(0);
                    kitchen.KitchenDepthsizeMin = Request.Form["txtKitchenDepthsizeMin"].ConvertToInt32(0);
                    kitchen.KitchenDepthsizeMax = Request.Form["txtKitchenDepthsizeMax"].ConvertToInt32(0);

                }
                else if (model == 2)
                {
                    kitchen.KitchenOpenSizeMin = Request.Form["txtWindowSizeMin"].ConvertToInt32(0);
                    kitchen.KitchenOpenSizeMax = Request.Form["txtWindowSizeMax"].ConvertToInt32(0);
                    kitchen.KitchenBasinSize = Request.Form["selectKitchenBasinSize"].ConvertToInt32(0);
                    kitchen.KitchenFridgSize = Request.Form["selectKitchenFridgSize"].ConvertToInt32(0);
                    kitchen.KitchenHearthSize = Request.Form["selectKitchenHearthSize"].ConvertToInt32(0);
                }
                int kitchenid = CadDrawingKitchenDetailDB.AddHandle(kitchen);
                if (kitchenid > 0)
                {
                    return Json(new { code = 100, message = "厨房原型图纸添加成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "厨房原型图纸添加失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        /// <summary>
        ///   厨房CAD原型属性表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/CadDrawingKitchenDetail/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            int Id = Request.QueryString["id"].ConvertToInt32(0);
            int rtv = CadDrawingKitchenDetailDB.DeleteHandleById(Id);
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
        ///   厨房CAD原型属性表-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/CadDrawingKitchenDetail/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            string ids = Request.QueryString["ids"].ConventToString(string.Empty);
            int rtv = CadDrawingKitchenDetailDB.DeleteHandleByIds(ids);
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