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
using SunacCADApp.Library;
using System.Collections;
using System.Xml;
using System.Net;
using System.IO;


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
            string xml = service.GetProjectInfo("14");
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
            Bpm_Rsp_Result rspResult = service.Audit(I_REQUEST);
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

        private void btn_login_Click(object sender, EventArgs e)
        {
            String ServerUrl = "http://192.168.2.219:8001/WP_SUNAC/APP_IDM_SERVICES/Proxy_Services/TA_EOP/IDM_SUNAC_392_validatePwd_PS?wsdl";//得到WebServer地址

            string xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope\" \r\n" +
                               "    xmlns:v1=\"http://www.ekingwin.com/esb/header/v1\" \r\n" +
                               "    xmlns:idm=\"http://www.ekingwin.com/esb/IDM_SUNAC_392_validatePwd\"> \r\n" +
                               "       <soapenv:Header>\r\n" +
                               "              <v1:commonHeader>\r\n" +
                               "                  <v1:BIZTRANSACTIONID>dd</v1:BIZTRANSACTIONID>\r\n" +
                              "                   <v1:COUNT>?</v1:COUNT>\r\n" +
                              "                   <v1:CONSUMER>?</v1:CONSUMER>\r\n" +
                              "                   <v1:SRVLEVEL>?</v1:SRVLEVEL>\r\n" +
                               "                  <v1:ACCOUNT>idmadmin</v1:ACCOUNT>\r\n" +
                              "                   <v1:PASSWORD>idmpass</v1:PASSWORD>\r\n" +
                              "               </v1:commonHeader>\r\n" +
                              "        </soapenv:Header>\r\n" +
                              "        <soapenv:Body>\r\n" +
                               "              <idm:IDM_SUNAC_392_validatePwd>\r\n" +
                               "                    <idm:user>\r\n" +
                               "                       <idm:password>123423</idm:password>\r\n" +
                               "                       <idm:username>456546</idm:username>\r\n" +
                               "                  </idm:user>\r\n" +
                               "             </idm:IDM_SUNAC_392_validatePwd>\r\n" +
                               "        </soapenv:Body>\r\n" +
                               "</soapenv:Envelope>\r\n";

            string rxml = GetSOAPReSource(ServerUrl,xml);


        }

        private void button4_Click(object sender, EventArgs e)
        {
            WebService932.Header header = new WebService932.Header();
            header.BIZTRANSACTIONID = API_Common.BIZTRANSACTIONIDValidatePwd;
            header.COUNT = "";
            header.CONSUMER = "";
            header.ACCOUNT = "wdaccount";
            header.PASSWORD = "wdpwd";
            WebService932.user webUser = new WebService932.user()
            {
                username = "hellow",
                password = "world"
            };
            WebService932.IDM_SUNAC_392_validatePwd_pttbindingQSService client = new WebService932.IDM_SUNAC_392_validatePwd_pttbindingQSService();
            client.commonHeader = header;
            string LIST = "";
            WebService932.HEADER backheader = client.IDM_SUNAC_392_validatePwd(webUser, out LIST);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            string BIZTRANSACTIONID = API_Common.BIZTRANSACTIONIDValidatePwd;
            String ServerUrl = "http://192.168.2.219:8001/WP_SUNAC/APP_IDM_SERVICES/Proxy_Services/TA_EOP/IDM_SUNAC_392_validatePwd_PS";//得到WebServer地址

            string xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope\">" +
                               "       <soapenv:Header>" +
                               "              <v1:commonHeader xmlns:v1=\"http://www.ekingwin.com/esb/header/v1\">" +
                               "                  <v1:BIZTRANSACTIONID>" + BIZTRANSACTIONID + "</v1:BIZTRANSACTIONID>" +
                              "                   <v1:COUNT>?</v1:COUNT>" +
                              "                   <v1:CONSUMER>?</v1:CONSUMER>" +
                              "                   <v1:SRVLEVEL>?</v1:SRVLEVEL>" +
                               "                  <v1:ACCOUNT>idmadmin</v1:ACCOUNT>" +
                              "                   <v1:PASSWORD>idmpass</v1:PASSWORD>" +
                              "               </v1:commonHeader>" +
                              "        </soapenv:Header>" +
                              "        <soapenv:Body>" +
                               "              <idm:IDM_SUNAC_392_validatePwd xmlns:idm=\"http://www.ekingwin.com/esb/IDM_SUNAC_392_validatePwd\">" +
                               "                    <idm:user>" +
                               "                       <idm:password>123423</idm:password>" +
                               "                       <idm:username>456546</idm:username>" +
                               "                  </idm:user>" +
                               "             </idm:IDM_SUNAC_392_validatePwd>" +
                               "        </soapenv:Body>" +
                               "</soapenv:Envelope>";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(ServerUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "text/xml; charset=utf-8";
            //mediate为调用方法
            myRequest.Headers.Add("SOAPAction", "/IDM_SUNAC_392_validatePwd");
            myRequest.UserAgent = "Apache-HttpClient/4.1.1";
            myRequest.ContentLength = xml.Length;
            byte[] bs = Encoding.UTF8.GetBytes(xml);
            using (Stream reqStream = myRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
            {
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string res= sr.ReadToEnd();
                //Console.WriteLine("反馈结果" + responseString);
            }

        }

        private   string GetSOAPReSource(string url, string datastr)
         {
             try
             {
                 //request
                 Uri uri = new Uri(url);
                 WebRequest webRequest = WebRequest.Create(uri);
                webRequest.ContentType = "application/soap+xml; charset=utf-8";
                 webRequest.Method = "POST";
                 using (Stream requestStream = webRequest.GetRequestStream())
                 {
                    byte[] paramBytes = Encoding.UTF8.GetBytes(datastr.ToString());
                     requestStream.Write(paramBytes, 0, paramBytes.Length);
                 }
                 //response
                 WebResponse webResponse = webRequest.GetResponse();
                 using (StreamReader myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                 {
                     string result = "";
                     return result = myStreamReader.ReadToEnd();
                }
 
             }
             catch (Exception ex)
             {
                 throw ex;
             }
 
         }

        private void button01_CreateResult_Click(object sender, EventArgs e)
        {
            WebBMPService.BMPService service = new WebBMPService.BMPService();
            WebBMPService.Bpm_Req_ApproveClose I_REQUEST = new WebBMPService.Bpm_Req_ApproveClose();
            WebBMPService.Bpm_Req_BaseInfo BaseInfo=new WebBMPService.Bpm_Req_BaseInfo();
            BaseInfo.BIZTRANSACTIONID = API_Common.BIZTRANSACTIONID;
            BaseInfo.REQ_TRACE_ID = "f7ded48d-ce65-41de-a3e3-8ebec0174d72";
            BaseInfo.REQ_SEND_TIME = "20191030112004";
            BaseInfo.REQ_SRC_SYS = "BS-BPM-D";
            BaseInfo.REQ_TAR_SYS = "BS-CAD-D";
            BaseInfo.REQ_SERVER_NAME = "BPM_SUNAC_568_CreateResult_PS";
            BaseInfo.REQ_SYN_FLAG = 0;
            BaseInfo.REQ_BSN_ID = "170";
            BaseInfo.REQ_RETRY_TIMES = 3;
            BaseInfo.REQ_REPEAT_FLAG = 0;
            BaseInfo.REQ_REPEAT_CYCLE = 0;
            BaseInfo.BIZTRANSACTIONID = "BPM_SUNAC_568_CreateResult_PS20191030111959022";
            BaseInfo.COUNT = 1;
            I_REQUEST.REQ_BASEINFO = BaseInfo;

            //Entity.Bpm_Req_ApproveClose_Param param = new Entity.Bpm_Req_ApproveClose_Param();
            //param.eProcessInstanceResult = "1";
            //param.iProcInstID = "2510001555";
            //param.strBOID = "170";
            //param.strBTID = "P11";
            //param.strComment = "流程发起成功";
            //Entity.Bpm_Req_ApproveClose_Message message = new Entity.Bpm_Req_ApproveClose_Message();
            //message.REQ_ITEM = param;
            //I_REQUEST.MESSAGE = message;


            WebBMPService.Bpm_Req_ApproveClose_Param param = new WebBMPService.Bpm_Req_ApproveClose_Param();
            param.eProcessInstanceResult = "1";
            param.iProcInstID = "2510001555";
            param.strBOID = "170";
            param.strBTID = "P11";
            param.strComment = "流程发起成功";
            WebBMPService.Bpm_Req_ApproveClose_Message Message = new WebBMPService.Bpm_Req_ApproveClose_Message();
            Message.REQ_ITEM = param;

            I_REQUEST.MESSAGE = Message;


            WebBMPService.Bpm_Rsp_Result result = service.ApproveClose(I_REQUEST);
        }

    }
}
