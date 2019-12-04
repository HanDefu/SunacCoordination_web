using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    [XmlRoot("DATA")]
    public  class BPMHandrail
    {
        /// <summary>
        /// 表单编号
        /// </summary>
        public string PageCode { get; set; }
        /// <summary>
        /// 原型编号
        /// </summary>
        public string prototypeID { get; set; }
        /// <summary>
        /// 适用范围
        /// </summary>
        public string region { get; set; }
        /// <summary>
        /// 动态类型
        /// </summary>
        public string railingType { get; set; }
        /// <summary>
        /// 相关附件地址
        /// </summary>
        public string filePath { get; set; }
        public BPMStaticAttachment ATTACHMENTS1 { get; set; }
    }
}
