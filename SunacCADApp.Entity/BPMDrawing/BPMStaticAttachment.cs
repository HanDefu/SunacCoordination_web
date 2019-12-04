using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    public class BPMStaticAttachment
    {
        [XmlElement("ITEM")]
        public BPMStaticFile[] ITEM { get; set; }
    }
}
