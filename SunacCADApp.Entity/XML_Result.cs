using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    [XmlRoot("Root")]
    [Serializable]
    public  class XML_Result
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public int KeyId { get; set; }
        public string CadUriPath { get; set; }
    }
}
