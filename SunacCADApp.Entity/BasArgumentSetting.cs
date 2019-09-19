
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///参数配置表
    [Serializable]
    public class BasArgumentSetting
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ArgumentText { get; set; }
        /// <summary>
        /// 参数编号
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 父类ID
        /// </summary>
        public int ParentID { get; set; }

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





