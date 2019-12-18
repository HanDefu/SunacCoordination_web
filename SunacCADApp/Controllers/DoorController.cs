using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using Newtonsoft.Json;
using System.Data;
using SunacCADApp.Library;

namespace SunacCADApp.Controllers
{
    public class DoorController : MyController
    {

        public DoorController()
        {
            ViewBag.SelectModel = 6;
        }
        // GET: /Door/Index
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = string.Format(@"a.TypeCode='Area' And a.ParentID!=0 {0}", _power_area_where);
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GeBasArgumentSettingAreaByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;
            string _search_where = "";
            string _url = "1";


            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(-1);
            if (area > 0)
            {
                _search_where += string.Format(@" AND EXISTS(SELECT * FROM dbo.CadDrawingByArea pa WHERE  pa.MId=a.Id  AND pa.AreaID={0}  {1})", area, _power_wh);
                _url += "&area=" + area;
            }
            else if (area == -9999)
            {
                _search_where += "AND Scope=1";
                _url += "&area=" + area;
            }
            ViewBag.area = area;

            int doortype = HttpUtility.UrlDecode(Request.QueryString["doortype"]).ConvertToInt32(-1);
            if (doortype > 0)
            {
                _search_where += string.Format(@"  AND b.DoorType={0}", doortype);
                _url += "&doortype=" + doortype;
            }
            ViewBag.doortype = doortype;

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

            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword))
            {
                _search_where = string.Format(@" AND  (( a.BillStatus!=3 And a.CreateUserId={0}) OR a.BillStatus=3)", UserId);
                _search_where += string.Format(@" AND a.DrawingCode like  '%{0}%'", keyword);
                ViewBag.bpmstate = "-1";
            }
            ViewBag.Keyword = keyword;
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
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            if (Id < 1)
            {
              return  Redirect("/door/Index");
            }
            string _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;

            int SeftUserId = master.CreateUserId;
            int BillStatus = master.BillStatus;
            HasUserRole(BillStatus, SeftUserId);
            _where = string.Format(@"  a.MId={0}",Id);
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;

            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;

            CadDrawingDoorDetail door = CadDrawingDoorDetailDB.GetSingleEntityByparam(_where);
            ViewBag.Door = door;

            IList<CadDrawingParameter> cadDrawingParams = CadDrawingParameterDB.GetCadDrawingParameterByWhereList(_where);
            ViewBag.CadDrawingParams = cadDrawingParams;


            return View();
        }

        public ActionResult Add()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;

            IList<DataSourceMember> Members = CommonLib.GetWindowArgument();
            ViewBag.Members = Members;


            return View();
        }

        public ActionResult Addhandle() 
        {
            try
            {
                if (UserId < 1)
                {
                    return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string drawingcode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                string hasDrawingCode = CadDrawingMasterDB.HasDrawingCode(drawingcode);
                if (!string.IsNullOrEmpty(hasDrawingCode))
                {
                    return Json(new { code = -110, message = "原型已增加" }, JsonRequestBehavior.AllowGet);
                }
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                string areaid = Request.Form["checkbox_areaid"].ConventToString(string.Empty);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chekbox_group"].ConvertToInt32(0);
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
                caddrawingmaster.CreateOn = DateTime.Now;
                caddrawingmaster.Reorder = 2;
                caddrawingmaster.Enabled = 1;
                caddrawingmaster.CreateUserId = UserId;
                caddrawingmaster.CreateBy =UserName;
                caddrawingmaster.CreateOn = DateTime.Now;
                caddrawingmaster.ModifiedBy = UserName;
                caddrawingmaster.ModifiedUserId = UserId;
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
                        dwg.DWGPath  = arr_IMGFile[index];
                        dwg.CADPath   = arr_CADFile[index];
                        dwg.FileClass    = arr_FileName[index];
                        dwg.CADType   = arr_DrawingType[index];
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
                        CadDrawingByAreaDB.AddHandle(byArea);
                    }
                }

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
                        cadParam.CreateOn = DateTime.Now;
                        CadDrawingParameterDB.AddHandle(cadParam);
                    }
                }
                int DoorType = Request.Form["selectDoorType"].ConvertToInt32(0);
                int WindowSizeMin = 0;
                int WindowSizeMax = 0;
                string windowDesignFormula = Request.Form["txtWindowDesignFormula"].ConventToString(string.Empty);
                if (DynamicType == 1) 
                {
                    WindowSizeMin = Request.Form["txtWindowSizeMin"].ConvertToInt32(0);
                    WindowSizeMax = Request.Form["txtWindowSizeMax"].ConvertToInt32(0);
                }
                else if (DynamicType == 2) 
                {
                    WindowSizeMin = Request.Form["txtSizeMin"].ConvertToInt32(0);
                    WindowSizeMax = Request.Form["txtSizeMax"].ConvertToInt32(0);
                }
                CadDrawingDoorDetail door = new CadDrawingDoorDetail();
                door.MId = mId;
                door.DoorType = DoorType;
                door.WindowSizeMin = WindowSizeMin;
                door.WindowSizeMax = WindowSizeMax;
                door.WindowDesignFormula = windowDesignFormula;
                door.CreateOn = DateTime.Now;
                door.ModifiedOn = DateTime.Now;
                int rtv=  CadDrawingDoorDetailDB.AddHandle(door);
          

                string operate = Request.Form["hid_operate"].ConventToString(string.Empty);
                if (operate == "commit")
                {
                    int doorID = mId;
                    string statecode = DynamicType.ConventToString(string.Empty);

                    return BPMDoorApproval(doorID, statecode,1);
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
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            if (Id < 1)
            {
                return Redirect("/door/Index");
            }
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;

            IList<DataSourceMember> Members = CommonLib.GetWindowArgument();
            ViewBag.Members = Members;

            _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            int SeftUserId = master.CreateUserId;
            int BillStatus = master.BillStatus;
            HasUserRole(BillStatus, SeftUserId);
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.byAreaList = ByAreas;
            _where = string.Format("  a.MId={0} ", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;

            _where = string.Format("  a.MId={0} ", Id);
            CadDrawingDoorDetail door = CadDrawingDoorDetailDB.GetSingleEntityByparam(_where);

            IList<CadDrawingParameter> cadDrawingParams = CadDrawingParameterDB.GetCadDrawingParameterByWhereList(_where);
            ViewBag.CadDrawingParams = cadDrawingParams;


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
                if (UserId < 1)
                {
                    return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                string areaid = Request.Form["checkbox_areaid"].ConventToString(string.Empty);
                int Id = Request.Form["Id"].ConvertToInt32(-1);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["hid_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chekbox_group"].ConvertToInt32(0);
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
                int mId = CadDrawingMasterDB.EditHandle(caddrawingmaster, string.Format(@" And id={0}",Id));
                mId = Id;
                CadDrawingDWG dwg = null;
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_FileName = filenames.Split(',');
                string[] arr_DrawingType = drawingtype.Split(',');
                int index = 0;
                CadDrawingDWGDB.DeleteHandleByParam(string.Format(@" MId={0}", Id));
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
                        CadDrawingDWGDB.AddHandle(dwg);
                        index++;
                    }
                }

                CadDrawingByAreaDB.DeleteHandleByParam(string.Format(@" MId={0}", Id));
                string[] arr_Area = areaid.Split(',');
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
                int DoorType = Request.Form["selectDoorType"].ConvertToInt32(0);
                int WindowSizeMin = 0;
                int WindowSizeMax = 0;
                if (DynamicType == 1)
                {
                    WindowSizeMin = Request.Form["txtWindowSizeMin"].ConvertToInt32(0);
                    WindowSizeMax = Request.Form["txtWindowSizeMax"].ConvertToInt32(0);
                }
                else if (DynamicType == 2)
                {
                    WindowSizeMin = Request.Form["txtSizeMin"].ConvertToInt32(0);
                    WindowSizeMax = Request.Form["txtSizeMax"].ConvertToInt32(0);
                }

                string windowDesignFormula = Request.Form["txtWindowDesignFormula"];
                CadDrawingDoorDetail door = new CadDrawingDoorDetail();
                door.MId = mId;
                door.DoorType = DoorType;
                door.WindowSizeMin = WindowSizeMin;
                door.WindowSizeMax = WindowSizeMax;
                door.WindowDesignFormula = windowDesignFormula;
                door.ModifiedOn = DateTime.Now;

                int rtv = CadDrawingDoorDetailDB.AddHandle(door);
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
                        cadParam.CreateOn = DateTime.Now;
                        CadDrawingParameterDB.AddHandle(cadParam);
                    }
                }

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
            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            if (UserId < 1)
            {
                return Redirect("/home");
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
                string BTID = master.DynamicType == 1 ? "P21" : "P22";

                BPMOperationCommonLib.CadWindowBPMDoInvalid(UserCode, BOID, BSID, BTID, bpmprocinstid);
            }
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
        ///   门原型属性表-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>caddrawingdoordetail/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
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


        /// <summary>
        /// 门原型提交
        /// </summary>
        /// <returns></returns>
        /// <get>/Door/WriteBPMDoorApproval</get>
        public ActionResult WriteBPMDoorApproval()
        {

            if (UserId < 1)
            {
                return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            int doorID = Request.Form["Id"].ConvertToInt32(0);
            string statecode = Request.Form["State"].ConvertToTrim();
            int billstatus = Request.Form["billstatus"].ConvertToInt32(0);
            string bpmprocinstid = Request.Form["bpmprocinstid"];
            string bpmjobid = Request.Form["bpmjobid"];
            return BPMDoorApproval(doorID, statecode, billstatus, bpmprocinstid, bpmjobid);

        }


        private JsonResult BPMDoorApproval(int doorID, string state, int billstatus = 0, string bpmprocinstid="",string bpmjobid="") 
        {
            try
            {
                if (UserId < 1)
                {
                    return Json(new { code = 100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }

                string BOID = doorID.ConventToString("0");
                string State = state;
                string BTID = "P21";
                WeService.BPM.WriteSAP.I_REQUEST request = new WeService.BPM.WriteSAP.I_REQUEST();
                IList<WeService.BPM.WriteSAP.REQ_ITEM> peq_item = new List<WeService.BPM.WriteSAP.REQ_ITEM>();
                WeService.BPM.WriteSAP.REQ_ITEM item = new WeService.BPM.WriteSAP.REQ_ITEM();
                string BPMXml = string.Empty;
                if (State == "1")
                {
                    BPMDynamicDoor door = CadDrawingDoorDetailDB.GetBPMDynamicDoorById(doorID);
                    door.FSubject = string.Format(@"动态门原型审批-{0}", doorID);
                    BPMXml = XmlSerializeHelper.XmlSerialize<BPMDynamicDoor>(door);
                    BTID = "P21";
                }
                else if (State == "2")
                {
                    BPMStaticDoor door = CadDrawingDoorDetailDB.GetBPMStaticDoorById(doorID);
                    door.FSubject = string.Format(@"定态门原型审批-{0}", doorID);
                    BPMXml = XmlSerializeHelper.XmlSerialize<BPMStaticDoor>(door);
                    BTID = "P22";
                }
                string BSID = API_Common.GetBSID;
                string UserCode = UserName;
                int returnValue = -100;
                returnValue = BPMOperationCommonLib.CadWindowBPMWriteSAPXmlToBPM(BSID, BTID, BOID, BPMXml, bpmprocinstid, UserCode);
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
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}