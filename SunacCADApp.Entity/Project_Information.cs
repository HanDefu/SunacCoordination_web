
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///项目信息表
    [Serializable]
    public class Project_Information
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 公司代码
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 项目阶段
        /// </summary>
        public string ProjectPhase { get; set; }
        /// <summary>
        /// 城市公司
        /// </summary>
        public string CityCompany { get; set; }
        /// <summary>
        /// 所属城市ID
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// 所属城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 项目预计开始时间
        /// </summary>
        public DateTime Project_StartTime { get; set; }
        /// <summary>
        /// 项目预计结束时间
        /// </summary>
        public DateTime ProjectEndTime { get; set; }
        /// <summary>
        /// 项目结构层级
        /// </summary>
        public int ProjectStufe { get; set; }
        /// <summary>
        /// 地块编号
        /// </summary>
        public string PlaceCode { get; set; }
        /// <summary>
        /// 地块名称
        /// </summary>
        public string PlaceName { get; set; }

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