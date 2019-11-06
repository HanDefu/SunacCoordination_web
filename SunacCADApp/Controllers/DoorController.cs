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
    public class DoorController : Controller
    {
        public DoorController()
        {
            ViewBag.SelectModel = 6;
        }
        // GET: /Door/Index
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


            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(-1);
            if (area > 0)
            {
                _search_where += string.Format(@"  AND  EXISTS(SELECT 1 FROM dbo.CadDrawingByArea ba WHERE a.Id=ba.MId AND ba.AreaID={0})", area);
                _url += "&area=" + area;
            }
            else if (area == -9999)
            {
                _search_where += "  AND Scope=1";
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
            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword))
            {
                _search_where = string.Format(@" AND a.DrawingCode like  '%{0}%'", keyword);
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
            if (Id < 1)
            {
              return  Redirect("/door/Index");
            }
            string _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
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
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string filenames = Request.Form["txt_filename"];
                string drawingtype  =Request.Form["hid_drawing_type"];
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
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string filenames = Request.Form["txt_filename"];
                string drawingtype = Request.Form["hid_drawing_type"];
                string areaid = Request.Form["checkbox_areaid"];
                int Id = Request.Form["Id"].ConvertToInt32(-1);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
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
                    return Json(new { code = 100, message = "门原型图纸修改成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "门原型图纸修改失败" }, JsonRequestBehavior.AllowGet);

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
            int Id = Request.Form["id"].ConvertToInt32(0);
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
        public ActionResult   WriteBPMDoorApproval()
        {
            try
            {

                int doorID = Request.Form["Id"].ConvertToInt32(0);
                string BOID = doorID.ConventToString("0");
                string State = Request.Form["State"].ConventToString(string.Empty);
                string BTID = "P21";
                WeService.BPM.WriteSAP.I_REQUEST request = new WeService.BPM.WriteSAP.I_REQUEST();
                IList<WeService.BPM.WriteSAP.REQ_ITEM> peq_item = new List<WeService.BPM.WriteSAP.REQ_ITEM>();
                WeService.BPM.WriteSAP.REQ_ITEM item = new WeService.BPM.WriteSAP.REQ_ITEM();
                string BPMXml = string.Empty;
                if (State == "1")
                {
                    BPMDynamicDoor door = CadDrawingDoorDetailDB.GetBPMDynamicDoorById(doorID);
                    BPMXml = XmlSerializeHelper.XmlSerialize<BPMDynamicDoor>(door);
                    BTID = "P21";
                }
                else if (State == "2")
                {
                    BPMStaticDoor door = CadDrawingDoorDetailDB.GetBPMStaticDoorById(doorID);
                    BPMXml = XmlSerializeHelper.XmlSerialize<BPMStaticDoor>(door);
                    BTID = "P22";
                }
                item.BSID = "vsheji";
                item.BTID = BTID;
                item.BOID = BOID;
                item.BSXML = BPMXml;
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
                request.REQ_BASEINFO = baseInfo;
                request.MESSAGE = peq_item.ToArray<WeService.BPM.WriteSAP.REQ_ITEM>();
                WeService.BPM.WriteSAP.CAD_SUNAC_564_WriteSAPXmlToBPM_pttbindingQSService service = new WeService.BPM.WriteSAP.CAD_SUNAC_564_WriteSAPXmlToBPM_pttbindingQSService();
                WeService.BPM.WriteSAP.E_RESPONSE response = service.CAD_SUNAC_564_WriteSAPXmlToBPM(request);
                WeService.BPM.WriteSAP.E_RESPONSERSP_ITEM Message = response.MESSAGE.First();
                if (Message.STATUSCODE == "1")
                {
                    CadDrawingMasterDB.ChangeBpmStateusByMId(doorID, 2);
                    return Json(new { code = 100, message = "BPM提交成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "BPM提交失败" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        

    }
}