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
    public class BathroomController : MyController
    {
        public BathroomController()
        {
            ViewBag.SelectModel = 9;
        }
        // GET:  /bathroom/index
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = string.Format(@"a.TypeCode='Area' And a.ParentID!=0 {0}", _power_area_where);
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GeBasArgumentSettingAreaByWhere(_where);
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
                _where += string.Format(@" AND EXISTS(SELECT * FROM dbo.CadDrawingByArea pa WHERE  pa.MId=a.Id  AND pa.AreaID={0}  {1})", area, _power_wh);
                _url += "&area=" + area;
            }
            else if (area == -9999)
            {
                _where += string.Format(@"  AND Scope=1 AND EXISTS(SELECT * FROM dbo.CadDrawingByArea pa WHERE  pa.MId=a.Id  {0})", _power_wh);
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
                _where = string.Format(@" AND  (( a.BillStatus!=3 And a.CreateUserId={0}) OR a.BillStatus=3)", UserId);
                _where += string.Format(@" AND a.DrawingCode like  '%{0}%'", keyword);
                ViewBag.bpmstate = "-1";
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
            if (UserId < 1)
            {
                return Redirect("/home");
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
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(1);
                caddrawingmaster.DrawingCode = Request.Form["txt_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chk_area"].ConvertToInt32(0);
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
                caddrawingmaster.Reorder = 2;
                caddrawingmaster.Enabled = 1;
                caddrawingmaster.CreateUserId =UserId;
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

                bathroom.CreateUserId = UserId;
                bathroom.CreateBy = UserName;
                bathroom.CreateOn = DateTime.Now;
                int detailId = CadDrawingBathroomDetailDB.AddHandle(bathroom);
                string operate = Request.Form["hid_operate"].ConventToString(string.Empty);
                if (operate == "commit")
                {
                    int bathroomId = mId;
                    int statecode = DynamicType;
                    return CadBathroomBPMApproval(bathroomId, statecode,1);
                }
             

                if (mId > 0 && detailId > 0)
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
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///  GET:/Bathroom/Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int Id = 0)
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
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

            int SeftUserId = master.CreateUserId;
            int BillStatus = master.BillStatus;
            HasUserRole(BillStatus, SeftUserId);

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
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
                string cadFile = Request.Form["txt_drawingcad"].ConventToString(string.Empty);
                string imgFile = Request.Form["hid_drawing_img"].ConventToString(string.Empty);
                string filenames = Request.Form["txt_filename"].ConventToString(string.Empty);
                string drawingtype = Request.Form["hid_drawing_type"].ConventToString(string.Empty);
                int DynamicType = Request.Form["radio_module"].ConvertToInt32(0);
                int Id = Request.Form["Id"].ConvertToInt32(0);
                caddrawingmaster.DrawingCode = Request.Form["hid_drawingcode"].ConventToString(string.Empty);
                caddrawingmaster.DrawingName = Request.Form["txt_drawingname"].ConventToString(string.Empty);
                caddrawingmaster.Scope = Request.Form["chk_area"].ConvertToInt32(0);
                caddrawingmaster.AreaId = 0;
                caddrawingmaster.DynamicType = DynamicType;
                caddrawingmaster.Reorder = 2;
                caddrawingmaster.Enabled = 1;
                caddrawingmaster.ModifiedUserId = UserId;
                caddrawingmaster.ModifiedBy = UserName;
                caddrawingmaster.ModifiedOn = DateTime.Now;
                caddrawingmaster.BillStatus = _OperateBillStatus;
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
                        dwg.CreateUserId = UserId;
                        dwg.CreateBy = UserName;
                        dwg.CreateOn = DateTime.Now;
                        CadDrawingDWGDB.AddHandle(dwg);
                        index++;
                    }
                }
                string areaid = Request.Form["checkbox_areaid"].ConvertToTrim();
                CadDrawingByAreaDB.DeleteHandleByParam(_where);
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
                bathroom.CreateUserId = UserId;
                bathroom.CreateBy = UserName;
                bathroom.CreateOn = DateTime.Now;
                int detailId = CadDrawingBathroomDetailDB.AddHandle(bathroom);

                if (mId > 0 && detailId > 0)
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
                string BTID = master.DynamicType == 1 ? "P31" : "P32";

                BPMOperationCommonLib.CadWindowBPMDoInvalid(UserCode, BOID, BSID, BTID, bpmprocinstid);
            }
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
                if (UserId < 1)
                {
                    return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
                }
                int Id = Request.Form["Id"].ConvertToInt32(0);
                int _btid = Request.Form["State"].ConvertToInt32(0);
                int billstatus = Request.Form["billstatus"].ConvertToInt32(0);
                string bpmprocinstid = Request.Form["bpmprocinstid"];
                string bpmjobid = Request.Form["bpmjobid"];
                return CadBathroomBPMApproval(Id, _btid, billstatus, bpmprocinstid, bpmjobid);
            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CadBathroomBPMApproval(int bathroomId, int state, int billstatus = 0, string bpmprocinstid = "", string bpmjobid = "") 
        {
            try 
            {
                int Id = bathroomId;
                int _btid = state;
                string BOID = string.Empty,
                          BTID = "P41",
                          Bsxml = string.Empty;
                if (_btid == 1)
                {
                    BOID = Id.ConventToString(string.Empty);
                    BTID = "P41";

                    BPMDynamicBathroom bathroom = CadDrawingBathroomDetailDB.GetBPMDynamicBathroomByBathroomId(Id);
                    bathroom.FSubject = string.Format(@"动态卫生间原型审批-{0}", Id);
                    Bsxml = XmlSerializeHelper.XmlSerialize<BPMDynamicBathroom>(bathroom);
                }
                else if (_btid == 2)
                {
                    BOID = Id.ConventToString(string.Empty);
                    BTID = "P42";
                    BPMStaticBathroom bathroom = CadDrawingBathroomDetailDB.GetBPMStaticBathroomByBathroomId(Id);
                    bathroom.FSubject = string.Format(@"定态卫生间原型审批-{0}", Id);
                    Bsxml = XmlSerializeHelper.XmlSerialize<BPMStaticBathroom>(bathroom);

                }
                string BSID = "vsheji";
                string UserCode = UserName;
                int returnValue = -100;
                returnValue = BPMOperationCommonLib.CadWindowBPMWriteSAPXmlToBPM(BSID, BTID, BOID, Bsxml, bpmprocinstid, UserCode);
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