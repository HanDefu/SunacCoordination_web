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
    public class XMLCadDrawingWindow
    { /// <summary>
        /// 参数编号
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 具体信息提示
        /// </summary>
        public string Message { get; set; }

        public Window[] Windows { get; set; }
    }
}
