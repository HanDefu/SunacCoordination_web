using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.Library;

namespace SunacCADApp.Controllers
{
    public class KitchenController : Controller
    {
        private int UserId = 0;
        private string UserName = string.Empty;
        public KitchenController()
        {
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            ViewBag.SelectModel = 8;
        }
        // GET: Kitchen
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
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

            _where = string.Empty;  //查询
            string _orderby = string.Empty;  //排序
            string _url = string.Empty;

            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(0);
            if (area > 0)
            {
                _where += string.Format(@" AND EXISTS(SELECT pa.Id FROM dbo.CadDrawingByArea pa WHERE pa.MId=a.Id AND pa.AreaID={0})", area);
                _url += "&area=" + area;
            }
            else if (area == -9999)
            {
                _where += "  AND Scope=1";
                _url += "&area=" + area;
            }
            ViewBag.area = area;

            int kitchentypeid = HttpUtility.UrlDecode(Request.QueryString["kitchentype"]).ConvertToInt32(0);
            if (kitchentypeid > 0)
            {
                _where += string.Format(@" And  b.KitchenType={0}", kitchentypeid);
                _url += "&kitchentypeid=" + kitchentypeid;
            }
            ViewBag.kitchentypeid = kitchentypeid;

            int doorwindowpostionid = HttpUtility.UrlDecode(Request.QueryString["doorwindowpostion"]).ConvertToInt32(0);
            if (doorwindowpostionid > 0)
            {
                _where += string.Format(@" And  b.KitchenPosition={0}", doorwindowpostionid);
                _url += "&doorwindowpostionid=" + kitchentypeid;
            }
            ViewBag.doorwindowpostionid = doorwindowpostionid;

            int is_airduct = HttpUtility.UrlDecode(Request.QueryString["is_airduct"]).ConvertToInt32(-1);
            if (is_airduct > -1)
            {
                _where += string.Format(@" And  b.KitchenIsAirduct={0}", is_airduct);
                _url += "&is_airduct=" + is_airduct;
            }
            ViewBag.airduct = is_airduct;


            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword))
            {
                _where = string.Format(@" AND a.DrawingCode like  '%{0}%'", keyword);
            }
            ViewBag.Keyword = keyword;

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
            if (UserId < 1)
            {
                return Redirect("/home");
            }
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
            if (UserId < 1)
            {
                return Redirect("/home");
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
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string filenames = Request.Form["txt_filename"];
                string drawingtype = Request.Form["hid_drawing_type"];
                string areaid = Request.Form["checkbox_areaid"];
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chk_area"].ConvertToInt32(0);
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
             
                caddrawingmaster.Reorder = 2;
                caddrawingmaster.Enabled = 1;
                caddrawingmaster.CreateUserId = 0;
                caddrawingmaster.CreateBy = "admin";
                caddrawingmaster.CreateOn = DateTime.Now;

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
        ///  GET:/Kitchen/Edit
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
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string filenames = Request.Form["txt_filename"];
                string drawingtype = Request.Form["hid_drawing_type"];
                string areaid = Request.Form["checkbox_areaid"];
                int Id = Request.Form["hid_id"].ConvertToInt32(0);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chk_area"].ConvertToInt32(0);
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
                string[] arr_FileName = filenames.Split(',');
                string[] arr_DrawingType = drawingtype.Split(',');
                CadDrawingDWG dwg = null;
                int index = 0;
                CadDrawingDWGDB.DeleteHandleByParam(string.Format(@" MId={0}",Id));
                foreach (string cad in arr_CADFile)
                {
                    if (!string.IsNullOrEmpty(cad))
                    {
                        dwg = new CadDrawingDWG();
                        dwg.MId = mId;
                        dwg.DWGPath  =  arr_IMGFile[index];
                        dwg.FileClass    =  arr_FileName[index];
                        dwg.CADPath   =  arr_CADFile[index];
                        dwg.CADType   =  arr_DrawingType[index];
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
        ///   厨房CAD原型属性表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/CadDrawingKitchenDetail/deleteHandlebyid</get>
        /// <author>alon<84789887@qq.com></author>  
        public ActionResult DeleteHandleById()
        {
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            int Id = Request.Form["id"].ConvertToInt32(0);
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
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
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

        /// <summary>
        /// 厨房原型提交
        /// </summary>
        /// <returns></returns>
        /// <get>/Door/WriteBPMDoorApproval</get>
        public ActionResult WriteBPMDoorApproval()
        {
            try
            {
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                int kitchenID = Request.Form["Id"].ConvertToInt32(0);
                string BOID = kitchenID.ConventToString("0");
                string State = Request.Form["State"].ConventToString(string.Empty);
                string BTID = "P31";
                WeService.BPM.WriteSAP.I_REQUEST request = new WeService.BPM.WriteSAP.I_REQUEST();
                IList<WeService.BPM.WriteSAP.REQ_ITEM> peq_item = new List<WeService.BPM.WriteSAP.REQ_ITEM>();
                WeService.BPM.WriteSAP.REQ_ITEM item = new WeService.BPM.WriteSAP.REQ_ITEM();
                string BPMXml = string.Empty;
                if (State == "1")
                {

                    BPMDynamicKitchen kitchen = CadDrawingKitchenDetailDB.GetBPMDynamicKitchenById(kitchenID);
                    BPMXml = XmlSerializeHelper.XmlSerialize<BPMDynamicKitchen>(kitchen);
                    BTID = "P31";
                }
                else if (State == "2")
                {
                    BPMStaticKitchen kitchen = CadDrawingKitchenDetailDB.GetBPMStaticKitchenById(kitchenID);
                    BPMXml = XmlSerializeHelper.XmlSerialize<BPMStaticKitchen>(kitchen);
                    BTID = "P32";
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
                    CadDrawingMasterDB.ChangeBpmStateusByMId(kitchenID, 2);
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