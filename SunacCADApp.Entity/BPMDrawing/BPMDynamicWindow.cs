using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    [XmlRoot("DATA")]
    public  class BPMDynamicWindow
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
        /// 动态外窗原型审批
        /// </summary>
        public string prototypeID { get; set; }
        /// <summary>
        /// 使用范围
        /// </summary>
        public string region { get; set; }
        /// <summary>
        /// 动态类型
        /// </summary>
        public string dynamicType { get; set; }
        /// <summary>
        /// 功能区类型
        /// </summary>
        public string functionalAreas { get; set; }
        /// <summary>
        /// 开启类型
        /// </summary>
        public string openType { get; set; }
        /// <summary>
        /// 开启扇数量
        /// </summary>
        public string openCount { get; set; }
        /// <summary>
        /// 是否转角窗
        /// </summary>
        public string isCorner { get; set; }
        /// <summary>
        /// 是否对称窗型
        /// </summary>
        public string isMirror { get; set; }
        /// <summary>
        /// 宽度尺寸范围(mm)
        /// </summary>
        public string widthRange { get; set; }
        /// <summary>
        /// 通风量计算公式
        /// </summary>
        public string airVolumeFormula { get; set; }
        /// <summary>
        /// 相关附件地址
        /// </summary>
        public string filePath { get; set; }

        public BPMStaticAttachment ATTACHMENTS1 { get; set; }
         /// <summary>
         /// 尺码属性
         /// </summary>
        public SizePara[] SizeParas { get; set; }
    }
}
