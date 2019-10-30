using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    /// <summary>
    ///  BPM 动态模型
    /// </summary>
    [XmlRoot("DATA")]
    public  class BPMDynamicDoor
    {
        /// <summary>
        /// 表单编号
        /// </summary>
        public string PageCode { get; set; }
        /// <summary>
        /// 静态门原型审批
        /// </summary>
        public string prototypeID { get; set; }
        /// <summary>
        /// 适用范围
        /// </summary>
        public string region { get; set; }
        /// <summary>
        ///门的类型：既：1、动态  2  静态
        /// </summary>
        public string dynamicType { get; set; }
        /// <summary>
        /// 门的类型
        /// </summary>
        public string doortype { get; set; }
        /// <summary>
        /// 宽度范围
        /// </summary>
        public string widthrange { get; set; }
        /// <summary>
        /// 相关附件地址
        /// </summary>
        public string filePath { get; set; }
    }
}
