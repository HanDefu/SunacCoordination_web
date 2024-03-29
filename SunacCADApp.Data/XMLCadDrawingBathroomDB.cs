﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunacCADApp.Entity;
using Common.Utility.Extender;

namespace SunacCADApp.Data
{
    public  class XMLCadDrawingBathroomDB
    {
        protected static IList<Bathroom> GetCadDrawingBathroomByWidth(double Width, double Height, string BathroomDoorWindowPosition, string ToiletType, string AirVent)
        {
            IList<Bathroom> listDoor = new List<Bathroom>();
            string _dynamic_where = "m.DynamicType=1";
            string _static_where = "m.DynamicType=2";

            _dynamic_where += Width > 0 ? string.Format(@" AND (b.BathroomShortSideMin>='{0}' AND b.BathroomShortSideMax<='{0}')  ", Width) : string.Empty;
            _static_where += Width > 0 ? string.Format(@" AND b.BathroomShortSideMin='{0}'", Width) : string.Empty;

            _dynamic_where += Height > 0 ? string.Format(@" AND (b.BathroomLongSizeMin>='{0}' AND b.BathroomLongSizeMax<='{0}')  ", Height) : string.Empty;
            _static_where += Height > 0 ? string.Format(@" AND b.BathroomShortSideMax='{0}'", Height) : string.Empty;

            _dynamic_where += string.IsNullOrEmpty(BathroomDoorWindowPosition) ? string.Empty:string.Format(@" AND ba.ArgumentText = '{0}' ", BathroomDoorWindowPosition);
            _static_where += string.IsNullOrEmpty(BathroomDoorWindowPosition) ? string.Empty : string.Format(@" AND ba.ArgumentText = '{0}' ", BathroomDoorWindowPosition);

            _dynamic_where += string.IsNullOrEmpty(ToiletType) ? string.Empty : string.Format(@" AND b.ArgumentText='{0}' ", ToiletType);
            _static_where += string.IsNullOrEmpty(ToiletType) ? string.Empty : string.Format(@" AND b.ArgumentText='{0}' ", ToiletType);

            int _airVent = string.IsNullOrEmpty(AirVent) ? -1 : (AirVent == "是" ? 1 : 0);
            _dynamic_where += _airVent > 0 ? string.Format(@" AND a.BathroomIsAirduct={0} ", _airVent) : string.Empty;
            _static_where += _airVent > 0 ? string.Format(@" AND a.BathroomIsAirduct={0} ", _airVent) : string.Empty;
            string sql = string.Format(@"      SELECT  m.Id,m.DrawingCode,m.DrawingName,m.Scope,m.DynamicType,
			                                                             CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '定性模块' END AS DynamicType,
			                                                             a.BathroomType,b.ArgumentText AS BathroomTypeName,a.BathroomDoorWindowPosition,ba.ArgumentText AS BathroomDoorWindowPositionName,
			                                                             a.BathroomBasinSize,c.ArgumentText AS BathroomBasinSizeName,a.BathroomClosestoolSize,d.ArgumentText AS BathroomClosestoolSizeName,
			                                                             a.BathroomShortSideMin,a.BathroomShortSideMax,a.BathroomLongSizeMin,a.BathroomLongSizeMax,a.BathroomIsAirduct
                                                                   FROM  dbo.CadDrawingBathroomDetail a 
                                                             INNER JOIN dbo.CaddrawingMaster m ON m.Id=a.MId
                                                              LEFT JOIN  dbo.BasArgumentSetting b ON a.BathroomType=b.Id AND b.TypeCode='ToiletType'
                                                              LEFT JOIN  dbo.BasArgumentSetting ba ON a.BathroomDoorWindowPosition=ba.Id AND ba.TypeCode='DoorWindowPosition'
                                                              LEFT JOIN  dbo.BasArgumentSetting c ON c.Id=a.BathroomBasinSize AND c.TypeCode='ToiletBasinWidth'
                                                              LEFT JOIN  dbo.BasArgumentSetting d ON d.Id=a.BathroomClosestoolSize AND d.TypeCode='ClosesToolWidth' 
                                                         WHERE {0}", _dynamic_where);
            sql += string.Format(@"     UNION ALL   SELECT  m.Id,m.DrawingCode,m.DrawingName,m.Scope,m.DynamicType,
			                                                             CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '定性模块' END AS DynamicType,
			                                                             a.BathroomType,b.ArgumentText AS BathroomTypeName,a.BathroomDoorWindowPosition,ba.ArgumentText AS BathroomDoorWindowPositionName,
			                                                             a.BathroomBasinSize,c.ArgumentText AS BathroomBasinSizeName,a.BathroomClosestoolSize,d.ArgumentText AS BathroomClosestoolSizeName,
			                                                             a.BathroomShortSideMin,a.BathroomShortSideMax,a.BathroomLongSizeMin,a.BathroomLongSizeMax,a.BathroomIsAirduct
                                                                   FROM  dbo.CadDrawingBathroomDetail a 
                                                             INNER JOIN dbo.CaddrawingMaster m ON m.Id=a.MId
                                                              LEFT JOIN  dbo.BasArgumentSetting b ON a.BathroomType=b.Id AND b.TypeCode='ToiletType'
                                                              LEFT JOIN  dbo.BasArgumentSetting ba ON a.BathroomDoorWindowPosition=ba.Id AND ba.TypeCode='DoorWindowPosition'
                                                              LEFT JOIN  dbo.BasArgumentSetting c ON c.Id=a.BathroomBasinSize AND c.TypeCode='ToiletBasinWidth'
                                                              LEFT JOIN  dbo.BasArgumentSetting d ON d.Id=a.BathroomClosestoolSize AND d.TypeCode='ClosesToolWidth' 
                                                         WHERE {0}", _static_where);

            listDoor = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Bathroom>(new Bathroom());
            return listDoor;
        }

        public static IList<Bathroom> GetCadDrawingBathroomListByWidth(double Width, double Height, string BathroomDoorWindowPosition, string ToiletType, string AirVent)
        {

            IList<Bathroom> listBathroom = GetCadDrawingBathroomByWidth(Width,Height,BathroomDoorWindowPosition,ToiletType,AirVent);
            foreach (Bathroom bathroom in listBathroom)
            {
                string _where = string.Format(@" MId={0}", bathroom.Id);
                IList<Drawing> drawingList = CadDrawingDWGDB.GetDrawingByWhere(_where);
                bathroom.Drawings = drawingList.ToArray<Drawing>();

                IList<Area> areaList = CadDrawingByAreaDB.GetAreaByWhere(_where);
                bathroom.Areas = areaList.ToArray<Area>();
            }

            return listBathroom;
        }
    }
}
