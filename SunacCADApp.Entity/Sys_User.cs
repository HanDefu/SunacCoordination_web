
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
namespace SunacCADApp.Entity
{

    ///用户表
    [Serializable]
    public class Sys_User
    {
        /// <summary>
        /// 主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string User_Name { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string User_Psd { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string True_Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public string Is_Used { get; set; }
        /// <summary>
        /// 账户有效期开始
        /// </summary>
        public DateTime Used_Begin_DateTime { get; set; }
        /// <summary>
        /// 账号有效期结束
        /// </summary>
        public DateTime Used_End_DateTime { get; set; }
        /// <summary>
        /// 是否内部员工
        /// </summary>
        public int Is_Internal { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string Orgnazation_Name { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 组织编码
        /// </summary>
        public string UserDeptNo { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaID { get; set; }

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


    [XmlRoot("User")]
    [Serializable]
    public class XMLUser 
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否内部用户
        /// </summary>
        public int IsInternal { get; set; }
    }

    [XmlRoot("Root")]
    [Serializable]
    public class XMLResultUser
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Code{get;set;}
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message{get;set;}
        /// <summary>
        /// 用户信息
        /// </summary>
        public XMLUser User{get;set;}

    }

}