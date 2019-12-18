using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Library;

namespace SunacCADApp.Controllers
{
    public class AirconditionerController : MyController
    {

        public AirconditionerController() 
        {
            ViewBag.SelectModel = 10;
        }
        // GET: /airconditioner/Index
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = string.Format(@"a.TypeCode='Area' And a.ParentID!=0 {0}", _power_area_where);
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GeBasArgumentSettingAreaByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;
            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;
            
            _where = " 1=1";  //查询
            string _orderby = string.Empty;  //排序
            string _url = string.Empty;

            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(0);
            if (area > 0)
            {
                _where += string.Format(@" AND EXISTS(SELECT * FROM dbo.CadDrawingByArea pa WHERE  pa.MId=a.Id  AND pa.AreaID={0}  {1})", area, _power_wh);
                _url += "&area=" + area;
            }
            else if (area == -9999)
            {
                _where += "AND Scope=1";
                _url += "&area=" + area;
            }

            ViewBag.area = area;

            int AirConditionNumber = HttpUtility.UrlDecode(Request.QueryString["AirConditionNumber"]).ConvertToInt32(0);
            if (AirConditionNumber > 0)
            {
                _where += string.Format(@" And  b.AirconditionerPower={0}", AirConditionNumber);
                _url += "&AirConditionNumber=" + AirConditionNumber;
            }
            ViewBag.AirConditionNumber = AirConditionNumber;

            int CondensatePipePosition = HttpUtility.UrlDecode(Request.QueryString["CondensatePipePosition"]).ConvertToInt32(0);
            if (CondensatePipePosition > 0)
            {
                _where += string.Format(@" And  b.AirconditionerPipePosition={0}", CondensatePipePosition);
                _url += "&CondensatePipePosition=" + CondensatePipePosition;
            }

            ViewBag.CondensatePipePosition = CondensatePipePosition;

            int IsRainPipe = HttpUtility.UrlDecode(Request.QueryString["IsRainPipe"]).ConvertToInt32(-1);
            if (IsRainPipe > -1)
            {
                _where += string.Format(@" And  b.AirconditionerIsRainPipe={0}", IsRainPipe);
                _url += "&IsRainPipe=" + IsRainPipe;
            }
            ViewBag.IsRainPipe = IsRainPipe;


