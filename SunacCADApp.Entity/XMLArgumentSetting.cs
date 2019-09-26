using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    /// <summary>
    ///  提供给CAD端参数
    /// </summary>
    
    [XmlRoot("Root")]
    [Serializable]
    public  class XMLArgumentSetting
    {
        /// <summary>
        /// 参数编号
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 具体信息提示
        /// </summary>
        public string Message { get; set; }

        public ArgumentSetting[] ArgumentSettings { get; set; }


    }
}
