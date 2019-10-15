using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public  class Bpm_Req_CreateResult_Param
    {
        public string strBTID{get;set;}
        public  string strBOID{get;set;} 
        public string bSuccess{get;set;}
        public string iProcInstID{get;set;}
        public string procURL{get;set;}
        public string strMessage { get; set; }
    }
}
