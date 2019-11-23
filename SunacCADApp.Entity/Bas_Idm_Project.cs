
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///项目信息表
    [Serializable]
    public class Bas_Idm_Project
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string POSID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string POST1 { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string PLONG_TX { get; set; }
        /// <summary>
        /// 公司代码
        /// </summary>
        public string PBUKRS { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string COMP_NAME { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string PAREA_GS_NAME { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string PAREANAME { get; set; }
        /// <summary>
        /// 项目阶段
        /// </summary>
        public int STEP_ID { get; set; }
        /// <summary>
        /// 项目预计开始时间
        /// </summary>
        public DateTime PLFAZ { get; set; }
        /// <summary>
        /// 项目预计结束时间
        /// </summary>
        public DateTime PLSEZ { get; set; }
        /// <summary>
        /// 城市公司
        /// </summary>
        public string PCITY { get; set; }
        /// <summary>
        /// 城市公司名称
        /// </summary>
        public string PCITYNAME { get; set; }

        /// <summary>
        /// 所属城市
        /// </summary>
        public string CITY_NAME { get; set; }
        /// <summary>
        /// 项目结构层级
        /// </summary>
        public int STUFE { get; set; }
        /// <summary>
        /// 地块编码
        /// </summary>
        public string DK_POSID { get; set; }
        /// <summary>
        /// 地块名称
        /// </summary>
        public string DK_NAME { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime MTIMESTAMP { get; set; }
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





