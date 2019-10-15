using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunacCADApp.Entity;
using SunacCADApp.Library;
using SunacCADApp.Data;

namespace SunacCADApp.Data.ESB
{
    public class BpmLibDB
    {
        public static Bpm_Rsp_BaseInfo ReqToRspBaseInfo(Bpm_Req_BaseInfo request) 
        {
            Bpm_Rsp_BaseInfo response = new Bpm_Rsp_BaseInfo();
            response.RSP_TRACE_ID = request.REQ_TRACE_ID;
            response.RSP_REQ_TRACEID = API_Common.UUID;
            response.RSP_SEND_TIME = request.REQ_SEND_TIME;
            response.RSP_SRC_SYS = request.REQ_SRC_SYS;
            response.RSP_TAR_SYS = request.REQ_TAR_SYS;
            response.RSP_SERVER_NAME = request.REQ_SERVER_NAME;
            response.RSP_BSN_ID = request.REQ_BSN_ID;
            response.RSP_REQ_TRACEID = request.REQ_TRACE_ID;
            response.RSP_RETRY_TIMES = request.REQ_RETRY_TIMES;
            response.RSP_STATUS_CODE = request.REQ_REPEAT_FLAG;
            response.BIZTRANSACTIONID = request.BIZTRANSACTIONID;
            return response;
        }
    }
}
