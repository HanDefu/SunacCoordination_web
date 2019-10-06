using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public class Kitchen
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
        /// 厨房类型
        /// </summary>
        public int KitchenType { get; set; }
        /// <summary>
        /// 厨房类型名称
        /// </summary>
        public string KitchenTypeName { get; set; }
        /// <summary>
        /// 厨房门窗位置
        /// </summary>
        public int KitchenPosition { get; set; }
        /// <summary>
        /// 厨房门窗名称
        /// </summary>
        public string KitchenPositionName { get; set; }
        /// <summary>
        /// 是否含排气管
        /// </summary>
        public int KitchenIsAirduct { get; set; }
        /// <summary>
        /// 厨房开间尺寸最小值
        /// </summary>
        public int KitchenOpenSizeMin { get; set; }
        /// <summary>
        /// 厨房开间尺寸最大值
        /// </summary>
        public int KitchenOpenSizeMax { get; set; }
        /// <summary>
        /// 厨房进深尺寸最小值
        /// </summary>
        public int KitchenDepthsizeMin { get; set; }
        /// <summary>
        /// 厨房进深尺寸最大值
        /// </summary>
        public int KitchenDepthsizeMax { get; set; }
        /// <summary>
        /// 厨房水盆尺寸
        /// </summary>
        public int KitchenBasinSize { get; set; }
        /// <summary>
        /// 厨房水盆尺寸名称
        /// </summary>
        public string KitchenBasinSizeName { get; set; }
        /// <summary>
        /// 厨房冰箱尺寸
        /// </summary>
        public int KitchenFridgSize { get; set; }

        /// <summary>
        /// 厨房冰箱尺寸名称
        /// </summary>
        public string KitchenFridgSizeName { get; set; }
        /// <summary>
        /// 厨房灶台尺寸
        /// </summary>
        public int KitchenHearthSize { get; set; }

        /// <summary>
        /// 厨房灶台尺寸名称
        /// </summary>
        public string KitchenHearthSizeName { get; set; }
    }

    public class XMLKitchen
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Kitchen[] Kitchens { get; set; }
    }
}
