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
    ///  系统操作日志
    ///</summary>
    public class SysOperateLogDB
    {
        ///<summary>
        /// 系统操作日志 分页查询
        ///</summary>
        public static IList<Sys_Operate_Log> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<Sys_Operate_Log> _sys_operate_logs = new List<Sys_Operate_Log>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.Sys_Operate_Log  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.CreateOn DESC {3}", _where, start, end, orderby);

            _sys_operate_logs = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Sys_Operate_Log>(new Sys_Operate_Log());
            return _sys_operate_logs;
        }

        ///<summary>
        /// 系统操作日志  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.Sys_Operate_Log WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 系统操作日志 根据ID查询
        ///</summary>
        public static Sys_Operate_Log GetSingleEntityById(int id)
        {

            Sys_Operate_Log sys_operate_log = new Sys_Operate_Log();
            string sql = string.Format("select top 1 *  from dbo.Sys_Operate_Log where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_Operate_Log>(new Sys_Operate_Log());
        }
        ///<summary>
        /// 系统操作日志 查询根据条件
        ///</summary>
        public static Sys_Operate_Log GetSingleEntityByparam(string param)
        {

            Sys_Operate_Log sys_operate_log = new Sys_Operate_Log();
            string sql = string.Format("select top 1 *  from dbo.Sys_Operate_Log where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Sys_Operate_Log>(new Sys_Operate_Log());

        }

        ///<summary>
        /// 系统操作日志-添加方法
        ///</summary>

        public static int AddHandle(Sys_Operate_Log sys_operate_log)
        {


            string sql = string.Format(@"INSERT INTO dbo.sys_operate_log(SysTypeCode,SysTypeName,LogInfo,LogDesc,
                                                                             Enabled ,CreateOn ,CreateUserId ,CreateBy)  
                                                                VALUES ({0},'{1}','{2}','{3}',{4},getdate(),{5},'{6}')", sys_operate_log.SysTypeCode, sys_operate_log.SysTypeName,
                                                                    sys_operate_log.LogInfo, sys_operate_log.LogDesc, sys_operate_log.Enabled, sys_operate_log.CreateUserId, sys_operate_log.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }

        public static int SaveLogHandle(int logCode, string logName, string logInfo, string logDesc, string createBy, int CreateUserId)
        {
            Sys_Operate_Log log = new Sys_Operate_Log
            {
                SysTypeCode=logCode,
                SysTypeName=logName,
                LogInfo=logInfo,
                LogDesc=logDesc,
                CreateBy=createBy,
                CreateUserId=CreateUserId
            };
           return  AddHandle(log);

        }
        ///<summary>
        /// 系统操作日志-修改方法
        ///</summary>

        public static int EditHandle(Sys_Operate_Log sys_operate_log, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + sys_operate_log.Id : editparam;
            string sql = "UPDATE [dbo].[Sys_Operate_Log] SET [SysTypeCode]=" + sys_operate_log.SysTypeCode + ",[SysTypeName]='" + sys_operate_log.SysTypeName + "',[LogInfo]='" + sys_operate_log.LogInfo + "',[Enabled]=" + sys_operate_log.Enabled + ",[Reorder]=" + sys_operate_log.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 系统操作日志-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Operate_Log WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 系统操作日志-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Operate_Log WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 系统操作日志-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.Sys_Operate_Log WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        /// <summary>
        ///  系统操作日志-变更排序
        ///</summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SetRecordHandle(int record, int id)
        {
            string sql = string.Format("UPDATE [dbo].[Sys_Operate_Log] SET Reorder={0} WHERE Id={1}", record, id);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}
