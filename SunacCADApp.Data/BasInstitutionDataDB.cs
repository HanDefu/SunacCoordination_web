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
    ///  机构数据
    ///</summary>
    public class BasInstitutionDataDB
    {
        ///<summary>
        /// 机构数据 分页查询
        ///</summary>
        public static IList<BasInstitutionData> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<BasInstitutionData> _basinstitutiondatas = new List<BasInstitutionData>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.BasInstitutionData  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _basinstitutiondatas = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<BasInstitutionData>(new BasInstitutionData());
            return _basinstitutiondatas;
        }

        ///<summary>
        /// 机构数据  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.BasInstitutionData WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 机构数据 根据ID查询
        ///</summary>
        public static BasInstitutionData GetSingleEntityById(int id)
        {

            BasInstitutionData basinstitutiondata = new BasInstitutionData();
            string sql = string.Format("select top 1 *  from dbo.BasInstitutionData where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BasInstitutionData>(new BasInstitutionData());
        }
        ///<summary>
        /// 机构数据 查询根据条件
        ///</summary>
        public static BasInstitutionData GetSingleEntityByparam(string param)
        {

            BasInstitutionData basinstitutiondata = new BasInstitutionData();
            string sql = string.Format("select top 1 *  from dbo.BasInstitutionData where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BasInstitutionData>(new BasInstitutionData());

        }

        ///<summary>
        /// 机构数据-添加方法
        ///</summary>

        public static int AddHandle(BasInstitutionData basinstitutiondata)
        {


            string sql = string.Format(@"INSERT INTO dbo.basinstitutiondata(InsCode,InsName,InsEnCode,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ('{0}','{1}','{2}',{3},{4},getdate(),{5},'{6}',getdate(),{7},'{8}')", basinstitutiondata.InsCode, basinstitutiondata.InsName, basinstitutiondata.InsEnCode, basinstitutiondata.Enabled, basinstitutiondata.Reorder, basinstitutiondata.CreateUserId, basinstitutiondata.CreateBy, basinstitutiondata.ModifiedUserId, basinstitutiondata.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 机构数据-修改方法
        ///</summary>

        public static int EditHandle(BasInstitutionData basinstitutiondata, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + basinstitutiondata.Id : editparam;
            string sql = "UPDATE [dbo].[BasInstitutionData] SET [InsCode]='" + basinstitutiondata.InsCode + "',[InsName]='" + basinstitutiondata.InsName + "',[InsEnCode]='" + basinstitutiondata.InsEnCode + "',[Enabled]=" + basinstitutiondata.Enabled + ",[Reorder]=" + basinstitutiondata.Reorder + ",[ModifiedUserId]=" + basinstitutiondata.ModifiedUserId + ",[ModifiedBy]='" + basinstitutiondata.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 机构数据-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.BasInstitutionData WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 机构数据-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.BasInstitutionData WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 机构数据-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.BasInstitutionData WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

 
    }
}

