using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public class DataSourceMember
    {
        /// <summary>
        /// 数据源ID 
        /// </summary>
        public string ValueMember { get; set; }

        /// <summary>
        /// 数据源显示值
        /// </summary>
        public string DisplayMember { get; set; }

    }
}
