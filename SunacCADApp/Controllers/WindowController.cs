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
using SunacCADApp.Library;


namespace SunacCADApp.Controllers
{
    public class WindowController : Controller
    {
        private int UserId = 0;
        private string UserName = string.Empty;
        public WindowController()
        {
            ViewBag.SelectModel = 5;
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            ViewBag.StateList = CommonLib.GetBPMStateInfo();

        }
        // GET: /window/index
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
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
            string _search_where = " 1=1 ";
            string _url = "1";

            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(-1);
            if (area > 0)
            {
                _search_where += string.Format(@" AND EXISTS(SELECT * FROM dbo.CadDrawingByArea pa WHERE pa.MId=a.Id AND pa.AreaID={0})", area);
                _url += "&area=" + area;
            }
            else if (area == -9999)
            {
                _search_where += "  AND Scope=1";
                _url += "&area=" + area;
            }

            ViewBag.area = area;
            int action = HttpUtility.UrlDecode(Request.QueryString["action"]).ConvertToInt32(-1);
            if (action > 0)
            {
                _search_where += string.Format(@" AND EXISTS(SELECT pb.Id FROM dbo.CadDrawingFunction pb WHERE pb.MId=a.Id AND pb.FunctionId={0})", action);
                _url += "&action=" + action;
            }
            ViewBag.action = action;

            int opentype = HttpUtility.UrlDecode(Request.QueryString["opentype"]).ConvertToInt32(-1);
            if (opentype > 0)
            {
                _search_where += " and  b.WindowOpenTypeId='" + opentype + "'";
                _url += "&opentype=" + opentype;
            }
            ViewBag.opentype = opentype;

            int openwindownum = HttpUtility.UrlDecode(Request.QueryString["openwindownum"]).ConvertToInt32(-1);
            if (openwindownum > 0)
            {
                _search_where += " and  b.WindowOpenQtyId='" + openwindownum + "'";
                _url += "&openwindownum=" + openwindownum;
            }
            ViewBag.openwindownum = openwindownum;

            string _bpmState = Request.QueryString["bpmstate"];
            int bpmstate = _bpmState == null ? 3 : _bpmState.ConvertToInt32(0);
            if (bpmstate > 0)
            {
                _search_where += " and   a.BillStatus=" + bpmstate;
                _url += "&bpmstate=" + bpmstate;
            }
            ViewBag.bpmstate = bpmstate.ConvertToTrim();


