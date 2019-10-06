using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public class Handrail
    {
        public int Id { get; set; }
        /// <summary>
        /// 原型编号
        /// </summary>
        public string DrawingCode { get; set; }
        /// <summary>
        /// 原型名称
        /// </summary>
        public string DrawingName { get; set; }
        /// <summary>
        /// 原型平面图稿件地址
        /// </summary>
        public Drawing[] Drawings { get; set; }
        /// <summary>
        /// 集团
        /// </summary>
        public int Scope { get; set; }
        /// <summary>
        /// 区域信息
        /// </summary>
        public Area[] Areas { get; set; }

        /// <summary>
        /// 动态类型编号
        /// </summary>
        public int DynamicType { get; set; }

        /// <summary>
        /// 动态类型名称
        /// </summary>
        public int DynamicTypeName { get; set; }
        /// <summary>
        /// 栏杆类型
        /// </summary>
        public int HandrailType { get; set; }
        /// <summary>
        /// 栏杆类型名称
        /// </summary>
        public string HandrailTypeName { get; set; }
    }

    public class XMLHandrail
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Handrail[] Handrails { get; set; }
    }
}
