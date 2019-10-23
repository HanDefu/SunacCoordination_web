using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    [XmlRoot("SizePara")]
    public class SizePara
    {
        /// <summary>
        /// 尺寸代号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 值类型
        /// </summary>
        public string valueType { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public string minValue { get; set; }
        /// <summary>
        /// 最大专
        /// </summary>
        public string maxValue { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string defaultValue { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string valueDescriptione { get; set; }
       
    }
}
