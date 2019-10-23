using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utility.Extender;
using Common.Utility;
using AFrame.DBUtility;
using AFrame.DBUtility;
using SunacCADApp.Entity;
namespace SunacCADApp.Data
{

    /// <summary>
    ///  参数配置表
    ///</summary>
    public class BasArgumentSettingDB
    {
        ///<summary>
        /// 参数配置表 分页查询
        ///</summary>
        public static IList<BasArgumentSetting> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<BasArgumentSetting> _basargumentsettings = new List<BasArgumentSetting>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.BasArgumentSetting  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _basargumentsettings = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<BasArgumentSetting>(new BasArgumentSetting());
            return _basargumentsettings;
        }


        public static IList<BasArgumentSetting> GetBasArgumentSettingParent() 
        {
            IList<BasArgumentSetting> _basargumentsettings = new List<BasArgumentSetting>();
            string _sql = @"select Id,ArgumentText,TypeCode,TypeName,ParentID from BasArgumentSetting  where Enabled=1 and ParentID=0 order by ModifiedOn desc";
            _basargumentsettings = MsSqlHelperEx.ExecuteDataTable(_sql).ConvertListModel<BasArgumentSetting>(new BasArgumentSetting());
            return _basargumentsettings;
        }


        public static IList<BasArgumentSetting> GetBasArgumentSettingChild()
        {
            IList<BasArgumentSetting> _basargumentsettings = new List<BasArgumentSetting>();
            string _sql = @"select Id,ArgumentText,TypeCode,TypeName,ParentID from BasArgumentSetting  where Enabled=1 and ParentID!=0 order by ModifiedOn asc";
            _basargumentsettings = MsSqlHelperEx.ExecuteDataTable(_sql).ConvertListModel<BasArgumentSetting>(new BasArgumentSetting());
            return _basargumentsettings;
        }



        public static IList<BasArgumentSetting> GetBasArgumentSettingByWhere(string _wh)
        {
            IList<BasArgumentSetting> _basargumentsettings = new List<BasArgumentSetting>();
            string _sql = string.Format(@"select Id,ArgumentText,TypeCode,TypeName,ParentID from BasArgumentSetting  
                                                          where Enabled=1 and  {0} order by ModifiedOn asc", _wh);
            _basargumentsettings = MsSqlHelperEx.ExecuteDataTable(_sql).ConvertListModel<BasArgumentSetting>(new BasArgumentSetting());
            return _basargumentsettings;
        }


        public static IList<ArgumentSetting> GetXMLBasArgumentSettingByWhere(string _where) 
        {
            IList<ArgumentSetting> _basargumentsettings = new List<ArgumentSetting>();
            string _sql = string.Format(@"SELECT a.Id,a.ArgumentText,a.TypeCode,a.TypeName FROM BasArgumentSetting a inner join BasArgumentSetting b ON a.ParentID=b.Id
                                                          where a.Enabled=1 {0}  order by a.ModifiedOn asc", _where);
            _basargumentsettings = MsSqlHelperEx.ExecuteDataTable(_sql).ConvertListModel<ArgumentSetting>(new ArgumentSetting());
            return _basargumentsettings;
        }

        ///<summary>
        /// 参数配置表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.BasArgumentSetting WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 参数配置表 根据ID查询
        ///</summary>
        public static BasArgumentSetting GetSingleEntityById(int id)
        {

            BasArgumentSetting basargumentsetting = new BasArgumentSetting();
            string sql = string.Format("select top 1 *  from dbo.BasArgumentSetting where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BasArgumentSetting>(new BasArgumentSetting());
        }
        ///<summary>
        /// 参数配置表 查询根据条件
        ///</summary>
        public static BasArgumentSetting GetSingleEntityByparam(string param)
        {

            BasArgumentSetting basargumentsetting = new BasArgumentSetting();
            string sql = string.Format("select top 1 *  from dbo.BasArgumentSetting where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BasArgumentSetting>(new BasArgumentSetting());

        }

        ///<summary>
        /// 参数配置表-添加方法
        ///</summary>

        public static int AddHandle(BasArgumentSetting basargumentsetting)
        {


            string sql = string.Format(@"INSERT INTO dbo.basargumentsetting(ArgumentText,TypeCode,TypeName,ParentID,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ('{0}','{1}','{2}',{3},{4},{5},getdate(),{6},'{7}',getdate(),{8},'{9}')", basargumentsetting.ArgumentText, basargumentsetting.TypeCode, basargumentsetting.TypeName, basargumentsetting.ParentID, basargumentsetting.Enabled, basargumentsetting.Reorder, basargumentsetting.CreateUserId, basargumentsetting.CreateBy, basargumentsetting.ModifiedUserId, basargumentsetting.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 参数配置表-修改方法
        ///</summary>

        public static int EditHandle(BasArgumentSetting basargumentsetting, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + basargumentsetting.Id : editparam;
            string sql = "UPDATE [dbo].[BasArgumentSetting] SET [ArgumentText]='" + basargumentsetting.ArgumentText + "',[TypeCode]='" + basargumentsetting.TypeCode + "',[TypeName]='" + basargumentsetting.TypeName + "',[ParentID]=" + basargumentsetting.ParentID + ",[Enabled]=" + basargumentsetting.Enabled + ",[Reorder]=" + basargumentsetting.Reorder + ",[ModifiedUserId]=" + basargumentsetting.ModifiedUserId + ",[ModifiedBy]='" + basargumentsetting.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 参数配置表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.BasArgumentSetting WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }


        /// <summary>
        ///   / 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int DeleteArgumentSettingById(int Id) 
        {

            string sql = string.Format(@"DELETE FROM dbo.BasArgumentSetting WHERE Id={0};
                                                        DELETE  FROM BasArgumentSetting WHERE ParentID={0};", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 参数配置表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.BasArgumentSetting WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 参数配置表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.BasArgumentSetting WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static string GetArgumentSettingCode() 
        {
            string sql = string.Format(@"SELECT CONCAT('Setting_',COUNT(*)+1) AS TypeCode FROM dbo.BasArgumentSetting WHERE ParentID=0");
            return MsSqlHelperEx.ExecuteScalar(sql).ConventToString(string.Empty);
        }

    }
}
