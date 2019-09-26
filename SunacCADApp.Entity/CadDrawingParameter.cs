
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///门窗原型尺寸表
    [Serializable]
    public class CadDrawingParameter
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 原型信息主表ID
        /// </summary>
        public int MId { get; set; }
        /// <summary>
        /// 尺寸代号
        /// </summary>
        public string SizeNo { get; set; }
        /// <summary>
        /// 值类型
        /// </summary>
        public int ValueType { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Val { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public decimal MinValue { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public decimal MaxValue { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public decimal DefaultValue { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

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