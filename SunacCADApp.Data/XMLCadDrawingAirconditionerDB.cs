﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunacCADApp.Entity;
using Common.Utility.Extender;

namespace SunacCADApp.Data
{
    public  class XMLCadDrawingAirconditionerDB
    {
        protected static IList<Airconditioner> GetCadDrawingAirconditionerByParame(int AirconditionerPower, int AirconditionerPipePosition, int AirconditionerIsRainpipe, int RainpipePosition)
        {
            string _where = "1=1";
            _where = AirconditionerPower > 1 ? string.Format(@" AND a.AirconditionerPower={0}", AirconditionerPower) : string.Empty;
            _where = AirconditionerPipePosition > 1 ? string.Format(@" AND a.AirconditionerPipePosition,={0}", AirconditionerPipePosition) : string.Empty;
            _where = AirconditionerIsRainpipe > 1 ? string.Format(@" AND a.AirconditionerIsRainPipe={0}", AirconditionerIsRainpipe) : string.Empty;
            _where = RainpipePosition > 1 ? string.Format(@" AND a.AirconditionerRainPipePosition={0}", RainpipePosition) : string.Empty;
            IList<Airconditioner> listAirconditioner = new List<Airconditioner>();
            string _sql = string.Format(@"     SELECT  m.Id,m.DrawingCode,m.DrawingName,m.Scope,m.DynamicType,
		                                                                    CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '定性模块' END AS DynamicType,
			                                                                a.AirconditionerPower,ba.ArgumentText AS AirconditionerPowerName,
			                                                                ,a.AirconditionerPipePosition,bb.ArgumentText AS AirconditionerPipePositionName,
			                                                                a.AirconditionerRainPipePosition, bc.ArgumentText AS AirconditionerRainPipePositionName,
                                                                            a.AirconditionerMinWidth,a.AirconditionerMinLength,a.AirconditionerIsRainPipe
                                                                      FROM   dbo.CadDrawingAirconditionerDetail a 
                                                                INNER JOIN dbo.CaddrawingMaster m ON m.Id=a.MId
                                                                 LEFT JOIN  dbo.BasArgumentSetting ba ON a.AirconditionerPower=ba.Id AND ba.TypeCode='AirConditionNumber'
                                                                 LEFT JOIN  dbo.BasArgumentSetting bb ON a.AirconditionerPipePosition=bb.Id AND bb.TypeCode='CondensatePipePosition'
                                                                 LEFT JOIN  dbo.BasArgumentSetting bc ON a.AirconditionerRainPipePosition=bc.Id AND bc.TypeCode='RainPipePosition'
                                                                          WHERE {0}", _where);
            listAirconditioner = MsSqlHelperEx.ExecuteDataTable(_sql).ConvertListModel<Airconditioner>(new Airconditioner());
            return listAirconditioner;
        }

        public static IList<Airconditioner> GetCadDrawingAirconditionerListByParam(int AirconditionerPower, int AirconditionerPipePosition, int AirconditionerIsRainpipe, int RainpipePosition)
        {

            IList<Airconditioner> listAirconditioner = GetCadDrawingAirconditionerByParame(AirconditionerPower, AirconditionerPipePosition, AirconditionerIsRainpipe, RainpipePosition);
            foreach (Airconditioner airconditioner in listAirconditioner)
            {
                string _where = string.Format(@" MId={0}", airconditioner.Id);
                IList<Drawing> drawingList = CadDrawingDWGDB.GetDrawingByWhere(_where);
                airconditioner.Drawings = drawingList.ToArray<Drawing>();

                IList<Area> areaList = CadDrawingByAreaDB.GetAreaByWhere(_where);
                airconditioner.Areas = areaList.ToArray<Area>();
            }
            return listAirconditioner;
        }
    }
}
