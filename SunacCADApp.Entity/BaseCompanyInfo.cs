
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///机构信息
    [Serializable]
    public class BaseCompanyInfo
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 机构代码
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string CompanyRemark { get; set; }

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