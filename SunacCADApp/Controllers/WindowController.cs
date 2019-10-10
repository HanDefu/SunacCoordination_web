﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility;
using Common.Utility.Extender;
using Newtonsoft.Json;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using System.Data;

namespace SunacCADApp.Controllers
{
    public class WindowController : Controller
    {
        public WindowController() 
        {
            ViewBag.SelectModel = 5;
        }
        // GET: /window/index
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
            string _search_where = " 1=1 ";
            string _url = "1";
     
            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(-1);
            if (area >0)
            {
                _search_where += string.Format(@" AND EXISTS(SELECT * FROM dbo.CadDrawingByArea pa WHERE pa.MId=a.Id AND pa.AreaID={0})", area);
                _url += "&area=" + area;
            }

            ViewBag.area = area;

            int action = HttpUtility.UrlDecode(Request.QueryString["action"]).ConvertToInt32(-1);
            if (action > 0)
            {
                _search_where += string.Format(@" AND EXISTS(SELECT * FROM dbo.CadDrawingFunction pb WHERE pb.MId=a.Id AND pb.FunctionId={0})", action);
                _url += "&action=" + action;
            }
            ViewBag.action = action;

            int opentype = HttpUtility.UrlDecode(Request.QueryString["opentype"]).ConvertToInt32(-1);
            if (opentype >0)
            {
                _search_where += " and  b.WindowOpenTypeId='" + opentype + "'";
                _url += "&opentype=" + opentype;
            }
            ViewBag.opentype = opentype;

            int openwindownum = HttpUtility.UrlDecode(Request.QueryString["openwindownum"]).ConvertToInt32(-1);
            if (openwindownum >0)
            {
                _search_where += " and  b.WindowOpenQtyId='" + openwindownum + "'";
                _url += "&openwindownum=" + openwindownum;
            }
            ViewBag.openwindownum = openwindownum;


