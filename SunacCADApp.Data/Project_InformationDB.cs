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
    ///  项目信息表
    ///</summary>
    public class Project_InformationDB
    {
        ///<summary>
        /// 项目信息表 分页查询
        ///</summary>
        public static IList<Project_Information> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Project_Information> _project_informations = new List<Project_Information>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Project_Information  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _project_informations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Project_Information>(new Project_Information());
            return _project_informations;
        }

        ///<summary>
        /// 项目信息表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Project_Information WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 项目信息表 根据ID查询
        ///</summary>
        public static Project_Information GetSingleEntityById(int id)
        {

            Project_Information project_information = new Project_Information();
            string sql = string.Format("select top 1 *  from dbo.Project_Information where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Project_Information>(new Project_Information());
        }
        ///<summary>
        /// 项目信息表 查询根据条件
        ///</summary>
        public static Project_Information GetSingleEntityByparam(string param)
        {

            Project_Information project_information = new Project_Information();
            string sql = string.Format("select top 1 *  from dbo.Project_Information where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Project_Information>(new Project_Information());

        }

        ///<summary>
        /// 项目信息表-添加方法
        ///</summary>

        public static int AddHandle(Project_Information project_information)
        {


            string sql = string.Format(@"INSERT INTO dbo.project_information(ProjectCode,ProjectName,CompanyCode,CompanyName,AreaID,AreaName,ProjectPhase,CityCompany,CityID,CityName,Project_StartTime,ProjectEndTime,ProjectStufe,PlaceCode,PlaceName,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}',{8},'{9}','{10}','{11}',{12},'{13}','{14}',{15},{16},getdate(),{17},'{18}',getdate(),{19},'{20}')", project_information.ProjectCode, project_information.ProjectName, project_information.CompanyCode, project_information.CompanyName, project_information.AreaID, project_information.AreaName, project_information.ProjectPhase, project_information.CityCompany, project_information.CityID, project_information.CityName, project_information.Project_StartTime, project_information.ProjectEndTime, project_information.ProjectStufe, project_information.PlaceCode, project_information.PlaceName, project_information.Enabled, project_information.Reorder, project_information.CreateUserId, project_information.CreateBy, project_information.ModifiedUserId, project_information.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 项目信息表-修改方法
        ///</summary>

        public static int EditHandle(Project_Information project_information, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + project_information.Id : editparam;
            string sql = "UPDATE [dbo].[Project_Information] SET [ProjectCode]='" + project_information.ProjectCode + "',[ProjectName]='" + project_information.ProjectName + "',[CompanyCode]='" + project_information.CompanyCode + "',[CompanyName]='" + project_information.CompanyName + "',[AreaID]=" + project_information.AreaID + ",[AreaName]='" + project_information.AreaName + "',[ProjectPhase]='" + project_information.ProjectPhase + "',[CityCompany]='" + project_information.CityCompany + "',[CityID]=" + project_information.CityID + ",[CityName]='" + project_information.CityName + "',[Project_StartTime]='" + project_information.Project_StartTime + "',[ProjectEndTime]='" + project_information.ProjectEndTime + "',[ProjectStufe]=" + project_information.ProjectStufe + ",[PlaceCode]='" + project_information.PlaceCode + "',[PlaceName]='" + project_information.PlaceName + "',[Enabled]=" + project_information.Enabled + ",[Reorder]=" + project_information.Reorder + ",[ModifiedUserId]=" + project_information.ModifiedUserId + ",[ModifiedBy]='" + project_information.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 项目信息表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Project_Information WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 项目信息表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Project_Information WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 项目信息表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Project_Information WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<Project_Information> GetProjectInformationByAreaName(string areaName) 
        {
            IList<Project_Information> _project_informations = new List<Project_Information>();
            string sql = string.Format(@"SELECT CityID,CityCompany FROM  Project_Information 
                                                        WHERE AreaName='{0}' GROUP BY CityID,CityCompany", areaName);
            _project_informations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Project_Information>(new Project_Information());
            return _project_informations;
        }

        public static IList<ProjectInfo> GetProjectInformationByAreaName(string areaName, string cityCompany, string keyword)
        {
            IList<ProjectInfo> _project_informations = new List<ProjectInfo>();
            string wh = string.IsNullOrEmpty(keyword) ? " 1=1 ": string.Format(@" ProjectName like '{0}%'", keyword);
            wh += string.IsNullOrEmpty(areaName) ? string.Empty : string.Format(@" AND AreaName='{0}'",areaName);
            wh += string.IsNullOrEmpty(cityCompany) ? string.Empty : string.Format(@"  AND CityCompany='{0}'", cityCompany);
            string sql = string.Format(@"SELECT Id,ProjectCode,ProjectName,CompanyName,AreaName,CityCompany,CONVERT(varchar(10), Project_StartTime, 23) as Project_StartTime FROM Project_Information 
                                                        WHERE {0} ", wh);
            _project_informations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<ProjectInfo>(new ProjectInfo());
            return _project_informations;
        }

    }
}

