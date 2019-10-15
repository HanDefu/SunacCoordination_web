using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SunacCADApp.Entity
{

    public class Rsp_Result 
    {
        public Bpm_Rsp_Result E_RESPONSE
        {
            get;
            set;
        }
    }
    public class Bpm_Rsp_Result
    {
        public Bpm_Rsp_BaseInfo RSP_BASEINFO { get; set; }
        public Bpm_Rsp_Message MESSAGE { get; set; }
    }
}
