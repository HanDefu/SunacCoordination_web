using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("Root")]
    [Serializable]
    public class XML_IDM_Project
    {
        /// <summary>
        /// 返回值Code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 项目信息
        /// </summary>
       [XmlArray(ElementName = "Projects") ]
       [XmlArrayItem(ElementName = "Project")]
        public XML_IDM_ProjectInfo[] Projects { get; set; }
    }
}
