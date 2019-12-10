using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
       [XmlRoot("DATA")]
    public  class BPMStaticKitchen
    {
        /// <summary>
        /// 流程主题
        /// </summary>
        public string FSubject { get; set; }
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
        public string dynamicType { get; set; }
        /// <summary>
        /// 厨房类型
        /// </summary>
        public string kitchenType { get; set; }

        /// <summary>
        /// 是否含排气道
        /// </summary>
        public string hasAirVent { get; set; }
        /// <summary>
        ///  门窗位置
        /// </summary>
        public string doorWindowPos { get; set; }
        /// <summary>
        /// 开间尺寸
        /// </summary>
        public string width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public string Height { get; set; }
        /// <summary>
        /// 水盆尺寸
        /// </summary>
        public string basinSize { get; set; }
        /// <summary>
        /// 冰箱尺寸
        /// </summary>
        public string fridgeSize { get; set; }
        /// <summary>
        /// 灶台尺寸
        /// </summary>
        public string gasstoveSize { get; set; }
        /// <summary>
        /// 相关附件地址
        /// </summary>
        public string filePath { get; set; }

        public BPMStaticAttachment ATTACHMENTS1 { get; set; }
    }
}
