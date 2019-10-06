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
    public class AirconditionerController : Controller
    {
        // GET: Airconditioner
        public ActionResult Index()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;

            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;
            
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

            IList<CadDrawingWindowSearch> lst = CadDrawingAirconditionerDetailDB.GetSearchPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingAirconditionerDetailDB.GetSearchPageCountByParameter(_where);
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
        public ActionResult LookOver()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;
            //空调匹数
            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;

            //冷凝管位置
            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;


            //雨水管位置
            _where = "TypeCode='RainPipePosition' And ParentID!=0";
            IList<BasArgumentSetting> RainPipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.RainPipePositions = RainPipePositions;

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
            //空调匹数
            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;
            
            //冷凝管位置
            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;


            //雨水管位置
            _where = "TypeCode='RainPipePosition' And ParentID!=0";
            IList<BasArgumentSetting> RainPipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.RainPipePositions = RainPipePositions;
           
            return View();
        }


       /// <summary>
       ///   空调CAD原型属性表-新增方法
       /// </summary>
       /// <returns></returns>
       /// <get>/manage/CadDrawingAirconditionerDetail/addhandle</get>
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


                int AirConditionNumber = Request.Form["AirConditionNumber"].ConvertToInt32(-1);
                int AirconditionerMinWidth = Request.Form["txtAirconditionerMinWidth"].ConvertToInt32(-1);
                int AirconditionerMinLength = Request.Form["txtAirconditionerMinLength"].ConvertToInt32(-1);
                int CondensatePipePosition = Request.Form["selectCondensatePipePosition"].ConvertToInt32(-1);
                int AirconditionerIsRainPipe = Request.Form["Checkbox_AirconditionerIsRainPipe"].ConvertToInt32(-1);
                int RainPipePosition = Request.Form["selectRainPipePosition"].ConvertToInt32(-1);
                CadDrawingAirconditionerDetail airconditioner = new CadDrawingAirconditionerDetail();
                airconditioner.MId = mId;
                airconditioner.AirconditionerIsRainPipe = AirconditionerIsRainPipe;
                airconditioner.AirconditionerMinWidth = AirconditionerMinWidth;
                airconditioner.AirconditionerMinLength = AirconditionerMinLength;
                airconditioner.AirconditionerPower = AirConditionNumber;
                airconditioner.AirconditionerPipePosition = CondensatePipePosition;
                airconditioner.AirconditionerIsRainPipe = AirconditionerIsRainPipe;
                if (AirconditionerIsRainPipe == 1) 
                {
                    airconditioner.AirconditionerRainPipePosition = RainPipePosition;
                }
                int rtv = CadDrawingAirconditionerDetailDB.AddHandle(airconditioner);
                if (rtv > 0 && mId > 0)
                {
                    return Json(new { code = 100, message = "空调原型图纸添加成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "空调原型图纸添加失败" }, JsonRequestBehavior.AllowGet);

                }




            }
            catch (Exception ex)
            {
                return Json(new { code = -110, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  /Handrail/Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;
            //空调匹数
            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;

            //冷凝管位置
            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;


            //雨水管位置
            _where = "TypeCode='RainPipePosition' And ParentID!=0";
            IList<BasArgumentSetting> RainPipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.RainPipePositions = RainPipePositions;

            return View();
        }
    }
}