
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///项目对应CAD文件
    [Serializable]
    public class Bas_Idm_ProjectFile
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 保存文件名称
        /// </summary>
        public string SaveName { get; set; }
        /// <summary>
        /// 上传文件地址
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// 文件目录ID
        /// </summary>
        public int DirId { get; set; }
        /// <summary>
        /// 文件目录名称
        /// </summary>
        public string DirName { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int OID { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string Project_ID { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
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