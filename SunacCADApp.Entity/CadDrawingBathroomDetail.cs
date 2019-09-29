using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///卫生间CAD原型属性表
    [Serializable]
    public class CadDrawingBathroomDetail
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