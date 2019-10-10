
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///CAD原型图纸信息表
    [Serializable]
    public class CadDrawingDWG
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// CAD原型主表
        /// </summary>
        public int MId { get; set; }
        /// <summary>
        /// CAD文件地址IMG
        /// </summary>
        public string DWGPath { get; set; }
        /// <summary>
        /// CAD文件类别
        /// </summary>
        public string FileClass { get; set; }
        /// <summary>
        /// CAD文件地址DWG
        /// </summary>
        public string CADPath { get; set; }
        /// <summary>
        /// CAD文件类型
        /// </summary>
        public string CADType { get; set; }
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