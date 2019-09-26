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
    public class WindowController : Controller
    {
        // GET: Window
        public ActionResult Index()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='ActionType' And ParentID!=0";
            IList<BasArgumentSetting> ActionTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ActionTypes = ActionTypes;

            _where = "TypeCode='OpenType' And ParentID!=0";
            IList<BasArgumentSetting> OpenTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.OpenTypes = OpenTypes;

            _where = "TypeCode='OpenWindowNum' And ParentID!=0";
            IList<BasArgumentSetting> OpenWindowNums = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.OpenWindowNums = OpenWindowNums;
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
            IList<CadDrawingWindowSearch> lst = CadDrawingWindowSearchDB.GetPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingWindowSearchDB.GetPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageCount = pageCount;
            return View();
        }

        /// <summary>
        /// GET:/Window/LookOver
        /// </summary>
        /// <returns></returns>
        public ActionResult LookOver(int Id=0)
        {
            if (Id > 0) 
            {
                Redirect("/Window/Index");
            }
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetPageInfoByParameter("  a.MId="+Id,string.Empty,0,200);
            ViewBag.ByAreas = master;
            
            return View();
        }
        /// <summary>
        /// 外窗添加
        /// </summary>
        /// <returns></returns>
        ///<url>/Window/Add</url>

        public ActionResult Add()
        {

            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='ActionType' And ParentID!=0";

            IList<BasArgumentSetting> ActionTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ActionTypes = ActionTypes;

            _where = "TypeCode='OpenType' And ParentID!=0";

            IList<BasArgumentSetting> OpenTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.OpenTypes = OpenTypes;

            _where = "TypeCode='OpenWindowNum' And ParentID!=0";
            IList<BasArgumentSetting> OpenWindowNums = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.OpenWindowNums = OpenWindowNums;
            IList<DataSourceMember> Members = CommonLib.GetWindowArgument();
            ViewBag.Members = Members;

            return View();
        }
        /// <summary>
        ///   CAD原型信息-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/Window/Addhandle</get>
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
                string actionType = Request.Form["ActionType"];
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chekbox_group"].ConvertToInt32(0);
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
                caddrawingmaster.CreateOn = DateTime.Now;
                caddrawingmaster.Reorder = Request.Form["txt_reorder"].ConvertToInt32(0);
                caddrawingmaster.Enabled = Request.Form["select_enabled"].ConvertToInt32(0);
                caddrawingmaster.CreateUserId = 0;
                caddrawingmaster.CreateBy = "admin";
                int mId = CadDrawingMasterDB.AddHandle(caddrawingmaster);
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_Area = areaid.Split(',');
                string[] arr_ActionType = actionType.Split(',');
                CadDrawingDWG dwg = null;
                foreach (string cad in arr_CADFile)
                {
                    if (!string.IsNullOrEmpty(cad))
                    {
                        dwg = new CadDrawingDWG();
                        dwg.MId = mId;
                        dwg.DWGPath = cad;
                        dwg.FileClass = "DWG";
                        CadDrawingDWGDB.AddHandle(dwg);
                    }
                }
                foreach (string img in arr_IMGFile)
                {
                    if (!string.IsNullOrEmpty(img))
                    {
                        dwg = new CadDrawingDWG();
                        dwg.MId = mId;
                        dwg.DWGPath = img;
                        dwg.FileClass = "JPG";
                        CadDrawingDWGDB.AddHandle(dwg);
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


                foreach (string action in arr_ActionType)
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        CadDrawingFunction function = new CadDrawingFunction();
                        function.MId = mId;
                        function.FunctionId = action.ConvertToInt32(-1);
                        CadDrawingFunctionDB.AddHandle(function);
                    }

                }

                int OpenType = Request.Form["OpenType"].ConvertToInt32(0);
                int OpenWindowNum =Request.Form["OpenWindowNum"].ConvertToInt32(0);
                int WindowHasCorner = Request.Form["Radio_WindowHasCorner"].ConvertToInt32(0);
                int WindowHasSymmetry = Request.Form["Radio_WindowHasSymmetry"].ConvertToInt32(0);
                decimal WindowSizeMin = Request.Form["txtWindowSizeMin"].ConventToDecimal(0);
                decimal WindowSizeMax = Request.Form["txtWindowSizeMax"].ConventToDecimal(0);
                string WindowDesignFormula = Request.Form["WindowDesignFormula"].ConventToString(string.Empty);
                if (DynamicType == 2) 
                {
                     WindowSizeMin = Request.Form["txt_Window_Width"].ConventToDecimal(0);
                     WindowSizeMax = Request.Form["txt_Window_Height"].ConventToDecimal(0);
                }
                int WindowVentilationQuantity = Request.Form["WindowVentilationQuantity"].ConvertToInt32(0);
                int WindowPlugslotSize = Request.Form["WindowVentilationQuantity"].ConvertToInt32(0);
                int WindowAuxiliaryFrame = Request.Form["Checkbox_WindowAuxiliaryFrame"].ConvertToInt32(0);

                CadDrawingWindowDetail window = new CadDrawingWindowDetail();
                window.MId = mId;
                window.WindowOpenTypeId = OpenType;
                window.WindowOpenQtyId = OpenWindowNum;
                window.WindowHasCorner = WindowHasCorner;
                window.WindowHasSymmetry = WindowHasSymmetry;
                window.WindowSizeMax = WindowSizeMax;
                window.WindowSizeMin = WindowSizeMin;
                window.WindowDesignFormula = WindowDesignFormula;
                window.WindowVentilationQuantity = WindowVentilationQuantity;
                window.WindowPlugslotSize = WindowPlugslotSize;
                window.WindowAuxiliaryFrame = WindowAuxiliaryFrame;
                int detail =   CadDrawingWindowDetailDB.AddHandle(window);
                if (mId > 0 && detail>0)
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
                return Json(new { code = -110, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///  外窗编辑
        /// 
        /// </summary>
        /// <returns></returns>
        /// <url>/Window/Edit</url>
        public ActionResult Edit()
        {
            return View();
        }
    }
}