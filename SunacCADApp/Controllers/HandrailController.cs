﻿using System;
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
    public class HandrailController : Controller
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


            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='HandRail' And ParentID!=0";
            IList<BasArgumentSetting> HandRails = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.HandRails = HandRails;


            _where = string.Empty;  //查询
            string _url = string.Empty;
            int area = HttpUtility.UrlDecode(Request.QueryString["area"]).ConvertToInt32(0);
            if (area >0)
            {
                _where += string.Format(@"  AND  EXISTS(SELECT 1 FROM dbo.CadDrawingByArea ba WHERE a.Id=ba.MId AND ba.AreaID={0})", area);
                _url += "&area=" + area;
            }
            else if (area == -9999)
            {
                _where += "  AND Scope=1";
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
                CadDrawingHandrailDetail handrail = new CadDrawingHandrailDetail();
                handrail.MId= mId;
                handrail.HandrailType = Request.Form["HandRailType"].ConvertToInt32(0);
                handrail.CreateOn = DateTime.Now;
                handrail.ModifiedOn = DateTime.Now;
                int detailId = CadDrawingHandrailDetailDB.AddHandle(handrail);

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
            CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
            int Id = Request.Form["Id"].ConvertToInt32(0);
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
            caddrawingmaster.CreateOn = DateTime.Now;
            caddrawingmaster.Reorder = 2;
            caddrawingmaster.Enabled = 1;
            caddrawingmaster.CreateUserId = 0;
            caddrawingmaster.CreateBy = "admin";
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
            handrail.CreateOn = DateTime.Now;
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
            int Id = Request.Form["id"].ConvertToInt32(0);
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

                int Id = Request.Form["Id"].ConvertToInt32(0);
                int _btid = Request.Form["State"].ConvertToInt32(0);
                string BOID = string.Empty,
                          BTID = "P51",
                          Bsxml = string.Empty;
                BOID = Id.ConventToString(string.Empty);
                BPMHandrail handrail = CadDrawingHandrailDetailDB.GetBPMHandrailByHandrailId(Id);
                Bsxml = XmlSerializeHelper.XmlSerialize<BPMHandrail>(handrail);
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