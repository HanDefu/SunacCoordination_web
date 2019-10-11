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
    ///  CAD原型信息
    ///</summary>
    public class CadDrawingMasterDB
    {
        ///<summary>
        /// CAD原型信息 分页查询
        ///</summary>
        public static IList<CadDrawingMaster> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingMaster> _caddrawingmasters = new List<CadDrawingMaster>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingMaster  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingmasters = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingMaster>(new CadDrawingMaster());
            return _caddrawingmasters;
        }

        ///<summary>
        /// CAD原型信息  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingMaster WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// CAD原型信息 根据ID查询
        ///</summary>
        public static CadDrawingMaster GetSingleEntityById(int id)
        {

            CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingMaster where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingMaster>(new CadDrawingMaster());
        }
        ///<summary>
        /// CAD原型信息 查询根据条件
        ///</summary>
        public static CadDrawingMaster GetSingleEntityByparam(string param)
        {

            CadDrawingMaster caddrawingmaster = new CadDrawingMaster();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingMaster where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingMaster>(new CadDrawingMaster());

        }

        ///<summary>
        /// CAD原型信息-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingMaster caddrawingmaster)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingmaster(DrawingCode,DrawingName,Scope,AreaId,DrawingType,DynamicType,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ('{0}','{1}','{2}',{3},{4},{5},{6},{7},getdate(),{8},'{9}',getdate(),{10},'{11}');SELECT @@IDENTITY AS ID", caddrawingmaster.DrawingCode, caddrawingmaster.DrawingName, caddrawingmaster.Scope, caddrawingmaster.AreaId, caddrawingmaster.DrawingType, caddrawingmaster.DynamicType, caddrawingmaster.Enabled, caddrawingmaster.Reorder, caddrawingmaster.CreateUserId, caddrawingmaster.CreateBy, caddrawingmaster.ModifiedUserId, caddrawingmaster.ModifiedBy);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(-1);
        }
        ///<summary>
        /// CAD原型信息-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingMaster caddrawingmaster, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingmaster.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingMaster] SET [DrawingCode]='" + caddrawingmaster.DrawingCode + "',[DrawingName]='" + caddrawingmaster.DrawingName + "',[Scope]='" + caddrawingmaster.Scope + "',[AreaId]=" + caddrawingmaster.AreaId + ",[DrawingType]=" + caddrawingmaster.DrawingType + ",[DynamicType]=" + caddrawingmaster.DynamicType + ",[Enabled]=" + caddrawingmaster.Enabled + ",[Reorder]=" + caddrawingmaster.Reorder + ",[ModifiedUserId]=" + caddrawingmaster.ModifiedUserId + ",[ModifiedBy]='" + caddrawingmaster.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// CAD原型信息-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingMaster WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// CAD原型信息-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingMaster WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// CAD原型信息-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingMaster WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static string HasDrawingCode(string DrawingCode) 
        {
            string sql = string.Format(@"SELECT DrawingCode FROM dbo.CadDrawingMaster WHERE DrawingCode='{0}'",DrawingCode);
            return MsSqlHelperEx.ExecuteScalar(sql).ConventToString(string.Empty);
        }

    }
}
