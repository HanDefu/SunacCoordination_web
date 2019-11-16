﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utility.Extender;
using Common.Utility;
using AFrame.DBUtility;
using SunacCADApp.Entity;
namespace SunacCADApp.Data
{

    /// <summary>
    ///  用户表
    ///</summary>
    public class Sys_UserDB
    {
        ///<summary>
        /// 用户表 分页查询
        ///</summary>
        public static IList<Sys_User> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Sys_User> _sys_users = new List<Sys_User>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , a.*,b.CompanyName
                                                      FROM    dbo.Sys_User  a left join baseCompanyInfo b on a.CompanyID=B.Id
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _sys_users = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_User>(new Sys_User());
            return _sys_users;
        }

        ///<summary>
        /// 用户表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Sys_User a WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 用户表 根据ID查询
        ///</summary>
        public static Sys_User GetSingleEntityById(int id)
        {

            Sys_User sys_user = new Sys_User();
            string sql = string.Format("select top 1 *  from dbo.Sys_User where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_User>(new Sys_User());
        }
        ///<summary>
        /// 用户表 查询根据条件
        ///</summary>
        public static Sys_User GetSingleEntityByparam(string param)
        {

            Sys_User sys_user = new Sys_User();
            string sql = string.Format("select top 1 *  from dbo.Sys_User where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_User>(new Sys_User());

        }

        ///<summary>
        /// 用户表-添加方法
        ///</summary>

        public static int AddHandle(Sys_User sys_user)
        {


            string sql = string.Format(@"INSERT INTO dbo.sys_user(User_Name,User_Psd,True_Name,Telephone,Email,Is_Used,Used_Begin_DateTime,Used_End_DateTime,Is_Internal,Orgnazation_Name,CompanyID,RoleID,AreaID,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},{11},{12},{13},{14},getdate(),{15},'{16}',getdate(),{17},'{18}');select @@IDENTITY", sys_user.User_Name, sys_user.User_Psd, sys_user.True_Name, sys_user.Telephone, sys_user.Email, sys_user.Is_Used, sys_user.Used_Begin_DateTime, sys_user.Used_End_DateTime, sys_user.Is_Internal, sys_user.Orgnazation_Name, sys_user.CompanyID, sys_user.RoleID, sys_user.AreaID, sys_user.Enabled, sys_user.Reorder, sys_user.CreateUserId, sys_user.CreateBy, sys_user.ModifiedUserId, sys_user.ModifiedBy);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(-1);
        }
        ///<summary>
        /// 用户表-修改方法
        ///</summary>

        public static int EditHandle(Sys_User sys_user, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + sys_user.Id : editparam;
            string sql = "UPDATE [dbo].[Sys_User] SET [True_Name]='" + sys_user.True_Name + "',[Telephone]='" + sys_user.Telephone + "',[Email]='" + sys_user.Email + "',[Is_Used]='" + sys_user.Is_Used + "',[Used_Begin_DateTime]='" + sys_user.Used_Begin_DateTime + "',[Used_End_DateTime]='" + sys_user.Used_End_DateTime + "',[Is_Internal]='" + sys_user.Is_Internal + "',[Orgnazation_Name]='" + sys_user.Orgnazation_Name + "',[CompanyID]=" + sys_user.CompanyID + ",[RoleID]=" + sys_user.RoleID + ",[AreaID]=" + sys_user.AreaID + ",[Enabled]=" + sys_user.Enabled + ",[Reorder]=" + sys_user.Reorder + ",[ModifiedUserId]=" + sys_user.ModifiedUserId + ",[ModifiedBy]='" + sys_user.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 用户表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format(@"DELETE FROM dbo.Sys_User WHERE Id={0};
                                                        DELETE FROM dbo.sys_user_area_relation WHERE User_ID='{0}';
                                                        DELETE FROM dbo.Sys_User_Project_Relation WHERE User_ID='{0}'", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 用户表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format(@"DELETE FROM dbo.Sys_User WHERE Id in ({0});
                                                        DELETE FROM dbo.sys_user_area_relation WHERE User_ID in ({0});
                                                        DELETE FROM dbo.Sys_User_Project_Relation WHERE User_ID in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 用户表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_User WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static int HasSysUserByUserName(string userName) 
        {
            string sql = string.Format(@"SELECT top 1 Id FROM dbo.Sys_User WHERE User_Name='{0}'",userName);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        public static int InsertUserAndProjectRelation(int userID, string ObjID,int OperateId=0,string Operater="Test") 
        {
            string sql = string.Format(@"INSERT INTO dbo.sys_user_project_relation(User_ID,Project_ID,CreateOn ,CreateUserId ,CreateBy)
                                                                 VALUES ({0},'{1}',getdate(),{2},'{3}')", userID, ObjID, OperateId, Operater);
            return MsSqlHelperEx.Execute(sql);
        }

        public static int DeleteSysUserProjectRelationByUId(int userId) 
        {
            string sql = string.Format(@"DELETE FROM dbo.sys_user_project_relation WHERE  [User_ID] = {0}", userId);
            return MsSqlHelperEx.Execute(sql);
        }

       


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Sys_User GetSysUserByUserName(string userName) 
        {
            string sql = string.Format(@"SELECT * FROM dbo.Sys_User WHERE USER_NAME ='{0}'",userName);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_User>(new Sys_User());
        }

        public static int ChangePassword(string password, int userid)
        {
            string sql = string.Format(@"UPDATE dbo.Sys_User SET User_Psd='{0}' WHERE Id={1}",password,userid);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<Sys_Role> GetSysRoleListByWh(string wh) 
        {
            string sql = string.Format(@"SELECT Id,Role_Name,Role_Remark FROM dbo.Sys_Role WHERE {0}",wh);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_Role>(new Sys_Role());
        }
            
    }
}