            string _bpmState = Request.QueryString["bpmstate"];
            int bpmstate = _bpmState == null ? 3 : _bpmState.ConvertToInt32(0);
            switch (bpmstate)
            {
                case 1:
                    _where += string.Format(" and  ((a.BillStatus=0  OR a.BillStatus=4 OR a.BillStatus=5 OR a.BillStatus=6) and a.CreateUserId={0})", UserId);
                    _url += "&bpmstate=" + bpmstate;
                    break;
                case 2:
                    _where += string.Format(" and  ((a.BillStatus=1  OR a.BillStatus=2) and a.CreateUserId={0})", UserId);
                    _url += "&bpmstate=" + bpmstate;
                    break;
                case 3:
                    _where += string.Format(" and   a.BillStatus=3", bpmstate);
                    _url += "&bpmstate=" + bpmstate;
                    break;
                default:
                    _where += string.Format(" and  (( a.BillStatus!=3 And a.CreateUserId={0}) OR a.BillStatus=3)", UserId);
                    _url += "&bpmstate=" + bpmstate;
                    break;
            }
            ViewBag.bpmstate = bpmstate.ConvertToTrim();

            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword))
            {
                _where = string.Format(@"  (( a.BillStatus!=3 And a.CreateUserId={0}) OR a.BillStatus=3)", UserId);
                _where += string.Format(@" AND a.DrawingCode like  '%{0}%'", keyword);
                ViewBag.bpmstate = "-1";
            }
            ViewBag.Keyword = keyword;


            int recordCount = 0;    //记录总数
            int pageSize = 30;      //每页条数
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
        public ActionResult LookOver(int Id = 0)
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            if (Id < 1)
            {
                return Redirect("/Bathroom/Index");
            }
            string _where = string.Empty;
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

            CadDrawingAirconditionerDetail Airconditioner = CadDrawingAirconditionerDetailDB.GetCadDrawingAirconditionerDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.Airconditioner = Airconditioner;

            return View();
        }

        /// <summary>
        /// GET:/Handrail/Add
        /// </summary>
        /// <returns></returns>

        public ActionResult Add()
        {

            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = string.Format(" a.TypeCode='Area' And a.ParentID!=0 {0}", _power_area_where);
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
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string drawingcode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                string _where = string.Format(@"  And DrawingCode='{0}'", drawingcode);
                CadDrawingMaster hasMaster = CadDrawingMasterDB.GetSingleEntityByparam(_where);
                if (hasMaster.Id > 0 && hasMaster.BillStatus == 0)
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
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chk_area"].ConvertToInt32(0);
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
                string areaid = Request.Form["checkbox_areaid"].ConvertToTrim();
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


                int AirConditionNumber = Request.Form["AirConditionNumber"].ConvertToInt32(-1);
                int AirconditionerMinWidth = Request.Form["txtAirconditionerMinWidth"].ConvertToInt32(-1);
                int AirconditionerMinLength = Request.Form["txtAirconditionerMinLength"].ConvertToInt32(-1);
                int CondensatePipePosition = Request.Form["selectCondensatePipePosition"].ConvertToInt32(-1);
                int AirconditionerIsRainPipe = Request.Form["Checkbox_AirconditionerIsRainPipe"].ConvertToInt32(0);
                int RainPipePosition = Request.Form["selectRainPipePosition"].ConvertToInt32(-1);
                int AirconditionerWidth = Request.Form["txtAirconditionerWidth"].ConvertToInt32(0);
                int AirconditionerHeight = Request.Form["txtAirconditionerHeight"].ConvertToInt32(0);
                int AirconditionerDepth = Request.Form["txtAirconditionerDepth"].ConvertToInt32(0);
                CadDrawingAirconditionerDetail airconditioner = new CadDrawingAirconditionerDetail();
                airconditioner.MId = mId;
                airconditioner.AirconditionerIsRainPipe = AirconditionerIsRainPipe;
                airconditioner.AirconditionerMinWidth = AirconditionerMinWidth;
                airconditioner.AirconditionerMinLength = AirconditionerMinLength;
                airconditioner.AirconditionerPower = AirConditionNumber;
                airconditioner.AirconditionerPipePosition = CondensatePipePosition;
                airconditioner.AirconditionerIsRainPipe = AirconditionerIsRainPipe;
                airconditioner.AirconditionerWidth = AirconditionerWidth;
                airconditioner.AirconditionerHeight = AirconditionerHeight;
                airconditioner.AirconditionerDepth = AirconditionerDepth;
                airconditioner.CreateUserId = UserId;
                airconditioner.CreateBy = UserName;
                airconditioner.CreateOn = DateTime.Now;
                airconditioner.ModifiedUserId = UserId;
                airconditioner.ModifiedBy = UserName;
                airconditioner.ModifiedOn = DateTime.Now;
                if (AirconditionerIsRainPipe == 1) 
                {
                    airconditioner.AirconditionerRainPipePosition = RainPipePosition;
                }
                int rtv = CadDrawingAirconditionerDetailDB.AddHandle(airconditioner);
                string operate = Request.Form["hid_operate"].ConventToString(string.Empty);
                if (operate == "commit")
                {
                    int airconditionerid= mId;
                    int statecode = DynamicType;
                    return CadAirconditionerBPMApproval(airconditionerid, statecode,1);
                }
                if (rtv > 0 && mId > 0)
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
        ///  /Handrail/Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int Id=0)
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            if (Id < 1)
            {
                return Redirect("/Bathroom/Index");
            }
            string _where = string.Format(" a.TypeCode='Area' And a.ParentID!=0 {0}", _power_area_where);
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

      
            _where = string.Empty;
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

            CadDrawingAirconditionerDetail Airconditioner = CadDrawingAirconditionerDetailDB.GetCadDrawingAirconditionerDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.Airconditioner = Airconditioner;

            return View();
        }

        /// <summary>
        ///   空调CAD原型属性表-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/Bathroom/addhandle</get>
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
                int Id = Request.Form["Id"].ConvertToInt32(0);
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["hid_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chk_area"].ConvertToInt32(0);
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
                caddrawingmaster.CreateOn = DateTime.Now;
                caddrawingmaster.Reorder = 2;
                caddrawingmaster.Enabled = 1;
                caddrawingmaster.ModifiedUserId = UserId;
                caddrawingmaster.ModifiedBy = UserName;
                caddrawingmaster.ModifiedOn = DateTime.Now;
                caddrawingmaster.BillStatus = _OperateBillStatus;
                caddrawingmaster.Id = Id;
                CadDrawingMasterDB.EditHandle(caddrawingmaster,string.Empty);
                int mId = Id;
                string _where = string.Format(@" MId={0}", mId);
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_FileName = filenames.Split(',');
                string[] arr_DrawingType = drawingtype.Split(',');
                CadDrawingDWG dwg = null;
                int index = 0;
                CadDrawingDWGDB.DeleteHandleByParam(_where);
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
                string areaid = Request.Form["checkbox_areaid"].ConvertToTrim();
                string[] arr_Area = areaid.Split(',');
                CadDrawingByAreaDB.DeleteHandleByParam(_where);
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

                CadDrawingAirconditionerDetailDB.DeleteHandleByParam(_where);
                int AirConditionNumber = Request.Form["AirConditionNumber"].ConvertToInt32(-1);
                int AirconditionerMinWidth = Request.Form["txtAirconditionerMinWidth"].ConvertToInt32(-1);
                int AirconditionerMinLength = Request.Form["txtAirconditionerMinLength"].ConvertToInt32(-1);
                int CondensatePipePosition = Request.Form["selectCondensatePipePosition"].ConvertToInt32(-1);
                int AirconditionerIsRainPipe = Request.Form["Checkbox_AirconditionerIsRainPipe"].ConvertToInt32(0);
                int RainPipePosition = Request.Form["selectRainPipePosition"].ConvertToInt32(-1);
                int AirconditionerWidth = Request.Form["txtAirconditionerWidth"].ConvertToInt32(0);
                int AirconditionerHeight = Request.Form["txtAirconditionerHeight"].ConvertToInt32(0);
                int AirconditionerDepth = Request.Form["txtAirconditionerDepth"].ConvertToInt32(0);
                CadDrawingAirconditionerDetail airconditioner = new CadDrawingAirconditionerDetail();
                airconditioner.MId = mId;
                airconditioner.AirconditionerIsRainPipe = AirconditionerIsRainPipe;
                airconditioner.AirconditionerMinWidth = AirconditionerMinWidth;
                airconditioner.AirconditionerMinLength = AirconditionerMinLength;
                airconditioner.AirconditionerPower = AirConditionNumber;
                airconditioner.AirconditionerPipePosition = CondensatePipePosition;
                airconditioner.AirconditionerIsRainPipe = AirconditionerIsRainPipe;
                airconditioner.AirconditionerWidth = AirconditionerWidth;
                airconditioner.AirconditionerHeight = AirconditionerHeight;
                airconditioner.AirconditionerDepth = AirconditionerDepth;
                airconditioner.CreateUserId = UserId;
                airconditioner.CreateBy = UserName;
                airconditioner.CreateOn = DateTime.Now;
                airconditioner.ModifiedUserId = UserId;
                airconditioner.ModifiedBy = UserName;
                airconditioner.ModifiedOn = DateTime.Now;
                if (AirconditionerIsRainPipe == 1)
                {
                    airconditioner.AirconditionerRainPipePosition = RainPipePosition;
                }
                int rtv = CadDrawingAirconditionerDetailDB.AddHandle(airconditioner);
                if (rtv > 0 && mId > 0)
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
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///   卫生间CAD原型属性表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/CadDrawingBathroomDetail/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            int Id = Request.Form["id"].ConvertToInt32(0);
            string billstatus = Request.Form["billstatus"];
            string bpmprocinstid = Request.Form["bpmprocinstid"];
            string UserCode = UserName;
            UserCode = "zhaoy58";
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

            if (billstatus == "2")
            {
                CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
                string BOID = master.Id.ConventToString(string.Empty);
                string BSID = "vsheji";
                string BTID = "61";
                BPMOperationCommonLib.CadWindowBPMDoInvalid(UserCode, BOID, BSID, BTID, bpmprocinstid);
            }

            int rtv = CadDrawingAirconditionerDetailDB.DeleteHandleById(Id);
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
        /// <get>/airconditioner/CadWirteBPMApproval</get>
        public ActionResult CadWirteBPMApproval()
        {
            try
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
                return CadAirconditionerBPMApproval(Id, _btid, billstatus, bpmprocinstid, bpmjobid);

            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }



        }

        private JsonResult CadAirconditionerBPMApproval(int airconditionerId, int state, int billstatus = 0, string bpmprocinstid = "", string bpmjobid = "") 
        {
            try 
            {
                int Id = airconditionerId;
                int _btid = state;
                string BOID = string.Empty,
                          BTID = "P61",
                          Bsxml = string.Empty;
                BOID = Id.ConventToString(string.Empty);
                BTID = "P61";
                BPMAirConditioner ariConditoner = CadDrawingAirconditionerDetailDB.GetBPMAirConditionerById(Id);
                ariConditoner.FSubject= string.Format(@"空调原型审批-{0}",Id);
                Bsxml = XmlSerializeHelper.XmlSerialize<BPMAirConditioner>(ariConditoner);
                string BSID = API_Common.GetBSID;
                string UserCode = UserName;
                int returnValue = -100;
                returnValue = BPMOperationCommonLib.CadWindowBPMWriteSAPXmlToBPM(BSID, BTID, BOID, Bsxml, bpmprocinstid, UserCode);
              
                if (returnValue == 100)
                {
                    return Json(new { code = 110, message = "提交成功",BSID = BSID, BTID = BTID, BOID = BOID }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -110, message = "提交失败", BSID = BSID, BTID = BTID, BOID = BOID}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) 
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}