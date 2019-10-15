using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public  class Bpm_Req_Audit
    {
        public Bpm_Req_BaseInfo REQ_BASEINFO { get; set; }
        public Bpm_Req_Audit_Message MESSAGE { get; set; }
    }
}
