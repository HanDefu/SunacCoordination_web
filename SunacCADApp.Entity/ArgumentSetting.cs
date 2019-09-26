using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    [Serializable]
    public  class ArgumentSetting
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ArgumentText { get; set; }
        /// <summary>
        /// 参数编号
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
