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


            string sql = string.Format(@"INSERT INTO dbo.caddrawingbathroomdetail(MId,BathroomType,BathroomDoorWindowPosition,BathroomShortSideMin,
                                                        BathroomShortSideMax,BathroomLongSizeMin,BathroomLongSizeMax,BathroomBasinSize,BathroomClosestoolSize,
                                                       Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,BathroomIsAirduct)  
                                                       VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},getdate(),{11},'{12}',{13})", 
                                                       caddrawingbathroomdetail.MId, caddrawingbathroomdetail.BathroomType, caddrawingbathroomdetail.BathroomDoorWindowPosition,
                                                       caddrawingbathroomdetail.BathroomShortSideMin, caddrawingbathroomdetail.BathroomShortSideMax, 
                                                       caddrawingbathroomdetail.BathroomLongSizeMin, caddrawingbathroomdetail.BathroomLongSizeMax, 
                                                       caddrawingbathroomdetail.BathroomBasinSize, caddrawingbathroomdetail.BathroomClosestoolSize, 
                                                       caddrawingbathroomdetail.Enabled, caddrawingbathroomdetail.Reorder, caddrawingbathroomdetail.CreateUserId,
                                                       caddrawingbathroomdetail.CreateBy,caddrawingbathroomdetail.BathroomIsAirduct);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 卫生间CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingBathroomDetail caddrawingbathroomdetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingbathroomdetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingBathroomDetail] SET [MId]=" + caddrawingbathroomdetail.MId + ",[BathroomType]=" + caddrawingbathroomdetail.BathroomType + ",[BathroomDoorWindowPosition]=" + caddrawingbathroomdetail.BathroomDoorWindowPosition + ",[BathroomShortSideMin]=" + caddrawingbathroomdetail.BathroomShortSideMin + ",[BathroomShortSideMax]=" + caddrawingbathroomdetail.BathroomShortSideMax + ",[BathroomLongSizeMin]=" + caddrawingbathroomdetail.BathroomLongSizeMin + ",[BathroomLongSizeMax]=" + caddrawingbathroomdetail.BathroomLongSizeMax + ",[BathroomBasinSize]=" + caddrawingbathroomdetail.BathroomBasinSize + ",[BathroomClosestoolSize]=" + caddrawingbathroomdetail.BathroomClosestoolSize + ",[Enabled]=" + caddrawingbathroomdetail.Enabled + ",[Reorder]=" + caddrawingbathroomdetail.Reorder + ",[BathroomIsAirduct] =" + caddrawingbathroomdetail.BathroomIsAirduct+ "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 卫生间CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format(@"DELETE FROM dbo.CaddrawingMaster WHERE Id={0};
                                                        DELETE FROM dbo.CadDrawingBathroomDetail WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingDWG WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingByArea WHERE MId={0};", Id);
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
        /// 卫生间原型查询 分页查询
        ///</summary>
        public static IList<CadDrawingWindowSearch> GetSearchPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingWindowSearch> _caddrawingwindowsearchs = new List<CadDrawingWindowSearch>();
            string sql = string.Format(@"SELECT  * FROM 
                                                           (SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber, a.Id,
                                                                        a.DrawingCode,a.DrawingName,d.DWGPath,a.Reorder,a.CreateOn,a.BillStatus
                                                             FROM  dbo.CaddrawingMaster a 
                                                    INNER JOIN  dbo.CadDrawingBathroomDetail b ON a.Id=b.MId
													  LEFT JOIN  dbo.CadDrawingDWG d ON d.MId=a.Id AND d.CADType='ExpandViewFile'  WHERE 1=1  {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);
            _caddrawingwindowsearchs = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingWindowSearch>(new CadDrawingWindowSearch());
            return _caddrawingwindowsearchs;
        }

        ///<summary>
        /// 卫生间原型查询  分页数据总数量
        ///<summary>
        public static int GetSearchPageCountByParameter(string _where)
        {
            string sql = string.Format(@"      SELECT   COUNT(*) AS CNT   FROM dbo.CaddrawingMaster a 
                                                       INNER JOIN  dbo.CadDrawingBathroomDetail b ON a.Id=b.MId
													     LEFT JOIN  dbo.CadDrawingDWG d ON d.MId=a.Id AND d.CADType='ExpandViewFile' 
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

        public static BPMDynamicBathroom GetBPMDynamicBathroomByBathroomId(int bathroomId) 
        {
            BPMDynamicBathroom bathroom = new BPMDynamicBathroom();
            string sql = string.Format(@" SELECT  'P41' AS PageCode, m.Id AS prototypeID,
                                                                      CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '静态模块' END AS dynamicType,
		                                                              b.ArgumentText AS bathroomType,
			                                                          CASE a.BathroomIsAirduct WHEN 1 THEN '是' ELSE '否' END AS hasAirVent, 
			                                                          ba.ArgumentText AS doorWindowPos,
			                                                         CONCAT(a.BathroomShortSideMin,'mm','-',a.BathroomShortSideMax,'mm') AS widthRange,
			                                                         CONCAT(a.BathroomLongSizeMin,'mm','-',a.BathroomLongSizeMax,'mm') AS HeightRange
                                                          FROM  dbo.CadDrawingBathroomDetail a 
                                                    INNER JOIN  dbo.CaddrawingMaster m ON m.Id=a.MId
                                                     LEFT JOIN  dbo.BasArgumentSetting b ON a.BathroomType=b.Id AND b.TypeCode='ToiletType'
                                                     LEFT JOIN  dbo.BasArgumentSetting ba ON a.BathroomDoorWindowPosition=ba.Id AND ba.TypeCode='BathroomDoorWindowPosition'
                                                     WHERE m.Id={0}", bathroomId);
            bathroom = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BPMDynamicBathroom>(new BPMDynamicBathroom());
            string _where = string.Format(@" MId={0}", bathroomId);
            string _str_area = "";
            IList<CadDrawingByArea> areas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            foreach (CadDrawingByArea area in areas)
            {
                _str_area += area.AreaName + ",";
            }

            string scopeName = CadDrawingMasterDB.GetScopeNameByMId(bathroomId);
            if (!string.IsNullOrEmpty(scopeName)) 
            {
                _str_area += "集团" + ",";
            }
            _str_area = _str_area.TrimEnd(',');
            bathroom.region = _str_area;
            string _str_file = string.Empty;
            IList<Drawing> DWGS = CadDrawingDWGDB.GetDrawingByWhere(_where);
            foreach (Drawing drawing in DWGS)
            {
                _str_file += string.Format(@"{1}/{0},", CommonLib.WebURL, drawing.CADPath);
            }
            _str_file = _str_file.TrimEnd(',');
            bathroom.filePath = _str_file;
            return bathroom;
        }

        public static BPMStaticBathroom GetBPMStaticBathroomByBathroomId(int bathroomId) 
        {
            BPMStaticBathroom bathroom = new BPMStaticBathroom();
            string sql = string.Format(@" SELECT  'P42' AS PageCode, m.Id AS prototypeID,
                                                                CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '静态模块' END AS dynamicType,
		                                                        b.ArgumentText AS bathroomType,
			                                                    CASE a.BathroomIsAirduct WHEN 1 THEN '是' ELSE '否' END AS hasAirVent, 
			                                                    ba.ArgumentText AS doorWindowPos,
			                                                    a.BathroomShortSideMin AS width,
			                                                    a.BathroomShortSideMax AS Height,
		                                                        c.ArgumentText AS basinSize,
			                                                    d.ArgumentText AS closestoolSize
                                                          FROM  dbo.CadDrawingBathroomDetail a 
                                                    INNER JOIN  dbo.CaddrawingMaster m ON m.Id=a.MId
                                                     LEFT JOIN  dbo.BasArgumentSetting b ON a.BathroomType=b.Id AND b.TypeCode='ToiletType'
                                                     LEFT JOIN  dbo.BasArgumentSetting ba ON a.BathroomDoorWindowPosition=ba.Id AND ba.TypeCode='BathroomDoorWindowPosition'
                                                     LEFT JOIN  dbo.BasArgumentSetting c ON c.Id=a.BathroomBasinSize AND c.TypeCode='ToiletBasinWidth'
                                                     LEFT JOIN  dbo.BasArgumentSetting d ON d.Id=a.BathroomClosestoolSize AND d.TypeCode='ClosesToolWidth' 
                                                     WHERE m.Id={0}", bathroomId);
            bathroom = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BPMStaticBathroom>(new BPMStaticBathroom());
            string _where = string.Format(@" MId={0}", bathroomId);
            string _str_area = "";
            IList<CadDrawingByArea> areas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            foreach (CadDrawingByArea area in areas)
            {
                _str_area += area.AreaName + ",";
            }
            string scopeName = CadDrawingMasterDB.GetScopeNameByMId(bathroomId);
            if (!string.IsNullOrEmpty(scopeName))
            {
                _str_area += "集团" + ",";
            }
            _str_area = _str_area.TrimEnd(',');
            bathroom.region = _str_area;
            string _str_file = string.Empty;
            IList<Drawing> DWGS = CadDrawingDWGDB.GetDrawingByWhere(_where);
            foreach (Drawing drawing in DWGS)
            {
                _str_file += string.Format(@"{1}/{0},",CommonLib.WebURL, drawing.CADPath);
            }
            _str_file = _str_file.TrimEnd(',');
            bathroom.filePath = _str_file;
            return bathroom;
        }
    }
}