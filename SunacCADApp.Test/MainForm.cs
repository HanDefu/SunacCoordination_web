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
            WebBPMService.BMPService service = new WebBPMService.BMPService();

             WebBPMService.Bpm_Req_BaseInfo _REQ=new WebBPMService.Bpm_Req_BaseInfo();
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
            _REQ.COUNT=1;
             WebBPMService.Bpm_Req_CreateResult_Param param=new WebBPMService.Bpm_Req_CreateResult_Param();
             param.bSuccess = "1";
             param.iProcInstID = "2510001555";
             param.strBOID = "170";
             param.strBTID = "P11";
             param.strMessage = "流程发起成功";
             param.procURL = "http://192.168.2.110/Workflow/MTApprovalView.aspx?procInstId=2510001555";
            WebBPMService.Bpm_Req_CreateResult_Message message = new WebBPMService.Bpm_Req_CreateResult_Message();
            message.REQ_ITEM = param;
            WebBPMService.Bpm_Req_CreateResult createResult= new WebBPMService.Bpm_Req_CreateResult();
            createResult.REQ_BASEINFO=_REQ;
            createResult.MESSAGE = message;
            WebBPMService.Rsp_Result result =   service.CreateResult(createResult);
        }
    }
}
