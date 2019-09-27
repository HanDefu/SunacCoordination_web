
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///厨房CAD原型属性表
    [Serializable]
    public class CadDrawingKitchenDetail
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 原型主表ID
        /// </summary>
        public int MId { get; set; }
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