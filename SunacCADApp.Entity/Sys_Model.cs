
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///模块编号
    [Serializable]
    public class Sys_Model
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 模块权限编号
        /// </summary>
        public string Model_Code { get; set; }
        /// <summary>
        /// 模块权限名称
        /// </summary>
        public string Model_Name { get; set; }
        /// <summary>
        /// 模块权限描述
        /// </summary>
        public string Model_Remark { get; set; }
        /// <summary>
        /// 父类模块ID
        /// </summary>
        public int Parent_ID { get; set; }
        /// <summary>
        /// 是否权限是菜单
        /// </summary>
        public string IsPower { get; set; }
        /// <summary>
        /// 模块链接
        /// </summary>
        public string Model_URL { get; set; }

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