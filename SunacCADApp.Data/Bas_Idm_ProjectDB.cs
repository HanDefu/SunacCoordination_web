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
    public class BasIdmProjectDB
    {
        ///<summary>
        /// 项目信息表 分页查询
        ///</summary>
        public static IList<Bas_Idm_Project> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Bas_Idm_Project> _bas_idm_projects = new List<Bas_Idm_Project>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Bas_Idm_Project  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _bas_idm_projects = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Bas_Idm_Project>(new Bas_Idm_Project());
            return _bas_idm_projects;
        }

        ///<summary>
        /// 项目信息表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Bas_Idm_Project WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 项目信息表 根据ID查询
        ///</summary>
        public static Bas_Idm_Project GetSingleEntityById(int id)
        {

            Bas_Idm_Project bas_idm_project = new Bas_Idm_Project();
            string sql = string.Format("select top 1 *  from dbo.Bas_Idm_Project where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Bas_Idm_Project>(new Bas_Idm_Project());
        }
        ///<summary>
        /// 项目信息表 查询根据条件
        ///</summary>
        public static Bas_Idm_Project GetSingleEntityByparam(string param)
        {

            Bas_Idm_Project bas_idm_project = new Bas_Idm_Project();
            string sql = string.Format("select top 1 *  from dbo.Bas_Idm_Project where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Bas_Idm_Project>(new Bas_Idm_Project());

        }

        ///<summary>
        /// 项目信息表-添加方法
        ///</summary>

        public static int AddHandle(Bas_Idm_Project bas_idm_project)
        {


            string sql = string.Format(@"INSERT INTO dbo.bas_idm_project(POSID,POST1,PLONG_TX,PBUKRS,COMP_NAME,PAREA_GS_NAME,STEP_ID,PLFAZ,PLSEZ,PCITY,CITY_NAME,STUFE,DK_POSID,DK_NAME,MTIMESTAMP,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}','{10}',{11},'{12}','{13}','{14}',{15},{16},getdate(),{17},'{18}',getdate(),{19},'{20}')", bas_idm_project.POSID, bas_idm_project.POST1, bas_idm_project.PLONG_TX, bas_idm_project.PBUKRS, bas_idm_project.COMP_NAME, bas_idm_project.PAREA_GS_NAME, bas_idm_project.STEP_ID, bas_idm_project.PLFAZ, bas_idm_project.PLSEZ, bas_idm_project.PCITY, bas_idm_project.CITY_NAME, bas_idm_project.STUFE, bas_idm_project.DK_POSID, bas_idm_project.DK_NAME, bas_idm_project.MTIMESTAMP, bas_idm_project.Enabled, bas_idm_project.Reorder, bas_idm_project.CreateUserId, bas_idm_project.CreateBy, bas_idm_project.ModifiedUserId, bas_idm_project.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 项目信息表-修改方法
        ///</summary>

        public static int EditHandle(Bas_Idm_Project bas_idm_project, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + bas_idm_project.Id : editparam;
            string sql = "UPDATE [dbo].[Bas_Idm_Project] SET [POSID]='" + bas_idm_project.POSID + "',[POST1]='" + bas_idm_project.POST1 + "',[PLONG_TX]='" + bas_idm_project.PLONG_TX + "',[PBUKRS]='" + bas_idm_project.PBUKRS + "',[COMP_NAME]='" + bas_idm_project.COMP_NAME + "',[PAREA_GS_NAME]='" + bas_idm_project.PAREA_GS_NAME + "',[STEP_ID]=" + bas_idm_project.STEP_ID + ",[PLFAZ]='" + bas_idm_project.PLFAZ + "',[PLSEZ]='" + bas_idm_project.PLSEZ + "',[PCITY]='" + bas_idm_project.PCITY + "',[CITY_NAME]='" + bas_idm_project.CITY_NAME + "',[STUFE]=" + bas_idm_project.STUFE + ",[DK_POSID]='" + bas_idm_project.DK_POSID + "',[DK_NAME]='" + bas_idm_project.DK_NAME + "',[MTIMESTAMP]='" + bas_idm_project.MTIMESTAMP + "',[Enabled]=" + bas_idm_project.Enabled + ",[Reorder]=" + bas_idm_project.Reorder + ",[ModifiedUserId]=" + bas_idm_project.ModifiedUserId + ",[ModifiedBy]='" + bas_idm_project.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 项目信息表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_Project WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 项目信息表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_Project WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 项目信息表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Bas_Idm_Project WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        /// <summary>
        ///  项目信息表-变更排序
        ///</summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SetRecordHandle(int record, int id)
        {
            string sql = string.Format("UPDATE [dbo].[Bas_Idm_Project] SET Reorder={0} WHERE Id={1}", record, id);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<Bas_Idm_Project> GetBasIdmProjectByWhere(string PAREA_Gs_Name,string PCITY,string Keyword) 
        {

            string _where = string.Empty;
            _where += string.IsNullOrEmpty(PAREA_Gs_Name) ? string.Empty : string.Format(@" And a.PAREA_GS_NAME = '{0}'", PAREA_Gs_Name);
            _where += string.IsNullOrEmpty(PCITY) ? string.Empty : string.Format(@" And a.PCITY='{0}'", PCITY);
            _where += string.IsNullOrEmpty(Keyword) ? string.Empty : string.Format(@" And a.POST1 like '%{0}%'", Keyword);
            IList<Bas_Idm_Project> projects = new List<Bas_Idm_Project>();
            string sql = string.Format(@"  SELECT  a.Id,a.POSID,a.POST1,a.PLONG_TX,a.PBUKRS,a.COMP_NAME,a.PAREA_GS_NAME,
                                                                       a.STEP_ID,a.PLFAZ,a.PCITY,a.CITY_NAME,a.STUFE,a.DK_POSID,a.DK_NAME,a.MTIMESTAMP,b.OrgName AS PAREANAME,c.OrgName AS PCITYNAME
                                                              FROM  dbo.Bas_Idm_Project a  
                                                         LEFT JOIN dbo.Bas_Idm_Organization   b ON a.PAREA_GS_NAME=b.OrgCode AND b.UpOrgCode='E01' AND b.OrgTypeCode='A'
                                                         LEFT JOIN dbo.Bas_Idm_Organization   c ON c.OrgTypeCode='C'  AND c.OrgCode=a.PCITY
                                                         WHERE a.STUFE=1   {0}", _where);
            projects = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Bas_Idm_Project>(new Bas_Idm_Project());
            return projects;

        }
    }
}

