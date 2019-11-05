using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    public class XML_IDM_FileDir
    {
        /// <summary>
        /// 目录ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// CAD原型文件
        /// </summary>
        [XmlElement("File")]
        public XML_IDM_File[] File { get; set; }

         [XmlElement("FileDir")]
        public XML_IDM_FileDir[] FileDirs { get; set; }
    }
}
