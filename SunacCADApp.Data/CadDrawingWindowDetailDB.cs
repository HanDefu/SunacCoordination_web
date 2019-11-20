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

            string sql = string.Format(@"DELETE FROM dbo.CaddrawingMaster WHERE Id={0};
                                                        DELETE FROM dbo.CadDrawingWindowDetail WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingDWG WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingByArea WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingFunction WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingParameter WHERE MId={0};", Id);
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
                                                           (     SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber, a.Id,
                                                                              a.DrawingCode,a.DrawingName,c.DWGPath,a.Reorder,a.CreateOn,a.BillStatus 
                                                                   FROM  dbo.CaddrawingMaster a 
                                                          INNER JOIN  dbo.CadDrawingWindowDetail b ON a.Id=b.MId
                                                            LEFT JOIN  dbo.CadDrawingDWG c ON c.MId = a.Id AND c.CADType='ExpandViewFile' WHERE  {0}
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
            string sql = string.Format(@"      SELECT   COUNT(*) AS CNT    FROM  dbo.CaddrawingMaster a 
                                                          INNER JOIN  dbo.CadDrawingWindowDetail b ON a.Id=b.MId
                                                            LEFT JOIN  dbo.CadDrawingDWG c ON c.MId = a.Id AND c.CADType='ExpandViewFile'
                                                               WHERE  {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }


        public static CadDrawingWindowDetail GetCadDrawingWindowDetailByWhere(string _where)
        {
            CadDrawingWindowDetail _caddrawingwindow = new CadDrawingWindowDetail();
            string sql = string.Format(@"SELECT a.Id,a.MId,a.WindowOpenTypeId,b.ArgumentText AS WindowOpenTypeName,
                                                                    a.WindowOpenQtyId,c.ArgumentText AS WindowOpenQtyName,a.WindowHasCorner,
                                                                    a.WindowHasSymmetry,a.WindowSizeMin,a.WindowSizeMax,
                                                                    a.WindowDesignFormula AS WindowDesignFormula,
                                                                    a.WindowVentilationQuantity	,a.WindowPlugslotSize	,a.WindowAuxiliaryFrame  
                                                        FROM  dbo.CadDrawingWindowDetail a 
                                                        INNER JOIN dbo.BasArgumentSetting b ON a.WindowOpenTypeId=b.Id AND b.TypeCode='OpenType'
                                                        INNER JOIN dbo.BasArgumentSetting c ON c.Id=a.WindowOpenQtyId AND c.TypeCode='OpenWindowNum'
                                                               WHERE {0}",_where);
            _caddrawingwindow = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingWindowDetail>(new CadDrawingWindowDetail());
            return _caddrawingwindow;
        }

        public static IList<Window> GetXMLWindow(double width=0, double height = 0, string openType = "", string openNum = "", string gongNengQu = "") 
        {
            string _where = string.Empty;
            string _where2 = string.Empty;
            if (width > 0)
            {
                _where += string.Format(@" and 	(a.WindowSizeMin<={0} AND a.WindowSizeMax>={0}) ", width);
                _where2 += string.Format(@" and a.WindowSizeMin={0}", width);
            }

            if (height > 0)
            {
                _where2 += string.Format(@" and a.WindowSizeMax={0}", height);
            }

            if (!string.IsNullOrEmpty(openType)) 
            {
                _where += string.Format(@" and 	 b.ArgumentText	='{0}' ", openType);
                _where2 += string.Format(@" and b.ArgumentText	 ='{0}'", openType);
            }
            if (!string.IsNullOrEmpty(openNum))
            {
                _where += string.Format(@" and 	 c.ArgumentText	='{0}' ", openNum);
                _where2 += string.Format(@" and c.ArgumentText	 ='{0}'", openNum);
            }

            if (!string.IsNullOrEmpty(gongNengQu))
            {
                _where += string.Format(@" AND EXISTS (SELECT Area.AreaID,setting.ArgumentText 
	                                                                    FROM dbo.CadDrawingByArea Area 
                                                              INNER  JOIN dbo.BasArgumentSetting setting ON setting.Id=Area.AreaID
                                                                     WHERE Area.MId=m.Id AND  setting.ArgumentText IN ({0})) ", gongNengQu);
                _where2 += string.Format(@" AND EXISTS (SELECT Area.AreaID,setting.ArgumentText 
	                                                                    FROM dbo.CadDrawingByArea Area 
                                                              INNER  JOIN dbo.BasArgumentSetting setting ON setting.Id=Area.AreaID
                                                                      WHERE Area.MId=m.Id AND  setting.ArgumentText IN ({0})) ", gongNengQu);
            }
            IList<Window> _window = new List<Window>();
            string xmlsql = string.Format(@"SELECT  m.Id, m.DrawingCode,m.DrawingName,m.Scope,m.DrawingType,m.DynamicType, a.MId,a.WindowOpenTypeId,b.ArgumentText AS WindowOpenTypeName,
                                                                    a.WindowOpenQtyId,c.ArgumentText AS WindowOpenQtyName,a.WindowHasCorner,
                                                                    a.WindowHasSymmetry,a.WindowSizeMin,a.WindowSizeMax,a.WindowDesignFormula	,
                                                                    a.WindowVentilationQuantity	,a.WindowPlugslotSize	,a.WindowAuxiliaryFrame  

                                                             FROM dbo.CaddrawingMaster m 
                                                             INNER JOIN  dbo.CadDrawingWindowDetail a ON m.Id=a.MId
                                                             INNER JOIN dbo.BasArgumentSetting b ON a.WindowOpenTypeId=b.Id AND b.TypeCode='OpenType'
                                                             INNER JOIN dbo.BasArgumentSetting c ON c.Id=a.WindowOpenQtyId AND c.TypeCode='OpenWindowNum'
                                                             WHERE m.DynamicType=1 {0} ",_where);

             xmlsql += string.Format(@"  	 UNION ALL  SELECT  m.Id, m.DrawingCode,m.DrawingName,m.Scope,m.DrawingType,m.DynamicType, a.MId,a.WindowOpenTypeId,b.ArgumentText AS WindowOpenTypeName,
                                                                    a.WindowOpenQtyId,c.ArgumentText AS WindowOpenQtyName,a.WindowHasCorner,
                                                                    a.WindowHasSymmetry,a.WindowSizeMin,a.WindowSizeMax,a.WindowDesignFormula	,
                                                                    a.WindowVentilationQuantity	,a.WindowPlugslotSize	,a.WindowAuxiliaryFrame  

                                                             FROM dbo.CaddrawingMaster m 
                                                             INNER JOIN  dbo.CadDrawingWindowDetail a ON m.Id=a.MId
                                                             INNER JOIN dbo.BasArgumentSetting b ON a.WindowOpenTypeId=b.Id AND b.TypeCode='OpenType'
                                                             INNER JOIN dbo.BasArgumentSetting c ON c.Id=a.WindowOpenQtyId AND c.TypeCode='OpenWindowNum'
                                                             WHERE m.DynamicType=2 {0} ", _where2);

            _window = MsSqlHelperEx.ExecuteDataTable(xmlsql).ConvertListModel<Window>(new Window());
            return _window;
        }

        /// <summary>
        /// 动态模型
        /// </summary>
        /// <param name="windowId"></param>
        /// <returns></returns>
        public static BPMDynamicWindow GetBPMDynamicWindowByWhere(int windowId) 
        {
            BPMDynamicWindow window = new BPMDynamicWindow();
            string sql = string.Format(@"SELECT  'P11' AS PageCode, m.Id AS prototypeID, 
                                                                     CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '静态模块' END AS dynamicType,
		                                                             b.ArgumentText AS openType,
                                                                     CONCAT(c.ArgumentText,'扇')  AS openCount,
		                                                             CASE a.WindowHasCorner WHEN 1 THEN '是' ELSE '否' END AS isCorner, 
                                                                     CASE a.WindowHasSymmetry WHEN 1 THEN '是' ELSE '否' END AS isMirror,
                                                                     CONCAT(a.WindowSizeMin,'mm','- ',a.WindowSizeMax,'mm') AS widthRange,
		                                                             a.WindowDesignFormula AS airVolumeFormula
                                                           FROM dbo.CaddrawingMaster m 
                                                   INNER JOIN  dbo.CadDrawingWindowDetail a ON m.Id=a.MId
                                                      LEFT JOIN dbo.BasArgumentSetting b ON a.WindowOpenTypeId=b.Id AND b.TypeCode='OpenType'
                                                      LEFT JOIN dbo.BasArgumentSetting c ON c.Id=a.WindowOpenQtyId AND c.TypeCode='OpenWindowNum'
                                                          WHERE  m.id={0}", windowId);
            window = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BPMDynamicWindow>(new BPMDynamicWindow());

            string _where = string.Format(@" MId={0}", windowId);
            string _str_area = "";
            IList<CadDrawingByArea> areas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            foreach (CadDrawingByArea area in areas)
            {
                _str_area += area.AreaName + ",";
            }

            string scopeName = CadDrawingMasterDB.GetScopeNameByMId(windowId);
            if (!string.IsNullOrEmpty(scopeName))
            {
                _str_area += "集团,";
            }

            _str_area = _str_area.TrimEnd(',');

            string _str_function = "";
            IList<CadDrawingFunction> funcs = CadDrawingFunctionDB.GetCadDrawingFunctionByWhereList(_where);
            foreach (CadDrawingFunction func in funcs)
            {
                _str_function += string.Format(@"{0},", func.FunctionName);
            }
            _str_function = _str_function.TrimEnd(',');

            string _str_file = string.Empty;
            IList<Drawing> DWGS = CadDrawingDWGDB.GetDrawingByWhere(_where);
            foreach (Drawing drawing in DWGS)
            {
                _str_file += string.Format(@"{0},", drawing.CADPath);
            }
            _str_file = _str_file.TrimEnd(',');

            window.region = _str_area;
            window.functionalAreas = _str_function;
            window.filePath = _str_file;
            window.SizeParas = CadDrawingParameterDB.GetBPMSizeParamList(_where).ToArray<SizePara>();
            return window;
        }

        public static BPMStaticWindow GetBPMStaticWindowByWindowId(int windowId) 
        {
            BPMStaticWindow window = new BPMStaticWindow();
            string sql = string.Format(@"SELECT  'P12' AS PageCode, m.Id AS prototypeID, 
                                                                     CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '静态模块' END AS dynamicType,
		                                                             b.ArgumentText AS openType,
                                                                     CONCAT(c.ArgumentText,'扇')  AS openCount,
		                                                             CASE a.WindowHasCorner WHEN 1 THEN '是' ELSE '否' END AS isCorner, 
                                                                     CASE a.WindowHasSymmetry WHEN 1 THEN '是' ELSE '否' END AS isMirror,
                                                                     CONCAT(a.WindowSizeMin,'mm','- ',a.WindowSizeMax,'mm') AS widthRange,
		                                                             CASE a.WindowAuxiliaryFrame WHEN 1 THEN '是' ELSE '否' END  AS hasAuxiliaryFrame
                                                           FROM dbo.CaddrawingMaster m 
                                                   INNER JOIN  dbo.CadDrawingWindowDetail a ON m.Id=a.MId
                                                      LEFT JOIN dbo.BasArgumentSetting b ON a.WindowOpenTypeId=b.Id AND b.TypeCode='OpenType'
                                                      LEFT JOIN dbo.BasArgumentSetting c ON c.Id=a.WindowOpenQtyId AND c.TypeCode='OpenWindowNum'
                                                          WHERE  m.id={0}", windowId);
            window = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BPMStaticWindow>(new BPMStaticWindow());
            string _where = string.Format(@" MId={0}", windowId);
            string _str_area = "";
            IList<CadDrawingByArea> areas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            foreach (CadDrawingByArea area in areas)
            {
                _str_area += area.AreaName + ",";
            }

            string scopeName = CadDrawingMasterDB.GetScopeNameByMId(windowId);
            if (!string.IsNullOrEmpty(scopeName)) 
            {
                _str_area += "集团,";
            }

            _str_area = _str_area.TrimEnd(',');

            string _str_function = "";
            IList<CadDrawingFunction> funcs = CadDrawingFunctionDB.GetCadDrawingFunctionByWhereList(_where);
            foreach (CadDrawingFunction func in funcs)
            {
                _str_function += string.Format(@"{0},", func.FunctionName);
            }
            _str_function = _str_function.TrimEnd(',');

            string _str_file = string.Empty;
            IList<Drawing> DWGS = CadDrawingDWGDB.GetDrawingByWhere(_where);
            foreach (Drawing drawing in DWGS)
            {
                _str_file += string.Format(@"{0},", drawing.CADPath);
            }
            _str_file = _str_file.TrimEnd(',');
            window.region = _str_area;
            window.functionAreas = _str_function;
            window.filePath = _str_file;
            return window;
        }



    }
}