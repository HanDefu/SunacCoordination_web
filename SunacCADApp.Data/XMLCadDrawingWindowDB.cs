using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunacCADApp.Entity;
using SunacCADApp.Library;
using Common.Utility.Extender;

namespace SunacCADApp.Data
{
    public  class XMLCadDrawingWindowDB
    {
        public static XMLCadDrawingWindow GetXMLCadDrawingWindow(double width = 0, double height = 0, string openType = "", string openNum = "", string gongNengQu = "") 
        {
            IList<Window> xmlWindows = CadDrawingWindowDetailDB.GetXMLWindow(width,height,openType,openNum,gongNengQu);
            foreach (Window window in xmlWindows) 
            {
                string _where = string.Format(@" MId={0}",window.Id);

                IList<Drawing> drawingList = CadDrawingDWGDB.GetDrawingByWhere(_where);
                window.Drawings = drawingList.ToArray<Drawing>();
                IList<Function> Funs = CadDrawingFunctionDB.GetFunctionByWhereList(_where);
                window.Functions = Funs.ToArray<Function>();
                IList<Area> areas = CadDrawingByAreaDB.GetAreaByWhere(_where);
                window.Areas=areas.ToArray<Area>();
                IList<Item> items = CadDrawingParameterDB.GetCadDrawingItemByWhereList(_where);
                window.SizePara = items.ToArray<Item>();

            }

            return new XMLCadDrawingWindow() { Code = 100, Message = "查询成功", Windows = xmlWindows.ToArray<Window>() };
        }

        public static string GetCADFileDownloadByWhere(int Id,string Type="CAD") 
        {
            string _where = string.Format(@" Id={0}", Id);
            Drawing drawing = CadDrawingDWGDB.GetDrawingSingeByWhere(_where);
            if (Type.ToUpper() == "CAD") 
            {
                if (string.IsNullOrEmpty(drawing.CADPath))
                    return string.Empty;

                string base64Img = XmlSerializeHelper.DecodeImageToBase64(drawing.CADPath);
                return base64Img;
            }
            else if (Type.ToUpper() == "IMG")
            {
                if (string.IsNullOrEmpty(drawing.ImgPath))
                    return string.Empty;

                string base64Img = XmlSerializeHelper.DecodeImageToBase64(drawing.ImgPath);
                return base64Img;
            }
            else 
            {
                return string.Empty;
            }
          

        }


        public static string GeImgFileDownloadByID(int Id) 
        {
            string _where = string.Format(@" Id={0}", Id);
            Drawing drawing = CadDrawingDWGDB.GetDrawingSingeByWhere(_where);
            if (string.IsNullOrEmpty(drawing.CADPath))
                return string.Empty;
            string base64Img = XmlSerializeHelper.DecodeImageToBase64(drawing.ImgPath);
            return base64Img;
        }


        public static BPMDynamicWindow  BPM_XML_Window(int Id) 
        {
            string xmlsql = string.Format(@"    SELECT  'P11' AS PageCode, m.Id AS prototypeID, 
                                                                        m.DrawingType AS dynamicType,
		                                                                b.ArgumentText AS openType,
                                                                        CONCAT(c.ArgumentText,'扇')  AS openCount,
		                                                                CASE a.WindowHasCorner WHEN 1 THEN '是' ELSE '否' END AS isCorner, 
                                                                        CASE a.WindowHasSymmetry WHEN 1 THEN '是' ELSE '否' END AS isMirror,
		                                                                CONCAT(a.WindowSizeMin,'MM - ',a.WindowSizeMax,'MM') AS widthRange,
		                                                                a.WindowDesignFormula AS airVolumeFormula
                                                                  FROM dbo.CaddrawingMaster m 
                                                            INNER JOIN  dbo.CadDrawingWindowDetail a ON m.Id=a.MId
                                                             LEFT JOIN dbo.BasArgumentSetting b ON a.WindowOpenTypeId=b.Id AND b.TypeCode='OpenType'
                                                             LEFT JOIN dbo.BasArgumentSetting c ON c.Id=a.WindowOpenQtyId AND c.TypeCode='OpenWindowNum'
                                                                 WHERE m.Id={0}",Id);
            BPMDynamicWindow window = MsSqlHelperEx.ExecuteDataTable(xmlsql).ConverToModel<BPMDynamicWindow>(null);
            string _where = string.Format(@" MId={0}", Id);
            string _str_area = "";
            IList<CadDrawingByArea> areas =  CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            foreach (CadDrawingByArea area in areas) 
            {
                _str_area += area.AreaName + ",";
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
            window.SizeParas = CadDrawingParameterDB.GetBPMSizeParamList(_where).ToArray<SizePara>();
            return window;
           
        }

    }
}
