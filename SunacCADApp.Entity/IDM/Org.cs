using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SunacCADApp.Entity.IDM
{

    [XmlRoot("ORG")]
    public class Org
    {
        public string OrganName { get; set; }
        public string OrganNumber { get; set; }
        public string OrganParentNo { get; set; }
        public string OrganStatus { get; set; }

    }
}
