using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public  class Bpm_Rsp_Param
    {
        public int Code { get; set; }
        public int Success { get; set; }
        public int Error { get; set; }
        public int EProcessInstanceResult {get; set; }
    }
}
