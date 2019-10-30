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
    public class AirconditionerController : Controller
    {
        public AirconditionerController() 
        {
            ViewBag.SelectModel = 10;
        }
        // GET: /airconditioner/Index
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
            
            _where = " 1=1";  //查询
            string _orderby = string.Empty;  //排序
            string _url = string.Empty;

            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(0);
            if (area > 0)
            {
                _where += string.Format(@" AND EXISTS(SELECT pa.Id FROM dbo.CadDrawingByArea pa WHERE pa.MId=a.Id AND pa.AreaID={0})", area);
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
                _where += string.Format(@" And  b.AirconditionerRainPipePosition={0}", CondensatePipePosition);
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

            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword))
            {
                _where = string.Format(@" a.DrawingCode like  '%{0}%'", keyword);
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
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string filenames = Request.Form["txt_filename"];
                string drawingtype = Request.Form["hid_drawing_type"];
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
                string areaid = Request.Form["checkbox_areaid"];
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
        public ActionResult Edit(int Id=0)
        {
            if (Id < 1)
            {
                return Redirect("/Bathroom/Index");
            }
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

      
            _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
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
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                int Id = Request.Form["Id"].ConvertToInt32(0);
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string filenames = Request.Form["txt_filename"];
                string drawingtype = Request.Form["hid_drawing_type"];
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
                        CadDrawingDWGDB.AddHandle(dwg);
                        index++;
                    }
                }
                string areaid = Request.Form["checkbox_areaid"];
                string[] arr_Area = areaid.Split(',');
                CadDrawingByAreaDB.DeleteHandleByParam(_where);
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

                CadDrawingAirconditionerDetailDB.DeleteHandleByParam(_where);
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
                    return Json(new { code = 100, message = "空调原型图纸修改成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "空调原型图纸修改失败" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
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

                int Id = Request.Form["Id"].ConvertToInt32(0);
                int _btid = Request.Form["State"].ConvertToInt32(0);
                string BOID = string.Empty,
                          BTID = "P61",
                          Bsxml = string.Empty;
                BOID = Id.ConventToString(string.Empty);
                BTID = "P61";
                BPMAirConditioner ariConditoner = CadDrawingAirconditionerDetailDB.GetBPMAirConditionerById(Id);
                Bsxml = XmlSerializeHelper.XmlSerialize<BPMAirConditioner>(ariConditoner);
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
                request.REQ_BASEINFO = baseInfo;
                request.MESSAGE = peq_item.ToArray<WeService.BPM.WriteSAP.REQ_ITEM>();
                WeService.BPM.WriteSAP.CAD_SUNAC_564_WriteSAPXmlToBPM_pttbindingQSService service = new WeService.BPM.WriteSAP.CAD_SUNAC_564_WriteSAPXmlToBPM_pttbindingQSService();
                WeService.BPM.WriteSAP.E_RESPONSE response = service.CAD_SUNAC_564_WriteSAPXmlToBPM(request);
                WeService.BPM.WriteSAP.E_RESPONSERSP_ITEM Message = response.MESSAGE.First();
                if (Message.STATUSCODE == "1")
                {
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

    
    }
}