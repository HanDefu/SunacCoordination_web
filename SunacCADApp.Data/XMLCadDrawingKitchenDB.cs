using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunacCADApp.Entity;
using Common.Utility.Extender;

namespace SunacCADApp.Data
{
    public  class XMLCadDrawingKitchenDB
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="KitchenDoorWindowPosition"></param>
        /// <param name="KitchenType"></param>
        /// <param name="AirVent"></param>
        /// <returns></returns>
        protected static IList<Kitchen> GetKitchenByParam(double Width, double Height, string KitchenDoorWindowPosition, string KitchenType, string AirVent) 
        {
            IList<Kitchen> listKitchen = new List<Kitchen>();
            string _where = string.Empty;
            int _airVent=string.IsNullOrEmpty(AirVent)?-1:(AirVent=="是"?1:0);
            _where += Width > 0 ? string.Format(@" AND (a.KitchenOpenSizeMin >={0}  AND a.KitchenOpenSizeMax<={0})",Width) : string.Empty;
            _where += Height > 0 ? string.Format(@" AND (a.KitchenDepthsizeMin >={0}  AND a.KitchenDepthsizeMax<={0})", Height) : string.Empty;
            _where += string.IsNullOrEmpty(KitchenDoorWindowPosition) ? string.Empty : string.Format(@" AND c.ArgumentText in ({0})", KitchenDoorWindowPosition);
            _where +=string.IsNullOrEmpty(KitchenType) ?string.Empty: string.Format(@" AND a.KitchenType in ({0})", KitchenType);
            _where += _airVent > -1 ? string.Format(@" AND a.KitchenIsAirduct={0}", _airVent) : string.Empty;
            string sql = string.Format(@"	 SELECT m.Id,m.DrawingCode,m.DrawingName,m.Scope,m.DynamicType,
			                                                             CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '定性模块' END AS DynamicType,
			                                                            a.KitchenType,b.ArgumentText AS KitchenTypeName,a.KitchenPosition,c.ArgumentText as KitchenPositionName,
			                                                            a.KitchenBasinSize,d.ArgumentText AS KitchenBasinSizeName,a.KitchenFridgSize,e.ArgumentText AS KitchenFridgSizeName,
			                                                            a.KitchenHearthSize, f.ArgumentText AS KitchenHearthSizeName,a.KitchenIsAirduct,a.KitchenOpenSizeMin,a.KitchenOpenSizeMax,
		                                                                a.KitchenDepthsizeMin,a.KitchenDepthsizeMax
	                                                                FROM dbo.CadDrawingKitchenDetail a
                                                             INNER JOIN dbo.CaddrawingMaster m ON m.Id=a.MId
                                                              LEFT JOIN dbo.BasArgumentSetting b ON a.KitchenType =b.Id AND b.TypeCode='KitchenType'
                                                              LEFT JOIN dbo.BasArgumentSetting c ON a.KitchenPosition =c.Id AND c.TypeCode='DoorWindowPosition'
                                                              LEFT JOIN dbo.BasArgumentSetting d ON a.KitchenBasinSize =d.Id AND d.TypeCode='KitchenBasinType'
                                                              LEFT JOIN dbo.BasArgumentSetting e ON a.KitchenFridgSize =e.Id AND e.TypeCode='RefrigeratorType'
                                                              LEFT JOIN dbo.BasArgumentSetting f ON a.KitchenHearthSize =f.Id AND f.TypeCode='HearthWidth' 
                                                              WHERE 1=1 {0}",_where);
            listKitchen = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Kitchen>(new Kitchen());
            return listKitchen;
        }


        public static IList<Kitchen> GetKitchenListByWidth(double Width, double Height, string KitchenDoorWindowPosition, string KitchenType, string AirVent)
        {

            IList<Kitchen> listKitchen = GetKitchenByParam(Width, Height, KitchenDoorWindowPosition, KitchenType, AirVent);
            foreach (Kitchen kitchen in listKitchen)
            {
                string _where = string.Format(@" MId={0}", kitchen.Id);
                IList<Drawing> drawingList = CadDrawingDWGDB.GetDrawingByWhere(_where);
                kitchen.Drawings = drawingList.ToArray<Drawing>();

                IList<Area> areaList = CadDrawingByAreaDB.GetAreaByWhere(_where);
                kitchen.Areas = areaList.ToArray<Area>();
            }

            return listKitchen;
        }
    }
}
