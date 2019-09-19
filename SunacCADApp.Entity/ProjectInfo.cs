using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public  class ProjectInfo
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 城市公司
        /// </summary>
        public string CityCompany { get; set; }
        /// <summary>
        /// 项目预计开始时间
        /// </summary>
        public string Project_StartTime { get; set; }
    }
}
