using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public class Bpm_Req_ApproveClose_Param
    {
        public string strBTID { get; set; }
        public string strBOID { get; set; }
        public string iProcInstID { get; set; }
        public string eProcessInstanceResult { get; set; }
        public string strComment { get; set; }
        public string dtTime { get; set; }
    }
}