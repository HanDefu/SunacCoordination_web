using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Data
{
    public   class TreeSource
    {
        public int id { get; set; }
        public string name { get; set; }
        public string isParent { get; set; }
        public int pId { get; set; }
        public string halfCheck { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string UpOrgName { get; set; }
        public bool open { get; set; }
        public IList<TreeSource> children { get; set; }
    }
}
