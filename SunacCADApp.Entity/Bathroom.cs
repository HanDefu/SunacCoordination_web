using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public class Bathroom
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
        /// 卫生间类型
        /// </summary>
        public int BathroomType { get; set; }
        /// <summary>
        /// 卫生间类型名称
        /// </summary>
        public string BathroomTypeName { get; set; }
        /// <summary>
        /// 卫生间门窗位置
        /// </summary>
        public int BathroomDoorWindowPosition { get; set; }
        /// <summary>
        /// 卫生间门窗位置名称
        /// </summary>
        public string BathroomDoorWindowPositionName { get; set; }
        /// <summary>
        /// 卫生间短边最小值
        /// </summary>
        public decimal BathroomShortSideMin { get; set; }
        /// <summary>
        /// 卫生间短边最大值
        /// </summary>
        public decimal BathroomShortSideMax { get; set; }
        /// <summary>
        /// 卫生间长边最小值
        /// </summary>
        public decimal BathroomLongSizeMin { get; set; }
        /// <summary>
        /// 卫生间长边最大值
        /// </summary>
        public decimal BathroomLongSizeMax { get; set; }
        /// <summary>
        /// 卫生间水盆柜宽
        /// </summary>
        public int BathroomBasinSize { get; set; }

        /// <summary>
        /// 卫生间水盆柜宽名称
        /// </summary>
        public string BathroomBasinSizeName { get; set; }

        /// <summary>
        /// 卫生间马桶尺寸
        /// </summary>
        public int BathroomClosestoolSize { get; set; }

        /// <summary>
        /// 卫生间马桶尺寸名称
        /// </summary>
        public int BathroomClosestoolSizeName { get; set; }
    }

    public class XMLBathroom 
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Bathroom[] Bathrooms { get; set; }
    }
}
