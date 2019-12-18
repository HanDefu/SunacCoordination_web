using System;
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
using SunacCADApp;

namespace SunacCADApp.Controllers
{
    public class WindowController : MyController
    {
        public WindowController()
        {
            ViewBag.SelectModel = 5;

        }
        // GET: /window/index
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = string.Format(@"a.TypeCode='Area' And a.ParentID!=0");
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GeBasArgumentSettingAreaByWhere(_where);
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
                _search_where += string.Format(@" AND EXISTS(SELECT * FROM dbo.CadDrawingByArea pa WHERE  pa.MId=a.Id  AND pa.AreaID={0}  {1})", area, _power_wh);
                _url += "&area=" + area;
            }
            else if (area == -9999)
            {
                _search_where += "  AND Scope=1 ";
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
            switch (bpmstate)
            {
                case 1:
                    _search_where += string.Format(" and  ((a.BillStatus=0  OR a.BillStatus=4 OR a.BillStatus=5 OR a.BillStatus=6) and a.CreateUserId={0})", UserId);
                    _url += "&bpmstate=" + bpmstate;
                    break;
                case 2:
                    _search_where += string.Format(" and  ((a.BillStatus=1  OR a.BillStatus=2) and a.CreateUserId={0})", UserId);
                    _url += "&bpmstate=" + bpmstate;
                    break;
                case 3:
                    _search_where += string.Format(" and   a.BillStatus=3", bpmstate);
                    _url += "&bpmstate=" + bpmstate;
                    break;
                default:
                    _search_where += string.Format(" and  (( a.BillStatus!=3 And a.CreateUserId={0}) OR a.BillStatus=3)", UserId);
                    _url += "&bpmstate=" + bpmstate;
                    break;
            }
            ViewBag.bpmstate = bpmstate.ConvertToTrim();


            _where = _search_where;  //查询

            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword))
            {
                _where = string.Format(@"  (( a.BillStatus!=3 And a.CreateUserId={0}) OR a.BillStatus=3)", UserId);
                _where += string.Format(@" AND a.DrawingCode like  '%{0}%'", keyword);
                ViewBag.bpmstate = "-1";
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
            int SeftUserId  =  master.CreateUserId;
            int BillStatus = master.BillStatus;
            HasUserRole(BillStatus, SeftUserId);


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
            string _where = string.Format(" a.TypeCode='Area' And a.ParentID!=0 {0}", _power_area_where);
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
                string drawingcode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                string _where = string.Format(@"  And DrawingCode='{0}'",drawingcode);
                CadDrawingMaster hasMaster = CadDrawingMasterDB.GetSingleEntityByparam(_where);
                if (hasMaster.Id> 0 && hasMaster.BillStatus==0)
                {
                    string _deletewhere = string.Format("  a.MId={0}", hasMaster.Id);
                    IList<CadDrawingDWG> CadDwgs = CadDrawingDWGDB.GetPageInfoByParameter(_deletewhere, string.Empty, 0, 50);
                    foreach (CadDrawingDWG cadDwg in CadDwgs)
                    {
                        string cadpath = cadDwg.CADPath;
                        string mapCadPath = Server.MapPath(cadpath);
                        System.IO.File.Delete(mapCadPath);
                        string imgpath = cadDwg.DWGPath;
                        string mapImgPath = Server.MapPath(imgpath);
                        System.IO.File.Delete(mapImgPath);
                    }
                    CadDrawingWindowDetailDB.DeleteHandleById(hasMaster.Id);
                }
                else if (hasMaster.Id > 0 && hasMaster.BillStatus != 0) 
                {
                    return Json(new { code = -110, message = "原型已提交审核" }, JsonRequestBehavior.AllowGet);
                }

                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                string areaid = Request.Form["checkbox_areaid"].ConventToString(string.Empty);
                string actionType = Request.Form["ActionType"].ConventToString(string.Empty);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                int Scope = Request.Form["chekbox_group"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = drawingcode;
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
                caddrawingmaster.BillStatus = _OperateBillStatus;
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
                    return CadWindowBPMApproval(mId, DynamicType, 1);
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

            string _where = string.Format(" a.TypeCode='Area' And a.ParentID!=0 {0}", _power_area_where);
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
            int SeftUserId = master.CreateUserId;
            int BillStatus = master.BillStatus;
            HasUserRole(BillStatus, SeftUserId);
        
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
                caddrawingmaster.ModifiedUserId = UserId;
                caddrawingmaster.ModifiedBy = UserName;
                caddrawingmaster.ModifiedOn = DateTime.Now;
                caddrawingmaster.BillStatus = _OperateBillStatus;
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
            string billstatus = Request.Form["billstatus"];
            string bpmprocinstid = Request.Form["bpmprocinstid"];
            string UserCode = UserName;
            UserCode = "zhaoy58";
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

            if (billstatus == "4")
            {
                CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
                string BOID = master.Id.ConventToString(string.Empty);
                string BSID = "vsheji";
                string BTID = master.DynamicType == 1 ? "P11" : "P12";

                BPMOperationCommonLib.CadWindowBPMDoInvalid(UserCode, BOID, BSID, BTID, bpmprocinstid);
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
            int billstatus = Request.Form["billstatus"].ConvertToInt32(0);
            string bpmprocinstid = Request.Form["bpmprocinstid"];
            string bpmjobid = Request.Form["bpmjobid"];
            return CadWindowBPMApproval(Id, _btid, billstatus, bpmprocinstid, bpmjobid);
        }

        public JsonResult CadWindowBPMApproval(int windowId, int statecode, int billstatus = 0, string bpmprocinstid = "", string bpmjobid = "")
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
                    window.FSubject = string.Format(@"动态外窗原型审批-{0}", BOID);
                    Bsxml = XmlSerializeHelper.XmlSerialize<BPMDynamicWindow>(window);
                }
                else if (_btid == 2)
                {
                    BOID = Id.ConventToString(string.Empty);
                    BTID = "P12";
                    BPMStaticWindow window = CadDrawingWindowDetailDB.GetBPMStaticWindowByWindowId(Id);
                    window.FSubject = string.Format(@"定态外窗原型审批-{0}", BOID);
                    Bsxml = XmlSerializeHelper.XmlSerialize<BPMStaticWindow>(window);
                }
                string BSID = API_Common.GetBSID;
                string UserId = UserName;
                int returnValue = -100;
                returnValue = BPMOperationCommonLib.CadWindowBPMWriteSAPXmlToBPM(BSID, BTID, BOID, Bsxml, bpmprocinstid, UserId);
                if (returnValue == 100)
                {
                    return Json(new { code = 110, message = "提交成功", BSID = BSID, BTID = BTID, BOID = BOID }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -110, message = "提交失败", BSID = BSID, BTID = BTID, BOID = BOID }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -120, message = ex.Message, BSID = string.Empty, BTID = string.Empty, BOID = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}