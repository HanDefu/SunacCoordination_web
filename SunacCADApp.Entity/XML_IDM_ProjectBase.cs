using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
    [XmlRoot("ProjectInfo")]
    [Serializable]
    public class XML_IDM_ProjectBase
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        public string ProjectID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目应用区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 城市公司
        /// </summary>
        public string CityCompany { get; set; }
    }
}
