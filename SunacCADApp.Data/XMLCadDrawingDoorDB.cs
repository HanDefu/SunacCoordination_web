using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility.Extender;
using System.Threading.Tasks;
using SunacCADApp.Entity;

namespace SunacCADApp.Data
{
    public  class XMLCadDrawingDoorDB
    {

        protected static IList<Door> GetCadDrawingDoorsByWidth(double width, string doorType) 
        {
            IList<Door> listDoor = new List<Door>();
            string _where = "1=1";
            _where+=width>0?string.Format(@" AND (b.WindowSizeMin>='{0}' AND b.WindowSizeMax<='{0}')  ",width):string.Empty;
            _where +=string.IsNullOrEmpty(doorType) ?string.Empty: string.Format(@" AND c.ArgumentText in ({0})", doorType); 
            string sql = string.Format(@" SELECT  a.Id,a.DrawingCode,a.DrawingName,a.Scope,a.DynamicType,
                                                                      CASE a.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '定性模块' END AS DynamicType,
                                                                      b.DoorType,c.ArgumentText AS DoorTypeName,b.WindowSizeMin,b.WindowSizeMax,b.WindowDesignFormula
                                                           FROM  dbo.CaddrawingMaster a 
                                                  INNER JOIN  dbo.CadDrawingDoorDetail b ON a.Id=b.MId
                                                     LEFT JOIN  dbo.BasArgumentSetting c ON c.Id=b.DoorType AND c.TypeCode='DoorType' 
                                                         WHERE {0}", _where);

            listDoor = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Door>(new Door());
            return listDoor;
        }

        public static IList<Door> GetCadDrawingDoorsListByWidth(double width, string doorType) 
        {
            
            IList<Door> listDoor = GetCadDrawingDoorsByWidth(width, doorType);
            foreach (Door door in listDoor)
            {
                string _where = string.Format(@" MId={0}", door.Id);
                IList<Drawing> drawingList = CadDrawingDWGDB.GetDrawingByWhere(_where);
                door.Drawings = drawingList.ToArray<Drawing>();



                IList<Area> areaList = CadDrawingByAreaDB.GetAreaByWhere(_where);
                door.Areas = areaList.ToArray<Area>();

                IList<Item> items = CadDrawingParameterDB.GetCadDrawingItemByWhereList(_where);
                door.SizePara = items.ToArray<Item>();




            }

             return listDoor;
        }
    }
}
