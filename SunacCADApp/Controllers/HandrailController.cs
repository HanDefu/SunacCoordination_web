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
    public class HandrailController : MyController
    {
        public HandrailController() 
        {
            ViewBag.SelectModel = 7;
        }
        // GET: Handrail
        /// <summary>
        ///  Handrail/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            string _where = string.Format(@"a.TypeCode='Area' And a.ParentID!=0 {0}", _power_area_where);
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GeBasArgumentSettingAreaByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='HandRail' And ParentID!=0";
            IList<BasArgumentSetting> HandRails = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HandRails = HandRails;


            _where = string.Empty;  //查询
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
            ViewBag.Area = area;

            int handrailType = HttpUtility.UrlDecode(Request.QueryString["handrailtype"]).ConvertToInt32(0);
            if (handrailType >1)
            {
                _where += string.Format(@"  AND b.HandrailType={0}", handrailType);
                _url += "&handrailtype=" + handrailType;
            }
            ViewBag.HandrailType = handrailType;


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
                _where = string.Format(@" AND a.DrawingCode like  '%{0}%'", keyword);
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

            IList<CadDrawingWindowSearch> lst = CadDrawingHandrailDetailDB.GetSearchPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingHandrailDetailDB.GetSearchPageCountByParameter(_where);
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
        public ActionResult LookOver(int Id=0)
        {
            if (UserId < 1)
            {
                return Redirect("/home");
            }
            if (Id < 1) 
            {
                return Redirect("/handrail/index");
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
            CadDrawingHandrailDetail handrail = CadDrawingHandrailDetailDB.GetCadDrawingHandrailDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.handrail = handrail;
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
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='HandRail' And ParentID!=0";
            IList<BasArgumentSetting> HandRails = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HandRails = HandRails;


            return View();
        }

         /// <summary>
       ///   栏杆CAD原型属性表-新增方法
       /// </summary>
       /// <returns></returns>
        /// <get>/Handrail/addhandle</get>
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
                caddrawingmaster.BillStatus = 0;
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
                string areaid = Request.Form["checkbox_areaid"].ConventToString(string.Empty);
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
                CadDrawingHandrailDetail handrail = new CadDrawingHandrailDetail();
                handrail.MId= mId;
                handrail.HandrailType = Request.Form["HandRailType"].ConvertToInt32(0);
                handrail.CreateUserId = UserId;
                handrail.CreateBy = UserName;
                handrail.CreateOn = DateTime.Now;
                handrail.ModifiedUserId = UserId;
                handrail.ModifiedBy = UserName;
                handrail.ModifiedOn = DateTime.Now;
                int detailId = CadDrawingHandrailDetailDB.AddHandle(handrail);
                string operate = Request.Form["hid_operate"].ConventToString(string.Empty);
                if (operate == "commit")
                {
                    return CadBpmHandrailApproval(mId,1);
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
                return Redirect("/handrail/index");
            }
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='HandRail' And ParentID!=0";
            IList<BasArgumentSetting> HandRails = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HandRails = HandRails;


            _where = string.Empty;
            CadDrawingMaster master = CadDrawingMasterDB.GetSingleEntityById(Id);
            ViewBag.CadDrawingMaster = master;
            _where = "  a.MId=" + Id;
            IList<CadDrawingByArea> ByAreas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            ViewBag.ByAreas = ByAreas;

            _where = string.Format("  a.MId={0}", Id);
            IList<CadDrawingDWG> Dwgs = CadDrawingDWGDB.GetPageInfoByParameter(_where, string.Empty, 0, 50);
            ViewBag.Dwgs = Dwgs;

            CadDrawingHandrailDetail HandrailDetail = CadDrawingHandrailDetailDB.GetCadDrawingHandrailDetailByWhere(string.Format(@" MId={0}", Id));
            ViewBag.HandrailDetail = HandrailDetail;
            return View();
        }

        /// <summary>
        ///   栏杆CAD原型属性表-修改方法
        /// </summary>
        /// <returns></returns>
        /// <get>/handrail/edithandle</get> 
        /// <author>alon<84789887@qq.com></author>  
        [ValidateInput(false)]
        public ActionResult Edithandle()
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
            string areaid = Request.Form["checkbox_areaid"].ConventToString(string.Empty);
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
            caddrawingmaster.BillStatus = 0;
            caddrawingmaster.Id = Id;
            int mId = CadDrawingMasterDB.EditHandle(caddrawingmaster,string.Empty);
            mId = Id;
          
            CadDrawingDWGDB.DeleteHandleByParam(string.Format(@" MId={0}",mId));
            CadDrawingDWG dwg = null;
            int index = 0;
            string[] arr_CADFile = cadFile.Split(',');
            string[] arr_IMGFile = imgFile.Split(',');
            string[] arr_FileName = filenames.Split(',');
            string[] arr_DrawingType = drawingtype.Split(',');
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
            CadDrawingHandrailDetailDB.DeleteHandleByParam(string.Format(@" MId={0}", mId));
            CadDrawingHandrailDetail handrail = new CadDrawingHandrailDetail();
            handrail.MId = mId;
            handrail.HandrailType = Request.Form["HandRailType"].ConvertToInt32(0);
            handrail.ModifiedUserId = UserId;
            handrail.ModifiedBy = UserName;
            handrail.ModifiedOn = DateTime.Now;
            int detailId = CadDrawingHandrailDetailDB.AddHandle(handrail);

            if (mId > 0 && detailId > 0)
            {
                return Json(new { code = 100, message = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -100, message = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   栏杆CAD原型属性表-删除根据主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/CadDrawingHandrailDetail/deleteHandlebyid</get>
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
                string BTID = "P51";

                BPMOperationCommonLib.CadWindowBPMDoInvalid(UserCode, BOID, BSID, BTID, bpmprocinstid);
            }
            int rtv = CadDrawingHandrailDetailDB.DeleteHandleById(Id);
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
        ///   栏杆CAD原型属性表-删除多条根据多个主键
        /// </summary>
        /// <returns></returns>
        /// <get>/manage/CadDrawingHandrailDetail/deletehandlebyids</get> 
        /// <author>alon<84789887@qq.com></author> 
        public ActionResult DeleteHandleByIds()
        {
            if (UserId < 1)
            {
                return Json(new { code = -100, message = "非法操作" }, JsonRequestBehavior.AllowGet);
            }
            string ids = Request.QueryString["ids"].ConventToString(string.Empty);
            int rtv = CadDrawingHandrailDetailDB.DeleteHandleByIds(ids);
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
        /// 
        /// </summary>
        /// <returns></returns>
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
                string statecode = Request.Form["State"].ConvertToTrim();
                int billstatus = Request.Form["billstatus"].ConvertToInt32(0);
                string bpmprocinstid = Request.Form["bpmprocinstid"];
                string bpmjobid = Request.Form["bpmjobid"];
                return CadBpmHandrailApproval(Id, billstatus, bpmprocinstid, bpmjobid);
            }
            catch (Exception ex)
            {
                return Json(new { code = -100, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private JsonResult CadBpmHandrailApproval(int handrailId, int billstatus = 0, string bpmprocinstid = "", string bpmjobid = "")
        {
            try
            {
                int Id = handrailId;
                string BSID = "vsheji",
                          BOID = string.Empty,
                          BTID = "P51",
                          Bsxml = string.Empty;
                BOID = Id.ConventToString(string.Empty);
                BPMHandrail handrail = CadDrawingHandrailDetailDB.GetBPMHandrailByHandrailId(Id);
                handrail.FSubject = string.Format(@"栏杆原型审批-{0}", Id);
                Bsxml = XmlSerializeHelper.XmlSerialize<BPMHandrail>(handrail);
                string UserCode = UserName;
                int returnValue = -100;
                returnValue = BPMOperationCommonLib.CadWindowBPMWriteSAPXmlToBPM(BSID, BTID, BOID, Bsxml, bpmprocinstid, UserCode);
               
                if (returnValue == 100)
                {
                    return Json(new { code = 110, message = "提交成功",BSID = BSID, BTID = BTID, BOID = BOID }, JsonRequestBehavior.AllowGet);
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