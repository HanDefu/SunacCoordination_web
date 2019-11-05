using System;
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
    ///  用户与项目
    ///</summary>
    public class SysUserProjectRelationDB
    {
        ///<summary>
        /// 用户与项目 分页查询
        ///</summary>
        public static IList<Sys_User_Project_Relation> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Sys_User_Project_Relation> _sys_user_project_relations = new List<Sys_User_Project_Relation>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Sys_User_Project_Relation  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _sys_user_project_relations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_User_Project_Relation>(new Sys_User_Project_Relation());
            return _sys_user_project_relations;
        }

        ///<summary>
        /// 用户与项目  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Sys_User_Project_Relation WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 用户与项目 根据ID查询
        ///</summary>
        public static Sys_User_Project_Relation GetSingleEntityById(int id)
        {

            Sys_User_Project_Relation sys_user_project_relation = new Sys_User_Project_Relation();
            string sql = string.Format("select top 1 *  from dbo.Sys_User_Project_Relation where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_User_Project_Relation>(new Sys_User_Project_Relation());
        }
        ///<summary>
        /// 用户与项目 查询根据条件
        ///</summary>
        public static Sys_User_Project_Relation GetSingleEntityByparam(string param)
        {

            Sys_User_Project_Relation sys_user_project_relation = new Sys_User_Project_Relation();
            string sql = string.Format("select top 1 *  from dbo.Sys_User_Project_Relation where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_User_Project_Relation>(new Sys_User_Project_Relation());

        }

        ///<summary>
        /// 用户与项目-添加方法
        ///</summary>

        public static int AddHandle(Sys_User_Project_Relation sys_user_project_relation)
        {


            string sql = string.Format(@"INSERT INTO dbo.sys_user_project_relation(User_ID,Project_ID,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},'{1}',{2},{3},getdate(),{4},'{5}',getdate(),{6},'{7}')", sys_user_project_relation.User_ID, sys_user_project_relation.Project_ID, sys_user_project_relation.Enabled, sys_user_project_relation.Reorder, sys_user_project_relation.CreateUserId, sys_user_project_relation.CreateBy, sys_user_project_relation.ModifiedUserId, sys_user_project_relation.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 用户与项目-修改方法
        ///</summary>

        public static int EditHandle(Sys_User_Project_Relation sys_user_project_relation, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + sys_user_project_relation.Id : editparam;
            string sql = "UPDATE [dbo].[Sys_User_Project_Relation] SET [User_ID]=" + sys_user_project_relation.User_ID + ",[Project_ID]='" + sys_user_project_relation.Project_ID + "',[Enabled]=" + sys_user_project_relation.Enabled + ",[Reorder]=" + sys_user_project_relation.Reorder + ",[ModifiedUserId]=" + sys_user_project_relation.ModifiedUserId + ",[ModifiedBy]='" + sys_user_project_relation.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 用户与项目-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_User_Project_Relation WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 用户与项目-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_User_Project_Relation WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 用户与项目-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_User_Project_Relation WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        /// <summary>
        ///  用户与项目-变更排序
        ///</summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SetRecordHandle(int record, int id)
        {
            string sql = string.Format("UPDATE [dbo].[Sys_User_Project_Relation] SET Reorder={0} WHERE Id={1}", record, id);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<SysUserAndProject> GetUserAndProjectWhere(int  userID) 
        {
            string sql = string.Format(@"   SELECT a.Project_ID AS ProjectID,b.POST1 AS ProjectName,c.OrgName AS ProjectArea,d.OrgName AS ProjectCity,b.MTIMESTAMP AS ModifiedOn FROM dbo.Sys_User_Project_Relation a
                                                    INNER JOIN dbo.Bas_Idm_Project b ON a.Project_ID=b.POSID
                                                    INNER JOIN dbo.Bas_Idm_Organization c ON c.OrgCode=b.PAREA_GS_NAME AND c.OrgTypeCode='A'
                                                    INNER JOIN dbo.Bas_Idm_Organization d ON b.PCITY=d.OrgCode 
                                                    WHERE a.[User_ID]='{0}'", userID);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<SysUserAndProject>(new SysUserAndProject());
        }

    }
}