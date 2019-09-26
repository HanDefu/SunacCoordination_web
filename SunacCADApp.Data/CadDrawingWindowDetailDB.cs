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

        ///<summary>
        /// 外窗原型查询 分页查询
        ///</summary>
        public static IList<CadDrawingWindowSearch> GetSearchPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingWindowSearch> _caddrawingwindowsearchs = new List<CadDrawingWindowSearch>();
            string sql = string.Format(@"SELECT  * FROM 
                                                           (SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber, a.Id,
                                                                        a.DrawingCode,a.DrawingName,d.DWGPath,a.Reorder,a.CreateOn 
                                                             FROM  dbo.CaddrawingMaster a 
                                                    INNER JOIN  dbo.CadDrawingWindowDetail b ON a.Id=b.MId
													   LEFT JOIN  (SELECT MIN(Id) AS Id, MId FROM dbo.CadDrawingDWG   GROUP BY MId) c ON c.MId = a.Id
													   LEFT JOIN  dbo.CadDrawingDWG d ON d.Id=c.Id  WHERE 1=1  {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);
            _caddrawingwindowsearchs = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingWindowSearch>(new CadDrawingWindowSearch());
            return _caddrawingwindowsearchs;
        }

        ///<summary>
        /// 外窗原型查询  分页数据总数量
        ///<summary>
        public static int GetSearchPageCountByParameter(string _where)
        {
            string sql = string.Format(@"      SELECT   COUNT(*) AS CNT   FROM dbo.CaddrawingMaster a 
                                                       INNER JOIN  dbo.CadDrawingWindowDetail b ON a.Id=b.MId
													     LEFT JOIN  (SELECT MIN(Id) AS Id, MId FROM dbo.CadDrawingDWG WHERE  FileClass='JPG' GROUP BY MId) c ON c.MId = a.Id
													     LEFT JOIN  dbo.CadDrawingDWG d ON d.Id=c.Id
                                                               WHERE  1=1  {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }


        public static CadDrawingWindowDetail GetCadDrawingWindowDetailByWhere(string _where)
        {
            CadDrawingWindowDetail _caddrawingwindow = new CadDrawingWindowDetail();
            string sql = string.Format(@"SELECT a.Id,a.MId,a.WindowOpenTypeId,b.ArgumentText AS WindowOpenTypeName,
                                                                    a.WindowOpenQtyId,c.ArgumentText AS WindowOpenQtyName,a.WindowHasCorner,
                                                                    a.WindowHasSymmetry,a.WindowSizeMin,a.WindowSizeMax,a.WindowDesignFormula	,
                                                                    a.WindowVentilationQuantity	,a.WindowPlugslotSize	,a.WindowAuxiliaryFrame  
                                                        FROM  dbo.CadDrawingWindowDetail a 
                                                        INNER JOIN dbo.BasArgumentSetting b ON a.WindowOpenTypeId=b.Id AND b.TypeCode='OpenType'
                                                        INNER JOIN dbo.BasArgumentSetting c ON c.Id=a.WindowOpenQtyId AND c.TypeCode='OpenWindowNum'");


            _caddrawingwindow = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingWindowDetail>(new CadDrawingWindowDetail());
            return _caddrawingwindow;
        }


    }
}