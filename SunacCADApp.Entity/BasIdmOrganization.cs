
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{

    ///IDM公司组织
    [Serializable]
    public class BasIdmOrganization
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 组织编码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 组织类型编号
        /// </summary>
        public string OrgTypeCode { get; set; }
        /// <summary>
        /// 组织类型描述
        /// </summary>
        public string OrgTypeDesc { get; set; }
        /// <summary>
        /// 上级组织编码
        /// </summary>
        public string UpOrgCode { get; set; }
        /// <summary>
        /// 上级组织名称
        /// </summary>
        public string UpOrgName { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public string OrgDate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string OrgTime { get; set; }

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





