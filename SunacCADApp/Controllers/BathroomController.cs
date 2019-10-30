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
    public class BathroomController : Controller
    {
        public BathroomController()
        {
            ViewBag.SelectModel = 9;
        }
        // GET:  /bathroom/index
        public ActionResult Index()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='ToiletType' And ParentID!=0";
            IList<BasArgumentSetting> ToiletType = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletTypes = ToiletType;

            _where = "TypeCode='BathroomDoorWindowPosition' And ParentID!=0";
            IList<BasArgumentSetting> BathroomDoorWindowPositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.BathroomDoorWindowPositions = BathroomDoorWindowPositions;

            _where = string.Empty;  //查询
            string _orderby = string.Empty;  //排序
            string _url = string.Empty;

            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(0);
            if (area > 0)
            {
                _where += string.Format(@" AND EXISTS(SELECT pa.Id FROM dbo.CadDrawingByArea pa WHERE pa.MId=a.Id AND pa.AreaID={0})", area);
                _url += "&area=" + area;
            }

            ViewBag.area = area;

            int bathroomtype = HttpUtility.UrlDecode(Request.QueryString["bathroomtype"]).ConvertToInt32(0);
            if (bathroomtype > 0)
            {
                _where += string.Format(@" And  b.BathroomType={0}", bathroomtype);
                _url += "&bathroomtype=" + bathroomtype;
            }
            ViewBag.BathroomType = bathroomtype;

            int BathroomDoorWindowPosition = HttpUtility.UrlDecode(Request.QueryString["bathroomdoorwindowposition"]).ConvertToInt32(0);
            if (BathroomDoorWindowPosition > 0)
            {
                _where += string.Format(@" And  b.BathroomDoorWindowPosition={0}", BathroomDoorWindowPosition);
                _url += "&BathroomDoorWindowPosition=" + BathroomDoorWindowPosition;
            }

            ViewBag.BathroomDoorWindowPosition = BathroomDoorWindowPosition;

            int is_airduct = HttpUtility.UrlDecode(Request.QueryString["is_airduct"]).ConvertToInt32(-1);
            if (is_airduct > -1)
            {
                _where += string.Format(@" And  b.BathroomIsAirduct={0}", is_airduct);
                _url += "&is_airduct=" + is_airduct;
            }
            ViewBag.airduct = is_airduct;

            string keyword = HttpUtility.UrlDecode(Request.QueryString["keyword"].ConventToString(string.Empty));
            if (!string.IsNullOrEmpty(keyword))
            {
                _where = string.Format(@" AND  a.DrawingCode like  '%{0}%'", keyword);
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

            IList<CadDrawingWindowSearch> lst = CadDrawingBathroomDetailDB.GetSearchPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingBathroomDetailDB.GetSearchPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageCount = pageCount;
            return View();

        }

        /// <summary>
        /// GET:/Bathroom/LookOver
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

            CadDrawingBathroomDetail bathroom = CadDrawingBathroomDetailDB.GetCadDrawingBathroomDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.Bathroom = bathroom;

            return View();
        }

        /// <summary>
        /// GET:/Bathroom/Add
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;
            //卫生间类型
            _where = "TypeCode='ToiletType' And ParentID!=0";
            IList<BasArgumentSetting> ToiletTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletTypes = ToiletTypes;
            //卫生间门窗位置
            _where = "TypeCode='BathroomDoorWindowPosition' And ParentID!=0";
            IList<BasArgumentSetting> BathroomDoorWindowPositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.BathroomDoorWindowPositions = BathroomDoorWindowPositions;


            //马桶宽度
            _where = "TypeCode='ClosesToolWidth' And ParentID!=0";
            IList<BasArgumentSetting> ClosesToolWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ClosesToolWidths = ClosesToolWidths;
            //卫生间水盆宽度
            _where = "TypeCode='ToiletBasinWidth' And ParentID!=0";
            IList<BasArgumentSetting> ToiletBasinWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletBasinWidths = ToiletBasinWidths;



            return View();
        }


        /// <summary>
        ///   卫生间CAD原型属性表-新增方法
        /// </summary>
        /// <returns></returns>
        /// <get>/Bathroom/addhandle</get>
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
                CadDrawingBathroomDetail bathroom = new CadDrawingBathroomDetail();

                bathroom.MId = mId;
                bathroom.BathroomType = Request.Form["ToiletType"].ConvertToInt32(0);
                bathroom.BathroomDoorWindowPosition = Request.Form["selectBathroomDoorWindowPosition"].ConvertToInt32(0);
                bathroom.BathroomIsAirduct = Request.Form["radio_bathroomisairduct"].ConvertToInt32(0);
                int module = Request.Form["radio_module"].ConvertToInt32(0);
                if (module == 1)
                {
                    bathroom.BathroomShortSideMin = Request.Form["txtBathroomShortSideMin"].ConvertToInt32(0);
                    bathroom.BathroomShortSideMax = Request.Form["txtBathroomShortSideMax"].ConvertToInt32(0);
                    bathroom.BathroomLongSizeMin = Request.Form["txtBathroomLongSizeMin"].ConvertToInt32(0);
                    bathroom.BathroomLongSizeMax = Request.Form["txtBathroomLongSizeMax"].ConvertToInt32(0);
                }
                else if (module == 2)
                {
                    bathroom.BathroomShortSideMin = Request.Form["txtSizeSideMin"].ConvertToInt32(0);
                    bathroom.BathroomShortSideMax = Request.Form["txtSizeSideMax"].ConvertToInt32(0);
                    bathroom.BathroomClosestoolSize = Request.Form["selectClosesToolWidth"].ConvertToInt32(0);
                    bathroom.BathroomBasinSize = Request.Form["selectBathroomBasinSize"].ConvertToInt32(0);
                }

                bathroom.CreateOn = DateTime.Now;
                int detailId = CadDrawingBathroomDetailDB.AddHandle(bathroom);

                if (mId > 0 && detailId > 0)
                {
                    return Json(new { code = 100, message = "卫生间图纸原型添加成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "卫生间图纸原型添加失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///  GET:/Bathroom/Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int Id = 0)
        {
            if (Id < 1)
            {
                return Redirect("/bathroom/index");
            }
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;
            //卫生间类型
            _where = "TypeCode='ToiletType' And ParentID!=0";
            IList<BasArgumentSetting> ToiletTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletTypes = ToiletTypes;
            //卫生间门窗位置
            _where = "TypeCode='BathroomDoorWindowPosition' And ParentID!=0";
            IList<BasArgumentSetting> BathroomDoorWindowPositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.BathroomDoorWindowPositions = BathroomDoorWindowPositions;


            //马桶宽度
            _where = "TypeCode='ClosesToolWidth' And ParentID!=0";
            IList<BasArgumentSetting> ClosesToolWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ClosesToolWidths = ClosesToolWidths;
            //卫生间水盆宽度
            _where = "TypeCode='ToiletBasinWidth' And ParentID!=0";
            IList<BasArgumentSetting> ToiletBasinWidths = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.ToiletBasinWidths = ToiletBasinWidths;


            _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;
            _where = string.Format("  a.MId={0}", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;

            CadDrawingBathroomDetail bathroom = CadDrawingBathroomDetailDB.GetCadDrawingBathroomDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.Bathroom = bathroom;

            return View();
        }


        /// <summary>
        ///   卫生间CAD原型属性表-新增方法
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
                string cadFile = Request.Form["txt_drawingcad"];
                string imgFile = Request.Form["hid_drawing_img"];
                string filenames = Request.Form["txt_filename"];
                string drawingtype = Request.Form["hid_drawing_type"];
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                int Id = Request.Form["Id"].ConvertToInt32(0);
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
                int  mId = Id;
                string[] arr_CADFile = cadFile.Split(',');
                string[] arr_IMGFile = imgFile.Split(',');
                string[] arr_FileName = filenames.Split(',');
                string[] arr_DrawingType = drawingtype.Split(',');

                string _where = string.Format("  MId ={0}", Id);
                CadDrawingDWGDB.DeleteHandleByParam(_where);
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
                CadDrawingByAreaDB.DeleteHandleByParam(_where);
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

                CadDrawingBathroomDetailDB.DeleteHandleByParam(_where);
                CadDrawingBathroomDetail bathroom = new CadDrawingBathroomDetail();

                bathroom.MId = mId;
                bathroom.BathroomType = Request.Form["ToiletType"].ConvertToInt32(0);
                bathroom.BathroomDoorWindowPosition = Request.Form["selectBathroomDoorWindowPosition"].ConvertToInt32(0);
                bathroom.BathroomIsAirduct = Request.Form["radio_bathroomisairduct"].ConvertToInt32(0);
                int module = Request.Form["radio_module"].ConvertToInt32(0);
                if (module == 1)
                {
                    bathroom.BathroomShortSideMin = Request.Form["txtBathroomShortSideMin"].ConvertToInt32(0);
                    bathroom.BathroomShortSideMax = Request.Form["txtBathroomShortSideMax"].ConvertToInt32(0);
                    bathroom.BathroomLongSizeMin = Request.Form["txtBathroomLongSizeMin"].ConvertToInt32(0);
                    bathroom.BathroomLongSizeMax = Request.Form["txtBathroomLongSizeMax"].ConvertToInt32(0);
                }
                else if (module == 2)
                {
                    bathroom.BathroomShortSideMin = Request.Form["txtSizeSideMin"].ConvertToInt32(0);
                    bathroom.BathroomShortSideMax = Request.Form["txtSizeSideMax"].ConvertToInt32(0);
                    bathroom.BathroomClosestoolSize = Request.Form["selectClosesToolWidth"].ConvertToInt32(0);
                    bathroom.BathroomBasinSize = Request.Form["selectBathroomBasinSize"].ConvertToInt32(0);
                }

                bathroom.CreateOn = DateTime.Now;
                int detailId = CadDrawingBathroomDetailDB.AddHandle(bathroom);

                if (mId > 0 && detailId > 0)
                {
                    return Json(new { code = 100, message = "卫生间图纸原型修改成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = -100, message = "卫生间图纸原型修改失败" }, JsonRequestBehavior.AllowGet);
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
            int Id = Request.Form["id"].ConvertToInt32(0);
            int rtv = CadDrawingBathroomDetailDB.DeleteHandleById(Id);
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
        /// <get>/bathroom/CadWirteBPMApproval</get>
        public ActionResult CadWirteBPMApproval()
        {
            try
            {

                int Id = Request.Form["Id"].ConvertToInt32(0);
                int _btid = Request.Form["State"].ConvertToInt32(0);
                string BOID = string.Empty,
                          BTID = "P41",
                          Bsxml = string.Empty;
                if (_btid == 1)
                {
                    BOID = Id.ConventToString(string.Empty);
                    BTID = "P41";

                    BPMDynamicBathroom bathroom = CadDrawingBathroomDetailDB.GetBPMDynamicBathroomByBathroomId(Id);
                    Bsxml = XmlSerializeHelper.XmlSerialize<BPMDynamicBathroom>(bathroom);
                }
                else if (_btid == 2)
                {
                    BOID = Id.ConventToString(string.Empty);
                    BTID = "P42";
                    BPMStaticBathroom bathroom = CadDrawingBathroomDetailDB.GetBPMStaticBathroomByBathroomId(Id);
                    Bsxml = XmlSerializeHelper.XmlSerialize<BPMStaticBathroom>(bathroom);

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