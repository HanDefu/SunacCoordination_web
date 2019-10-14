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

        public static string GetCADFileDownloadByWhere(int Id) 
        {
            string _where = string.Format(@" Id={0}", Id);
            Drawing drawing = CadDrawingDWGDB.GetDrawingSingeByWhere(_where);
            if (string.IsNullOrEmpty(drawing.CADPath))
                return string.Empty;
            string base64Img = XmlSerializeHelper.DecodeImageToBase64(drawing.CADPath);
            return base64Img;

        }
    }
}
