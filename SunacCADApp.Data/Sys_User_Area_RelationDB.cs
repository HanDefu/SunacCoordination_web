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
    ///  用户与区域关联表
    ///</summary>
    public class Sys_User_Area_RelationDB
    {
        ///<summary>
        /// 用户与区域关联表 分页查询
        ///</summary>
        public static IList<Sys_User_Area_Relation> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Sys_User_Area_Relation> _sys_user_area_relations = new List<Sys_User_Area_Relation>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Sys_User_Area_Relation  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _sys_user_area_relations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_User_Area_Relation>(new Sys_User_Area_Relation());
            return _sys_user_area_relations;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="_where"></param>
        /// <returns></returns>
        public static IList<Sys_User_Area_Relation> GetListSysUserAreaRelationByWhere(string _where) 
        {
            IList<Sys_User_Area_Relation> _sys_user_area_relations = new List<Sys_User_Area_Relation>();
            string sql = string.Format(@"SELECT * FROM dbo.Sys_User_Area_Relation WHERE 1=1 {0}", _where);
            _sys_user_area_relations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_User_Area_Relation>(new Sys_User_Area_Relation());
            return _sys_user_area_relations;
        }

        ///<summary>
        /// 用户与区域关联表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Sys_User_Area_Relation WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 用户与区域关联表 根据ID查询
        ///</summary>
        public static Sys_User_Area_Relation GetSingleEntityById(int id)
        {

            Sys_User_Area_Relation sys_user_area_relation = new Sys_User_Area_Relation();
            string sql = string.Format("select top 1 *  from dbo.Sys_User_Area_Relation where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_User_Area_Relation>(new Sys_User_Area_Relation());
        }
        ///<summary>
        /// 用户与区域关联表 查询根据条件
        ///</summary>
        public static Sys_User_Area_Relation GetSingleEntityByparam(string param)
        {

            Sys_User_Area_Relation sys_user_area_relation = new Sys_User_Area_Relation();
            string sql = string.Format("select top 1 *  from dbo.Sys_User_Area_Relation where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_User_Area_Relation>(new Sys_User_Area_Relation());

        }

        ///<summary>
        /// 用户与区域关联表-添加方法
        ///</summary>

        public static int AddHandle(Sys_User_Area_Relation sys_user_area_relation)
        {


            string sql = string.Format(@"INSERT INTO dbo.sys_user_area_relation(User_ID,Area_ID,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},{1},{2},{3},getdate(),{4},'{5}',getdate(),{6},'{7}')", sys_user_area_relation.User_ID, sys_user_area_relation.Area_ID, sys_user_area_relation.Enabled, sys_user_area_relation.Reorder, sys_user_area_relation.CreateUserId, sys_user_area_relation.CreateBy, sys_user_area_relation.ModifiedUserId, sys_user_area_relation.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 用户与区域关联表-修改方法
        ///</summary>

        public static int EditHandle(Sys_User_Area_Relation sys_user_area_relation, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + sys_user_area_relation.Id : editparam;
            string sql = "UPDATE [dbo].[Sys_User_Area_Relation] SET [User_ID]=" + sys_user_area_relation.User_ID + ",[Area_ID]=" + sys_user_area_relation.Area_ID + ",[Enabled]=" + sys_user_area_relation.Enabled + ",[Reorder]=" + sys_user_area_relation.Reorder + ",[ModifiedUserId]=" + sys_user_area_relation.ModifiedUserId + ",[ModifiedBy]='" + sys_user_area_relation.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 用户与区域关联表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_User_Area_Relation WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 用户与区域关联表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_User_Area_Relation WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 用户与区域关联表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_User_Area_Relation WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}
