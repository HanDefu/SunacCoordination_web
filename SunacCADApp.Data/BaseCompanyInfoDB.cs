﻿using System;
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
    ///  机构信息
    ///</summary>
    public class BaseCompanyInfoDB
    {
        ///<summary>
        /// 机构信息 分页查询
        ///</summary>
        public static IList<BaseCompanyInfo> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<BaseCompanyInfo> _basecompanyinfos = new List<BaseCompanyInfo>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , a.*,
                                                                    b.InsCode,b.InsName,b.InsEnCode
                                                      FROM    dbo.BaseCompanyInfo  a 
                                              INNER JOIN   dbo.BasInstitutionData b ON a.CompanyID=b.Id
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.ModifiedOn DESC {3}", _where, start, end, orderby);

            _basecompanyinfos = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<BaseCompanyInfo>(new BaseCompanyInfo());
            return _basecompanyinfos;
        }

        ///<summary>
        /// 机构信息  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"  SELECT COUNT(*) AS RowNum   
                                                            FROM    dbo.BaseCompanyInfo  a 
                                                   INNER JOIN   dbo.BasInstitutionData b ON a.CompanyID=b.Id WHERE {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 机构信息 根据ID查询
        ///</summary>
        public static BaseCompanyInfo GetSingleEntityById(int id)
        {

            BaseCompanyInfo basecompanyinfo = new BaseCompanyInfo();
            string sql = string.Format("select top 1 *  from dbo.BaseCompanyInfo where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BaseCompanyInfo>(new BaseCompanyInfo());
        }
        ///<summary>
        /// 机构信息 查询根据条件
        ///</summary>
        public static BaseCompanyInfo GetSingleEntityByparam(string param)
        {

            BaseCompanyInfo basecompanyinfo = new BaseCompanyInfo();
            string sql = string.Format("select top 1 *  from dbo.BaseCompanyInfo where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BaseCompanyInfo>(new BaseCompanyInfo());

        }

        ///<summary>
        /// 机构信息-添加方法
        ///</summary>

        public static int AddHandle(BaseCompanyInfo basecompanyinfo)
        {


            string sql = string.Format(@"INSERT INTO dbo.basecompanyinfo(CompanyID,CompanyName,CompanyCode,CompanyRemark,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},'{1}','{2}','{3}',{4},{5},getdate(),{6},'{7}',getdate(),{8},'{9}')", basecompanyinfo.CompanyID, basecompanyinfo.CompanyName, basecompanyinfo.CompanyCode, basecompanyinfo.CompanyRemark, basecompanyinfo.Enabled, basecompanyinfo.Reorder, basecompanyinfo.CreateUserId, basecompanyinfo.CreateBy, basecompanyinfo.ModifiedUserId, basecompanyinfo.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 机构信息-修改方法
        ///</summary>

        public static int EditHandle(BaseCompanyInfo basecompanyinfo, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + basecompanyinfo.Id : editparam;
            string sql = "UPDATE [dbo].[BaseCompanyInfo] SET [CompanyID]=" + basecompanyinfo.CompanyID + ",[CompanyName]='" + basecompanyinfo.CompanyName + "',[CompanyCode]='" + basecompanyinfo.CompanyCode + "',[CompanyRemark]='" + basecompanyinfo.CompanyRemark + "',[Enabled]=" + basecompanyinfo.Enabled + ",[Reorder]=" + basecompanyinfo.Reorder + ",[ModifiedUserId]=" + basecompanyinfo.ModifiedUserId + ",[ModifiedBy]='" + basecompanyinfo.ModifiedBy + "' ,[ModifiedOn]= GETDATE()  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 机构信息-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.BaseCompanyInfo WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 机构信息-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.BaseCompanyInfo WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 机构信息-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.BaseCompanyInfo WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<DataSourceMember> GetInstitutionData() 
        {
            string sql = string.Format(@"select Id as ValueMember,InsName as DisplayMember from BasInstitutionData where Enabled=1 Order by InsName desc");
            return MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<DataSourceMember>(new DataSourceMember());
        }

        public static int IsExistsInstitutionDataById(int Id) 
        {
            string sql = string.Format(@"SELECT Id FROM dbo.BaseCompanyInfo WHERE CompanyID={0}", Id);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        /// <summary>
        /// 状态修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public static int IsChangeCompanyInfo(int id,int enabled) 
        {

            string sql = string.Format(@"UPDATE dbo.BaseCompanyInfo SET [Enabled]={0} WHERE Id={1}",enabled,id);
            return MsSqlHelperEx.Execute(sql).ConvertToInt32(0);
        }

    }
}