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
    ///  外窗原型查询
    ///</summary>
    public class CadDrawingWindowSearchDB
    {
        ///<summary>
        /// 外窗原型查询 分页查询
        ///</summary>
        public static IList<CadDrawingWindowSearch> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingWindowSearch> _caddrawingwindowsearchs = new List<CadDrawingWindowSearch>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   (     SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber, a.Id,
                                                                       a.DrawingCode,a.DrawingName,c.DWGPath,a.Reorder,a.CreateOn 
                                                             FROM dbo.CaddrawingMaster a 
                                                    INNER JOIN dbo.CadDrawingWindowDetail b ON a.Id=b.MId
                                                      LEFT JOIN (SELECT  Id  MId,DWGPath,FileClass FROM dbo.CadDrawingDWG  WHERE  FileClass='JPG') c ON c.MId = a.Id
                                                      WHERE 1=1  {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingwindowsearchs = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingWindowSearch>(new CadDrawingWindowSearch());
            return _caddrawingwindowsearchs;
        }

        ///<summary>
        /// 外窗原型查询  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"      SELECT   COUNT(*) AS CNT  FROM dbo.CaddrawingMaster a 
                                                        INNER JOIN   dbo.CadDrawingWindowDetail b ON a.Id=b.MId
                                                           LEFT JOIN   (SELECT  Id  MId,DWGPath,FileClass FROM dbo.CadDrawingDWG  WHERE  FileClass='JPG') c ON c.MId = a.Id
                                                               WHERE  1=1  {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

    }
}
