using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp;
using SunacCADApp.Library;
using System.Web;
using System.Web.Mvc;

using Common.Utility;
using Common.Utility.Extender;

namespace SunacCADApp
{
    public class BPMOperationCommonLib
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BSID"></param>
        /// <param name="BTID"></param>
        /// <param name="BOID"></param>
        /// <param name="BSXML"></param>
        /// <param name="ProcInstID"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int CadWindowBPMWriteSAPXmlToBPM(string BSID, string BTID, string BOID, string BSXML, string ProcInstID, string UserId) 
        {

            try
            {
                WeService.BPM.WriteSAP.I_REQUEST request = new WeService.BPM.WriteSAP.I_REQUEST();
                IList<WeService.BPM.WriteSAP.REQ_ITEM> peq_item = new List<WeService.BPM.WriteSAP.REQ_ITEM>();
                WeService.BPM.WriteSAP.REQ_ITEM item = new WeService.BPM.WriteSAP.REQ_ITEM();
                item.BSID = BSID;
                item.BTID = BTID;
                item.BOID = BOID;
                item.BSXML = BSXML;
                item.procInstID = ProcInstID;
                item.userid = UserId;
                peq_item.Add(item);
                WeService.BPM.WriteSAP.REQ_BASEINFO baseInfo = new WeService.BPM.WriteSAP.REQ_BASEINFO();
                baseInfo.REQ_TRACE_ID = API_Common.UUID;
                baseInfo.REQ_SEND_TIME = API_Common.SEND_DATETIME;
                baseInfo.REQ_SRC_SYS = "BS_CAD_BPM";
                baseInfo.REQ_TAR_SYS = "BS_CAD_BPM";
                baseInfo.REQ_SERVER_NAME = "CAD_SUNAC_564_WriteSAPXmlToBPM";
                baseInfo.REQ_SYN_FLAG = "0";
                string BIZTRANSACTIONID=API_Common.BIZTRANSACTIONID;
                baseInfo.BIZTRANSACTIONID = BIZTRANSACTIONID;
                request.REQ_BASEINFO = baseInfo;
                request.MESSAGE = peq_item.ToArray<WeService.BPM.WriteSAP.REQ_ITEM>();

                WeService.BPM.WriteSAP.CAD_SUNAC_564_WriteSAPXmlToBPM_pttbindingQSService service = new WeService.BPM.WriteSAP.CAD_SUNAC_564_WriteSAPXmlToBPM_pttbindingQSService();
                WeService.BPM.WriteSAP.E_RESPONSE response = service.CAD_SUNAC_564_WriteSAPXmlToBPM(request);

                WeService.BPM.WriteSAP.E_RESPONSERSP_ITEM Message = response.MESSAGE.First();

                if (Message.STATUSCODE == "1")
                {
                    string ParamInfo = string.Format(@"BSID      = {0}||BTID  = {1}||BOID   = {2}||BSXML    = {3}||
                                                                            REQ_TRACE_ID   = {4}||REQ_SEND_TIME = {5}||BIZTRANSACTIONID ={6}
                                                                            ", item.BSID, item.BTID, item.BOID, item.BSXML,
                                                                     baseInfo.REQ_TRACE_ID, baseInfo.REQ_SEND_TIME, baseInfo.BIZTRANSACTIONID);
                    string ReturnInfo = string.Format(@"STATUSCODE={0}||STATUSMESSAGE={1}", Message.STATUSCODE, Message.STATUSMESSAGE);
                    CadDrawingMasterDB.Insert_BPM_Commit_Log(item.BTID, item.BSID, "外窗流程提交", ParamInfo, ReturnInfo);
                    int MasterId = BOID.ConvertToInt32(0);
                    CadDrawingMasterDB.ChangeBpmStateusByMId(MasterId, 2);
                    return 100;
                }
                else
                {
                    string loginfo = string.Format(@"{0}||BSID={1}||BTID={2}||BOID={3}||BIZTRANSACTIONID={4}", Message.STATUSMESSAGE, BSID, BTID, BOID, BIZTRANSACTIONID);
                    Sys_Operate_Log log = new Sys_Operate_Log 
                    {
                        SysTypeCode=11,
                        SysTypeName="BPM提交",
                        LogInfo = Message.STATUSMESSAGE,
                        CreateBy=UserId
                    };
                    SysOperateLogDB.AddHandle(log);
                    return -100;
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
           
        }



        /// <summary>
        /// BPM 内容更新
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="jobid"></param>
        /// <param name="procInstId"></param>
        /// <param name="xmldata"></param>
        /// <returns></returns>
        public static int CadWindowBPMUpdateAndApproveFlow(string UserId, string JobId, string ProcInstId, string xmldata, string BOID, string BSID, string BTID) 
        {
            try
            {
                WebService.UpdataAndApproveFlow.I_REQUEST request = new WebService.UpdataAndApproveFlow.I_REQUEST();
                IList<WebService.UpdataAndApproveFlow.REQ_ITEM> peq_item = new List<WebService.UpdataAndApproveFlow.REQ_ITEM>();
                WebService.UpdataAndApproveFlow.REQ_ITEM item = new WebService.UpdataAndApproveFlow.REQ_ITEM();
                item.userid = UserId;
                item.jobid = JobId;
                item.procInstId = ProcInstId;
                item.comments = "重新提交";
                item.xmldata = xmldata;
                peq_item.Add(item);
                WebService.UpdataAndApproveFlow.REQ_BASEINFO baseInfo = new WebService.UpdataAndApproveFlow.REQ_BASEINFO();
                baseInfo.REQ_TRACE_ID = API_Common.UUID;
                baseInfo.REQ_SEND_TIME = API_Common.SEND_DATETIME;
                baseInfo.REQ_SRC_SYS = "BS_CAD_BPM";
                baseInfo.REQ_TAR_SYS = "BS_CAD_BPM";
                baseInfo.REQ_SERVER_NAME = "CAD_SUNAC_565_UpdateAndApproveFlow_PS";
                baseInfo.REQ_SYN_FLAG = "0";
                string BIZTRANSACTIONID = API_Common.BIZTRANSACTIONIDUpdateAndApproveFlow;
                baseInfo.BIZTRANSACTIONID = BIZTRANSACTIONID;
                request.REQ_BASEINFO = baseInfo;
                request.MESSAGE = peq_item.ToArray<WebService.UpdataAndApproveFlow.REQ_ITEM>();
                WebService.UpdataAndApproveFlow.CAD_SUNAC_565_UpdateAndApproveFlow_pttbindingQSService service = new WebService.UpdataAndApproveFlow.CAD_SUNAC_565_UpdateAndApproveFlow_pttbindingQSService();
                WebService.UpdataAndApproveFlow.E_RESPONSE response = service.CAD_SUNAC_565_UpdateAndApproveFlow(request);
                WebService.UpdataAndApproveFlow.E_RESPONSERSP_ITEM Message = response.MESSAGE.First();
                if (Message.STATUSCODE == "1")
                {
                    string ParamInfo = string.Format(@"BSID      = {0}||BTID  = {1}||BOID   = {2}||BSXML    = {3}||
                                                                            REQ_TRACE_ID   = {4}||REQ_SEND_TIME = {5}||BIZTRANSACTIONID ={6}
                                                                            ",BSID,BTID, BOID,xmldata,  baseInfo.REQ_TRACE_ID, baseInfo.REQ_SEND_TIME, baseInfo.BIZTRANSACTIONID);
                    string ReturnInfo = string.Format(@"STATUSCODE={0}||STATUSMESSAGE={1}", Message.STATUSCODE, Message.STATUSMESSAGE);
                    CadDrawingMasterDB.Insert_BPM_Commit_Log(BTID,BSID, "重新提交", ParamInfo, ReturnInfo);
                    int masterId = BOID.ConvertToInt32(0);
                    CadDrawingMasterDB.ChangeBpmStateusByMId(masterId, 2);
                    return 100;
                }
                else
                {
                    string loginfo = string.Format(@"{0}||BSID={1}||BTID={2}||BOID={3}||BIZTRANSACTIONID={4}", Message.STATUSMESSAGE, BSID, BTID, BOID, BIZTRANSACTIONID);
                    Sys_Operate_Log log = new Sys_Operate_Log
                    {
                        SysTypeCode = 12,
                        SysTypeName = "BPM重新提交",
                        LogInfo = loginfo,
                        CreateBy = UserId
                    };
                    SysOperateLogDB.AddHandle(log);
                    return -100;
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
      
        }


        /// <summary>
        /// BPM撤销接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="BOID"></param>
        /// <param name="BSID"></param>
        /// <param name="BTID"></param>
        /// <param name="procInstID"></param>
        /// <returns></returns>
        public static int CadWindowBPMDoInvalid(string UserId, string BOID, string BSID, string BTID, string ProcInstID)
        {
            try
            {
                WebServiceSunacBPM.DoInvalid.I_REQUEST request = new WebServiceSunacBPM.DoInvalid.I_REQUEST();
                IList<WebServiceSunacBPM.DoInvalid.REQ_ITEM> peq_item = new List<WebServiceSunacBPM.DoInvalid.REQ_ITEM>();
                WebServiceSunacBPM.DoInvalid.REQ_ITEM item = new WebServiceSunacBPM.DoInvalid.REQ_ITEM();
                item.userid = UserId;
                item.BOID = BOID;
                item.BSID = BSID;
                item.BTID = BTID;
                item.procInstID = ProcInstID;
                item.invalidComment = string.Format(@"{0}用户撤销", UserId);
                peq_item.Add(item);
                WebServiceSunacBPM.DoInvalid.REQ_BASEINFO baseInfo = new WebServiceSunacBPM.DoInvalid.REQ_BASEINFO();
                baseInfo.REQ_TRACE_ID = API_Common.UUID;
                baseInfo.REQ_SEND_TIME = API_Common.SEND_DATETIME;
                baseInfo.REQ_SRC_SYS = "BS_CAD_BPM";
                baseInfo.REQ_TAR_SYS = "BS_CAD_BPM";
                baseInfo.REQ_SERVER_NAME = "CAD_SUNAC_566_DoInvalid";
                baseInfo.REQ_SYN_FLAG = "0";
                string BIZTRANSACTIONID = API_Common.BIZTRANSACTIONIDDoInvalid;
                baseInfo.BIZTRANSACTIONID = BIZTRANSACTIONID;
                request.REQ_BASEINFO = baseInfo;
                request.MESSAGE = peq_item.ToArray<WebServiceSunacBPM.DoInvalid.REQ_ITEM>();

                WebServiceSunacBPM.DoInvalid.CAD_SUNAC_566_DoInvalid_pttbindingQSService service = new WebServiceSunacBPM.DoInvalid.CAD_SUNAC_566_DoInvalid_pttbindingQSService();
                WebServiceSunacBPM.DoInvalid.E_RESPONSE response = service.CAD_SUNAC_566_DoInvalid(request);
                WebServiceSunacBPM.DoInvalid.E_RESPONSERSP_ITEM Message = response.MESSAGE.First();
                if (Message.STATUSCODE == "1")
                {
                    string ParamInfo = string.Format(@"BSID      = {0}||BTID  = {1}||BOID   = {2}||
                                                                            REQ_TRACE_ID   = {4}||REQ_SEND_TIME = {5}||BIZTRANSACTIONID ={6}
                                                                            ", BSID, BTID, BOID, baseInfo.REQ_TRACE_ID, baseInfo.REQ_SEND_TIME, baseInfo.BIZTRANSACTIONID);
                    string ReturnInfo = string.Format(@"STATUSCODE={0}||STATUSMESSAGE={1}", Message.STATUSCODE, Message.STATUSMESSAGE);
                    CadDrawingMasterDB.Insert_BPM_Commit_Log(BTID, BSID, "BPM撤销", ParamInfo, ReturnInfo);
                    int masterId = BOID.ConvertToInt32(0);
                    CadDrawingMasterDB.ChangeBpmStateusByMId(masterId, 6);
                    return 100;
                }
                else 
                {
                    string loginfo = string.Format(@"{0}||BSID={1}||BTID={2}||BOID={3}||BIZTRANSACTIONID={4}", Message.STATUSMESSAGE, BSID, BTID, BOID, BIZTRANSACTIONID);
                    Sys_Operate_Log log = new Sys_Operate_Log
                    {
                        SysTypeCode = 13,
                        SysTypeName = "BPM撤销接口",
                        LogInfo = loginfo,
                        CreateBy = UserId
                    };
                    SysOperateLogDB.AddHandle(log);
                    return -100;
                }
              
              
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public static int CadWindowBPMGetFlowState(string Userid, string ProcInstID, string BSID, string BTID, string BOID) 
        {
            try
            {
                WebService.GetFlowState.I_REQUEST request = new WebService.GetFlowState.I_REQUEST();
                IList<WebService.GetFlowState.REQ_ITEM> peq_item = new List<WebService.GetFlowState.REQ_ITEM>();
                WebService.GetFlowState.REQ_ITEM item = new WebService.GetFlowState.REQ_ITEM();
                item.userid = Userid;
                item.procInstID = ProcInstID;
                peq_item.Add(item);
                WebService.GetFlowState.REQ_BASEINFO baseInfo = new WebService.GetFlowState.REQ_BASEINFO();
                baseInfo.REQ_TRACE_ID = API_Common.UUID;
                baseInfo.REQ_SEND_TIME = API_Common.SEND_DATETIME;
                baseInfo.REQ_SRC_SYS = "BS_CAD_BPM";
                baseInfo.REQ_TAR_SYS = "BS_CAD_BPM";
                baseInfo.REQ_SERVER_NAME = "CAD_SUNAC_567_GetFlowState_PS";
                baseInfo.REQ_SYN_FLAG = "0";
                string BIZTRANSACTIONID = API_Common.BIZTRANSACTIONIDGetFlowState;
                baseInfo.BIZTRANSACTIONID = BIZTRANSACTIONID;
                request.REQ_BASEINFO = baseInfo;
                request.MESSAGE = peq_item.ToArray<WebService.GetFlowState.REQ_ITEM>();
                WebService.GetFlowState.CAD_SUNAC_567_GetFlowState_pttbindingQSService service = new WebService.GetFlowState.CAD_SUNAC_567_GetFlowState_pttbindingQSService();
                WebService.GetFlowState.E_RESPONSE response = service.CAD_SUNAC_567_GetFlowState(request);
                WebService.GetFlowState.E_RESPONSERSP_ITEM Message = response.MESSAGE.First();
                if (Message.STATUSCODE == "1")
                {
                    string ParamInfo = string.Format(@"BSID      = {0}||BTID  = {1}||BOID   = {2}||
                                                                            REQ_TRACE_ID   = {4}||REQ_SEND_TIME = {5}||BIZTRANSACTIONID ={6}
                                                                            ", BSID, BTID, BOID, baseInfo.REQ_TRACE_ID, baseInfo.REQ_SEND_TIME, baseInfo.BIZTRANSACTIONID);
                    string ReturnInfo = string.Format(@"STATUSCODE={0}||STATUSMESSAGE={1}", Message.STATUSCODE, Message.STATUSMESSAGE);
                    CadDrawingMasterDB.Insert_BPM_Commit_Log(BTID, BSID, "BPM状态查询", ParamInfo, ReturnInfo);
                    return 100;
                }
                else 
                {
                    Sys_Operate_Log log = new Sys_Operate_Log
                    {
                        SysTypeCode = 14,
                        SysTypeName = "BPM状态查询",
                        LogInfo = Message.STATUSMESSAGE,
                        CreateBy = Userid
                    };
                    SysOperateLogDB.AddHandle(log);
                    return -100;
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
   
    }
}