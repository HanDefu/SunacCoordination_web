using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.Library;
using SunacCADApp.Data.ESB;
namespace SunacCADApp
{
    /// <summary>
    /// BMPService 的摘要说明
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class BMPService : System.Web.Services.WebService
    {

        
        [WebMethod(Description = "BMP提交成功")]
        [return: System.Xml.Serialization.XmlElement("E_RESPONSE")]
        public Bpm_Rsp_Result CreateResult(Bpm_Req_CreateResult I_REQUEST)
        {
              Bpm_Rsp_Result _createResult = new Bpm_Rsp_Result();
             Bpm_Rsp_BaseInfo _rsp_BaseInfo = new Bpm_Rsp_BaseInfo();
            
            try 
            {
                if (I_REQUEST == null)
                {

                    _createResult.MESSAGE = new Bpm_Rsp_Message()
                    {
                        RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = -1000 }
                    };
                }
                else
                {
                    Bpm_Req_BaseInfo _Req = I_REQUEST.REQ_BASEINFO;
                   
                    _rsp_BaseInfo = BpmLibDB.ReqToRspBaseInfo(_Req);
                    _createResult.RSP_BASEINFO = _rsp_BaseInfo;
                    Bpm_Req_CreateResult_Param request = I_REQUEST.MESSAGE.REQ_ITEM;
                    if (request.bSuccess == "1")
                    {
                        _rsp_BaseInfo.RESULT = 0;
                        _rsp_BaseInfo.RSP_STATUS_MSG = "成功";
                       
                        _createResult.MESSAGE = new Bpm_Rsp_Message()
                        {
                            RSP_ITEM = new Bpm_Rsp_Param { Code = 100, Success = 1111 }
                        };
                    }
                    else
                    {
                        _rsp_BaseInfo.RESULT = 0;
                        _rsp_BaseInfo.RSP_STATUS_MSG = "失败";
                        _createResult.MESSAGE = new Bpm_Rsp_Message()
                        {
                            RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = 2222 }
                        };
                    }
                }

            }
            catch (Exception ex) 
            {
                _createResult.MESSAGE = new Bpm_Rsp_Message()
                {
                    RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = -1000 }
                };
            }
            return _createResult;
        }


        [WebMethod(Description = "BMP流程审批(通过)")]
        [return: System.Xml.Serialization.XmlElement("E_RESPONSE")]
        public Bpm_Rsp_Result Audit(Bpm_Req_Audit I_REQUEST)
        {
            Bpm_Rsp_Result _Rsp_Result = new Bpm_Rsp_Result();
            Bpm_Rsp_BaseInfo _Rsp_BaseInfo =new Bpm_Rsp_BaseInfo();
            if (I_REQUEST == null)
            {
                _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                {
                    RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = -1000 }
                };
            }
            else
            {
                Bpm_Req_BaseInfo _Req = I_REQUEST.REQ_BASEINFO;
                _Rsp_BaseInfo = BpmLibDB.ReqToRspBaseInfo(_Req);
                _Rsp_Result.RSP_BASEINFO = _Rsp_BaseInfo;
                Bpm_Req_Audit_Param param = I_REQUEST.MESSAGE.REQ_ITEM;

                if (param.eAction == "1")
                {
                    _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                    {
                        RSP_ITEM = new Bpm_Rsp_Param { Code = 100, Success = 1111 }
                    };
                   
                }
                else if (param.eAction == "0")
                {
                    _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                    {
                        RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = 2222 }
                    };
                }
                else
                {
                    _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                    {
                        RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = 10000 }
                    };
                }


               
               
            }

            return _Rsp_Result;
        }

        [WebMethod(Description = "BPM流程审批(退回、发起人取消）")]
        [return: System.Xml.Serialization.XmlElement("E_RESPONSE")]
        public Bpm_Rsp_Result Rework(Bpm_Req_Rework I_REQUEST)
        {
            Bpm_Rsp_Result _Rsp_Result = new Bpm_Rsp_Result();
            Bpm_Rsp_BaseInfo _Rsp_BaseInfo = new Bpm_Rsp_BaseInfo();
            if (I_REQUEST == null)
            {
                _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                {
                    RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = -1000 }
                };
            }
            else
            {
                Bpm_Req_Rewok_Param param = I_REQUEST.MESSAGE.REQ_ITEM;
                Bpm_Req_BaseInfo _Req = I_REQUEST.REQ_BASEINFO;
                _Rsp_BaseInfo = BpmLibDB.ReqToRspBaseInfo(_Req);
                _Rsp_Result.RSP_BASEINFO = _Rsp_BaseInfo;
                if (param.eAction == "1") 
                {
                    _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                    {
                        RSP_ITEM = new Bpm_Rsp_Param { Code = 100, Success = 1111 }
                    };
                }
                else if (param.eAction == "0")
                {
                    _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                    {
                        RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = 2222 }
                    };
                }
                else 
                {
                    _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                    {
                        RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = -1000 }
                    };

                }
            }
            return _Rsp_Result;
        }

         [WebMethod(Description = "BPM流程审批结束(通过、拒绝、作废)")]
         [return: System.Xml.Serialization.XmlElement("E_RESPONSE")]
        public Bpm_Rsp_Result ApproveClose(Bpm_Req_ApproveClose I_REQUEST)
         {

             Bpm_Rsp_Result _Rsp_Result = new Bpm_Rsp_Result();
             Bpm_Rsp_BaseInfo _Rsp_BaseInfo = new Bpm_Rsp_BaseInfo();
             if (I_REQUEST == null)
             {

                 _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                 {
                     RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = -1000 }
                 };
             }
             else
             {
                 Bpm_Req_ApproveClose_Param  param = I_REQUEST.MESSAGE.REQ_ITEM;
                 Bpm_Req_BaseInfo _Req = I_REQUEST.REQ_BASEINFO;
                 _Rsp_BaseInfo = BpmLibDB.ReqToRspBaseInfo(_Req);
                 _Rsp_Result.RSP_BASEINFO = _Rsp_BaseInfo;
                 if (param.eProcessInstanceResult == "1")
                 {
                     _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                     {
                         RSP_ITEM = new Bpm_Rsp_Param { Code = 100, Success = 1111 }
                     };
                 }
                 else if (param.eProcessInstanceResult == "0")
                 {
                     _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                     {
                         RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = 2222 }
                     };
                 }
                 else
                 {
                     _Rsp_Result.MESSAGE = new Bpm_Rsp_Message()
                     {
                         RSP_ITEM = new Bpm_Rsp_Param { Code = -100, Error = -1000 }
                     };

                 }
             }
             return _Rsp_Result;
         }
    }
}
