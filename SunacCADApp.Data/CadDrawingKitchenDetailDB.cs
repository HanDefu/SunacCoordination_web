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
    ///  厨房CAD原型属性表
    ///</summary>
    public class CadDrawingKitchenDetailDB
    {
        ///<summary>
        /// 厨房CAD原型属性表 分页查询
        ///</summary>
        public static IList<CadDrawingKitchenDetail> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingKitchenDetail> _caddrawingkitchendetails = new List<CadDrawingKitchenDetail>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingKitchenDetail  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingkitchendetails = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingKitchenDetail>(new CadDrawingKitchenDetail());
            return _caddrawingkitchendetails;
        }

        ///<summary>
        /// 厨房CAD原型属性表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingKitchenDetail WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 厨房CAD原型属性表 根据ID查询
        ///</summary>
        public static CadDrawingKitchenDetail GetSingleEntityById(int id)
        {

            CadDrawingKitchenDetail caddrawingkitchendetail = new CadDrawingKitchenDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingKitchenDetail where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingKitchenDetail>(new CadDrawingKitchenDetail());
        }
        ///<summary>
        /// 厨房CAD原型属性表 查询根据条件
        ///</summary>
        public static CadDrawingKitchenDetail GetSingleEntityByparam(string param)
        {

            CadDrawingKitchenDetail caddrawingkitchendetail = new CadDrawingKitchenDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingKitchenDetail where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingKitchenDetail>(new CadDrawingKitchenDetail());

        }

        ///<summary>
        /// 厨房CAD原型属性表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingKitchenDetail caddrawingkitchendetail)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingkitchendetail(MId,KitchenType,KitchenPosition,KitchenIsAirduct,KitchenOpenSizeMin,KitchenOpenSizeMax,KitchenDepthsizeMin,KitchenDepthsizeMax,KitchenBasinSize,KitchenFridgSize,KitchenHearthSize,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},getdate(),{13},'{14}',getdate(),{15},'{16}');SELECT @@IDENTITY AS KitchenId", caddrawingkitchendetail.MId, caddrawingkitchendetail.KitchenType, caddrawingkitchendetail.KitchenPosition, caddrawingkitchendetail.KitchenIsAirduct, caddrawingkitchendetail.KitchenOpenSizeMin, caddrawingkitchendetail.KitchenOpenSizeMax, caddrawingkitchendetail.KitchenDepthsizeMin, caddrawingkitchendetail.KitchenDepthsizeMax, caddrawingkitchendetail.KitchenBasinSize, caddrawingkitchendetail.KitchenFridgSize, caddrawingkitchendetail.KitchenHearthSize, caddrawingkitchendetail.Enabled, caddrawingkitchendetail.Reorder, caddrawingkitchendetail.CreateUserId, caddrawingkitchendetail.CreateBy, caddrawingkitchendetail.ModifiedUserId, caddrawingkitchendetail.ModifiedBy);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(-1);
        }
        ///<summary>
        /// 厨房CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingKitchenDetail caddrawingkitchendetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingkitchendetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingKitchenDetail] SET [MId]=" + caddrawingkitchendetail.MId + ",[KitchenType]=" + caddrawingkitchendetail.KitchenType + ",[KitchenPosition]=" + caddrawingkitchendetail.KitchenPosition + ",[KitchenIsAirduct]=" + caddrawingkitchendetail.KitchenIsAirduct + ",[KitchenOpenSizeMin]=" + caddrawingkitchendetail.KitchenOpenSizeMin + ",[KitchenOpenSizeMax]=" + caddrawingkitchendetail.KitchenOpenSizeMax + ",[KitchenDepthsizeMin]=" + caddrawingkitchendetail.KitchenDepthsizeMin + ",[KitchenDepthsizeMax]=" + caddrawingkitchendetail.KitchenDepthsizeMax + ",[KitchenBasinSize]=" + caddrawingkitchendetail.KitchenBasinSize + ",[KitchenFridgSize]=" + caddrawingkitchendetail.KitchenFridgSize + ",[KitchenHearthSize]=" + caddrawingkitchendetail.KitchenHearthSize + ",[Enabled]=" + caddrawingkitchendetail.Enabled + ",[Reorder]=" + caddrawingkitchendetail.Reorder + ",[ModifiedUserId]=" + caddrawingkitchendetail.ModifiedUserId + ",[ModifiedBy]='" + caddrawingkitchendetail.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 厨房CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format(@"DELETE FROM dbo.CaddrawingMaster WHERE Id={0};
                                                        DELETE FROM dbo.CadDrawingKitchenDetail WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingDWG WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingByArea WHERE MId={0};", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 厨房CAD原型属性表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingKitchenDetail WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 厨房CAD原型属性表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingKitchenDetail WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<CadDrawingWindowSearch> GetSearchPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingWindowSearch> _caddrawingwindowsearchs = new List<CadDrawingWindowSearch>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   (     SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber, a.Id,
                                                                       a.DrawingCode,a.DrawingName,c.DWGPath,a.Reorder,a.CreateOn,a.BillStatus
                                                             FROM dbo.CaddrawingMaster a 
                                                    INNER JOIN dbo.CadDrawingKitchenDetail b ON a.Id=b.MId
                                                     LEFT JOIN dbo.CadDrawingDWG  c ON c.MId = a.Id AND c.CADType='ExpandViewFile'
                                                      WHERE 1=1  {0}
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
            string sql = string.Format(@"      SELECT   COUNT(*) AS CNT  FROM dbo.CaddrawingMaster a 
                                                        INNER JOIN   dbo.CadDrawingKitchenDetail b ON a.Id=b.MId
                                                           LEFT JOIN   dbo.CadDrawingDWG  c ON c.MId = a.Id
                                                               WHERE  1=1  {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }


        public static CadDrawingKitchenDetail GetCadDrawingKitchenDetailById(int Id) 
        {
            string sql = string.Format(@"SELECT TOP 1 a.*,b.ArgumentText AS KitchenTypeName,c.ArgumentText as KitchenPositionName,
                                                                    d.ArgumentText AS KitchenBasinSizeName,e.ArgumentText AS KitchenFridgSizeName,
                                                                    f.ArgumentText AS KitchenHearthSizeName
                                                        FROM dbo.CadDrawingKitchenDetail a 
                                                        LEFT JOIN dbo.BasArgumentSetting b ON a.KitchenType =b.Id AND b.TypeCode='KitchenType'
                                                        LEFT JOIN dbo.BasArgumentSetting c ON a.KitchenPosition =c.Id AND c.TypeCode='DoorWindowPosition'
                                                        LEFT JOIN dbo.BasArgumentSetting d ON a.KitchenBasinSize =d.Id AND d.TypeCode='KitchenBasinType'
                                                        LEFT JOIN dbo.BasArgumentSetting e ON a.KitchenFridgSize =e.Id AND e.TypeCode='RefrigeratorType'
                                                        LEFT JOIN dbo.BasArgumentSetting f ON a.KitchenHearthSize =f.Id AND f.TypeCode='HearthWidth'
                                                        WHERE a.MId={0}",Id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingKitchenDetail>(new CadDrawingKitchenDetail());

        }


        /// <summary>
        /// BPM 厨房原型提交
        /// </summary>
        /// <param name="KitchenId"></param>
        /// <returns></returns>
        public static BPMDynamicKitchen GetBPMDynamicKitchenById(int kitchenId)
        {
            string sql = string.Format(@"SELECT 'P31' AS PageCode ,m.Id AS prototypeID,
                                                                     CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '静态模块' END AS dynamicType,
	                                                                 b.ArgumentText AS kitchenType,c.ArgumentText as doorWindowPos,
	                                                                 CASE a.KitchenIsAirduct WHEN 1 THEN '是' ELSE  '否' END AS hasAirVent,
	                                                                 CONCAT(a.KitchenOpenSizeMin,'mm','-',a.KitchenOpenSizeMax,'mm') AS widthRange,
	                                                                 CONCAT(a.KitchenDepthsizeMin,'mm','-',a.KitchenDepthsizeMax,'mm') AS HeightRange
                                                          FROM dbo.CadDrawingKitchenDetail a
                                                 INNER JOIN dbo.CaddrawingMaster m ON m.Id=a.MId
                                                    LEFT JOIN dbo.BasArgumentSetting b ON a.KitchenType =b.Id AND b.TypeCode='KitchenType'
                                                    LEFT JOIN dbo.BasArgumentSetting c ON a.KitchenPosition =c.Id AND c.TypeCode='DoorWindowPosition'
                                                        WHERE m.Id={0}", kitchenId);
            BPMDynamicKitchen kitchen = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BPMDynamicKitchen>(new BPMDynamicKitchen());
            string _where = string.Format(@" MId={0}", kitchenId);
            string _str_area = "";
            IList<CadDrawingByArea> areas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            foreach (CadDrawingByArea area in areas)
            {
                _str_area += area.AreaName + ",";
            }
            _str_area = _str_area.TrimEnd(',');
            kitchen.region = _str_area;

            string _str_file = string.Empty;
            IList<Drawing> DWGS = CadDrawingDWGDB.GetDrawingByWhere(_where);
            foreach (Drawing drawing in DWGS)
            {
                _str_file += string.Format(@"http://10.4.64.91/{0},", drawing.CADPath);
            }
            _str_file = _str_file.TrimEnd(',');
            kitchen.filePath = _str_file;

            return kitchen;
        }

        public static BPMStaticKitchen GetBPMStaticKitchenById(int kitchenId) 
        {
            string sql = string.Format(@"SELECT 'P32' AS PageCode ,m.Id AS prototypeID,
                                                                    CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '静态模块' END AS dynamicType,
	                                                                b.ArgumentText AS kitchenType,c.ArgumentText as doorWindowPos,
	                                                                CASE a.KitchenIsAirduct WHEN 1 THEN '是' ELSE  '否' END AS hasAirVent,
	                                                                CONCAT(a.KitchenOpenSizeMin,'mm') AS width,
	                                                                CONCAT(a.KitchenOpenSizeMax,'mm') AS Height,
	                                                               d.ArgumentText AS basinSize,
	                                                               e.ArgumentText AS fridgeSize,
	                                                               f.ArgumentText AS gasstoveSize
                                                         FROM dbo.CadDrawingKitchenDetail a
                                                 INNER JOIN dbo.CaddrawingMaster m ON m.Id=a.MId
                                                    LEFT JOIN dbo.BasArgumentSetting b ON a.KitchenType =b.Id AND b.TypeCode='KitchenType'
                                                    LEFT JOIN dbo.BasArgumentSetting c ON a.KitchenPosition =c.Id AND c.TypeCode='DoorWindowPosition'
                                                    LEFT JOIN dbo.BasArgumentSetting d ON a.KitchenBasinSize =d.Id AND d.TypeCode='KitchenBasinType'
                                                    LEFT JOIN dbo.BasArgumentSetting e ON a.KitchenFridgSize =e.Id AND e.TypeCode='RefrigeratorType'
                                                    LEFT JOIN dbo.BasArgumentSetting f ON a.KitchenHearthSize =f.Id AND f.TypeCode='HearthWidth' 
                                                        WHERE m.Id={0}",kitchenId);
            BPMStaticKitchen kitchen= MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BPMStaticKitchen>(new BPMStaticKitchen());
            string _where = string.Format(@" MId={0}", kitchenId);
            string _str_area = "";
            IList<CadDrawingByArea> areas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            foreach (CadDrawingByArea area in areas)
            {
                _str_area += area.AreaName + ",";
            }
            _str_area = _str_area.TrimEnd(',');
            kitchen.region = _str_area;
            string _str_file = string.Empty;
            IList<Drawing> DWGS = CadDrawingDWGDB.GetDrawingByWhere(_where);
            foreach (Drawing drawing in DWGS)
            {
                _str_file += string.Format(@"http://10.4.64.91/{0},", drawing.CADPath);
            }
            _str_file = _str_file.TrimEnd(',');
            kitchen.filePath = _str_file;
            return kitchen;
        }
    }
}
