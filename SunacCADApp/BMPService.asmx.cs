using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.Library;
using SunacCADApp.Entity;

namespace SunacCADApp
{
    /// <summary>
    /// BMPService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class BMPService : System.Web.Services.WebService
    {

        [WebMethod(Description="BMP提交成功")]
        public string CreateResult(string strBTID, string strBOID, string bSuccess, string iProcInstID, string procURL, string strMessage)
        {
            if (bSuccess == "1")
            {
                var result = new ResultSuccess() { Success = 1111 };
                return XmlSerializeHelper.XmlSerialize<ResultSuccess>(result);
            }
            else if (bSuccess == "0")
            {
                var result = new ResultError() { Error = 2222 };
                return XmlSerializeHelper.XmlSerialize<ResultError>(result);
            }
            else 
            {
                var result = new { Error = -1000 };
                return XmlSerializeHelper.XmlSerialize(result);
            }
           
        }


        [WebMethod(Description = "BMP流程审批(通过)")]
        public String Audit(string strBTID, string strBOID, string iProcInstID, string strStepName, string strApproverId, string eAction, string strComment, string dtTime)
        {
            if (eAction == "1")
            {
                var result = new ResultSuccess() { Success = 1111 };
                return XmlSerializeHelper.XmlSerialize<ResultSuccess>(result);
            }
            else if (eAction == "0")
            {
                var result = new ResultError() { Error = 2222 };
                return XmlSerializeHelper.XmlSerialize<ResultError>(result);
            }
            else
            {
                var result = new { Error = -1000 };
                return XmlSerializeHelper.XmlSerialize(result);
            }

        }

         [WebMethod(Description = "BPM流程审批(退回、发起人取消）")]
        public String Rework(string strBTID, string strBOID, string iProcInstID, string strStepName, string strApproverId, string eAction, string strComment, string dtTime) 
        {
            if (eAction == "1")
            {
                var result = new ResultSuccess() { Success = 1111 };
                return XmlSerializeHelper.XmlSerialize<ResultSuccess>(result);
            }
            else if (eAction == "0")
            {
                var result = new ResultError() { Error = 2222 };
                return XmlSerializeHelper.XmlSerialize<ResultError>(result);
            }
            else
            {
                var result = new { Error = -1000 };
                return XmlSerializeHelper.XmlSerialize(result);
            }
        }

         [WebMethod(Description = "BPM流程审批结束(通过、拒绝、作废)")]
         public String ApproveClose(string strBTID, string strBOID, string iProcInstID, string eProcessInstanceResult, string strComment, string dtTime)
         {
             if (eProcessInstanceResult == "1")
             {
                 var result = new ResultSuccess() { Success = 1111 };
                 return XmlSerializeHelper.XmlSerialize<ResultSuccess>(result);
             }
             else if (eProcessInstanceResult == "0")
             {
                 var result = new ResultError() { Error = 2222 };
                 return XmlSerializeHelper.XmlSerialize<ResultError>(result);
             }
             else
             {
                 var result = new { eProcessInstanceResult = -1000 };
                 return XmlSerializeHelper.XmlSerialize(result);
             }
         }


    }
}
