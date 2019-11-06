using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public class Airconditioner
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
        /// 空调匹数
        /// </summary>
        public int AirconditionerPower { get; set; }

        /// <summary>
        /// 空调匹数名称
        /// </summary>
        public string AirconditionerPowerName { get; set; }
        /// <summary>
        /// 空调宽最小值
        /// </summary>
        public int AirconditionerMinWidth { get; set; }
        /// <summary>
        /// 空调长最小值
        /// </summary>
        public int AirconditionerMinLength { get; set; }
        /// <summary>
        /// 空调冷凝管位置
        /// </summary>
        public int AirconditionerPipePosition { get; set; }

        public string AirconditionerPipePositionName { get; set; }
        /// <summary>
        /// 空调是否含雨水
        /// </summary>
        public int AirconditionerIsRainPipe { get; set; }


        /// <summary>
        /// 空调雨水管位置
        /// </summary>
        public int AirconditionerRainPipePosition { get; set; }

        public string AirconditionerRainPipePositionName { get; set; }

        /// <summary>
        /// 空调宽
        /// </summary>
        public int AirconditionerWidth { get; set; }
        /// <summary>
        /// 空调高
        /// </summary>
        public int AirconditionerHeight { get; set; }
        /// <summary>
        /// 空调进深
        /// </summary>
        public int AirconditionerDepth { get; set; }
    }

    public class XMLAirconditioner
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Airconditioner[] Airconditioners { get; set; }
    }
}
