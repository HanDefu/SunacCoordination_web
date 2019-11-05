using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    [Serializable]

    [XmlRoot(ElementName = "Project")]
    public class XML_IDM_ProjectInfo
    {
        /// <summary>
        /// 项目信息
        /// </summary>
       [XmlElement("ProjectInfo")]
        public XML_IDM_ProjectBase ProjectInfo { get; set; }
        /// <summary>
        /// 文件信息
        /// </summary>
       [XmlElement("FileDir")]
       public XML_IDM_FileDir[] FileDir { get; set; }
    }
}
