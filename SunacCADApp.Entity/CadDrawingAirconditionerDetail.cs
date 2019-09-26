
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///空调CAD原型属性表
    [Serializable]
    public class CadDrawingAirconditionerDetail
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
        /// 空调匹数
        /// </summary>
        public int AirconditionerPower { get; set; }
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
        /// <summary>
        /// 空调是否含雨水
        /// </summary>
        public int AirconditionerIsRainPipe { get; set; }

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