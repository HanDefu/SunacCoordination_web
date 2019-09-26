
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///外窗CAD原型属性表
    [Serializable]
    public class CadDrawingWindowDetail
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// CAD原型信息表ID
        /// </summary>
        public int MId { get; set; }
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
        /// 是否有效
        ///</summary>
        public int Enabled { get; set; }
        /// <summary>
        /// 内容排序
        ///</summary>
        public int Reorder { get; set; }
        /// <summary>
        /// 添加日期
        ///</summary>     
        public DateTime CreateOn { get; set; }
        /// <summary>
        /// 创建用户主键
        ///</summary>
        public int CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        ///</summary>
        public string CreateBy { get; set; }

    }
}