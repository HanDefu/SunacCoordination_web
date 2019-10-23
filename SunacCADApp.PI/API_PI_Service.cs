using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.Library;


namespace SunacCADApp.PI
{
    public   class API_PI_Service
    {
        public static int ReadPIService(DateTime stateDateTime,DateTime endDateTime)
        {
            string soap = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"+
                                 "  <ns0:MT_COMMON_Projectstage_REQ xmlns:ns0=\"urn:Sunac:com:mdm:project\">"+
                                 "     <BaseInfo>"  +
                                 "           <SourceSystem>BS-CAD-P</SourceSystem>"  +
                                 "           <TargetSystem>S4PCLNT800</TargetSystem>"  +
                                 "           <ServiceName>SI_MD_COMMON_OUT</ServiceName>"  +
                                 "     </BaseInfo>"+
                                 "     <SEL_DATA>"+
                                 "         <POSID></POSID>"+
                                 "         <BBSTAT></BBSTAT>"+
                                 "     </SEL_DATA>"+
                                 "     <Begindate>20191015011327.3474250 </Begindate>"+
                                 "     <Enddate></Enddate>"+
                                 "     <Numb>100</Numb>"+
                                 "     <GET_AREA>X</GET_AREA>"+
                                 "     <MJ_CODE>S021</MJ_CODE>"+
                                 "     <MJ_CODE>S022</MJ_CODE>"+
                                 "     <MJ_CODE>S023</MJ_CODE>"+
                                 "</ns0:MT_COMMON_Projectstage_REQ>";
            string url = "http://sappoqas.sunac.com.cn:50100/dir/wsdl?p=ic/6caf9c56825a3d638c4580be9ec9134a";
            string _returnHTML=  API_Common.GetSOAPReSource(url, soap);
            return 0;

        }
    }
}
