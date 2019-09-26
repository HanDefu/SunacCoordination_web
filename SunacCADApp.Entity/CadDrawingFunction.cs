
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///CAD原型功能区表
    [Serializable]
    public class CadDrawingFunction
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
        /// 功能区类型ID
        /// </summary>
        public int FunctionId { get; set; }

        /// <summary>
        /// 功能区类型名称
        /// </summary>
        public string FunctionName { get; set; }
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