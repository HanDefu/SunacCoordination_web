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
    ///  栏杆CAD原型属性表
    ///</summary>
    public class CadDrawingHandrailDetailDB
    {
        ///<summary>
        /// 栏杆CAD原型属性表 分页查询
        ///</summary>
        public static IList<CadDrawingHandrailDetail> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingHandrailDetail> _caddrawinghandraildetails = new List<CadDrawingHandrailDetail>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingHandrailDetail  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawinghandraildetails = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingHandrailDetail>(new CadDrawingHandrailDetail());
            return _caddrawinghandraildetails;
        }

        ///<summary>
        /// 栏杆CAD原型属性表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingHandrailDetail WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 栏杆CAD原型属性表 根据ID查询
        ///</summary>
        public static CadDrawingHandrailDetail GetSingleEntityById(int id)
        {

            CadDrawingHandrailDetail caddrawinghandraildetail = new CadDrawingHandrailDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingHandrailDetail where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingHandrailDetail>(new CadDrawingHandrailDetail());
        }
        ///<summary>
        /// 栏杆CAD原型属性表 查询根据条件
        ///</summary>
        public static CadDrawingHandrailDetail GetSingleEntityByparam(string param)
        {

            CadDrawingHandrailDetail caddrawinghandraildetail = new CadDrawingHandrailDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingHandrailDetail where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingHandrailDetail>(new CadDrawingHandrailDetail());

        }

        ///<summary>
        /// 栏杆CAD原型属性表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingHandrailDetail caddrawinghandraildetail)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawinghandraildetail(MId,HandrailType,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},{1},{2},{3},getdate(),{4},'{5}',getdate(),{6},'{7}')", caddrawinghandraildetail.MId, caddrawinghandraildetail.HandrailType, caddrawinghandraildetail.Enabled, caddrawinghandraildetail.Reorder, caddrawinghandraildetail.CreateUserId, caddrawinghandraildetail.CreateBy, caddrawinghandraildetail.ModifiedUserId, caddrawinghandraildetail.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 栏杆CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingHandrailDetail caddrawinghandraildetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawinghandraildetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingHandrailDetail] SET [MId]=" + caddrawinghandraildetail.MId + ",[HandrailType]=" + caddrawinghandraildetail.HandrailType + ",[Enabled]=" + caddrawinghandraildetail.Enabled + ",[Reorder]=" + caddrawinghandraildetail.Reorder + ",[ModifiedUserId]=" + caddrawinghandraildetail.ModifiedUserId + ",[ModifiedBy]='" + caddrawinghandraildetail.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 栏杆CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format(@"DELETE FROM dbo.CaddrawingMaster WHERE Id={0};
                                                        DELETE FROM dbo.CadDrawingHandrailDetail WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingDWG WHERE MId={0};
                                                        DELETE FROM dbo.CadDrawingByArea WHERE MId={0};", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 栏杆CAD原型属性表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingHandrailDetail WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 栏杆CAD原型属性表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingHandrailDetail WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        ///<summary>
        /// 卫生间原型查询 分页查询
        ///</summary>
        public static IList<CadDrawingWindowSearch> GetSearchPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingWindowSearch> _caddrawingwindowsearchs = new List<CadDrawingWindowSearch>();
            string sql = string.Format(@"SELECT  * FROM 
                                                           (SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber, a.Id,
                                                                        a.DrawingCode,a.DrawingName,d.DWGPath,a.Reorder,a.CreateOn 
                                                             FROM  dbo.CaddrawingMaster a 
                                                    INNER JOIN  dbo.CadDrawingHandrailDetail b ON a.Id=b.MId
													   LEFT JOIN  (SELECT MIN(Id) AS Id, MId FROM dbo.CadDrawingDWG   GROUP BY MId) c ON c.MId = a.Id
													   LEFT JOIN  dbo.CadDrawingDWG d ON d.Id=c.Id  WHERE 1=1  {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);
            _caddrawingwindowsearchs = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingWindowSearch>(new CadDrawingWindowSearch());
            return _caddrawingwindowsearchs;
        }

        ///<summary>
        /// 卫生间原型查询  分页数据总数量
        ///<summary>
        public static int GetSearchPageCountByParameter(string _where)
        {
            string sql = string.Format(@"      SELECT   COUNT(*) AS CNT   FROM dbo.CaddrawingMaster a 
                                                       INNER JOIN  dbo.CadDrawingHandrailDetail b ON a.Id=b.MId
													     LEFT JOIN  (SELECT MIN(Id) AS Id, MId FROM dbo.CadDrawingDWG WHERE  FileClass='JPG' GROUP BY MId) c ON c.MId = a.Id
													     LEFT JOIN  dbo.CadDrawingDWG d ON d.Id=c.Id
                                                               WHERE  1=1  {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }


        public static CadDrawingHandrailDetail GetCadDrawingHandrailDetailByWhere(string _where)
        {
            CadDrawingHandrailDetail _caddrawingwindow = new CadDrawingHandrailDetail();
            string bathroom_sql = string.Format(@"   SELECT  a.*,b.ArgumentText AS HandrailTypeName
                                                                              FROM   dbo.CadDrawingHandrailDetail a 
                                                                        LEFT JOIN  dbo.BasArgumentSetting b ON a.HandrailType=b.Id AND b.TypeCode='HandRail'
                                                                        WHERE {0}", _where);
            _caddrawingwindow = MsSqlHelperEx.ExecuteDataTable(bathroom_sql).ConverToModel<CadDrawingHandrailDetail>(new CadDrawingHandrailDetail());
            return _caddrawingwindow;
        }

        public static BPMHandrail GetBPMHandrailByHandrailId(int handraidId) 
        {
            BPMHandrail handrail = new BPMHandrail();
            string sql = string.Format(@"SELECT   'P51' AS PageCode, m.Id AS prototypeID,
		                                                              b.ArgumentText AS railingType
                                                              FROM   dbo.CadDrawingHandrailDetail a
                                                            INNER JOIN dbo.CaddrawingMaster m ON m.Id=a.MId
                                                             LEFT JOIN  dbo.BasArgumentSetting b ON a.HandrailType=b.Id AND b.TypeCode='HandRail'
                                                             WHERE a.Id={0}", handraidId);
            handrail = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<BPMHandrail>(new BPMHandrail());
            string _where = string.Format(@" MId={0}", handraidId);
            string _str_area = "";
            IList<CadDrawingByArea> areas = CadDrawingByAreaDB.GetCadDrawingByAreasByWhere(_where);
            foreach (CadDrawingByArea area in areas)
            {
                _str_area += area.AreaName + ",";
            }
            _str_area = _str_area.TrimEnd(',');
            handrail.region = _str_area;
            return handrail;
        }


    }
}