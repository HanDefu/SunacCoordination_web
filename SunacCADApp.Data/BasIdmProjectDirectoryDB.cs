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
    ///  项目文件目录
    ///</summary>
    public class BasIdmProjectDirectoryDB
    {
        ///<summary>
        /// 项目文件目录 分页查询
        ///</summary>
        public static IList<Bas_Idm_ProjectDirectory> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Bas_Idm_ProjectDirectory> _bas_idm_projectdirectorys = new List<Bas_Idm_ProjectDirectory>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Bas_Idm_ProjectDirectory  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _bas_idm_projectdirectorys = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Bas_Idm_ProjectDirectory>(new Bas_Idm_ProjectDirectory());
            return _bas_idm_projectdirectorys;
        }

        ///<summary>
        /// 项目文件目录  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Bas_Idm_ProjectDirectory WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 项目文件目录 根据ID查询
        ///</summary>
        public static Bas_Idm_ProjectDirectory GetSingleEntityById(int id)
        {

            Bas_Idm_ProjectDirectory bas_idm_projectdirectory = new Bas_Idm_ProjectDirectory();
            string sql = string.Format("select top 1 *  from dbo.Bas_Idm_ProjectDirectory where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Bas_Idm_ProjectDirectory>(new Bas_Idm_ProjectDirectory());
        }
        ///<summary>
        /// 项目文件目录 查询根据条件
        ///</summary>
        public static Bas_Idm_ProjectDirectory GetSingleEntityByparam(string param)
        {

            Bas_Idm_ProjectDirectory bas_idm_projectdirectory = new Bas_Idm_ProjectDirectory();
            string sql = string.Format("select top 1 *  from dbo.Bas_Idm_ProjectDirectory where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Bas_Idm_ProjectDirectory>(new Bas_Idm_ProjectDirectory());

        }

        ///<summary>
        /// 项目文件目录-添加方法
        ///</summary>

        public static int AddHandle(Bas_Idm_ProjectDirectory bas_idm_projectdirectory)
        {


            string sql = string.Format(@"INSERT INTO dbo.bas_idm_projectdirectory(DirName,ParentDirName,OID,Enabled ,Reorder ,
                                                                            CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy,ParentDirId)
                                     VALUES ('{0}','{1}','{2}',{3},{4},getdate(),{5},'{6}',getdate(),{7},'{8}','{9}');SELECT @@IDENTITY", bas_idm_projectdirectory.DirName, bas_idm_projectdirectory.ParentDirName,
                        bas_idm_projectdirectory.OID, bas_idm_projectdirectory.Enabled, bas_idm_projectdirectory.Reorder, 
                        bas_idm_projectdirectory.CreateUserId, bas_idm_projectdirectory.CreateBy, 
                        bas_idm_projectdirectory.ModifiedUserId, bas_idm_projectdirectory.ModifiedBy,bas_idm_projectdirectory.ParentDirId);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }
        ///<summary>
        /// 项目文件目录-修改方法
        ///</summary>

        public static int EditHandle(Bas_Idm_ProjectDirectory bas_idm_projectdirectory, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + bas_idm_projectdirectory.Id : editparam;
            string sql = "UPDATE [dbo].[Bas_Idm_ProjectDirectory] SET [DirName]='" + bas_idm_projectdirectory.DirName + "',[ParentDirName]='" + bas_idm_projectdirectory.ParentDirName + "',[OID]='" + bas_idm_projectdirectory.OID + "',[Enabled]=" + bas_idm_projectdirectory.Enabled + ",[Reorder]=" + bas_idm_projectdirectory.Reorder + ",[ModifiedUserId]=" + bas_idm_projectdirectory.ModifiedUserId + ",[ModifiedBy]='" + bas_idm_projectdirectory.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 项目文件目录-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_ProjectDirectory WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 项目文件目录-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_ProjectDirectory WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 项目文件目录-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_ProjectDirectory WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        public static int SetEnabledProjectDirectoryById(int Enabled, int Id) 
        {
            string sql = string.Format("UPDATE [dbo].[Bas_Idm_ProjectDirectory] SET Enabled={0} WHERE Id={1}", Enabled, Id);
            return MsSqlHelperEx.Execute(sql);
        }


        /// <summary>
        ///  项目文件目录-变更排序
        ///</summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SetRecordHandle(int record, int id)
        {
            string sql = string.Format("UPDATE [dbo].[Bas_Idm_ProjectDirectory] SET Reorder={0} WHERE Id={1}", record, id);
            return MsSqlHelperEx.Execute(sql);
        }

        public static int HasExistsDirectory(string OID, string DrawingDir, int ParentDirId) 
        {
            string sql = string.Format(@"SELECT Id FROM dbo.Bas_Idm_ProjectDirectory 
                                                        WHERE  OID={0} AND  DirName='{1}'  AND ParentDirId='{2}' AND [Enabled]!=-1", OID, DrawingDir, ParentDirId);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }


        public static int RenameDirectory(string dirName,int dirID) 
        {
            string sql = string.Format(@"UPDATE dbo.Bas_Idm_ProjectDirectory SET DirName='{0}' WHERE Id={1}", dirName, dirID);
            return MsSqlHelperEx.Execute(sql);
        }

        public static string GetDirNameByDirID(int Id) 
        {
            string sql = string.Format(@"SELECT DirName FROM dbo.Bas_Idm_ProjectDirectory WHERE Id={0}",Id);
            return MsSqlHelperEx.ExecuteScalar(sql).ConventToString(string.Empty);
        }

        

    }
}