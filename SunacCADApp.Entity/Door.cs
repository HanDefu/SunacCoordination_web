using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public   class Door
    {

        public int Id { get; set; }
        /// <summary>
        /// 原型编号
        /// </summary>
        public string DrawingCode { get; set; }
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
        /// 门类型
        /// </summary>
        public int DoorType { get; set; }

        /// <summary>
        /// 门类型名称
        /// </summary>
        public string DoorTypeName { get; set; }

        /// <summary>
        /// 宽度尺寸范围最小值
        /// </summary>
        public decimal WindowSizeMin { get; set; }

        /// <summary>
        /// 宽度尺寸范围最大值
        /// </summary>
        public decimal WindowSizeMax { get; set; }
        /// <summary>
        /// 通风量计算公式
        /// </summary>
        public string WindowDesignFormula { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public Item[] SizePara { get; set; }
    }

    public class XMLDoor 
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Door[] Doors { get; set; }
    }
}
