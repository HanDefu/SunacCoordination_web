using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunacCADApp.Entity;
using Common.Utility.Extender;

namespace SunacCADApp.Data
{
    public  class XMLCadDrawingHandrailDB
    {
        protected static IList<Handrail> GetCadDrawingHandrailByParame(string RailingType)
        {
            string _where = "1=1";
            _where += string.IsNullOrEmpty(RailingType) ? string.Empty : string.Format(@" AND b.ArgumentText in ({0})", RailingType);
            IList<Handrail> listHandrail = new List<Handrail>();
            string _sql = string.Format(@" SELECT    m.Id,m.DrawingCode,m.DrawingName,m.Scope,m.DynamicType,
			                                                             CASE m.DynamicType WHEN 1 THEN '动态模块' WHEN 2 THEN '定性模块' END AS DynamicType,
			                                                             a.HandrailType,b.ArgumentText AS HandrailTypeName
                                                              FROM   dbo.CadDrawingHandrailDetail a
                                                      INNER JOIN dbo.CaddrawingMaster m ON m.Id=a.MId
                                                         LEFT JOIN  dbo.BasArgumentSetting b ON a.HandrailType=b.Id AND b.TypeCode='HandRail'
                                                                          WHERE {0}", _where);
            listHandrail = MsSqlHelperEx.ExecuteDataTable(_sql).ConvertListModel<Handrail>(new Handrail());
            return listHandrail;
        }

        public static IList<Handrail> GetCadDrawingHandrailListByParam(string RailingType)
        {

            IList<Handrail> listHandrail = GetCadDrawingHandrailByParame(RailingType);
            foreach (Handrail handrail in listHandrail)
            {
                string _where = string.Format(@" MId={0}", handrail.Id);
                IList<Drawing> drawingList = CadDrawingDWGDB.GetDrawingByWhere(_where);
                handrail.Drawings = drawingList.ToArray<Drawing>();

                IList<Area> areaList = CadDrawingByAreaDB.GetAreaByWhere(_where);
                handrail.Areas = areaList.ToArray<Area>();
            }

            return listHandrail;
        }
    }
}
