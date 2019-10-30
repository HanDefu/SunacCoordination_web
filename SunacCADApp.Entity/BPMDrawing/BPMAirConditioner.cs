using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    [XmlRoot("DATA")]
    public  class BPMAirConditioner
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
        public string power { get; set; }
        /// <summary>
        /// 最小尺寸范围
        /// </summary>
        public string minSize { get; set; }
        /// <summary>
        /// 冷凝水管位置
        /// </summary>
        public string condensatePipePos { get; set; }
        /// <summary>
        /// 是否含下水管
        /// </summary>
        public string hasSewerPipe { get; set; }
        /// <summary>
        /// 下水管位置
        /// </summary>
        public string sewerPipePos { get; set; }
    }
}
