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
    ///  项目对应CAD文件
    ///</summary>
    public class BasIdmProjectFileDB
    {
        ///<summary>
        /// 项目对应CAD文件 分页查询
        ///</summary>
        public static IList<Bas_Idm_ProjectFile> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Bas_Idm_ProjectFile> _bas_idm_projectfiles = new List<Bas_Idm_ProjectFile>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Bas_Idm_ProjectFile  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _bas_idm_projectfiles = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Bas_Idm_ProjectFile>(new Bas_Idm_ProjectFile());
            return _bas_idm_projectfiles;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IList<Bas_Idm_ProjectFile> GetIdmProjectFileListByParam(string param) 
        {
            IList<Bas_Idm_ProjectFile> _bas_idm_projectfiles = new List<Bas_Idm_ProjectFile>();
            string sql = string.Format(@"SELECT Id,[FileName],SaveName,DirId,DirName,OID,Project_ID,ModifiedOn FROM dbo.Bas_Idm_ProjectFile WHERE {0}",param);
            _bas_idm_projectfiles = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Bas_Idm_ProjectFile>(new Bas_Idm_ProjectFile());
            return _bas_idm_projectfiles;
        }



        ///<summary>
        /// 项目对应CAD文件  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Bas_Idm_ProjectFile WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 项目对应CAD文件 根据ID查询
        ///</summary>
        public static Bas_Idm_ProjectFile GetSingleEntityById(int id)
        {

            Bas_Idm_ProjectFile bas_idm_projectfile = new Bas_Idm_ProjectFile();
            string sql = string.Format("select top 1 *  from dbo.Bas_Idm_ProjectFile where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Bas_Idm_ProjectFile>(new Bas_Idm_ProjectFile());
        }
        ///<summary>
        /// 项目对应CAD文件 查询根据条件
        ///</summary>
        public static Bas_Idm_ProjectFile GetSingleEntityByparam(string param)
        {

            Bas_Idm_ProjectFile bas_idm_projectfile = new Bas_Idm_ProjectFile();
            string sql = string.Format("select top 1 *  from dbo.Bas_Idm_ProjectFile where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Bas_Idm_ProjectFile>(new Bas_Idm_ProjectFile());

        }

        ///<summary>
        /// 项目对应CAD文件-添加方法
        ///</summary>

        public static int AddHandle(Bas_Idm_ProjectFile bas_idm_projectfile)
        {


            string sql = string.Format(@"INSERT INTO dbo.bas_idm_projectfile(FileName,SaveName,FileUrl,DirName,OID,Project_ID,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy,DirId,FileSize)
                                     VALUES ('{0}','{1}','{2}','{3}',{4},'{5}',{6},{7},getdate(),{8},'{9}',getdate(),{10},'{11}',{12},'{13}');SELECT @@IDENTITY", bas_idm_projectfile.FileName, bas_idm_projectfile.SaveName, bas_idm_projectfile.FileUrl,
                                      bas_idm_projectfile.DirName, bas_idm_projectfile.OID, bas_idm_projectfile.Project_ID, bas_idm_projectfile.Enabled, 
                                      bas_idm_projectfile.Reorder, bas_idm_projectfile.CreateUserId, bas_idm_projectfile.CreateBy, 
                                      bas_idm_projectfile.ModifiedUserId, bas_idm_projectfile.ModifiedBy,bas_idm_projectfile.DirId,bas_idm_projectfile.FileSize);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }
        ///<summary>
        /// 项目对应CAD文件-修改方法
        ///</summary>

        public static int EditHandle(Bas_Idm_ProjectFile bas_idm_projectfile, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + bas_idm_projectfile.Id : editparam;
            string sql = "UPDATE [dbo].[Bas_Idm_ProjectFile] SET [FileName]='" + bas_idm_projectfile.FileName + "',[SaveName]='" + bas_idm_projectfile.SaveName + "',[FileUrl]='" + bas_idm_projectfile.FileUrl + "',[DirName]='" + bas_idm_projectfile.DirName + "',[OID]=" + bas_idm_projectfile.OID + ",[Project_ID]='" + bas_idm_projectfile.Project_ID + "',[Enabled]=" + bas_idm_projectfile.Enabled + ",[Reorder]=" + bas_idm_projectfile.Reorder + ",[ModifiedUserId]=" + bas_idm_projectfile.ModifiedUserId + ",[ModifiedBy]='" + bas_idm_projectfile.ModifiedBy + "' ,[DirId]='" + bas_idm_projectfile.DirId + "',[FileSize]='"+bas_idm_projectfile.FileSize+"'    where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 项目对应CAD文件-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_ProjectFile WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 项目对应CAD文件-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_ProjectFile WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 项目对应CAD文件-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_ProjectFile WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static int SetProjectFileEnabledById(int Enabled,int Id) 
        {
            string sql = string.Format("UPDATE [dbo].[Bas_Idm_ProjectFile] SET Enabled={0} WHERE Id={1}", Enabled, Id);
            return MsSqlHelperEx.Execute(sql);
        }


        public static int SetProjectFileEnabledById(int Enabled, string Ids)
        {
            string sql = string.Format("UPDATE [dbo].[Bas_Idm_ProjectFile] SET Enabled={0} WHERE Id in ({1})", Enabled, Ids);
            return MsSqlHelperEx.Execute(sql);
        }


        /// <summary>
        ///  项目对应CAD文件-变更排序
        ///</summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SetRecordHandle(int record, int id)
        {
            string sql = string.Format("UPDATE [dbo].[Bas_Idm_ProjectFile] SET Reorder={0} WHERE Id={1}", record, id);
            return MsSqlHelperEx.Execute(sql);
        }

        public static int HasBasIdmProjectFile(string OID, string FileSaveName, string DrawingFile, int DirId)
        {
            string sql = string.Format(@"SELECT Id FROM  dbo.Bas_Idm_ProjectFile WHERE [FileName]='{0}' AND OID='{1}' AND DirId='{2}'", DrawingFile, OID, DirId);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

       

    }
}