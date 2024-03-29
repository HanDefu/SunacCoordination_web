﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///门CAD原型属性表
    [Serializable]
    public class CadDrawingDoorDetail
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        public int MId { get; set; }
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
        public int WindowSizeMin { get; set; }
        /// <summary>
        /// 宽度尺寸范围最大值
        /// </summary>
        public int WindowSizeMax { get; set; }
        /// <summary>
        /// 通风量计算公式
        /// </summary>
        public string WindowDesignFormula { get; set; }
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
        /// <summary>
        /// 修改添加日期
        ///</summary>
        public DateTime ModifiedOn { get; set; }
        /// <summary>
        /// 修改用户主键
        ///</summary>
        public int ModifiedUserId { get; set; }
        /// <summary>
        /// 修改用户
        ///</summary>
        public string ModifiedBy { get; set; }

    }
}