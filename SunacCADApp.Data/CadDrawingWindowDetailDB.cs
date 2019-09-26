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
    ///  外窗CAD原型属性表
    ///</summary>
    public class CadDrawingWindowDetailDB
    {
        ///<summary>
        /// 外窗CAD原型属性表 分页查询
        ///</summary>
        public static IList<CadDrawingWindowDetail> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingWindowDetail> _caddrawingwindowdetails = new List<CadDrawingWindowDetail>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingWindowDetail  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingwindowdetails = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingWindowDetail>(new CadDrawingWindowDetail());
            return _caddrawingwindowdetails;
        }

        ///<summary>
        /// 外窗CAD原型属性表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingWindowDetail WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 外窗CAD原型属性表 根据ID查询
        ///</summary>
        public static CadDrawingWindowDetail GetSingleEntityById(int id)
        {

            CadDrawingWindowDetail caddrawingwindowdetail = new CadDrawingWindowDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingWindowDetail where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingWindowDetail>(new CadDrawingWindowDetail());
        }
        ///<summary>
        /// 外窗CAD原型属性表 查询根据条件
        ///</summary>
        public static CadDrawingWindowDetail GetSingleEntityByparam(string param)
        {

            CadDrawingWindowDetail caddrawingwindowdetail = new CadDrawingWindowDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingWindowDetail where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingWindowDetail>(new CadDrawingWindowDetail());

        }

        ///<summary>
        /// 外窗CAD原型属性表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingWindowDetail caddrawingwindowdetail)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingwindowdetail(MId,WindowOpenTypeId,WindowOpenQtyId,WindowHasCorner,WindowHasSymmetry,WindowSizeMin,WindowSizeMax,WindowDesignFormula,WindowVentilationQuantity,WindowPlugslotSize,WindowAuxiliaryFrame,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy)  
                                     VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8},{9},{10},{11},{12},getdate(),{13},'{14}')", caddrawingwindowdetail.MId, caddrawingwindowdetail.WindowOpenTypeId, caddrawingwindowdetail.WindowOpenQtyId, caddrawingwindowdetail.WindowHasCorner, caddrawingwindowdetail.WindowHasSymmetry, caddrawingwindowdetail.WindowSizeMin, caddrawingwindowdetail.WindowSizeMax, caddrawingwindowdetail.WindowDesignFormula, caddrawingwindowdetail.WindowVentilationQuantity, caddrawingwindowdetail.WindowPlugslotSize, caddrawingwindowdetail.WindowAuxiliaryFrame, caddrawingwindowdetail.Enabled, caddrawingwindowdetail.Reorder, caddrawingwindowdetail.CreateUserId, caddrawingwindowdetail.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 外窗CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingWindowDetail caddrawingwindowdetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingwindowdetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingWindowDetail] SET [MId]=" + caddrawingwindowdetail.MId + ",[WindowOpenTypeId]=" + caddrawingwindowdetail.WindowOpenTypeId + ",[WindowOpenQtyId]=" + caddrawingwindowdetail.WindowOpenQtyId + ",[WindowHasCorner]=" + caddrawingwindowdetail.WindowHasCorner + ",[WindowHasSymmetry]=" + caddrawingwindowdetail.WindowHasSymmetry + ",[WindowSizeMin]=" + caddrawingwindowdetail.WindowSizeMin + ",[WindowSizeMax]=" + caddrawingwindowdetail.WindowSizeMax + ",[WindowDesignFormula]='" + caddrawingwindowdetail.WindowDesignFormula + "',[WindowVentilationQuantity]=" + caddrawingwindowdetail.WindowVentilationQuantity + ",[WindowPlugslotSize]=" + caddrawingwindowdetail.WindowPlugslotSize + ",[WindowAuxiliaryFrame]=" + caddrawingwindowdetail.WindowAuxiliaryFrame + ",[Enabled]=" + caddrawingwindowdetail.Enabled + ",[Reorder]=" + caddrawingwindowdetail.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 外窗CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingWindowDetail WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 外窗CAD原型属性表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingWindowDetail WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 外窗CAD原型属性表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingWindowDetail WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}