            _where = _search_where;  //查询

            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword))
            {
                _where = string.Format(@" a.DrawingCode like  '%{0}%'", keyword);
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
        public ActionResult LookOver(int Id = 0)
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }

            if (Id < 1)
            {
                return Redirect("/Window/Index");
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
            _where = string.Format(@" a.MId={0}", Id);
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
            if (UserId < 1)
            {
                return Redirect("/home");
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
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                string areaid = Request.Form["checkbox_areaid"].ConventToString(string.Empty);
                string actionType = Request.Form["ActionType"].ConventToString(string.Empty);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                int Scope  =Request.Form["chekbox_group"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);

                caddrawingmaster.Scope = Scope;
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
                caddrawingmaster.Reorder = 2;
                caddrawingmaster.Enabled = 1;
                caddrawingmaster.CreateUserId = UserId;
                caddrawingmaster.CreateBy = UserName;
                caddrawingmaster.CreateOn = DateTime.Now;
                caddrawingmaster.ModifiedUserId = UserId;
                caddrawingmaster.ModifiedBy = UserName;
                caddrawingmaster.ModifiedOn = DateTime.Now;
                int mId = CadDrawingMasterDB.AddHandle(caddrawingmaster);
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_FileName = filenames.Split(',');
                string[] arr_DrawingType = drawingtype.Split(',');

                CadDrawingDWG dwg = null;
                int index = 0;
                foreach (string cad in arr_CADFile)
                {
                    if (!string.IsNullOrEmpty(cad))
                    {
                        dwg = new CadDrawingDWG();
                        dwg.MId = mId;
                        dwg.DWGPath = arr_IMGFile[index];
                        dwg.CADPath = arr_CADFile[index];
                        dwg.FileClass = arr_FileName[index];
                        dwg.CADType = arr_DrawingType[index];
                        dwg.CreateUserId = UserId;
                        dwg.CreateBy = UserName;
                        dwg.CreateOn = DateTime.Now;
                        CadDrawingDWGDB.AddHandle(dwg);
                        index++;
                    }
                }


                string[] arr_Area = areaid.Split(',');
                foreach (string area in arr_Area)
                {
                    if (!string.IsNullOrEmpty(area))
                    {
                        CadDrawingByArea byArea = new CadDrawingByArea();
                        byArea.AreaID = area.ConvertToInt32(-1);
                        byArea.MId = mId;
                        byArea.CreateUserId = UserId;
                        byArea.CreateBy = UserName;
                        byArea.CreateOn = DateTime.Now;
                        CadDrawingByAreaDB.AddHandle(byArea);
                    }

                }

                string[] arr_ActionType = actionType.Split(',');
                foreach (string action in arr_ActionType)
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        CadDrawingFunction function = new CadDrawingFunction();
                        function.MId = mId;
                        function.FunctionId = action.ConvertToInt32(-1);
                        function.CreateUserId = UserId;
                        function.CreateBy = UserName;
                        function.CreateOn = DateTime.Now;
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
                window.CreateUserId = UserId;
                window.CreateBy = UserName;
                window.CreateOn = DateTime.Now;
                int detail = CadDrawingWindowDetailDB.AddHandle(window);
                string param = Request.Form["param"].ConventToString(string.Empty);
                DataTable tableParam = JsonConvert.DeserializeObject<DataTable>(param);
                if (tableParam != null)
                {
                    foreach (DataRow row in tableParam.Rows)
                    {
                        CadDrawingParameter cadParam = new CadDrawingParameter();
                        cadParam.MId = mId;
                        cadParam.SizeNo = row["SizeNo"].ConventToString(string.Empty);
                        cadParam.ValueType = row["ValueType"].ConvertToInt32(0);
                        cadParam.Val = row["Val"].ConventToString(string.Empty);
                        cadParam.MinValue = row["MinValue"].ConvertToInt32(0);
                        cadParam.MaxValue = row["MaxValue"].ConvertToInt32(0);
                        cadParam.DefaultValue = row["DefaultValue"].ConvertToInt32(0);
                        cadParam.Desc = row["Desc"].ConventToString(string.Empty);
                        cadParam.CreateUserId = UserId;
                        cadParam.CreateBy = UserName;
                        cadParam.CreateOn = DateTime.Now;
                        CadDrawingParameterDB.AddHandle(cadParam);
                    }


                }
                string operate = Request.Form["hid_operate"].ConventToString(string.Empty);
                if (operate == "commit")
                {
                    return CadWindowBPMApproval(mId, DynamicType);
                }
                if (mId > 0 && detail > 0)
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
        public ActionResult Edit(int Id = 0)
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            if (Id < 1)
            {
                return Redirect("/Window/Index");
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
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                string areaid = Request.Form["checkbox_areaid"].ConventToString(string.Empty);
                string actionType = Request.Form["ActionType"].ConventToString(string.Empty);
                int Id = Request.Form["Id"].ConvertToInt32(-1);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["hid_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chekbox_group"].ConvertToInt32(0);
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
                caddrawingmaster.Reorder = 2;
                caddrawingmaster.Enabled = 1;
                caddrawingmaster.CreateUserId = UserId;
                caddrawingmaster.CreateBy = UserName;
                caddrawingmaster.CreateOn = DateTime.Now;
                caddrawingmaster.ModifiedUserId = UserId;
                caddrawingmaster.ModifiedBy = UserName;
                caddrawingmaster.ModifiedOn = DateTime.Now;
                caddrawingmaster.Id = Id;
                int mId = CadDrawingMasterDB.EditHandle(caddrawingmaster, string.Empty);
                mId = Id;
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_FileName = filenames.Split(',');
                string[] arr_DrawingType = drawingtype.Split(',');
                CadDrawingDWG dwg = null;
                int index = 0;
                CadDrawingDWGDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
                foreach (string cad in arr_CADFile)
                {
                    if (!string.IsNullOrEmpty(cad))
                    {
                        dwg = new CadDrawingDWG();
                        dwg.MId = mId;
                        dwg.DWGPath = arr_IMGFile[index];
                        dwg.CADPath = arr_CADFile[index];
                        dwg.FileClass = arr_FileName[index];
                        dwg.CADType = arr_DrawingType[index];
                        dwg.CreateUserId = UserId;
                        dwg.CreateBy = UserName;
                        dwg.CreateOn = DateTime.Now;
                        CadDrawingDWGDB.AddHandle(dwg);
                        index++;
                    }
                }

                CadDrawingByAreaDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
                string[] arr_Area = areaid.Split(',');
                foreach (string area in arr_Area)
                {
                    if (!string.IsNullOrEmpty(area))
                    {
                        CadDrawingByArea byArea = new CadDrawingByArea();
                        byArea.AreaID = area.ConvertToInt32(-1);
                        byArea.MId = mId;
                        byArea.CreateUserId = UserId;
                        byArea.CreateBy = UserName;
                        byArea.CreateOn = DateTime.Now;
                        CadDrawingByAreaDB.AddHandle(byArea);
                    }

                }

                string[] arr_ActionType = actionType.Split(',');
                CadDrawingFunctionDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
                foreach (string action in arr_ActionType)
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        CadDrawingFunction function = new CadDrawingFunction();
                        function.MId = mId;
                        function.FunctionId = action.ConvertToInt32(-1);
                        function.CreateUserId = UserId;
                        function.CreateBy = UserName;
                        function.CreateOn = DateTime.Now;
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
                window.CreateUserId = UserId;
                window.CreateBy = UserName;
                window.CreateOn = DateTime.Now;
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
                        cadParam.ValueType = row["ValueType"].ConvertToInt32(0);
                        cadParam.Val = row["Val"].ConventToString(string.Empty);
                        cadParam.MinValue = row["MinValue"].ConvertToInt32(0);
                        cadParam.MaxValue = row["MaxValue"].ConvertToInt32(0);
                        cadParam.DefaultValue = row["DefaultValue"].ConvertToInt32(0);
                        cadParam.Desc = row["Desc"].ConventToString(string.Empty);
                        cadParam.CreateUserId = UserId;
                        cadParam.CreateBy = UserName;
                        cadParam.CreateOn = DateTime.Now;
                        CadDrawingParameterDB.AddHandle(cadParam);
                    }
                }

                if (mId > 0 && detail > 0)
                {
                    return Json(new { code = 100, message = "修改成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "修改失败" }, JsonRequestBehavior.AllowGet);
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

        /// <summary>
        ///  BPM提交审核
        /// </summary>
        /// <param name="Id">原型ID</param>
        /// <param name="status">1、动态  2、静态</param>
        /// <returns></returns>
        /// <get>/window/CadWirteBPMApproval</get>
        public ActionResult CadWirteBPMApproval()
        {
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                int Id = Request.Form["Id"].ConvertToInt32(0);
                int _btid = Request.Form["State"].ConvertToInt32(0);
                return CadWindowBPMApproval(Id, _btid);
        }

        public JsonResult CadWindowBPMApproval(int windowId, int statecode)
        {
            try
            {
                int Id = windowId;
                int _btid = statecode;
                string BOID = string.Empty,
                          BTID = "P11",
                          Bsxml = string.Empty;
                if (_btid == 1)
                {
                    BOID = Id.ConventToString(string.Empty);
                    BTID = "P11";
                    BPMDynamicWindow window = CadDrawingWindowDetailDB.GetBPMDynamicWindowByWhere(Id);
                    Bsxml = XmlSerializeHelper.XmlSerialize<BPMDynamicWindow>(window);
                }
                else if (_btid == 2)
                {
                    BOID = Id.ConventToString(string.Empty);
                    BTID = "P12";
                    BPMStaticWindow window = CadDrawingWindowDetailDB.GetBPMStaticWindowByWindowId(Id);
                    Bsxml = XmlSerializeHelper.XmlSerialize<BPMStaticWindow>(window);
                }
                WeService.BPM.WriteSAP.I_REQUEST request = new WeService.BPM.WriteSAP.I_REQUEST();
                IList<WeService.BPM.WriteSAP.REQ_ITEM> peq_item = new List<WeService.BPM.WriteSAP.REQ_ITEM>();
                WeService.BPM.WriteSAP.REQ_ITEM item = new WeService.BPM.WriteSAP.REQ_ITEM();
                item.BSID = "vsheji";
                item.BTID = BTID;
                item.BOID = BOID;
                item.BSXML = Bsxml;
                item.procInstID = "0";
                item.userid = "zhaoy58";
                peq_item.Add(item);
                WeService.BPM.WriteSAP.REQ_BASEINFO baseInfo = new WeService.BPM.WriteSAP.REQ_BASEINFO();
                baseInfo.REQ_TRACE_ID = API_Common.UUID;
                baseInfo.REQ_SEND_TIME = API_Common.SEND_DATETIME;
                baseInfo.REQ_SRC_SYS = "BS_CAD_BPM";
                baseInfo.REQ_TAR_SYS = "BS_CAD_BPM";
                baseInfo.REQ_SERVER_NAME = "CAD_SUNAC_564_WriteSAPXmlToBPM";
                baseInfo.REQ_SYN_FLAG = "0";
                baseInfo.BIZTRANSACTIONID = API_Common.BIZTRANSACTIONID;
                request.REQ_BASEINFO = baseInfo;
                request.MESSAGE = peq_item.ToArray<WeService.BPM.WriteSAP.REQ_ITEM>();
                
                WeService.BPM.WriteSAP.CAD_SUNAC_564_WriteSAPXmlToBPM_pttbindingQSService service = new WeService.BPM.WriteSAP.CAD_SUNAC_564_WriteSAPXmlToBPM_pttbindingQSService();
                WeService.BPM.WriteSAP.E_RESPONSE response = service.CAD_SUNAC_564_WriteSAPXmlToBPM(request);
                 
               WeService.BPM.WriteSAP.E_RESPONSERSP_ITEM Message = response.MESSAGE.First();
               
                if (Message.STATUSCODE == "1")
                {
                    string ParamInfo = string.Format(@"BSID      = {0}||BTID  = {1}||BOID   = {2}||BSXML    = {3}||
                                                                            REQ_TRACE_ID   = {4}||REQ_SEND_TIME = {5}||BIZTRANSACTIONID ={6}
                                                                            ", item.BSID, item.BTID, item.BOID, item.BSXML,
                                                                     baseInfo.REQ_TRACE_ID, baseInfo.REQ_SEND_TIME, baseInfo.BIZTRANSACTIONID);
                    string ReturnInfo = string.Format(@"STATUSCODE={0}||STATUSMESSAGE={1}", Message.STATUSCODE, Message.STATUSMESSAGE);
                    CadDrawingMasterDB.Insert_BPM_Commit_Log(item.BTID, item.BSID, "门流程提交", ParamInfo, ReturnInfo);
                    CadDrawingMasterDB.ChangeBpmStateusByMId(Id, 2);
                    return Json(new { code = 100, message = "提交成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "提交失败" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CadWindowBPMUpdateAndApproveFlow(int windowId, int statecode) 
        {
            WebService.UpdataAndApproveFlow.I_REQUEST request = new WebService.UpdataAndApproveFlow.I_REQUEST();
            IList<WebService.UpdataAndApproveFlow.REQ_ITEM> peq_item = new List<WebService.UpdataAndApproveFlow.REQ_ITEM>();
            WebService.UpdataAndApproveFlow.REQ_ITEM item = new WebService.UpdataAndApproveFlow.REQ_ITEM();
            item.userid = "zhaoy58";
            item.jobid = "";
            item.procInstId = "";
            item.comments = "";
            item.xmldata = "";
            peq_item.Add(item);
            WebService.UpdataAndApproveFlow.REQ_BASEINFO baseInfo = new WebService.UpdataAndApproveFlow.REQ_BASEINFO();
            baseInfo.REQ_TRACE_ID = API_Common.UUID;
            baseInfo.REQ_SEND_TIME = API_Common.SEND_DATETIME;
            baseInfo.REQ_SRC_SYS = "BS_CAD_BPM";
            baseInfo.REQ_TAR_SYS = "BS_CAD_BPM";
            baseInfo.REQ_SERVER_NAME = "CAD_SUNAC_564_WriteSAPXmlToBPM";
            baseInfo.REQ_SYN_FLAG = "0";
            baseInfo.BIZTRANSACTIONID = API_Common.BIZTRANSACTIONID;
            request.REQ_BASEINFO = baseInfo;
            request.MESSAGE = peq_item.ToArray<WebService.UpdataAndApproveFlow.REQ_ITEM>();
            WebService.UpdataAndApproveFlow.CAD_SUNAC_565_UpdateAndApproveFlow_pttbindingQSService service = new WebService.UpdataAndApproveFlow.CAD_SUNAC_565_UpdateAndApproveFlow_pttbindingQSService();
            WebService.UpdataAndApproveFlow.E_RESPONSE response = service.CAD_SUNAC_565_UpdateAndApproveFlow(request);
            return Json(new { code = -100, message = "提交失败" }, JsonRequestBehavior.AllowGet);
        }
    }
}