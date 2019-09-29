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
    ///  卫生间CAD原型属性表
    ///</summary>
    public class CadDrawingBathroomDetailDB
    {
        ///<summary>
        /// 卫生间CAD原型属性表 分页查询
        ///</summary>
        public static IList<CadDrawingBathroomDetail> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingBathroomDetail> _caddrawingbathroomdetails = new List<CadDrawingBathroomDetail>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingBathroomDetail  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingbathroomdetails = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingBathroomDetail>(new CadDrawingBathroomDetail());
            return _caddrawingbathroomdetails;
        }

        ///<summary>
        /// 卫生间CAD原型属性表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingBathroomDetail WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 卫生间CAD原型属性表 根据ID查询
        ///</summary>
        public static CadDrawingBathroomDetail GetSingleEntityById(int id)
        {

            CadDrawingBathroomDetail caddrawingbathroomdetail = new CadDrawingBathroomDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingBathroomDetail where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingBathroomDetail>(new CadDrawingBathroomDetail());
        }
        ///<summary>
        /// 卫生间CAD原型属性表 查询根据条件
        ///</summary>
        public static CadDrawingBathroomDetail GetSingleEntityByparam(string param)
        {

            CadDrawingBathroomDetail caddrawingbathroomdetail = new CadDrawingBathroomDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingBathroomDetail where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingBathroomDetail>(new CadDrawingBathroomDetail());

        }

        ///<summary>
        /// 卫生间CAD原型属性表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingBathroomDetail caddrawingbathroomdetail)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingbathroomdetail(MId,BathroomType,BathroomDoorWindowPosition,BathroomShortSideMin,BathroomShortSideMax,BathroomLongSizeMin,BathroomLongSizeMax,BathroomBasinSize,BathroomClosestoolSize,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy)  
                                     VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},getdate(),{11},'{12}')", caddrawingbathroomdetail.MId, caddrawingbathroomdetail.BathroomType, caddrawingbathroomdetail.BathroomDoorWindowPosition, caddrawingbathroomdetail.BathroomShortSideMin, caddrawingbathroomdetail.BathroomShortSideMax, caddrawingbathroomdetail.BathroomLongSizeMin, caddrawingbathroomdetail.BathroomLongSizeMax, caddrawingbathroomdetail.BathroomBasinSize, caddrawingbathroomdetail.BathroomClosestoolSize, caddrawingbathroomdetail.Enabled, caddrawingbathroomdetail.Reorder, caddrawingbathroomdetail.CreateUserId, caddrawingbathroomdetail.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 卫生间CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingBathroomDetail caddrawingbathroomdetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingbathroomdetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingBathroomDetail] SET [MId]=" + caddrawingbathroomdetail.MId + ",[BathroomType]=" + caddrawingbathroomdetail.BathroomType + ",[BathroomDoorWindowPosition]=" + caddrawingbathroomdetail.BathroomDoorWindowPosition + ",[BathroomShortSideMin]=" + caddrawingbathroomdetail.BathroomShortSideMin + ",[BathroomShortSideMax]=" + caddrawingbathroomdetail.BathroomShortSideMax + ",[BathroomLongSizeMin]=" + caddrawingbathroomdetail.BathroomLongSizeMin + ",[BathroomLongSizeMax]=" + caddrawingbathroomdetail.BathroomLongSizeMax + ",[BathroomBasinSize]=" + caddrawingbathroomdetail.BathroomBasinSize + ",[BathroomClosestoolSize]=" + caddrawingbathroomdetail.BathroomClosestoolSize + ",[Enabled]=" + caddrawingbathroomdetail.Enabled + ",[Reorder]=" + caddrawingbathroomdetail.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 卫生间CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingBathroomDetail WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 卫生间CAD原型属性表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingBathroomDetail WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 卫生间CAD原型属性表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingBathroomDetail WHERE {0} ", param);
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
                                                    INNER JOIN  dbo.CadDrawingBathroomDetail b ON a.Id=b.MId
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
                                                       INNER JOIN  dbo.CadDrawingBathroomDetail b ON a.Id=b.MId
													     LEFT JOIN  (SELECT MIN(Id) AS Id, MId FROM dbo.CadDrawingDWG WHERE  FileClass='JPG' GROUP BY MId) c ON c.MId = a.Id
													     LEFT JOIN  dbo.CadDrawingDWG d ON d.Id=c.Id
                                                               WHERE  1=1  {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }


        public static CadDrawingBathroomDetail GetCadDrawingBathroomDetailByWhere(string _where)
        {
            CadDrawingBathroomDetail _caddrawingwindow = new CadDrawingBathroomDetail();
            string bathroom_sql = string.Format(@"   SELECT  a.*,b.ArgumentText AS BathroomTypeName,ba.ArgumentText AS BathroomDoorWindowPositionName,
                                                                                         c.ArgumentText AS BathroomBasinSizeName,d.ArgumentText AS BathroomClosestoolSizeName 
                                                                             FROM   dbo.CadDrawingBathroomDetail a 
                                                                        LEFT JOIN  dbo.BasArgumentSetting b ON a.BathroomType=b.Id AND b.TypeCode='ToiletType'
                                                                        LEFT JOIN  dbo.BasArgumentSetting ba ON a.BathroomDoorWindowPosition=ba.Id AND ba.TypeCode='BathroomDoorWindowPosition'
                                                                        LEFT JOIN  dbo.BasArgumentSetting c ON c.Id=a.BathroomBasinSize AND c.TypeCode='ToiletBasinWidth'
                                                                        LEFT JOIN  dbo.BasArgumentSetting d ON d.Id=a.BathroomClosestoolSize AND d.TypeCode='ClosesToolWidth'
                                                                      WHERE {0}", _where);
            _caddrawingwindow = MsSqlHelperEx.ExecuteDataTable(bathroom_sql).ConverToModel<CadDrawingBathroomDetail>(new CadDrawingBathroomDetail());
            return _caddrawingwindow;
        }
    }
}