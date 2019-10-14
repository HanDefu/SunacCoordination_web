using System;
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
        protected static IList<Airconditioner> GetCadDrawingAirconditionerByParame(string AirconditionerPower, string AirconditionerPipePosition, string AirconditionerIsRainpipe, string RainpipePosition)
        {
            string _where = "1=1";
            int _airVent = string.IsNullOrEmpty(AirconditionerIsRainpipe) ? -1 : (AirconditionerIsRainpipe == "是" ? 1 : 0);
            _where += string.IsNullOrEmpty(AirconditionerPower) ? string.Empty : string.Format(@" AND ba.ArgumentText in ({0})", AirconditionerPower);
            _where += string.IsNullOrEmpty(AirconditionerPipePosition) ? string.Empty : string.Format(@" AND bb.ArgumentText in ({0})", AirconditionerPipePosition);
            _where += _airVent > 1 ? string.Format(@" AND a.AirconditionerIsRainPipe={0}", _airVent) : string.Empty;
            _where += string.IsNullOrEmpty(RainpipePosition) ? string.Empty : string.Format(@" AND bc.ArgumentText in ({0})", RainpipePosition);
            IList<Airconditioner> listAirconditioner = new List<Airconditioner>();
            string _sql = string.Format(@"     SELECT  m.Id,m.DrawingCode,m.DrawingName,m.Scope,m.DynamicType,
		                                                                    CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '定性模块' END AS DynamicType,
			                                                                a.AirconditionerPower,ba.ArgumentText AS AirconditionerPowerName,
			                                                                a.AirconditionerPipePosition,bb.ArgumentText AS AirconditionerPipePositionName,
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

        public static IList<Airconditioner> GetCadDrawingAirconditionerListByParam(string AirconditionerPower, string AirconditionerPipePosition, string AirconditionerIsRainpipe, string RainpipePosition)
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
