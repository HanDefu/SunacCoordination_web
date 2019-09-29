using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using Common.Utility;
using Common.Utility.Extender;

namespace SunacCADApp.Controllers
{
    public class BathroomController : Controller
    {
        // GET: Bathroom
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

            IList<CadDrawingWindowSearch> lst = CadDrawingBathroomDetailDB.GetSearchPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingBathroomDetailDB.GetSearchPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageCount = pageCount;
            return View();

        }

        /// <summary>
        /// GET:/Bathroom/LookOver
        /// </summary>
        /// <returns></returns>
        public ActionResult LookOver(int Id=0)
        {
            if (Id < 1) 
            {
                return Redirect("/Bathroom/Index");
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

            CadDrawingBathroomDetail bathroom = CadDrawingBathroomDetailDB.GetCadDrawingBathroomDetailByWhere(string.Format(@" MId={0}",Id));
            ViewBag.Bathroom = bathroom;

            return View();
        }

        /// <summary>
        /// GET:/Bathroom/Add
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;
            //卫生间类型
            _where = "TypeCode='ToiletType' And ParentID!=0";
            IList<BasArgumentSetting> ToiletTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletTypes = ToiletTypes;
            //卫生间门窗位置
            _where = "TypeCode='BathroomDoorWindowPosition' And ParentID!=0";
            IList<BasArgumentSetting> BathroomDoorWindowPositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.BathroomDoorWindowPositions = BathroomDoorWindowPositions;


            //马桶宽度
            _where = "TypeCode='ClosesToolWidth' And ParentID!=0";
            IList<BasArgumentSetting> ClosesToolWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ClosesToolWidths = ClosesToolWidths;
            //卫生间水盆宽度
            _where = "TypeCode='ToiletBasinWidth' And ParentID!=0";
            IList<BasArgumentSetting> ToiletBasinWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletBasinWidths = ToiletBasinWidths;



            return View();
        }


        /// <summary>
        ///   卫生间CAD原型属性表-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/Bathroom/addhandle</get>
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
                CadDrawingBathroomDetail bathroom = new CadDrawingBathroomDetail();

                bathroom.MId = mId;
                bathroom.BathroomType = Request.Form["ToiletType"].ConvertToInt32(0);
                bathroom.BathroomDoorWindowPosition=Request.Form["selectBathroomDoorWindowPosition"].ConvertToInt32(0);
                int module = Request.Form["radio_module"].ConvertToInt32(0);
                if (module == 1) {
                    bathroom.BathroomShortSideMin = Request.Form["txtBathroomShortSideMin"].ConvertToInt32(0);
                    bathroom.BathroomShortSideMax = Request.Form["txtBathroomShortSideMax"].ConvertToInt32(0);
                    bathroom.BathroomLongSizeMin = Request.Form["txtBathroomLongSizeMin"].ConvertToInt32(0);
                    bathroom.BathroomLongSizeMax = Request.Form["txtBathroomLongSizeMax"].ConvertToInt32(0);
                }
                else if (module == 2) 
                {
                    bathroom.BathroomShortSideMin = Request.Form["txtSizeSideMin"].ConvertToInt32(0);
                    bathroom.BathroomShortSideMax = Request.Form["txtSizeSideMax"].ConvertToInt32(0);
                    bathroom.BathroomClosestoolSize = Request.Form["selectClosesToolWidth"].ConvertToInt32(0);
                    bathroom.BathroomBasinSize = Request.Form["selectBathroomBasinSize"].ConvertToInt32(0);
                }

                bathroom.CreateOn = DateTime.Now;
               int detailId=  CadDrawingBathroomDetailDB.AddHandle(bathroom);

               if (mId > 0 && detailId>0)
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
        ///  GET:/Bathroom/Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int Id=0)
        {
            if (Id < 1) 
            {
                return Redirect("/bathroom/index");
            }
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;
            //卫生间类型
            _where = "TypeCode='ToiletType' And ParentID!=0";
            IList<BasArgumentSetting> ToiletTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletTypes = ToiletTypes;
            //卫生间门窗位置
            _where = "TypeCode='BathroomDoorWindowPosition' And ParentID!=0";
            IList<BasArgumentSetting> BathroomDoorWindowPositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.BathroomDoorWindowPositions = BathroomDoorWindowPositions;


            //马桶宽度
            _where = "TypeCode='ClosesToolWidth' And ParentID!=0";
            IList<BasArgumentSetting> ClosesToolWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ClosesToolWidths = ClosesToolWidths;
            //卫生间水盆宽度
            _where = "TypeCode='ToiletBasinWidth' And ParentID!=0";
            IList<BasArgumentSetting> ToiletBasinWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletBasinWidths = ToiletBasinWidths;


            _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;
            _where = string.Format("  a.MId={0}", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;

            CadDrawingBathroomDetail bathroom = CadDrawingBathroomDetailDB.GetCadDrawingBathroomDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.Bathroom = bathroom;

            return View();
        }
       
         
    }
}