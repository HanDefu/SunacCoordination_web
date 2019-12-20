using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    ///系统操作日志
    [Serializable]
    public class Sys_Operate_Log
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 日志类型编号
        /// </summary>
        public int SysTypeCode { get; set; }
        /// <summary>
        /// 日志类型名称
        /// </summary>
        public string SysTypeName { get; set; }
        /// <summary>
        /// 详细记录
        /// </summary>
        public string LogInfo { get; set; }

        /// <summary>
        /// 日志记录说明
        /// </summary>
        public string LogDesc { get; set; }
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
