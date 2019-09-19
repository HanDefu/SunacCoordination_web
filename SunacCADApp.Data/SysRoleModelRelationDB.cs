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
    ///  角色与模块权限表
    ///</summary>
    public class SysRoleModelRelationDB
    {
        ///<summary>
        /// 角色与模块权限表 分页查询
        ///</summary>
        public static IList<Sys_Role_Model_Relation> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Sys_Role_Model_Relation> _sys_role_model_relations = new List<Sys_Role_Model_Relation>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Sys_Role_Model_Relation  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _sys_role_model_relations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_Role_Model_Relation>(new Sys_Role_Model_Relation());
            return _sys_role_model_relations;
        }

        ///<summary>
        /// 角色与模块权限表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Sys_Role_Model_Relation WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 角色与模块权限表 根据ID查询
        ///</summary>
        public static Sys_Role_Model_Relation GetSingleEntityById(int id)
        {

            Sys_Role_Model_Relation sys_role_model_relation = new Sys_Role_Model_Relation();
            string sql = string.Format("select top 1 *  from dbo.Sys_Role_Model_Relation where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_Role_Model_Relation>(new Sys_Role_Model_Relation());
        }
        ///<summary>
        /// 角色与模块权限表 查询根据条件
        ///</summary>
        public static Sys_Role_Model_Relation GetSingleEntityByparam(string param)
        {

            Sys_Role_Model_Relation sys_role_model_relation = new Sys_Role_Model_Relation();
            string sql = string.Format("select top 1 *  from dbo.Sys_Role_Model_Relation where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_Role_Model_Relation>(new Sys_Role_Model_Relation());

        }

        ///<summary>
        /// 角色与模块权限表-添加方法
        ///</summary>

        public static int AddHandle(Sys_Role_Model_Relation sys_role_model_relation)
        {


            string sql = string.Format(@"INSERT INTO dbo.sys_role_model_relation(Role_Id,Model_Id,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},{1},{2},{3},getdate(),{4},'{5}',getdate(),{6},'{7}')", sys_role_model_relation.Role_Id, sys_role_model_relation.Model_Id, sys_role_model_relation.Enabled, sys_role_model_relation.Reorder, sys_role_model_relation.CreateUserId, sys_role_model_relation.CreateBy, sys_role_model_relation.ModifiedUserId, sys_role_model_relation.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 角色与模块权限表-修改方法
        ///</summary>

        public static int EditHandle(Sys_Role_Model_Relation sys_role_model_relation, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + sys_role_model_relation.Id : editparam;
            string sql = "UPDATE [dbo].[Sys_Role_Model_Relation] SET [Role_Id]=" + sys_role_model_relation.Role_Id + ",[Model_Id]=" + sys_role_model_relation.Model_Id + ",[Enabled]=" + sys_role_model_relation.Enabled + ",[Reorder]=" + sys_role_model_relation.Reorder + ",[ModifiedUserId]=" + sys_role_model_relation.ModifiedUserId + ",[ModifiedBy]='" + sys_role_model_relation.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 角色与模块权限表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Role_Model_Relation WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 角色与模块权限表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Role_Model_Relation WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 角色与模块权限表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Role_Model_Relation WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        /// <summary>
        ///  角色与模块权限表-变更排序
        ///</summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SetRecordHandle(int record, int id)
        {
            string sql = string.Format("UPDATE [dbo].[Sys_Role_Model_Relation] SET Reorder={0} WHERE Id={1}", record, id);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}
