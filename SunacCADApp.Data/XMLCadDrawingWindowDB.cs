using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunacCADApp.Entity;
using SunacCADApp.Library;

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
                IList<CadDrawingDWG> dwgList = CadDrawingDWGDB.GetPageInfoByParameter(_where,string.Empty,0,10);
                CadDrawingDWG dwg=null;
                if (dwgList.Count > 1) 
                {
                    dwg = dwgList.ElementAt<CadDrawingDWG>(0);
                    window.DrawingPathTop = XmlSerializeHelper.DecodeImageToBase64(dwg.CADPath);
                }
                if (dwgList.Count > 2)
                {
                    dwg = dwgList.ElementAt<CadDrawingDWG>(1);
                    window.DrawingPathFront = dwg.CADPath;
                }
                if (dwgList.Count > 3)
                {
                    dwg = dwgList.ElementAt<CadDrawingDWG>(2);
                    window.DrawingPathLeft = dwg.CADPath;
                }
                if (dwgList.Count > 4)
                {
                    dwg = dwgList.ElementAt<CadDrawingDWG>(3);
                    window.DrawingPathExpanded = dwg.CADPath;
                }

                string function="";
                IList<CadDrawingFunction> functions = CadDrawingFunctionDB.GetCadDrawingFunctionByWhereList(_where);
                foreach (CadDrawingFunction fun in functions) 
                {
                    function = fun.FunctionName + ",";
                }
                window.WindowFunctionalArea = function.TrimEnd(',');
                string areaid = "";
                string areaname = "";
                IList<CadDrawingByArea> areas= CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);

                foreach (CadDrawingByArea area in areas)
                {
                    areaid = area.AreaID + ",";
                }
                window.AreaId = areaid.TrimEnd(',');


                foreach (CadDrawingByArea area in areas)
                {
                    areaname = area.AreaName + ",";
                }
                window.AreaName = areaname.TrimEnd(',');

                IList<Item> items = CadDrawingParameterDB.GetCadDrawingItemByWhereList(_where);
                window.SizePara = items.ToArray<Item>();

            }

            return new XMLCadDrawingWindow() { Code = 100, Message = "查询成功", Windows = xmlWindows.ToArray<Window>() };
        }
   
        
    }
}
