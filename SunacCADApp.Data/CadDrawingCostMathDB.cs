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
    ///  门窗成本算量表
    ///</summary>
    public class CadDrawingCostMathDB
    {
        ///<summary>
        /// 门窗成本算量表 分页查询
        ///</summary>
        public static IList<CadDrawingCostMath> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingCostMath> _caddrawingcostmaths = new List<CadDrawingCostMath>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingCostMath  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingcostmaths = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingCostMath>(new CadDrawingCostMath());
            return _caddrawingcostmaths;
        }

        ///<summary>
        /// 门窗成本算量表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingCostMath WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 门窗成本算量表 根据ID查询
        ///</summary>
        public static CadDrawingCostMath GetSingleEntityById(int id)
        {

            CadDrawingCostMath caddrawingcostmath = new CadDrawingCostMath();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingCostMath where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingCostMath>(new CadDrawingCostMath());
        }
        ///<summary>
        /// 门窗成本算量表 查询根据条件
        ///</summary>
        public static CadDrawingCostMath GetSingleEntityByparam(string param)
        {

            CadDrawingCostMath caddrawingcostmath = new CadDrawingCostMath();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingCostMath where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingCostMath>(new CadDrawingCostMath());

        }

        ///<summary>
        /// 门窗成本算量表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingCostMath caddrawingcostmath)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingcostmath(MId,FunType,FunMath,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy)  
                                     VALUES ('{0}',{1},'{2}',{3},{4},getdate(),{5},'{6}')", caddrawingcostmath.MId, caddrawingcostmath.FunType, caddrawingcostmath.FunMath, caddrawingcostmath.Enabled, caddrawingcostmath.Reorder, caddrawingcostmath.CreateUserId, caddrawingcostmath.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 门窗成本算量表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingCostMath caddrawingcostmath, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingcostmath.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingCostMath] SET [MId]='" + caddrawingcostmath.MId + "',[FunType]=" + caddrawingcostmath.FunType + ",[FunMath]='" + caddrawingcostmath.FunMath + "',[Enabled]=" + caddrawingcostmath.Enabled + ",[Reorder]=" + caddrawingcostmath.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 门窗成本算量表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingCostMath WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 门窗成本算量表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingCostMath WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 门窗成本算量表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingCostMath WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}