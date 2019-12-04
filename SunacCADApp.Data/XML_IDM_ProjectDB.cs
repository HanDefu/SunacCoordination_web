using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utility.Extender;
using Common.Utility;
using AFrame.DBUtility;
using SunacCADApp.Entity;
using SunacCADApp.Library;
namespace SunacCADApp.Data
{
    public   class XML_IDM_ProjectDB
    {
        public static IList<XML_IDM_ProjectBase> Get_IDM_ProjectBase(string uid) 
        {
            string sql = string.Format(@"    SELECT b.Id,b.POSID AS ProjectID,b.POST1 AS [Name],c.OrgName AS Area,d.OrgName AS CityCompany
                                                              FROM dbo.Sys_User_Project_Relation a 
                                                        INNER JOIN dbo.Bas_Idm_Project b ON a.Project_ID=b.POSID
                                                        INNER JOIN dbo.Bas_Idm_Organization C ON C.OrgCode=b.PAREA_GS_NAME
                                                        INNER JOIN dbo.Bas_Idm_Organization d ON d.OrgCode=b.PCITY
                                                        WHERE a.[User_ID]='{0}'",uid);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<XML_IDM_ProjectBase>(new XML_IDM_ProjectBase());

        }


        public static IList<XML_IDM_File> Get_IDM_File_List(int Id) 
        {
            string sql = string.Format(@"SELECT [Id],[FileName],[SaveName],[FileUrl], CONVERT(NVARCHAR(24), CreateOn,112) AS CreateTime, CreateBy AS [Creator],
                                                                   CONVERT(NVARCHAR(24), ModifiedOn,112) AS UpdateTime,ModifiedBy AS Updator FROM dbo.Bas_Idm_ProjectFile
	                                                  WHERE OID={0}", Id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<XML_IDM_File>(new XML_IDM_File());

        }


        public static IList<XML_IDM_File> Get_IDM_File_ListExt(int  DirId, string OID)
        {
            string webURL = API_Common.GlobalParam("WebURL");
            string sql = string.Format(@"SELECT [Id],[FileName],[SaveName], CONCAT('{2}','/','projectInfo/filedownload','/', Id) AS [FileUrl], CONVERT(NVARCHAR(24), CreateOn,112) AS CreateTime,
                                                                    CreateBy AS [Creator], CONVERT(NVARCHAR(24), ModifiedOn,112) AS UpdateTime,ModifiedBy AS Updator
                                                          FROM dbo.Bas_Idm_ProjectFile WHERE DirId='{0}' AND [Enabled]!=-1 AND OID='{1}'", DirId, OID, webURL);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<XML_IDM_File>(new XML_IDM_File());
        }

        public static IList<XML_IDM_ProjectInfo> Get_IDM_Project_Array(string uid) 
        {

            IList<XML_IDM_ProjectInfo> list=new List<XML_IDM_ProjectInfo>();
            IList<XML_IDM_ProjectBase> projectes= Get_IDM_ProjectBase(uid);
            foreach (XML_IDM_ProjectBase project in projectes) 
            {
                int pid = project.Id;
                XML_IDM_ProjectInfo projectInfo=new XML_IDM_ProjectInfo();
                projectInfo.ProjectInfo = project;
                projectInfo.FileDir = GetIDMFileDirWhere(pid, 0).ToArray();
                list.Add(projectInfo);
            }
            return list;
        }

        public static XML_IDM_Project Get_IDM_Project(string uid) 
        {
            XML_IDM_ProjectInfo[]  projects=Get_IDM_Project_Array(uid).ToArray();
            return new XML_IDM_Project() { Code = 100, Message = "查询成功", Projects = projects };
        }

        public static IList<XML_IDM_FileDir> GetIDMFileDirWhere(int OID, int ParentDirId) 
        {
            string sql = string.Format(@"SELECT Id,DirName AS Name FROM dbo.Bas_Idm_ProjectDirectory
                                                        WHERE OID={0} AND ParentDirId='{1}' and [Enabled]!=-1 ", OID, ParentDirId);
            IList<XML_IDM_FileDir> _fileDirs = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<XML_IDM_FileDir>(new XML_IDM_FileDir());
            if (_fileDirs.Count() > 0) 
            {
                foreach (XML_IDM_FileDir fileDir in _fileDirs) 
                {
                    int fileid = fileDir.Id.ConvertToInt32(0);
                    string oid = OID.ConventToString("");
                    fileDir.File = Get_IDM_File_ListExt(fileid, oid).ToArray();
                    fileDir.FileDirs = GetIDMFileDirWhere(OID, fileid).ToArray();
                }
            }
            return _fileDirs;
        }

    }
}
