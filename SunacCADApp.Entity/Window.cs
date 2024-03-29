﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public  class Window
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
        /// 功能区域
        /// </summary>
        public Area[] Areas { get; set; }

        /// <summary>
        /// 功能区类型
        /// </summary>
        public Function[] Functions { get; set; }

        /// <summary>
        /// 集团
        /// </summary>
        public int Scope { get; set; }
        /// <summary>
        /// 原型类型
        /// </summary>
        public int DrawingType { get; set; }
        /// <summary>
        /// 动态类型
        /// </summary>
        public int DynamicType { get; set; }
        /// <summary>
        /// 开启类型
        /// </summary>
        public int WindowOpenTypeId { get; set; }
        /// <summary>
        /// 开启类型名称
        /// </summary>
        public string WindowOpenTypeName { get; set; }
        /// <summary>
        /// 开启扇数量
        /// </summary>
        public int WindowOpenQtyId { get; set; }

        /// <summary>
        /// 开启扇数量名称
        /// </summary>
        public string WindowOpenQtyName { get; set; }
        /// <summary>
        /// 是否转角窗
        /// </summary>
        public int WindowHasCorner { get; set; }
        /// <summary>
        /// 是否对称窗
        /// </summary>
        public int WindowHasSymmetry { get; set; }
        /// <summary>
        /// 宽度尺寸最小值
        /// </summary>
        public decimal WindowSizeMin { get; set; }
        /// <summary>
        /// 宽度尺寸最大值
        /// </summary>
        public decimal WindowSizeMax { get; set; }
        /// <summary>
        /// 通风量计算公式
        /// </summary>
        public string WindowDesignFormula { get; set; }
        /// <summary>
        /// 通风量
        /// </summary>
        public int WindowVentilationQuantity { get; set; }
        /// <summary>
        /// 塞缝尺寸
        /// </summary>
        public int WindowPlugslotSize { get; set; }
        /// <summary>
        /// 是否有附框
        /// </summary>
        public int WindowAuxiliaryFrame { get; set; }
        /// <summary>
        /// 功能区
        /// </summary>
        public string WindowFunctionalArea { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public Item[] SizePara { get; set; }

    }
}
