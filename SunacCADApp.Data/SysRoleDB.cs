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
    ///  角色表
    ///</summary>
    public class SysRoleDB
    {
        ///<summary>
        /// 角色表 分页查询
        ///</summary>
        public static IList<Sys_Role> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Sys_Role> _sys_roles = new List<Sys_Role>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Sys_Role  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,{3} T.CreateOn DESC ", _where, start, end, orderby);

            _sys_roles = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_Role>(new Sys_Role());
            return _sys_roles;
        }


        public static IList<Sys_Role> GetSysRoleListById()
        {
            IList<Sys_Role> _sys_roles = new List<Sys_Role>();
            string sql = string.Format(@"SELECT * FROM Sys_Role where [Enabled]=1");
            _sys_roles = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_Role>(new Sys_Role());
            return _sys_roles;
        }
        ///<summary>
        /// 角色表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Sys_Role WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 角色表 根据ID查询
        ///</summary>
        public static Sys_Role GetSingleEntityById(int id)
        {

            Sys_Role sys_role = new Sys_Role();
            string sql = string.Format("select top 1 *  from dbo.Sys_Role where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_Role>(new Sys_Role());
        }
        ///<summary>
        /// 角色表 查询根据条件
        ///</summary>
        public static Sys_Role GetSingleEntityByparam(string param)
        {

            Sys_Role sys_role = new Sys_Role();
            string sql = string.Format("select top 1 *  from dbo.Sys_Role where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_Role>(null);

        }

        ///<summary>
        /// 角色表-添加方法
        ///</summary>

        public static int AddHandle(Sys_Role sys_role)
        {


            string sql = string.Format(@"INSERT INTO dbo.sys_role(Role_Name,Role_Remark,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ('{0}','{1}',{2},{3},getdate(),{4},'{5}',getdate(),{6},'{7}');select @@IDENTITY", sys_role.Role_Name, sys_role.Role_Remark, sys_role.Enabled, sys_role.Reorder, sys_role.CreateUserId, sys_role.CreateBy, sys_role.ModifiedUserId, sys_role.ModifiedBy);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(-1);
        }
        ///<summary>
        /// 角色表-修改方法
        ///</summary>

        public static int EditHandle(Sys_Role sys_role, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + sys_role.Id : editparam;
            string sql = "UPDATE [dbo].[Sys_Role] SET [Role_Name]='" + sys_role.Role_Name + "',[Role_Remark]='" + sys_role.Role_Remark + "',[Enabled]=" + sys_role.Enabled + ",[Reorder]=" + sys_role.Reorder + ",[ModifiedUserId]=" + sys_role.ModifiedUserId + ",[ModifiedBy]='" + sys_role.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 角色表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Role WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 角色表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Role WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 角色表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Role WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        /// <summary>
        ///  角色表-变更排序
        ///</summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SetRecordHandle(int record, int id)
        {
            string sql = string.Format("UPDATE [dbo].[Sys_Role] SET Reorder={0} WHERE Id={1}", record, id);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}
