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
    ///  模块编号
    ///</summary>
    public class Sys_ModelDB
    {
        ///<summary>
        /// 模块编号 分页查询
        ///</summary>
        public static IList<Sys_Model> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Sys_Model> _sys_models = new List<Sys_Model>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Sys_Model  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC, {3} T.CreateOn DESC", _where, start, end, orderby);

            _sys_models = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_Model>(new Sys_Model());
            return _sys_models;
        }

        ///<summary>
        /// 模块编号  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Sys_Model WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 模块编号 根据ID查询
        ///</summary>
        public static Sys_Model GetSingleEntityById(int id)
        {

            Sys_Model sys_model = new Sys_Model();
            string sql = string.Format("select top 1 *  from dbo.Sys_Model where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_Model>(new Sys_Model());
        }
        ///<summary>
        /// 模块编号 查询根据条件
        ///</summary>
        public static Sys_Model GetSingleEntityByparam(string param)
        {

            Sys_Model sys_model = new Sys_Model();
            string sql = string.Format("select top 1 *  from dbo.Sys_Model where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_Model>(new Sys_Model());

        }

        ///<summary>
        /// 模块编号-添加方法
        ///</summary>

        public static int AddHandle(Sys_Model sys_model)
        {


            string sql = string.Format(@"INSERT INTO dbo.sys_model(Model_Code,Model_Name,Model_Remark,Parent_ID,IsPower,Model_URL,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ('{0}','{1}','{2}',{3},'{4}','{5}',{6},{7},getdate(),{8},'{9}',getdate(),{10},'{11}')", sys_model.Model_Code, sys_model.Model_Name, sys_model.Model_Remark, sys_model.Parent_ID, sys_model.IsPower, sys_model.Model_URL, sys_model.Enabled, sys_model.Reorder, sys_model.CreateUserId, sys_model.CreateBy, sys_model.ModifiedUserId, sys_model.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 模块编号-修改方法
        ///</summary>

        public static int EditHandle(Sys_Model sys_model, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + sys_model.Id : editparam;
            string sql = "UPDATE [dbo].[Sys_Model] SET [Model_Code]='" + sys_model.Model_Code + "',[Model_Name]='" + sys_model.Model_Name + "',[Model_Remark]='" + sys_model.Model_Remark + "',[Parent_ID]=" + sys_model.Parent_ID + ",[IsPower]='" + sys_model.IsPower + "',[Model_URL]='" + sys_model.Model_URL + "',[Enabled]=" + sys_model.Enabled + ",[Reorder]=" + sys_model.Reorder + ",[ModifiedUserId]=" + sys_model.ModifiedUserId + ",[ModifiedBy]='" + sys_model.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 模块编号-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Model WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 模块编号-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Model WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 模块编号-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Model WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        public static IList<Sys_Model> GetSysModeByRoleID(int roleID) 
        {
            IList<Sys_Model> _sys_models = new List<Sys_Model>();
            string sql =string.Format(@"select b.*  from dbo.Sys_Role_Model_Relation a 
                                                       inner join Sys_Model b on a.Model_Id=b.Id where a.Role_Id='{0}'",roleID);
            _sys_models = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_Model>(new Sys_Model());
            return _sys_models;
        }

    }
}