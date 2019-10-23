using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.Library;

namespace SunacCADApp.Controllers
{
    public class BpmHandleController : Controller
    {
        // GET: BpmHandle
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  /bpmhandle/cadwirtebpmapproval
        /// </summary>
        /// <returns></returns>
        public ActionResult CadWirteBPMApproval() 
        {



             WeService.BPM.WriteSAP.I_REQUEST request = new WeService.BPM.WriteSAP.I_REQUEST();
             IList<WeService.BPM.WriteSAP.REQ_ITEM> peq_item = new List<WeService.BPM.WriteSAP.REQ_ITEM>();
             WeService.BPM.WriteSAP.REQ_ITEM item = new WeService.BPM.WriteSAP.REQ_ITEM();
             BPMDynamicWindow window = XMLCadDrawingWindowDB.BPM_XML_Window(97);
             string Bsxml = XmlSerializeHelper.XmlSerialize<BPMDynamicWindow>(window);
             item.BSID = "vsheji";
             item.BTID = "P12";
             item.BOID = "97";
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

     
            return Json(new { code = -100, message = "提交成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdataAndApproveFlow() 
        {
            WebService.UpdataAndApproveFlow.I_REQUEST request = new WebService.UpdataAndApproveFlow.I_REQUEST();
            WebService.UpdataAndApproveFlow.REQ_BASEINFO baseInfo = new WebService.UpdataAndApproveFlow.REQ_BASEINFO();
            baseInfo.REQ_TRACE_ID = API_Common.UUID;
            baseInfo.REQ_SEND_TIME = API_Common.SEND_DATETIME;
            baseInfo.REQ_SRC_SYS = "BS_CAD_BPM";
            baseInfo.REQ_TAR_SYS = "BS_CAD_BPM";
            baseInfo.REQ_SERVER_NAME = "CAD_SUNAC_564_WriteSAPXmlToBPM";
            baseInfo.REQ_SYN_FLAG = "0";
            WebService.UpdataAndApproveFlow.CAD_SUNAC_565_UpdateAndApproveFlow_pttbindingQSService service = new WebService.UpdataAndApproveFlow.CAD_SUNAC_565_UpdateAndApproveFlow_pttbindingQSService();
            service.CAD_SUNAC_565_UpdateAndApproveFlow(request);
            return Json(new { Code = 100, Message = "" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///   /bpmhandle/IDM_User
        /// </summary>
        /// <returns></returns>
        public ActionResult IDM_User() 
        {
            WebService.Idm.User.Header header = new WebService.Idm.User.Header();
            header.ACCOUNT = "idmadmin";
            header.PASSWORD = "idmpass";
            header.BIZTRANSACTIONID = "vsheji";
            WebService.Idm.User.queryDto dto = new WebService.Idm.User.queryDto();
            dto.beginDate = "2019-07-30 00:00:00.000";
            dto.endDate = "2019-08-01 00:00:00.000";
            dto.systemID = "CADSJXTUser";
            dto.pageNo = "1";
            dto.pageRowNo = "20";
            string list="";
            WebService.Idm.User.PUBLIC_SUNAC_300_queryIdmUserData_pttbindingQSService service = new WebService.Idm.User.PUBLIC_SUNAC_300_queryIdmUserData_pttbindingQSService();
            service.commonHeader = header;
            WebService.Idm.User.HEADER head =   service.PUBLIC_SUNAC_300_queryIdmUserData(dto,out list);
            return Json(new { Code = 100, Message = "测试成功" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///   /bpmhandle/Idm_Public
        /// </summary>
        /// <returns></returns>

        public ActionResult Idm_Public() 
        {
            WebService.IDM.Public.Header head = new WebService.IDM.Public.Header();
            head.ACCOUNT = "idmadmin";
            head.PASSWORD = "idmpass";
            head.BIZTRANSACTIONID = "vsheji";
            WebService.IDM.Public.queryDto dto = new WebService.IDM.Public.queryDto();
            dto.beginDate = "2019-07-31 00:00:00.000";
            dto.endDate = "2019-08-01  00:00:00.000";
            dto.systemID = "CADSJXTOrg";
            dto.pageNo = "1";
            dto.pageRowNo = "100";
            string list;
            WebService.IDM.Public.PUBLIC_SUNAC_301_queryIdmOrgData_pttbindingQSService service = new WebService.IDM.Public.PUBLIC_SUNAC_301_queryIdmOrgData_pttbindingQSService();
            service.commonHeader = head;
            service.PUBLIC_SUNAC_301_queryIdmOrgData(dto, out  list);
            return Json(new { Code = 100, Message = "测试成功" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  /bpmhandle/Idm_Validate
        /// </summary>
        /// <returns></returns>
        public ActionResult Idm_Validate() 
        {

            WebService932.Header header = new WebService932.Header();
            header.BIZTRANSACTIONID = "sdfdssdfds";
            header.COUNT = "";
            header.CONSUMER = "";
            header.ACCOUNT = "wdaccount";
            header.PASSWORD = "wdpwd";
            
           
            WebService932.user user = new WebService932.user();
            user.username="11";
            user.password = "1234";
            WebService932.IDM_SUNAC_392_validatePwd_pttbindingQSService client = new WebService932.IDM_SUNAC_392_validatePwd_pttbindingQSService();

            client.commonHeader = header;
            string LIST = "";
            WebService932.HEADER backheader = client.IDM_SUNAC_392_validatePwd(user, out LIST);
            return Json(new { Code = 100, Message = LIST }, JsonRequestBehavior.AllowGet);

        }

    }
}