            _where = _search_where;  //查询

            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword)) 
            {
                _where = string.Format(@" a.DrawingCode='{0}'", keyword);
            }
            ViewBag.Keyword = keyword;

            string _orderby = string.Empty;  //排序
     
            int recordCount = 0;    //记录总数
            int pageSize = 30;      //每页条数
            int currentPage = 0;    //当前页数
            int pageCount = 0;      //总页数
            int startRowNum = 0;    //开始行数
            int endRowNum = 0;      //结束行数
            currentPage = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : Request.QueryString["page"].ConvertToInt32(0);
            startRowNum = ((currentPage - 1) * pageSize) + 1;
            endRowNum = currentPage * pageSize;
            IList<CadDrawingWindowSearch> list = CadDrawingWindowDetailDB.GetSearchPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingWindowDetailDB.GetSearchPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = list;
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
            if (Id < 1) 
            {
               return   Redirect("/Window/Index");
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
            _where = "  a.MId=" + Id;
            IList<CadDrawingFunction> Functions = CadDrawingFunctionDB.GetCadDrawingFunctionByWhereList(_where);
            ViewBag.Functions = Functions;
            _where = string.Format(@" a.MId={0}", Id);
            CadDrawingWindowDetail windowDetail = CadDrawingWindowDetailDB.GetCadDrawingWindowDetailByWhere(_where);
            ViewBag.WindowDetail = windowDetail;
            _where=string.Format(@" a.MId={0}",Id);
            IList<CadDrawingParameter> cadDrawingParams = CadDrawingParameterDB.GetCadDrawingParameterByWhereList(_where);
            ViewBag.CadDrawingParams = cadDrawingParams;
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
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string filenames = Request.Form["txt_filename"];
                string areaid = Request.Form["checkbox_areaid"];
                string actionType = Request.Form["ActionType"];
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.Scope = 1;
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
                string[] arr_FileName = filenames.Split(',');
                string[] arr_Area = areaid.Split(',');
                string[] arr_ActionType = actionType.Split(',');
                CadDrawingDWG dwg = null;
                int index = 0;
                foreach (string cad in arr_CADFile)
                {
                    if (!string.IsNullOrEmpty(cad))
                    {
                        string dwgFileName = System.IO.Path.GetFileName(cad);
                        dwg = new CadDrawingDWG();
                        dwg.MId = mId;
                        dwg.DWGPath = arr_IMGFile[index];
                        dwg.FileClass   = arr_FileName[index];
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
                string WindowDesignFormula = Request.Form["txtWindowDesignFormula"].ConventToString(string.Empty);
                if (DynamicType == 2) 
                {
                     WindowSizeMin = Request.Form["txt_Window_Width"].ConventToDecimal(0);
                     WindowSizeMax = Request.Form["txt_Window_Height"].ConventToDecimal(0);
                }
                int WindowVentilationQuantity = Request.Form["txt_WindowVentilationQuantity"].ConvertToInt32(0);
                int WindowPlugslotSize = Request.Form["txtWindowPlugslotSize"].ConvertToInt32(0);
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
                string param = Request.Form["param"].ConventToString(string.Empty);
                DataTable tableParam = JsonConvert.DeserializeObject<DataTable>(param);
                if (tableParam != null) 
                {
                    foreach (DataRow row in tableParam.Rows) 
                    {
                        CadDrawingParameter cadParam = new CadDrawingParameter();
                        cadParam.MId = mId;
                        cadParam.SizeNo = row["SizeNo"].ConventToString(string.Empty);
                        cadParam.ValueType = row["ValueType"].ConvertToInt32(-1);
                        cadParam.Val = row["Val"].ConventToString(string.Empty);
                        cadParam.MinValue = row["MinValue"].ConvertToInt32(-1);
                        cadParam.MaxValue = row["MaxValue"].ConvertToInt32(-1);
                        cadParam.DefaultValue = row["DefaultValue"].ConvertToInt32(-1);
                        cadParam.Desc = row["Desc"].ConventToString(string.Empty);
                        cadParam.CreateOn = DateTime.Now;
                        CadDrawingParameterDB.AddHandle(cadParam);
                    }
                    

                }

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
        public ActionResult Edit(int Id=0)
        {
            if (Id < 1)
            {
               return  Redirect("/Window/Index");
            }

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

            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;
            _where = string.Format("  a.MId={0}", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;
            _where = "  a.MId=" + Id;
            IList<CadDrawingFunction> Functions = CadDrawingFunctionDB.GetCadDrawingFunctionByWhereList(_where);
            ViewBag.Functions = Functions;
            _where = string.Format(@" a.MId={0}", Id);
            CadDrawingWindowDetail windowDetail = CadDrawingWindowDetailDB.GetCadDrawingWindowDetailByWhere(_where);
            ViewBag.WindowDetail = windowDetail;
            _where = string.Format(@" a.MId={0}", Id);
            IList<CadDrawingParameter> cadDrawingParams = CadDrawingParameterDB.GetCadDrawingParameterByWhereList(_where);
            ViewBag.CadDrawingParams = cadDrawingParams;
            return View();
        }


          /// <summary>
       ///   外窗CAD原型属性表-修改方法
       /// </summary>
       /// <returns></returns>
       /// <get>/manage/CadDrawingWindowDetail/edithandle</get> 
       /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
        {
            try
            {
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string areaid = Request.Form["checkbox_areaid"];
                string filenames = Request.Form["txt_filename"];
                string actionType = Request.Form["ActionType"];
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
                int mId = CadDrawingMasterDB.EditHandle(caddrawingmaster,string.Empty);
                mId = Id;
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_Area = areaid.Split(',');
                string[] arr_ActionType = actionType.Split(',');
                string[] arr_FileName = filenames.Split(',');
                CadDrawingDWG dwg = null;
                int index = 0;
                CadDrawingDWGDB.DeleteHandleByParam(string.Format(@" MId={0}",mId));
                foreach (string cad in arr_CADFile)
                {
                    if (!string.IsNullOrEmpty(cad))
                    {
                        dwg = new CadDrawingDWG();
                        dwg.MId = mId;
                        dwg.DWGPath = arr_IMGFile[index];
                        dwg.FileClass = arr_FileName[index];
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

                CadDrawingFunctionDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
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
                int OpenWindowNum = Request.Form["OpenWindowNum"].ConvertToInt32(0);
                int WindowHasCorner = Request.Form["Radio_WindowHasCorner"].ConvertToInt32(0);
                int WindowHasSymmetry = Request.Form["Radio_WindowHasSymmetry"].ConvertToInt32(0);
                decimal WindowSizeMin = Request.Form["txtWindowSizeMin"].ConventToDecimal(0);
                decimal WindowSizeMax = Request.Form["txtWindowSizeMax"].ConventToDecimal(0);
                string WindowDesignFormula = Request.Form["txtWindowDesignFormula"].ConventToString(string.Empty);
                if (DynamicType == 2)
                {
                    WindowSizeMin = Request.Form["txt_Window_Width"].ConventToDecimal(0);
                    WindowSizeMax = Request.Form["txt_Window_Height"].ConventToDecimal(0);
                }
                int WindowVentilationQuantity = Request.Form["txt_WindowVentilationQuantity"].ConvertToInt32(0);
                int WindowPlugslotSize = Request.Form["txtWindowPlugslotSize"].ConvertToInt32(0);
                int WindowAuxiliaryFrame = Request.Form["Checkbox_WindowAuxiliaryFrame"].ConvertToInt32(0);

                CadDrawingWindowDetailDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
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
                int detail = CadDrawingWindowDetailDB.AddHandle(window);
                string param = Request.Form["param"].ConventToString(string.Empty);
                CadDrawingParameterDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
                DataTable tableParam = JsonConvert.DeserializeObject<DataTable>(param);
                if (tableParam != null)
                {
                    foreach (DataRow row in tableParam.Rows)
                    {
                        CadDrawingParameter cadParam = new CadDrawingParameter();
                        cadParam.MId = mId;
                        cadParam.SizeNo = row["SizeNo"].ConventToString(string.Empty);
                        cadParam.ValueType = Decimal.ToInt32(row["ValueType"].ConventToDecimal(0));
                        cadParam.Val = row["Val"].ConventToString(string.Empty);
                        cadParam.MinValue = Decimal.ToInt32(row["MinValue"].ConventToDecimal(0));
                        cadParam.MaxValue = Decimal.ToInt32(row["MaxValue"].ConventToDecimal(0));
                        cadParam.DefaultValue = row["DefaultValue"].ConvertToInt32(-1);
                        cadParam.Desc = row["Desc"].ConventToString(string.Empty);
                        cadParam.CreateOn = DateTime.Now;
                        CadDrawingParameterDB.AddHandle(cadParam);
                    }


                }

                if (mId > 0 && detail > 0)
                {
                    return Json(new { code = 100, message = "外窗修改成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "外窗修改失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -110, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///   外窗CAD原型属性表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/window/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>
        ///  [ValidateInput(false)]
        public ActionResult DeleteHandleById()
        {
            int Id = Request.Form["id"].ConvertToInt32(0);
            if (Id < 1) 
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
        
            string _where = string.Format("  a.MId={0}", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            foreach (CadDrawingDWG dwg in Dwgs) 
            {
                string cadpath = dwg.CADPath;
                string mapCadPath = Server.MapPath(cadpath);
                System.IO.File.Delete(mapCadPath);
                string imgpath = dwg.DWGPath;
                string mapImgPath = Server.MapPath(imgpath);
                System.IO.File.Delete(mapImgPath);
            }
            int rtv = CadDrawingWindowDetailDB.DeleteHandleById(Id);
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