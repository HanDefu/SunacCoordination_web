using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SunacCADApp.Test;
using SunacCADApp;
using SunacCADApp.Entity;


namespace SunacCADApp.Test
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            WebBMPService.BMPService service = new WebBMPService.BMPService();
            WebBMPService.Bpm_Req_BaseInfo _REQ = new WebBMPService.Bpm_Req_BaseInfo();
            _REQ.REQ_TRACE_ID = "1201DC68-576F-4043-97D0-A23935DA2FC7";
            _REQ.REQ_SEND_TIME = "20191031";
            _REQ.REQ_SRC_SYS = "BPM";
            _REQ.REQ_TAR_SYS = "CAD";
            _REQ.REQ_SERVER_NAME = "BPM_SUNAC_568_CreateResult_PS";
            _REQ.REQ_SYN_FLAG = 0;
            _REQ.REQ_BSN_ID = "176";
            _REQ.REQ_RETRY_TIMES = 3;
            _REQ.REQ_REPEAT_FLAG = 0;
            _REQ.REQ_REPEAT_CYCLE = 0;
            _REQ.BIZTRANSACTIONID = "BPM123";
            _REQ.COUNT = 1;
            WebBMPService.Bpm_Req_CreateResult_Param param = new WebBMPService.Bpm_Req_CreateResult_Param();
            param.bSuccess = "1";
            param.iProcInstID = "2510001694";
            param.strBOID = "170";
            param.strBTID = "P31";
            param.strMessage = "流程发起成功";
            param.procURL = "http://192.168.2.110/Workflow/MTApprovalView.aspx?procInstId=2510001555";
            WebBMPService.Bpm_Req_CreateResult_Message message = new WebBMPService.Bpm_Req_CreateResult_Message();
            message.REQ_ITEM = param;
            WebBMPService.Bpm_Req_CreateResult createResult = new WebBMPService.Bpm_Req_CreateResult();
            createResult.REQ_BASEINFO = _REQ;
            createResult.MESSAGE = message;
            WebBMPService.Bpm_Rsp_Result result = service.CreateResult(createResult);
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
              BMPService service = new BMPService();
            SunacCADApp.Entity.Bpm_Req_CreateResult I_REQUEST = new Entity.Bpm_Req_CreateResult();
            Entity.Bpm_Req_BaseInfo _REQ = new Entity.Bpm_Req_BaseInfo();
            _REQ.REQ_TRACE_ID = "f7ded48d-ce65-41de-a3e3-8ebec0174d72";
            _REQ.REQ_SEND_TIME = "20191030112004";
            _REQ.REQ_SRC_SYS = "BS-BPM-D";
            _REQ.REQ_TAR_SYS = "BS-CAD-D";
            _REQ.REQ_SERVER_NAME = "BPM_SUNAC_568_CreateResult_PS";
            _REQ.REQ_SYN_FLAG = 0;
            _REQ.REQ_BSN_ID = "170";
            _REQ.REQ_RETRY_TIMES = 3;
            _REQ.REQ_REPEAT_FLAG = 0;
            _REQ.REQ_REPEAT_CYCLE = 0;
            _REQ.BIZTRANSACTIONID = "BPM_SUNAC_568_CreateResult_PS20191030111959022";
            _REQ.COUNT = 1;
            I_REQUEST.REQ_BASEINFO = _REQ;
            Entity.Bpm_Req_CreateResult_Param param = new Entity.Bpm_Req_CreateResult_Param();
            param.bSuccess = "1";
            param.iProcInstID = "2510001555";
            param.strBOID = "170";
            param.strBTID = "P11";
            param.strMessage = "流程发起成功";
            param.procURL = "http://192.168.2.110/Workflow/MTApprovalView.aspx?procInstId=2510001555";
            Entity.Bpm_Req_CreateResult_Message message = new Entity.Bpm_Req_CreateResult_Message();
            message.REQ_ITEM = param;
            service.CreateResult(I_REQUEST);
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SunacCadService.ArgumentSettingService service = new SunacCadService.ArgumentSettingService();
            string xml=  service.GetProjectInfo("14");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BMPService service = new BMPService();
            SunacCADApp.Entity.Bpm_Req_Audit I_REQUEST = new Entity.Bpm_Req_Audit();
            Entity.Bpm_Req_BaseInfo _REQ = new Entity.Bpm_Req_BaseInfo();
            _REQ.REQ_TRACE_ID = "f7ded48d-ce65-41de-a3e3-8ebec0174d72";
            _REQ.REQ_SEND_TIME = "20191030112004";
            _REQ.REQ_SRC_SYS = "BS-BPM-D";
            _REQ.REQ_TAR_SYS = "BS-CAD-D";
            _REQ.REQ_SERVER_NAME = "BPM_SUNAC_568_CreateResult_PS";
            _REQ.REQ_SYN_FLAG = 0;
            _REQ.REQ_BSN_ID = "170";
            _REQ.REQ_RETRY_TIMES = 3;
            _REQ.REQ_REPEAT_FLAG = 0;
            _REQ.REQ_REPEAT_CYCLE = 0;
            _REQ.BIZTRANSACTIONID = "BPM_SUNAC_568_CreateResult_PS20191030111959022";
            _REQ.COUNT = 1;
            I_REQUEST.REQ_BASEINFO = _REQ;
           


            Entity.Bpm_Req_Audit_Param param = new Entity.Bpm_Req_Audit_Param();
            param.eAction = "1";
            param.iProcInstID = "2510001555";
            param.strBOID = "170";
            param.strBTID = "P11";
            param.strComment = "流程发起成功";
            Entity.Bpm_Req_Audit_Message message = new Entity.Bpm_Req_Audit_Message();
            message.REQ_ITEM = param;
            I_REQUEST.MESSAGE = message;
            Bpm_Rsp_Result  rspResult =  service.Audit(I_REQUEST);
        }

        private void btn_Rework_Click(object sender, EventArgs e)
        {
            BMPService service = new BMPService();
            SunacCADApp.Entity.Bpm_Req_Rework I_REQUEST = new Entity.Bpm_Req_Rework();
            Entity.Bpm_Req_BaseInfo _REQ = new Entity.Bpm_Req_BaseInfo();
            _REQ.REQ_TRACE_ID = "f7ded48d-ce65-41de-a3e3-8ebec0174d72";
            _REQ.REQ_SEND_TIME = "20191030112004";
            _REQ.REQ_SRC_SYS = "BS-BPM-D";
            _REQ.REQ_TAR_SYS = "BS-CAD-D";
            _REQ.REQ_SERVER_NAME = "BPM_SUNAC_568_CreateResult_PS";
            _REQ.REQ_SYN_FLAG = 0;
            _REQ.REQ_BSN_ID = "170";
            _REQ.REQ_RETRY_TIMES = 3;
            _REQ.REQ_REPEAT_FLAG = 0;
            _REQ.REQ_REPEAT_CYCLE = 0;
            _REQ.BIZTRANSACTIONID = "BPM_SUNAC_568_CreateResult_PS20191030111959022";
            _REQ.COUNT = 1;
            I_REQUEST.REQ_BASEINFO = _REQ;



            Entity.Bpm_Req_Rewok_Param param = new Entity.Bpm_Req_Rewok_Param();
            param.eAction = "1";
            param.iProcInstID = "2510001555";
            param.strBOID = "170";
            param.strBTID = "P11";
            param.strComment = "流程发起成功";
            Entity.Bpm_Req_Rework_Message message = new Entity.Bpm_Req_Rework_Message();
            message.REQ_ITEM = param;
            I_REQUEST.MESSAGE = message;
            Bpm_Rsp_Result rspResult = service.Rework(I_REQUEST);
        }

        private void btnApproveClose_Click(object sender, EventArgs e)
        {
            BMPService service = new BMPService();
            SunacCADApp.Entity.Bpm_Req_ApproveClose I_REQUEST = new Entity.Bpm_Req_ApproveClose();
            Entity.Bpm_Req_BaseInfo _REQ = new Entity.Bpm_Req_BaseInfo();
            _REQ.REQ_TRACE_ID = "f7ded48d-ce65-41de-a3e3-8ebec0174d72";
            _REQ.REQ_SEND_TIME = "20191030112004";
            _REQ.REQ_SRC_SYS = "BS-BPM-D";
            _REQ.REQ_TAR_SYS = "BS-CAD-D";
            _REQ.REQ_SERVER_NAME = "BPM_SUNAC_568_CreateResult_PS";
            _REQ.REQ_SYN_FLAG = 0;
            _REQ.REQ_BSN_ID = "170";
            _REQ.REQ_RETRY_TIMES = 3;
            _REQ.REQ_REPEAT_FLAG = 0;
            _REQ.REQ_REPEAT_CYCLE = 0;
            _REQ.BIZTRANSACTIONID = "BPM_SUNAC_568_CreateResult_PS20191030111959022";
            _REQ.COUNT = 1;
            I_REQUEST.REQ_BASEINFO = _REQ;

            Entity.Bpm_Req_ApproveClose_Param param = new Entity.Bpm_Req_ApproveClose_Param();
            param.eProcessInstanceResult = "1";
            param.iProcInstID = "2510001555";
            param.strBOID = "170";
            param.strBTID = "P11";
            param.strComment = "流程发起成功";
            Entity.Bpm_Req_ApproveClose_Message message = new Entity.Bpm_Req_ApproveClose_Message();
            message.REQ_ITEM = param;
            I_REQUEST.MESSAGE = message;
            Bpm_Rsp_Result rspResult = service.ApproveClose(I_REQUEST);
        }
    }
}
