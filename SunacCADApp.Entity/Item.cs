using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public class Item
    {
        /// <summary>
        /// 尺寸代号
        /// </summary>
        public string SizeNo { get; set; }
        /// <summary>
        /// 值类型
        /// </summary>
        public int ValueType { get; set; }

        /// <summary>
        ///  值类型名称
        /// </summary>
        public string ValueTypeName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public decimal MinValue { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public decimal MaxValue { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public decimal DefaultValue { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
    }
}
