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
    ///  CAD原型功能区表
    ///</summary>
    public class CadDrawingFunctionDB
    {
        ///<summary>
        /// CAD原型功能区表 分页查询
        ///</summary>
        public static IList<CadDrawingFunction> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingFunction> _caddrawingfunctions = new List<CadDrawingFunction>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingFunction  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingfunctions = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingFunction>(new CadDrawingFunction());
            return _caddrawingfunctions;
        }

        ///<summary>
        /// CAD原型功能区表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingFunction WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// CAD原型功能区表 根据ID查询
        ///</summary>
        public static CadDrawingFunction GetSingleEntityById(int id)
        {

            CadDrawingFunction caddrawingfunction = new CadDrawingFunction();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingFunction where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingFunction>(new CadDrawingFunction());
        }
        ///<summary>
        /// CAD原型功能区表 查询根据条件
        ///</summary>
        public static CadDrawingFunction GetSingleEntityByparam(string param)
        {

            CadDrawingFunction caddrawingfunction = new CadDrawingFunction();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingFunction where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingFunction>(new CadDrawingFunction());

        }

        ///<summary>
        /// CAD原型功能区表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingFunction caddrawingfunction)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingfunction(MId,FunctionId,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy)  
                                     VALUES ({0},{1},{2},{3},getdate(),{4},'{5}')", caddrawingfunction.MId, caddrawingfunction.FunctionId, caddrawingfunction.Enabled, caddrawingfunction.Reorder, caddrawingfunction.CreateUserId, caddrawingfunction.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// CAD原型功能区表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingFunction caddrawingfunction, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingfunction.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingFunction] SET [MId]=" + caddrawingfunction.MId + ",[FunctionId]=" + caddrawingfunction.FunctionId + ",[Enabled]=" + caddrawingfunction.Enabled + ",[Reorder]=" + caddrawingfunction.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// CAD原型功能区表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingFunction WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// CAD原型功能区表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingFunction WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// CAD原型功能区表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingFunction WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}